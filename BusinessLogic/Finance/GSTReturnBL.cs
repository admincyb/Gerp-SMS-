using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.Finance;
namespace BusinessLogic.Finance
{
    public class GSTReturnBL
    {
        public static GSTReturnHeader GetDetails(DateTime startDate, DateTime endDate, int bizUnit, byte Active, int companyPk)
        {
            try
            {
                GSTReturnHeader GSTReturnHeaderObj = new GSTReturnHeader();
                string GSTReturnList = GSTReturnDL.GetDetails(startDate, endDate, bizUnit, Active, companyPk);
                if (GSTReturnList != string.Empty)
                {
                    GSTReturnHeaderObj = (GSTReturnHeader)CommonFunctions.DeserializeObject(GSTReturnList, GSTReturnHeaderObj);
                    return GSTReturnHeaderObj;
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
        //Get GST Return Details with PK
        public static GSTReturnHeader GetDetailsWithPk(int GSTReturnPk, int bizUnit, byte Active, int companyPk)
        {
            try
            {
                GSTReturnHeader GSTReturnHeaderObj = new GSTReturnHeader();
                string GSTReturnList = GSTReturnDL.GetDetailsWithPk(GSTReturnPk, bizUnit, Active, companyPk);
                if (GSTReturnList != string.Empty)
                {
                    GSTReturnHeaderObj = (GSTReturnHeader)CommonFunctions.DeserializeObject(GSTReturnList, GSTReturnHeaderObj);
                    return GSTReturnHeaderObj;
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
        //Save GST Return
        public static int? SaveGSTReturn(string strxml)
        {
            return GSTReturnDL.SaveGSTReturn(strxml);
        }
        public static DataSet GetGSTReturnReport(int RecPK)
        {
            return GSTReturnDL.GetGSTReturnReport(RecPK);
        }
       //Get GSTReturn List
        public static DataSet GetReportList( byte Active, int bizUnit,int companyPk)
        {
            return GSTReturnDL.GetReportList(Active, bizUnit, companyPk);
        }
        //Delete GSTReturn
        public static int DeleteGSTReturn(int GSTReturnPk, DateTime lastModDate)
        {
            return GSTReturnDL.DeleteGSTReturn(GSTReturnPk, lastModDate);
        }
    }
}
