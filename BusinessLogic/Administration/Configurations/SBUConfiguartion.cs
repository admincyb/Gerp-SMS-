using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.Administration.Configurations
{
    public class SBUConfiguartion
    {
        /// <summary>
        /// Function Used To save sbu details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveSBUConfig(string requestData)
        {
            
            BusinessObject.Administration.Configurations.SBUConfiguartion sbuConfiguartion = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Configurations.SBUConfiguartion>(requestData);
            return DataAccess.Administration.Configurations.SBUConfiguartionDL.SaveSBUConfig(sbuConfiguartion);
        }

        /// <summary>
        /// Function Used To active / inactive sbu 
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static string ActiveSBUConfig(int sbuPK, int userPK, int active)
        {
            return DataAccess.Administration.Configurations.SBUConfiguartionDL.ActiveSBUConfig(sbuPK, userPK, active);
        }

        /// <summary>
        /// Function Used To get sbu details
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <returns></returns>
        public static string GetSBUConfig(BusinessObject.GridPrams gridPrams)
        {
            DataSet dsSBUList = DataAccess.Administration.Configurations.SBUConfiguartionDL.GetSBUConfig(gridPrams);
            string jString = string.Empty;
            if (dsSBUList.Tables.Count > 1 && dsSBUList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSBUList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo
        /// </summary>
        /// <returns></returns>
        public static string GetAllSBU()
        {
            DataTable dtCategory = DataAccess.Administration.Configurations.SBUConfiguartionDL.GetAllSBU(0);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Configurations.SBUConfig.Fields.SBUNAME, GTIService.Constants.Configurations.SBUConfig.Fields.SBUPK);
        }

        /// <summary>
        /// Function Used To get all active sbu details of the perticular user
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMasterSBU(int userPK)
        {
            return DataAccess.Administration.Configurations.SBUConfiguartionDL.GetAllSBU(userPK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetBizUnit(int bizUnit)
        {
            return DataAccess.Administration.Configurations.SBUConfiguartionDL.GetBizUnit(bizUnit);
        }


        /// <summary>
        /// Get Dispersion Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns></returns>
        public static string GetSBUConfigDetails(int SBUID)
        {


            // BusinessObject.Administration.Masters.CurrencyMaster CurrencyManagement.Currency obj = new BusinessObject.CurrencyManagement.Currency();
            //get Currency detail in xml format convert to json

            DataSet dsSBUList = DataAccess.Administration.Configurations.SBUConfiguartionDL.GetSBUConfigDetails(SBUID);
            string jString = string.Empty;
            if (dsSBUList.Tables[0].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSBUList.Tables[0]);
            }
            return jString;   
        }
        
        /// <summary>
        /// Get Footer Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns></returns>
        public static string GetSBUFooterDetails()
        {

            DataTable dtSBUfooterList = DataAccess.Administration.Configurations.SBUConfiguartionDL.GetSBUFooterDetails();
            string jString = string.Empty;
            if (dtSBUfooterList.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSBUfooterList);
            }
            return jString;   
        }

        

    }
}
