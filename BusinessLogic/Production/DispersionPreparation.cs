using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Production
{
    public class DispersionPreparation
    {
        /// <summary>
        /// Dispersion Preparation Print Report
        /// </summary>
        public static DataSet GetDispPreparationReport(int RequistID)
        {
            return DataAccess.Production.DispersionPreparationDL.GetDispPreparationReport(RequistID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetDispersionMaterialDetail(int dispersionID, int dept)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.DispersionPreparationDL.GetDispersionMaterialDetail(dispersionID, dept));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetDispersionPreparation(int dispersionID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.DispersionPreparationDL.GetDispersionPreparation(dispersionID));
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="purReqDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveDispersionPreparation(string purReqDetails, BusinessObject.User objUser)
        {
            string dispersionPreparationID = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(purReqDetails);
            List<object> retvals = DataAccess.Production.DispersionPreparationDL.SaveDispersionPreparation(xmlstr);
            dispersionPreparationID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }


        /// <summary>
        /// Get Purchase Request Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetDispersionPreparationList(GridPrams grid, BusinessObject.User objUser, int procID, string pageURL = null, int dept = 0)
        {
            DataSet dsReqstList = DataAccess.Production.DispersionPreparationDL.GetDispersionPreparationList(grid, objUser, procID, pageURL, dept);
            string jString = string.Empty;
            if (dsReqstList.Tables.Count > 1 && dsReqstList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsReqstList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispersionPk"></param>
        /// <returns></returns>
        public static string DeleteDispersionPreparation(int dispersionPk)
        {
            return DataAccess.Production.DispersionPreparationDL.DeleteDispersionPreparation(dispersionPk).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDISPNO()
        {
            return DataAccess.Production.DispersionPreparationDL.GetDISPNO();
        }

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, BusinessObject.User objUser, int procID)
        {
            DataTable dtSearch = DataAccess.Production.DispersionPreparationDL.GetSearchValues(searchBy, searchValue, objUser, procID);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK);
        }

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetInspectionDetails(int batchPK, int batchType, int bizUnit)
        {
            DataSet dsInspDtls = DataAccess.Production.DispersionPreparationDL.GetInspectionDetails(batchPK, batchType, bizUnit);
            string jString = string.Empty;
            if (dsInspDtls != null && dsInspDtls.Tables.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsInspDtls);
            }
            return jString;
        }


        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetRawMaterialInspectionDetails(int trxPK, int bizUnit)
        {
            DataTable dtRawMaterialInspectionTest = DataAccess.Production.DispersionPreparationDL.GetRawMaterialInspectionDetails(trxPK, bizUnit);
            string strXml = string.Empty;
            if (dtRawMaterialInspectionTest != null)
            {
                if (dtRawMaterialInspectionTest.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtRawMaterialInspectionTest.Rows)
                    {
                        strXml += Convert.ToString(drData[0]);
                    }
                }
            }
            return GTIService.CommonFunctions.XmlToJson1(strXml);
        }

        /// <summary>
        /// Get AuoComplete Search Details for Materials
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetMaterialNameAuto(int sBU, int catg, int cat_type = 0, string SearchVal = null)
        {
            DataTable dtSearch = DataAccess.Production.DispersionPreparationDL.GetMaterialNameAuto(sBU, catg, cat_type, SearchVal);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK);
        } 
    }
}
