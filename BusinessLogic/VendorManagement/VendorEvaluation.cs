using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using BusinessObject;
using BusinessObject.VendorManagement;
using DataAccess.VendorManagement;

namespace BusinessLogic.VendorManagement
{

     class VendorRegWorkflow
    {
        public string ProcessIDReg { get; set; }
        public string TaskIDReg { get; set; }
        public string ActionIDReg { get; set; }
        public string ApplicationIDReg { get; set; }
        public string ReferenceIDReg { get; set; }
        public string UserPk { get; set; }

    }

     public class VendorEvaluation
     {
         /// <summary>
         ///  Save Evaluation Details -  Details
         /// </summary>
         /// <param name="EvalDetails"></param>
         /// <param name="VendorID"></param>
         /// <param name="IsDraft"></param>
         /// <returns></returns>
         public static string SaveEvaluationDetails(string evalDetails, int vendorID)
         {
             string vendorevalID;
             string xmlstr = GTIService.CommonFunctions.JsonToXml(evalDetails);
             vendorevalID = VendorEvaluationDL.SaveEvaluationDetails(xmlstr).ToString();
             return vendorevalID;
         }

         /// <summary>
         /// Get  Evaluation Details As a XML Format and Convert in to JSON and Return As a JSON String Format
         /// </summary>
         /// <param name="EvalID"></param>
         /// <returns></returns>
         public static string GetEvaluationDetail(int evalID)
         {
             try
             {

                 BusinessObject.DispersionManagement.Dispersion obj = new BusinessObject.DispersionManagement.Dispersion();
                 //get  General Template detail in xml format convert to json
                 return GTIService.CommonFunctions.XmlToJson(VendorEvaluationDL.GetEvaluationDetails(evalID));
             }
             catch (Exception ex)
             {
                 NLog.Logger logger = NLog.LogManager.GetLogger("Template  Creation");
                 logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                 return null;
             }
         }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="pk"></param>
         /// <param name="bizUnit"></param>
         /// <returns></returns>
         public static DataSet GetPerformanceReport(int pk, int bizUnit)
         {
             return VendorEvaluationDL.GetPerformanceReport(pk, bizUnit);
         }

         /// <summary>
         /// Get Evaluation Details As XML For reporting
         /// </summary>
         /// <param name="EvalID"></param>
         /// <returns></returns>
         public static string GetEvaluationReport(int evalID)
         {
             try
             {
                 //get  General Template detail in xml format 
                 //return VendorEvaluationDL.GetEvaluationReport(evalID);
                 string xml = string.Empty;
                 DataTable dtPrcCtrlList = VendorEvaluationDL.GetEvaluationReport(evalID);
                 if (dtPrcCtrlList != null)
                 {
                     if (dtPrcCtrlList.Rows.Count > 0)
                     {
                         foreach (DataRow drData in dtPrcCtrlList.Rows)
                         {
                             xml += Convert.ToString(drData[0]);
                         }
                     }
                 }
                 return xml;
             }
             catch (Exception ex)
             {
                 NLog.Logger logger = NLog.LogManager.GetLogger("Template  Creation");
                 logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                 return null;
             }
         }


         /// <summary>
         ///  Returns the search result list for Autocomplete 
         /// </summary>
         /// <param name="searchBy"></param>
         /// <param name="searchValue"></param>
         /// <param name="BizUnit"></param>
         /// <returns></returns>
         public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
         {
             DataTable dtSearch = VendorEvaluationDL.GetSearchValues(searchBy, searchValue, bizUnit);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHVAL, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHID);
             return jString;
         }

         /// <summary>
         /// Returns Evaluation list in json string format
         /// </summary>
         /// <param name="grid"></param>
         /// <param name="VendorID"></param>
         /// <param name="BizUnit"></param>
         /// <returns></returns>
         public static string GetEvaluationList(GridPrams grid, int vendorID, int bizUnit, int procID, string PageUrl)
         {
             DataSet dsEvaluationList = VendorEvaluationDL.GetEvaluationList(grid, vendorID, bizUnit, procID, PageUrl);
             string jString = string.Empty;
             if (dsEvaluationList.Tables.Count > 1 && dsEvaluationList.Tables[1].Rows.Count > 0)
             {
                 jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsEvaluationList);
             }
             return jString;
         }
         /// <summary>
         /// Function Used To Get all Parameters To Bind Combo
         /// </summary>
         /// <param name="BizUnitPk"></param>
         /// <param name="TemplatePK"></param>
         /// <returns></returns>
         public static string GetParametersList(int bizUnitPk, int templatePK)
         {
             DataTable dtParameters = VendorEvaluationDL.GetParametersList(bizUnitPk, templatePK);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtParameters, GTIService.Constants.Vendor.Fields.PARAMETERNAME, GTIService.Constants.Vendor.Fields.PARAMETERPK);
             return jString;
         }

         //NewEval Start
         /// <summary>
         /// Function Used To Get all Parameters To Bind Combo
         /// </summary>
         /// <param name="BizUnitPk"></param>
         /// <param name="TemplatePK"></param>
         /// <returns></returns>
         public static string GetEvalGroupList(int bizUnitPk)
         {
             DataTable dtGroups = VendorEvaluationDL.GetEvalGroupList(bizUnitPk);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtGroups, GTIService.Constants.Vendor.Fields.GROUPNAME, GTIService.Constants.Vendor.Fields.GROUPPK);
             return jString;
         }
         //New End
         /// <summary>
         /// Function Used To Get all Parameters To Bind Combo
         /// </summary>
         /// <param name="BizUnitPk"></param>
         /// <param name="TemplatePK"></param>
         /// <returns></returns>
         public static string GetParameterValue(int tmdPK)
         {
             DataTable dtGroups = VendorEvaluationDL.GetParameterValue(tmdPK);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtGroups, GTIService.Constants.Vendor.Fields.TMDMAXPOINT, GTIService.Constants.Vendor.Fields.TMDPK);
             return jString;
         }
         

         /// <summary>
         /// Get Evaluations
         /// </summary>
         /// <param name="parameterID"></param>
         /// <returns>string</returns>
         public static string GetEvaluations(string parameterID)
         {
             DataTable dtEvaluations = VendorEvaluationDL.GetEvaluation(parameterID);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtEvaluations, GTIService.Constants.Vendor.Fields.EVALUATIONNAME, GTIService.Constants.Vendor.Fields.EVALUATIONPK);
             return jString;

         }
         /// <summary>
         /// Get Supplied Material list
         /// </summary>
         /// <param name="vendorID"></param>
         /// <returns>string</returns>
         public static string GetSuppliedMaterial(string vendorID)
         {
             DataTable dtSuppliedMaterial = VendorEvaluationDL.GetSuppliedMaterial(vendorID);
             string jString = GTIService.CommonFunctions.GetTextValueList(dtSuppliedMaterial, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALNAME, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALPK);
             return jString;

         }
         /// <summary>
         /// Delete Evaluations Details
         /// </summary>
         /// <param name="evalutionID"></param>
         /// <returns>String</returns>
         public static string DeleteEvaluations(int evalutionID)
         {
             return VendorEvaluationDL.DeleteEvaluationDtls(evalutionID).ToString();
         }

         /// <summary>
         /// Get VendorID for RefID
         /// </summary>
         /// <param name="evalutionID"></param>
         /// <returns></returns>
         public static int GetAppIDForRefID(int processID, int refid)
         {
             return VendorEvaluationDL.GetAppIDForRefID(processID, refid);
         }

         //====================== 30-09-2011 ===========================================
         /// <summary>
         /// Get Evaluation Application Id for RefID
         /// </summary>
         /// <param name="refid"></param>
         /// <returns></returns>
         public static int GetEvalAppIDForRefID(int refid)
         {
             return VendorEvaluationDL.GetEvalAppIDForRefID(refid);
         }
         /// <summary>
         /// Update Ref ID for Evaluation Table
         /// </summary>
         /// <param name="evalPk"></param>
         /// <param name="refid"></param>
         /// <returns></returns>
         public static int UpdateEvaluationRefID(string workFlowDetails, User objUser)
         {
             //return VendorEvaluationDL.UpdateEvaluationDtls(evalPk, refid, user);
             Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
             WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(workFlowDetails, settings);
             return VendorEvaluationDL.UpdateEvaluationDtls(wrkfReq, objUser);
         }
     }
}
