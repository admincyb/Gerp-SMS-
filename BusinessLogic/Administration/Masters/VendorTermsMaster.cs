using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using BusinessObject;
using DataAccess.Administration.Masters;
using GTIService.Constants.Vendor;

namespace BusinessLogic.Administration.Masters
{
    //for converting Terms type
    enum TermsType
    {
        Date=1,
        Text=2,
        Numeric=3

    };
    public class VendorTermsMaster
    {
        /// <summary>
        /// Returns Evaluation list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
       public static string SaveVendorTermsDetails(string vendorTerm)
       {
           BusinessObject.Administration.Masters.VendorTerm VendorTermObj = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.VendorTerm>(vendorTerm);
           return DataAccess.Administration.Masters.VenderTermsDL.SaveVendorterm(VendorTermObj);
       }
       /// <summary>
       /// Returns Vendor Terms list in json string format
       /// </summary>
       /// <param name="grid"></param>
       /// <returns>string</returns>
       public static string GetVendorTermsList(GridPrams grid, int bizUnitPk)
       {
           //trim off "VET_TYPE_STRING" if field list contains the field
           if (grid.Fields.Contains("VET_TYPE_STRING"))
           {
               grid.Fields = grid.Fields.Replace(",VET_TYPE_STRING", string.Empty);
           }
           DataSet dsVendorTrmsList = DataAccess.Administration.Masters.VenderTermsDL.GetVendorTermsList(grid, bizUnitPk);
           //For covnverting Terms type into Text 
           TermsType ttyep;
           //add new column for saving text
           dsVendorTrmsList.Tables[1].Columns.Add("VET_TYPE_STRING");
           foreach (DataRow dr in dsVendorTrmsList.Tables[1].Rows)
           {
               ttyep = (TermsType)(Convert.ToInt32(dr["VET_TYPE"]));
               dr["VET_TYPE_STRING"] = ttyep.ToString();
           }
           string jString = string.Empty;
           if (dsVendorTrmsList.Tables.Count > 1 && dsVendorTrmsList.Tables[1].Rows.Count > 0)
           {
               jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsVendorTrmsList);    
           }
           return jString;
       }
       /// <summary>
       /// Delete Vendor Terms Details
       /// </summary>
       /// <param name="TermID"></param>
       /// <returns>String</returns>
       public static string DeleteVendorTerms(int termID)
       {
           return VenderTermsDL.DeleteVendorTermsDtls(termID).ToString();
       }    
       /// <summary>
       /// Returns the search result list for Autocomplete 
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <returns></returns>
       public static string GetSearchValues(string searchBy, string searchValue,int bizUnitPk)
       {
           DataTable dtSearch = DataAccess.Administration.Masters.VenderTermsDL.GetSearchValues(searchBy, searchValue, bizUnitPk);
           string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, "VALUE", "PK");
           return jString;
       }
       /// <summary>
       /// Returns the search result list for Autocomplete 
       /// </summary>
       /// <param name="BizUnitPk"></param>
       /// <Createdby>Vineeth Babu</Createdby>
       /// <For>Po Generation</For>
       /// <usedin>Po Creation Listing Vendor Terms</usedin>
       /// <returns></returns>
       public static string GetVendorTerms(int vendorID,int termsPK)
       {
           string jString = string.Empty;
           if (termsPK == 0)
               jString = GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.VenderTermsDL.GetVendorTerms(vendorID, termsPK), GTIService.Constants.Vendor.Fields.VENDORTERMSTITLE, GTIService.Constants.Vendor.Fields.VENDORTERMSPK);
           else
           {
               DataTable dtTermsDetails = DataAccess.Administration.Masters.VenderTermsDL.GetVendorTerms(vendorID, termsPK);
               if (dtTermsDetails.Rows.Count > 0)
               {
                   jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTermsDetails);
               }
           }

           return jString; 
       }

        public static DataTable GetVendorTermsByID(int vendorID, int termsPK)
        {
            return DataAccess.Administration.Masters.VenderTermsDL.GetVendorTerms(vendorID, termsPK);
        }
    }
}
