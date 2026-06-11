using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using BusinessObject;
using BusinessObject.VendorManagement;
using DataAccess.UserControl;

namespace BusinessLogic.UserControl
{
    public class AdvanceSearch
    {
        /// <summary>
        /// Function used to save advance search details
        /// </summary>
        /// <param name="advSrchDetails"></param>
        /// <returns></returns>
        public static string SaveAdvSearchDetails(string advSrchDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(advSrchDetails);
            return AdvanceSearchDL.SaveAdvSearchDetails(xmlstr);
        }

        /// <summary>
        /// Function Used to Get Advance Search List
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static object GetAdvanceSearchList(GridPrams gridPrams, int user,string pageTitle)
        {
            DataSet dsSearchList = AdvanceSearchDL.GetAdvanceSearchList(gridPrams, user, pageTitle);
            string jString = string.Empty;
            if (dsSearchList.Tables.Count > 1 && dsSearchList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSearchList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used to get advance search details by search id
        /// </summary>
        /// <param name="srchPK"></param>
        /// <returns></returns>
        public static string GetAdvanceSearchDetails(int srchPK)
        {
            return GTIService.CommonFunctions.XmlToJson(AdvanceSearchDL.GetAdvanceSearchDetails(srchPK)).ToString();
        }
    }
}
