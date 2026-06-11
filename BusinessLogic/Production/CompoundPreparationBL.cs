using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.Production;
using GTIService.Constants.Production;

namespace BusinessLogic.Production
{
   public class CompoundPreparationBL
    {
        /// <summary>
        /// Compound Preparation Print Report
        /// </summary>
       public static DataSet GetCmpPreparationReport(int RequistID)
       {
           return DataAccess.Production.CompoundPreparationDL.GetCmpPreparationReport(RequistID);
       }

        /// <summary>
        /// Function Used To get the next Batch No
        /// </summary>
        /// <returns></returns>
       public static string GetCompoundBatchNo()
        {
            return DataAccess.Production.CompoundPreparationDL.GetCompoundBatchNo();
        }


       /// <summary>
       /// Get list of compounds to fill the dropdown
       /// <param name="grid"></param>
       /// <returns></returns>
       public static string GetCompoundsForCombo(int bizUnit)
       {
           //get dispersion list
           DataTable dsDispersionList = DataAccess.Production.CompoundPreparationDL.GetCompoundsForCombo(bizUnit);
           string jString = string.Empty;
           //convert to json string
           jString = GTIService.CommonFunctions.GetTextValueList(dsDispersionList, Fields.COMNAME, Fields.COMPOUNDPK);

           return jString;
       }

       /// <summary>
       /// Get list of plans to fill the dropdown
       /// <param name="grid"></param>
       /// <returns></returns>
       public static string GetPlanListCombo(int bizUnit)
       {
           //get dispersion list
           DataTable dsPlanList = DataAccess.Production.CompoundPreparationDL.GetPlansForCombo(bizUnit);
           string jString = string.Empty;
           //convert to json string
           jString = GTIService.CommonFunctions.GetTextValueList(dsPlanList, Fields.PLANNAME, Fields.PLANPK);

           return jString;
       }

       /// <summary>
       /// Get list OF Tanks to fill the dropdown
       /// <param name="grid"></param>
       /// <returns></returns>
       public static string GetTankForCombo(int bizUnit)
       {
           //get dispersion list
           DataTable dsPlanList = DataAccess.Production.CompoundPreparationDL.GetTankForCombo(bizUnit);
           string jString = string.Empty;
           //convert to json string
           jString = GTIService.CommonFunctions.GetTextValueList(dsPlanList, Fields.TANKNAME, Fields.TANKPK);

           return jString;
       }

       
       /// <summary>
       /// Get list OF Compounding Tanks to fill the dropdown
       /// <param name="grid"></param>
       /// <returns></returns>
       public static string GetTankForComboOnly(int bizUnit)
       {
           //get dispersion list
           DataTable dsPlanList = DataAccess.Production.CompoundPreparationDL.GetTankForComboOnly(bizUnit);
           string jString = string.Empty;
           //convert to json string
           jString = GTIService.CommonFunctions.GetTextValueList(dsPlanList, Fields.TANKNAME, Fields.TANKPK);

           return jString;
       }

       
       /// <summary>
       /// Get Compound Details As a XML Format and Convert in to JSON and Return As a JSON String Format
       /// </summary>
       /// <param name="DispersionID"></param>
       /// <returns></returns>
       public static string GetCompoundDetails(int compoundID,int deptID, int bizUnit)
       {
           try
           {

              // BusinessObject.DispersionManagement.Dispersion obj = new BusinessObject.DispersionManagement.Dispersion();
               //get compound detail in xml format convert to json
               return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.CompoundPreparationDL.GetCompoundDetails(compoundID, deptID, bizUnit));
           }
           catch (Exception ex)
           {
               NLog.Logger logger = NLog.LogManager.GetLogger("Compound Preparation");
               logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
               return null;
           }
       }
      

       /// <summary>
       /// Save Compound Details - Both Header And Compound Material Details
       /// </summary>
       /// <param name="purReqDetails"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
       public static string SaveCompoundTrxDetails(string compoundStr, User objUser)
       {
           string compoundId = string.Empty;
           List<object> retvals = new List<object>();
           //WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
           //objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(compoundStr);
           //objRequest.UserPK = objUser.PKUser;
           string xmlstr = GTIService.CommonFunctions.JsonToXml(compoundStr);
           retvals = DataAccess.Production.CompoundPreparationDL.SaveCompoundTrxDetails(xmlstr);
           compoundId = retvals[0].ToString();
           string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
           return jString;
         
       }

       /// <summary>
       /// Returns Compound list in json string format
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="VendorID"></param>
       /// <param name="BizUnit"></param>
       /// <returns></returns>
       public static string GetCompoundTrxList(GridPrams grid, User objuser, int procId)
       {
           DataSet dsCompoundList = DataAccess.Production.CompoundPreparationDL.GetCompoundTrxList(grid, objuser, procId);
           string jString = string.Empty;
           if (dsCompoundList.Tables.Count > 1 && dsCompoundList.Tables[1].Rows.Count > 0)
           {
               jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsCompoundList);
           }
           return jString;
       }

     /// <summary>
       ///  Returns Item list in json string format
     /// </summary>
     /// <param name="itemCatagory"></param>
     /// <param name="itemPk"></param>
     /// <returns></returns>
       public static string GetBatchesForItem(int itemCatagory, int itemPk, int batchPK, int compPK, int CompDtlPK)
       {
           DataTable dsCompoundList = DataAccess.Production.CompoundPreparationDL.GetBatchesForItem(itemCatagory, itemPk, batchPK, compPK, CompDtlPK);
           return Newtonsoft.Json.JsonConvert.SerializeObject(dsCompoundList);
       }

       /// <summary>
       ///  Returns Item list in json string format
       /// </summary>
       /// <param name="itemCatagory"></param>
       /// <param name="itemPk"></param>
       /// <returns></returns>
       public static string GetCategoryItemBatch(int itemCatagory, int itemPK)
       {
           DataTable dtBatchList = DataAccess.Production.CompoundPreparationDL.GetCategoryItemBatch(itemCatagory, itemPK);
           return GTIService.CommonFunctions.GetTextValueList(dtBatchList, "BATCH", "BATCH_PK");
       }

       /// <summary>
    /// Get stock for an item
    /// </summary>
    /// <param name="itemCatagory"></param>
    /// <param name="itemPk"></param>
    /// <param name="batchId"></param>
    /// <returns></returns>
       public static string GetStockValue(int itemCatagory, int itemPk, int batchId)
       {
           DataTable dsCompoundList = DataAccess.Production.CompoundPreparationDL.GetStockValue(itemCatagory, itemPk,batchId);

           string jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsCompoundList);

           return jString;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="compoundPk"></param>
       /// <returns></returns>
       public static string DeleteCompopundPreparation(int compoundPk)
       {
           return DataAccess.Production.CompoundPreparationDL.DeleteCompopundPreparation(compoundPk).ToString();
       }

       /// <summary>
       /// Get AuoComplete Search Details
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
       public static string GetSearchValues(string searchBy, string searchValue, User objUser, int procId)
       {
           DataTable dtSearch = DataAccess.Production.CompoundPreparationDL.GetSearchValues(searchBy, searchValue, objUser, procId);
           return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

       }

       /// <summary>
       /// Get Compound Transaction details  As a XML Format and Convert in to JSON and Return As a JSON String Format
       /// </summary>
       /// <param name="DispersionID"></param>
       /// <returns></returns>
       public static string GetCompoundPreparationDetails(int compoundID)
       {
           try
           {

               // BusinessObject.DispersionManagement.Dispersion obj = new BusinessObject.DispersionManagement.Dispersion();
               //get compound detail in xml format convert to json
               return GTIService.CommonFunctions.XmlToJson(DataAccess.Production.CompoundPreparationDL.GetCompoundPreparationDetails(compoundID));
           }
           catch (Exception ex)
           {
               NLog.Logger logger = NLog.LogManager.GetLogger("Compound Preparation");
               logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
               return null;
           }
       }


       /// <summary>
       /// Method to get tank details w. r. to tank pk
       /// <param name="grid"></param>
       /// <returns></returns>
       public static string GetSelectedTankDetails(int tnkPK, int tnkType, int bizUnit, int active)
       {
           DataTable dtSeltankDetails = DataAccess.Production.CompoundPreparationDL.GetSelectedTankDetails(tnkPK,tnkType,bizUnit,active);         
           //convert to json string
           string jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSeltankDetails);
           return jString;
       }

    }
}
