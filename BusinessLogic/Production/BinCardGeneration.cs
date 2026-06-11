using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Production
{
    public class BinCardGeneration
    {
        /// <summary>
        /// Function Used To get the running BinCard number
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin Card Generation</for>
        /// Used in fill the Bin Card number
        /// <returns></returns>
        public static string GetBinCardumber()
        {
            return DataAccess.Production.BinCardGenerationDL.GetBinCardNumber();
        }

        /// <summary>
        /// Function Used Save/update Bin Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin CardGeneration Creation</for>
        /// Used in Saving and updating Bin Details
        /// <returns></returns>
        public static string SaveBinDetails(string xmlBinDetails, BusinessObject.User objUser)
        {
            string binNumber = string.Empty;
            string binID =string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlBinDetails);
            binID = DataAccess.Production.BinCardGenerationDL.SaveBinDetails(xmlstr, out binNumber).ToString();
            return binNumber;
        }

        /// <summary>
        /// Function Used Get Bin Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin Card Creation</for>
        /// Used in Get Bin Details
        /// <returns></returns>
        public static string GetBinDetails(int binID)
        {
            try
            {
                //BusinessObject.OrderManagement.OrderMaster obj = new BusinessObject.OrderManagement.OrderMaster();
                return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.BinCardGenerationDL.GetBinDetails(binID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Bin Card Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }


        #region For PO LIsting

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.Production.BinCardGenerationDL.GetSearchValues(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }

        /// <summary>
        /// Returns the PO list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetBinCardDetails(GridPrams grid, int sbuID)
        {
            grid.SortBy = grid.SortBy == null ? "BCH_PK" : grid.SortBy;
            grid.SortDirection = grid.SortDirection == null ? "Desc" : grid.SortDirection;
            DataSet dsBinList = DataAccess.Production.BinCardGenerationDL.GetBinCardDetails(grid, sbuID);

            string jString = string.Empty;
            if (dsBinList.Tables.Count > 1 && dsBinList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsBinList);
            }
            return jString;
        }

        /// <summary>
        /// Logic Methord used to delete a PO details by passing PO ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>string</returns>
        public static string DeleteBinCardDetails(int binID)
        {
            return DataAccess.Production.BinCardGenerationDL.DeleteBinCardDetails(binID).ToString();
        }

        #endregion


       
    }
}
