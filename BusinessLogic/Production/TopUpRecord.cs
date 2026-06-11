using System.Data;
using BusinessObject;

namespace BusinessLogic.Production
{
    public class TopUpRecord
    {
        # region Methods
        /// <summary>
        /// Get Top Up Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetTopUpList(GridPrams grid, User objUser)
        {
            DataSet dsTopUpList = DataAccess.Production.TopUpRecordDL.GetTopUpList(grid, objUser);
            string jString = string.Empty;
            if (dsTopUpList.Tables.Count > 1 && dsTopUpList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTopUpList);
            }
            return jString;
        }

        /// <summary>
        /// Get AutoComplete Search value
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.Production.TopUpRecordDL.GetSearchValues(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// Delete TopUp Details
        /// </summary>
        /// <param name="requestPk"></param>
        /// <returns></returns>
        public static string DeleteTopUpDtls(int topUpPk)
        {
            return DataAccess.Production.TopUpRecordDL.DeleteTopUpDtls(topUpPk).ToString();
        }

        /// <summary>
        /// Get UOM List For TopUp Items
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static string GetUomList(int tankPK)
        {
            DataTable dtUOMList = DataAccess.Production.TopUpRecordDL.GetUomList(tankPK);
            return GTIService.CommonFunctions.GetTextValueList(dtUOMList, GTIService.Constants.Production.Fields_TopUpRecord.UOMCODE, GTIService.Constants.Production.Fields_TopUpRecord.UOMPK);
        }

        /// <summary>
       /// Save TopUp Details
       /// </summary>
       /// <param name="machineDetails"></param>
       /// <returns></returns>
        public static string SaveTopUpDetails(string machineDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(machineDetails);
            int topUpPk = DataAccess.Production.TopUpRecordDL.SaveTopUpDetails(xmlstr);
            return topUpPk.ToString();
        }

        /// <summary>
        /// Get Top Up Details By TopUp Pk
        /// </summary>
        /// <param name="toupPk"></param>
        /// <returns></returns>
        public static string GetTopUpDetails(int toupPk)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.TopUpRecordDL.GetTopUpDetails(toupPk)).ToString();
        }

        /// <summary>
        /// Check Stock Available or not
        /// </summary>
        /// <param name="itemCatg"></param>
        /// <param name="item"></param>
        /// <param name="qty"></param>
        /// <param name="toUOM"></param>
        /// <param name="topUpItemPk"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static string CheckStockAvailable(int itemCatg, int item, double qty, int toUOM, int topUpItemPk, int sbu)
        {
            return DataAccess.Production.TopUpRecordDL.CheckStockAvailable( itemCatg,  item,  qty,  toUOM,  topUpItemPk,  sbu).ToString();
        }
            
        /// <summary>
        /// Get Item name List For TopUp items
        /// </summary>
        /// <param name="sbu"></param>
        /// <param name="catg"></param>
        /// <param name="tankPK"></param>
        /// <returns></returns>
        public static string GetItemNameList(int sbu, int catg, int tankPK)
        {
            DataTable dtSearch = DataAccess.Production.TopUpRecordDL.GetItemNameList(sbu, catg, tankPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Production.Fields_TopUpRecord.VALUE,  GTIService.Constants.Production.Fields_TopUpRecord.PK);

        }

        # endregion
    }
}
