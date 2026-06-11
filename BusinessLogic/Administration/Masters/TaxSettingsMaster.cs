using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters.TaxSettings;
using System.Web;
using System.Text.RegularExpressions;
using GTIService;
using BusinessObject;


namespace BusinessLogic.Administration.Masters
{
    public class TaxSettingsMaster
    {
        /// <summary>
        /// Get list of Tax Materials for fill the dropdown
        /// <param name=""></param>
        /// <returns></returns>
        public static string GetTaxParameters()
        {

            DataTable dsDispersionList = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxParameters();
            //convert to json string
            return GTIService.CommonFunctions.GetTextValueList(dsDispersionList, Fields.PARAMNAME, Fields.PARAMVAL);
            
        }

        /// <summary>
        /// Saving Tax Settings
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveTaxSettings(string requestData, int taxType)
        {
            BusinessObject.Administration.Masters.TaxSettings taxsettings = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.TaxSettings>(requestData);

            string formulationEval = taxsettings.TAX_Formula;
            MatchCollection matchcol = Regex.Matches(formulationEval, @"\#[\w]+\#");
            foreach (var match in matchcol)
            {
                formulationEval = formulationEval.Replace(match.ToString(), "1");
            }
            if (taxsettings.TAX_CATEGORY == 4 && taxsettings.TAX_Formula == string.Empty)//Expression validation is not needed for Deduction without formula
            {
                taxsettings.TAX_TYPE = taxType;
                return DataAccess.Administration.Masters.TaxSettingsMasterDL.SaveTaxSettings(taxsettings);
            }
            else
            {
                if (GTIService.CommonFunctions.Evaluateexpression(formulationEval))
                {
                    taxsettings.TAX_TYPE = taxType;
                    return DataAccess.Administration.Masters.TaxSettingsMasterDL.SaveTaxSettings(taxsettings);
                }
                else
                {
                    return "-5";
                }
            }
        }

        /// <summary>
        /// Returns material list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetTaxDetails(GridPrams grid, int bizUnit)
        {
            DataSet dsTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxDetails(grid, bizUnit);
            string jString = string.Empty;
            if (dsTaxDetails.Tables.Count > 1 && dsTaxDetails.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTaxDetails);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="isTax"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetTaxTypeDetails(GridPrams grid,int isTax, int bizUnit)
        {
            DataSet dsTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxTypeDetails(grid,isTax, bizUnit);
            string jString = string.Empty;
            if (dsTaxDetails.Tables.Count > 1 && dsTaxDetails.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTaxDetails);
            }
            return jString;
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue,int bizUnit)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
        

        /// <summary>
        /// Methord used to get Active tax for the current day
        /// </summary>
        /// <createdby>Vineeth Babu</createdby>
        /// <for>Po Creation</for>
        /// <used in>Po creation assigning tax settings to controls </used in>
        /// <param name="bizUnitpk"></param>
        /// <returns> string</returns>
        public static string GetActiveTax(int bizUnitPK)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetActiveTax(bizUnitPK);
            string jString = string.Empty;
            if (dtTaxDetails.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTaxDetails);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTaxCategory(int bizUnitPK)
        {
            DataTable dtDispersionList = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxCategory(bizUnitPK);
            //convert to json string
            return GTIService.CommonFunctions.GetTextValueList(dtDispersionList, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGTEXT, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGVALUE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfgType"></param>
        /// <param name="bizUnitPK"></param>
        /// <returns></returns>
        public static string GetCfgValue(string cfgType, int bizUnitPK)
        {
            DataTable dtDispersionList = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetCfgValue(cfgType,bizUnitPK);
            //convert to json string
            return GTIService.CommonFunctions.GetTextValueList(dtDispersionList, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGTEXT, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGVALUE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsTax"></param>
        /// <param name="bizUnitPK"></param>
        /// <returns></returns>
        public static string GetTaxTypeCategory(int IsTax,int bizUnitPK)
        {
            DataTable dtDispersionList = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxTypeCategory(IsTax,bizUnitPK);
            //convert to json string
            return GTIService.CommonFunctions.GetTextValueList(dtDispersionList, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGTEXT, GTIService.Constants.Administration.Masters.TaxSettings.Fields.CONFIGVALUE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetActiveCategoryValue(int taxPK, int categoryPK, int bizUnit, int active)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetActiveCategoryValue(taxPK, categoryPK, bizUnit, active);
            string jString = string.Empty;
            var taxList = from lst in dtTaxDetails.AsEnumerable()
                          select new { 
                                    Value = lst.Field<int>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXPK),
                                    Text = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXHEAD),
                                    Formula = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.FORMULA),
                          };
            //if (dtTaxDetails.Rows.Count > 0)
            //{
            //    jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            //}
            jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            return jString;
            
        }

        /// <summary>
        /// Get Tax Details
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetTaxDetails(int taxPK, int categoryPK, int bizUnit, int active)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetActiveCategoryValue(taxPK, categoryPK, bizUnit, active);
            return dtTaxDetails;

        }

        /// <summary>
        /// Get Tax Category
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="subCategoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static string GetTaxCategoryValue(int categoryPK,int subCategoryPK, int bizUnit, int active)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetTaxCategoryValue(categoryPK, subCategoryPK, bizUnit, active);
            string jString = string.Empty;
            var taxList = from lst in dtTaxDetails.AsEnumerable()
                          select new
                          {
                              Value = lst.Field<int>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXPK),
                              Text = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXHEAD),
                              Formula = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.FORMULA),
                          };
            //if (dtTaxDetails.Rows.Count > 0)
            //{
            //    jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            //}
            jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            return jString;

        }

        /// <summary>
        /// Get Tax Category 
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="subCategoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetTaxTypes(int categoryPK, int subCategoryPK, int bizUnit, int active,int isTaxSale=0)
        {
            DataTable dtTaxDetails= DataAccess.Administration.Masters
                                    .TaxSettingsMasterDL
                                    .GetTaxCategoryValue(categoryPK, subCategoryPK, bizUnit, active,isTaxSale);
            return dtTaxDetails;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="taxPK"></param>
       /// <param name="categoryPK"></param>
       /// <param name="bizUnit"></param>
       /// <param name="active"></param>
       /// <param name="taxDate"></param>
       /// <returns></returns>
        public static string GetActiveCategoryDateValue(int categoryPK, int bizUnit, int active, DateTime taxDate, int subCategory, string specialCond = null, int taxDue = 0, int IsPurchaseTax=0)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetActiveCategoryDateValue(categoryPK, bizUnit, active, taxDate, subCategory, specialCond, taxDue,IsPurchaseTax);
            string jString = string.Empty;
            var taxList = from lst in dtTaxDetails.AsEnumerable()
                          select new
                          {
                              Value = lst.Field<int>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXPK),
                              Text = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXHEAD),
                              Formula = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.FORMULA),
                          };
            //if (dtTaxDetails.Rows.Count > 0)
            //{
            //    jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            //}
            jString = Newtonsoft.Json.JsonConvert.SerializeObject(taxList);
            return jString;

        }

        //Vendor tax
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static string GetVendorActiveCategoryDateValue(int itemPK, int venderPK,  int active, DateTime taxDate)
        {
            DataTable dtTaxDetails = DataAccess.Administration.Masters.TaxSettingsMasterDL.GetVendorActiveCategoryDateValue(itemPK, venderPK, active, taxDate);
            string jString = string.Empty;
            //var taxList = from lst in dtTaxDetails.AsEnumerable()
            //              select new
            //              {
            //                  Value = lst.Field<int>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXPK),
            //                  Text = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.TAXHEAD),
            //                  Formula = lst.Field<string>(GTIService.Constants.Administration.Masters.TaxSettings.Fields.FORMULA),
            //              };

            jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTaxDetails);
            return jString;

        }

        //end


        /// <summary>
        /// Get Active Tax Category by Date
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <returns></returns>
        public static DataTable GetActiveTaxCategoryDateValue(int categoryPK, int bizUnit, int active, DateTime taxDate, int subCategory, string specialCond = null, int taxDue = 1, int? purchaseTax = 0, int? isSalestax = 0,string CatXML=null)
        {
            return DataAccess.Administration.Masters.TaxSettingsMasterDL.GetActiveCategoryDateValue(categoryPK, bizUnit, active, taxDate, subCategory, specialCond, taxDue, purchaseTax, isSalestax,CatXML);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="taxPK"></param>
        /// <returns></returns>
        public static string DeleteTaxDetails(int taxPK)
        {
            return DataAccess.Administration.Masters.TaxSettingsMasterDL.DeleteTaxDetails(taxPK).ToString();
        }
    }
}
