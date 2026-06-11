using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class MaterialAccept
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPendingSearchAuto(string searchBy, string searchValue, int deptPK, int mahPK, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterialAcceptDL.GetPendingSearchAuto(searchBy, searchValue, deptPK, mahPK, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="mahID"></param>
        /// <returns></returns>
        public static string GetStoreIssuePending(GridPrams grid, int sbuID, int mahPK, int store, int miPK)
        {
            DataSet dsPOList = DataAccess.StoreManagement.MaterialAcceptDL.GetStoreIssuePending(grid, sbuID, mahPK, store, miPK);
            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialAcceptDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveMaterialAccept(string materialAcceptDetails, User objUser)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialAcceptDetails);
            List<object> retvals = DataAccess.StoreManagement.MaterialAcceptDL.SaveMaterialAccept(xmlstr);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string SaveSTAConversion(string STAConversionDetails, User objUser)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(STAConversionDetails);
            List<object> retvals = DataAccess.StoreManagement.MaterialAcceptDL.SaveSTAConversion(xmlstr);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

/// <summary>
/// 
/// </summary>
/// <param name="mahID"></param>
/// <returns></returns>
public static string GetMaterialAccept(int mahID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.MaterialAcceptDL.GetMaterialAccept(mahID));
        }
        public static DataTable GetAcceptStore(int appID)
        {
            return DataAccess.StoreManagement.MaterialAcceptDL.GetAcceptStore(appID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetMaterialAcceptList(GridPrams grid, User objUser,string pageURL)
        {
            DataSet dsMAList = DataAccess.StoreManagement.MaterialAcceptDL.GetMaterialAcceptList(grid, objUser, pageURL);
            string jString = string.Empty;
            if (dsMAList.Tables.Count > 1 && dsMAList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMAList);
            }
            return jString;
        }

        public static string GetSTAforConvert(int mahpk)
        {
            DataSet dsSTAConverted = DataAccess.StoreManagement.MaterialAcceptDL.GetSTAforConvert(mahpk);
            string jString = string.Empty;
            if (dsSTAConverted.Tables.Count > 1 && dsSTAConverted.Tables[1].Rows.Count > 0)
            {
                 jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSTAConverted);
            }

            System.Collections.ArrayList root = new System.Collections.ArrayList();
            List<Dictionary<string, object>> table;
            Dictionary<string, object> data;

            foreach (DataTable dt in dsSTAConverted.Tables)
            {
                table = new List<Dictionary<string, object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    data = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        data.Add(col.ColumnName, dr[col]);
                    }
                    table.Add(data);
                }
                root.Add(table);
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(root);
            // return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetMANO()
        {
            return DataAccess.StoreManagement.MaterialAcceptDL.GetMANO();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetMASearchValue(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterialAcceptDL.GetMASearchValue(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mahPK"></param>
        /// <returns></returns>
        public static string DeleteMaterialAccept(int mahPK)
        {            
            List<object> retvals = DataAccess.StoreManagement.MaterialAcceptDL.DeleteMaterialAccept(mahPK);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string DeletSTAConversion(int mahPK)
        {
            List<object> retvals = DataAccess.StoreManagement.MaterialAcceptDL.DeletSTAConversion(mahPK);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mihID"></param>
    /// <returns></returns>
    public static string GetPreviousMADetailsView(int mihID)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.MaterialAcceptDL.GetPreviousMADetailsView(mihID);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }
    }
}
