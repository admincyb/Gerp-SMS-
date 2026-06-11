using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using BusinessObject;
using DataAccess.Administration.Masters;
using GTIService.Constants.Administration.Masters;
using GTIService.Constants.Administration.Masters.GeneralTemplate;
using System.Web;

namespace BusinessLogic.Administration.Masters
{
    public  class GeneralTemplateMasterBL
    {

        /// <summary>
        /// Save General Template Details - as XML
        /// </summary>
        /// <param name="DispersionMaster"></param>
        /// <returns>TemplatePk 0-if duplicate entry</returns>
        public static string SaveTemplateDetail(string generalTemplate)
        {
            //convert json data to xml string
            string xmlstr = GTIService.CommonFunctions.JsonToXml(generalTemplate);
            return GeneralTemplateMasterDL.SaveTemplateDetails(xmlstr).ToString();
        }


        /// <summary>
        /// Save Template Group  Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="userPk"></param>
        /// <returns>string</returns>
        public static string SaveTemplateGroup(string requestData, int userPk)
        {
            BusinessObject.Administration.Masters.TemplateGroupMaster TemplGrp = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.TemplateGroupMaster>(requestData);
            TemplGrp.UserPK = userPk;
            return (DataAccess.Administration.Masters.GeneralTemplateMasterDL.SaveTemplateGroup(TemplGrp)).ToString();
        }

        /// <summary>
        /// Get list of Template Group to fill the dropdown
        /// <param name="BizUnitPk"></param>
        /// <returns></returns>
        public static string GetTemplateGroupListCombo(int bizUnitPk)
        {

            DataTable dsDispersionList = GeneralTemplateMasterDL.GetTemplateGroupCombo(bizUnitPk);
            string jString = string.Empty;
            //convert to json string
            jString = GTIService.CommonFunctions.GetTextValueList(dsDispersionList,Fields.TEMPLATEGRPNAME , Fields.TEMPLATEGRPPK);

            return jString;
        }
        /// <summary>
        /// Get Template Group - For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="BizUnitPk"></param>
        /// <returns></returns>
        public static string GetTemplateGroupDtls(GridPrams grid, int bizUnitPk)
        {
            DataSet dsTemplateGroupList = GeneralTemplateMasterDL.GetTemplateGroupDtls(grid, bizUnitPk);
            string jString = string.Empty;
            if (dsTemplateGroupList.Tables.Count > 1 && dsTemplateGroupList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTemplateGroupList);
            }
            return jString;
        }


       /// <summary>
        ///  Delete Template Group 
       /// </summary>
       /// <param name="TemplateGrpID"></param>
       /// <returns></returns>
        public static string DeleteTemplateGroupDtls(int templateGrpID)
        {
            return GeneralTemplateMasterDL.DeleteTemplateGroupDtls(templateGrpID).ToString();
        }

        /// <summary>
        /// Get list of  General Template to fill the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="BizUnitPk"></param>
        /// <returns></returns>
        public static string GetGeneralTemplateList(GridPrams grid,int bizUnitPk)
        {
            //get dispersion list
            DataSet dsGeneralTmplt = GeneralTemplateMasterDL.GetGeneralTemplateList(grid,bizUnitPk);
            string jString = string.Empty;
            if (dsGeneralTmplt.Tables.Count > 1 && dsGeneralTmplt.Tables[1].Rows.Count > 0)
            {
                //convert o tjson string
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsGeneralTmplt);
            }
            return jString;
        }

          /// <summary>
            /// Delete  General Template Details
          /// </summary>
          /// <param name="TemplateID"></param>
          /// <returns></returns>
        public static string DeleteGeneralTemplateDtls(int templateID)
        {
            return GeneralTemplateMasterDL.DeleteGeneralTemplateDtls(templateID).ToString();
        }


        /// <summary>
        /// Get  General Template Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public static string GetGeneralTemplateDetails(int templateID)
        {
            try
            {

                //get  General Template detail in xml format convert to json
                return GTIService.CommonFunctions.XmlToJson(GeneralTemplateMasterDL.GetGeneralTemplateDetails(templateID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnitPk)
        {
            DataTable dtSearch = GeneralTemplateMasterDL.GetSearchValues(searchBy, searchValue, bizUnitPk);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, "VALUE", "PK");
            return jString;
        }

        /// <summary>
        /// Method to get the General Template list For Filling Drop Down
        /// <createdBy>Vineeth Babu</createdBy>
        /// <for>Po Creation</for>
        /// <usedin>Po listing General templates</usedin>
        /// </summary>
        /// <param name="context"></param>
        public static string GetGeneralTemplate(int bizUnitPk,int termsPK)
        {
            string jString = string.Empty;
            if (termsPK == 0)
                jString = GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.GeneralTemplateMasterDL.GetGeneralTemplates(bizUnitPk, termsPK), GTIService.Constants.Administration.Masters.GeneralTemplate.Fields.TEMPLATENAME, GTIService.Constants.Administration.Masters.GeneralTemplate.Fields.TMDPK);
            else
            {
                DataTable dtTermsDetails = DataAccess.Administration.Masters.GeneralTemplateMasterDL.GetGeneralTemplates(bizUnitPk, termsPK);
                if (dtTermsDetails.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTermsDetails);
                }
            }

            return jString; 
        }
        public static string GetPOTemplate(int bizUnitPk, int termsPK)
        {
            string jString = string.Empty;
            if (termsPK == 0)
                jString = GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.GeneralTemplateMasterDL.GetPOTemplate(bizUnitPk, termsPK), GTIService.Constants.Administration.Masters.GeneralTemplate.Fields.TEMPLATENAME, GTIService.Constants.Administration.Masters.GeneralTemplate.Fields.TMDPK);
            else
            {
                DataTable dtTermsDetails = DataAccess.Administration.Masters.GeneralTemplateMasterDL.GetPOTemplate(bizUnitPk, termsPK);
                if (dtTermsDetails.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTermsDetails);
                }
            }

            return jString;
        }

        public static DataTable GetGeneralTemplatesByBizUnitAndTerms(int bizUnitPk, int termsPK, string terms = null)
        {
            return DataAccess.Administration.Masters.GeneralTemplateMasterDL.GetGeneralTemplates(bizUnitPk, termsPK, terms);
        }
    }
}

