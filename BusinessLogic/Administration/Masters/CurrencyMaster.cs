using System.Data;
using BusinessObject;
using GTIService.Constants.Currency;
using DataAccess.MaterialManagement;
using BusinessObject.CommonManagement;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BusinessLogic.Administration.Masters
{
   public class CurrencyMaster
    {
        /// <summary>
        ///  Function Used To Get all exchange from sbuPk
        /// </summary>
        /// <returns>string</returns>
       public static string GetExchangeType()
        {
            DataTable dtExchangeType = DataAccess.Administration.Masters.CurrencyMasterDL.GetExchangeType();
            string jString = string.Empty;
            if (dtExchangeType.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtExchangeType, GTIService.Constants.Currency.Fields.EXCHANGEPK, GTIService.Constants.Currency.Fields.EXCHANGEVALUE);

            }
            return jString;
        }
        /// <returns>string</returns>
       public static string GetExchangeTypeList()
        {
            DataTable dtExchangeType = DataAccess.Administration.Masters.CurrencyMasterDL.GetExchangeType();
            string jString = string.Empty;
            if (dtExchangeType.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtExchangeType);

            }
            return jString;
        }
        /// <summary>
       /// Delete exchange Details
        /// </summary>
       /// <param name="exchangeMasterId"></param>
        /// <returns>String</returns>
       public static string DeleteExchangeType(int exchangeMasterId)
        {
            return DataAccess.Administration.Masters.CurrencyMasterDL.DeleteExchangeType(exchangeMasterId).ToString();
        }
        /// <summary>
       /// Saving exchange details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
       public static string SaveExchangeType(string requestData)
        {
            BusinessObject.Administration.Masters.CurrencyMaster exchange = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.CurrencyMaster>(requestData);
            return DataAccess.Administration.Masters.CurrencyMasterDL.SaveExchangeMaster(exchange);
        }
        /// <summary>
        /// Returns Currency Master details list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetCurrencyMasterList(GridPrams grid,int bizUnit)
        {
            DataSet dsCurrencyMasterList = DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencyMasterList(grid,bizUnit);
            string jString = string.Empty;
            if (dsCurrencyMasterList.Tables.Count > 1 && dsCurrencyMasterList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsCurrencyMasterList);
            }
            return jString;
        }
        /// <summary>
        /// Saving Currency Master
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveCurrencyMaster(string strJson)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(strJson);
            return DataAccess.Administration.Masters.CurrencyMasterDL.SaveCurrencyMaster(xmlstr);
        }
       /// <summary>
        /// Save Currency Conversion
       /// </summary>
       /// <param name="strJson"></param>
       /// <returns></returns>
        public static string SaveCurrencyConversion(string strJson)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(strJson);
            return DataAccess.Administration.Masters.CurrencyMasterDL.SaveCurrencyConversion(xmlstr);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
       
        /// <returns>string</returns>
        public static string GetCurrencyMasterSearchValues(string searchValue, string searchBy,int bizUnit)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencyMasterSearchValues(searchValue, searchBy,bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
        /// <summary>
        /// Delete CurrencyMaster Details
        /// </summary>
        /// <param name="currencyMasterId"></param>
        /// <returns>String</returns>
        public static string DeleteCurrencyMaster(int currencyMasterId)
        {
            return DataAccess.Administration.Masters.CurrencyMasterDL.DeleteCurrencyMasterDtls(currencyMasterId).ToString();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="curConversionPK"></param>
       /// <returns></returns>
        public static string DeleteCurrencyConversionDtls(int curConversionPK)
        {
            return DataAccess.Administration.Masters.CurrencyMasterDL.DeleteCurrencyConversionDtls(curConversionPK).ToString();
        }

        /// <summary>
        /// Get Currencies for to fill the dropdown
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string GetCurrencyForCombo(int bizUnit)
        {
            //get Currency list
            DataTable dsDispersionList = DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencieForCombo(bizUnit);
            string jString = string.Empty;
            //convert to json string
            jString = GTIService.CommonFunctions.GetTextValueList(dsDispersionList, Fields.CURCODE, Fields.CURPK);

            return jString;
        }

        /// <summary>
        /// Get Dispersion Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns></returns>
        public static string GetCurrencyDetailsList(GridPrams grid,int CurrencyID)
        {

            DataSet dsCurrencyDetailList = DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencyDetailsList(grid, CurrencyID);
            string jString = string.Empty;
            if (dsCurrencyDetailList.Tables.Count > 1 && dsCurrencyDetailList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsCurrencyDetailList);
            }
            return jString;
               // BusinessObject.Administration.Masters.CurrencyMaster CurrencyManagement.Currency obj = new BusinessObject.CurrencyManagement.Currency();
                //get Currency detail in xml format convert to json
                //return GTIService.CommonFunctions.XmlToJson(DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencyDetailsList(grid,CurrencyID));
           
           
        }

        public static string GetCurrencyDetails( int CurrencyID)
        {


            // BusinessObject.Administration.Masters.CurrencyMaster CurrencyManagement.Currency obj = new BusinessObject.CurrencyManagement.Currency();
            //get Currency detail in xml format convert to json
            return GTIService.CommonFunctions.XmlToJson(DataAccess.Administration.Masters.CurrencyMasterDL.GetCurrencyDetail(CurrencyID));


        }

        /// <summary>
        /// Returns the search result list for Autocomplete for Vendor exchange Currencies
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="vendorPK"></param>
        /// <param name="excDate"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetVendorExchangeCurrencyAuto(string searchValue, int vendorPK, DateTime? excDate)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.Administration.Masters.CurrencyMasterDL.GetVendorExchangeCurrencyAuto(searchValue, vendorPK, excDate);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(Fields.SEARCHVALUEFIELD),
                    Name = row.Field<string>(Fields.SEARCHTEXTFIELD)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }
    }
}
