using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
   public  class ProjectSiteBL
    {
        /// <summary>
        /// Save Project Site - Details  
        /// </summary>
        /// <param name="MaterialIssueDetails"></param>
        /// <returns>string</returns>
       public static string SaveProjectSite(string requestData)
        {
            BusinessObject.Administration.Configurations.ProjectSiteBO projectSite = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Configurations.ProjectSiteBO>(requestData);
            return DataAccess.Administration.Configurations.ProjectSiteDL.SaveProjectSite(projectSite);
        }

        /// <summary>
        /// Delete ProjectSite Details
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>String</returns>
        public static string DeleteProjectSite(int ProjectPK)
        {
            return DataAccess.Administration.Configurations.ProjectSiteDL.DeleteProjectSite(ProjectPK);
        }

        /// <summary>
        /// Get ProjectSite Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="RequisitionID"></param>
        /// <returns></returns>
        public static string GetProjectSiteList(GridPrams grid, int bizUnit, User objUser)
        {
            DataSet dsSiteList = DataAccess.Administration.Configurations.ProjectSiteDL.GetProjectSiteList(grid, bizUnit, objUser);
            string jString = string.Empty;
            if (dsSiteList.Tables.Count > 1 && dsSiteList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSiteList);
            }
            return jString;
        }


        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string ProjectSiteGetSearchValue(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.Administration.Configurations.ProjectSiteDL.ProjectSiteGetSearchValue(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Configurations.ProjectSite.Fields.TEXT, GTIService.Constants.Configurations.ProjectSite.Fields.VALUE);

        }
    }
}
