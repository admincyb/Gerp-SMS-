using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Common;
using System.Configuration;

namespace BusinessLogic.Administration.Configurations
{
    public class MenuManagement
    {
        /// <summary>
        /// Function Used To Save  Menu Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveMenuDetails(string requestData)
        {
            BusinessObject.Administration.Configurations.MenuManagement menuManagement = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Configurations.MenuManagement>(requestData);
            
            return DataAccess.Administration.Configurations.MenuManagement.SaveMenuDetails(menuManagement);
        }

        /// <summary>
        /// Function Used To Get All Child Menu details by Menu id
        /// </summary>
        /// <param name="materialCategoryParentPK"></param>
        /// <returns></returns>
        public static string GetMenuListDtls(int menuParentID)
        {
            DataTable dtCategory = DataAccess.Administration.Configurations.MenuManagement.GetMenuListDtls(menuParentID);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Configurations.Menu.Fields.MENUID, GTIService.Constants.Configurations.Menu.Fields.MENUNAME, GTIService.Constants.Configurations.Menu.Fields.MENUPARENT, GTIService.Constants.Configurations.Menu.Fields.HASCHILD, string.Empty, string.Empty, string.Empty);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To Get Details Menu details by Menu id
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static string GetMenuDetails(int menuID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(DataAccess.Administration.Configurations.MenuManagement.GetMenuDetails(menuID));
        }

        /// <summary>
        /// Method Used to Delete  Menu details by Menu id
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public static string DeleteMenuDetails(int menuID)
        {
            return DataAccess.Administration.Configurations.MenuManagement.DeleteMenuDetails(menuID).ToString();
        }
        /// <summary>
        /// Save Menu Group Details
        /// </summary>
        /// <param name="menuGroupDetails"></param>
        /// <returns></returns>
        public static string SaveMenuGroupDetails(string menuGroupDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(menuGroupDetails);
            return DataAccess.Administration.Configurations.MenuManagement.SaveMenuGroupDetails(xmlstr);
        }
       /// <summary>
       /// get menu Group Details
       /// </summary>
       /// <param name="deptPK"></param>
       /// <param name="deptParentPK"></param>
       /// <param name="bizUnit"></param>
       /// <param name="userGroup"></param>
       /// <returns></returns>
        public static string GetMenuGrpDtls(int menuPK, int menuParentPK, int bizUnit, int userGroup)
        {
            DataTable dtDept = DataAccess.Administration.Configurations.MenuManagement.GetMenuUsergrpDtls( menuPK,  menuParentPK, bizUnit, userGroup);
            string jString = string.Empty;
            if (dtDept.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtDept, "MNU_ID","MNU_Name", "MNU_PARENT", GTIService.Constants.Common.Fields.HASCHILD, GTIService.Constants.Common.Fields.HASUSERGROUP, string.Empty, string.Empty);
            }
            return jString;
        }


        /// <summary>
        /// Returns 
        /// </summary>
        /// <returns></returns>
        public static string GetPageList()
        {
            //var s = DataAccess.Administration.Configurations.MenuManagement.GetPageList();
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        public static List<BusinessObject.MenuBO> GetMenuDetails(BusinessObject.User authUser,int menuMode)
        {
            Int16 module;
            module = 0;
            Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
            DataSet dsMenu = DataAccess.Administration.Configurations.MenuManagement.GetMenuDetails(authUser.SBUID, authUser.CurrentDeptPK, authUser.PKUser, module);
            DataTable dtSection = dsMenu.Tables[0];
            DataTable dtIcons = dsMenu.Tables[1];
            DataTable dtGroup = dsMenu.Tables[2];
            DataTable dtLinks = dsMenu.Tables[3];
            List<BusinessObject.MenuBO> Menudtls = new List<BusinessObject.MenuBO>();
            BusinessObject.MenuBO Section;
            BusinessObject.RightGroupNames groups;
            BusinessObject.RightLinks RightLinks;
            BusinessObject.IconList IconList;
            if (dtSection.Rows.Count > 0)
            {
                foreach (DataRow drSection in dtSection.Rows)
                {
                    Section = new BusinessObject.MenuBO();
                    Section.RightGroupNames = new List<BusinessObject.RightGroupNames>();
                    Section.IconList = new List<BusinessObject.IconList>();
                    switch (menuMode)
                    {
                        case 1:
                            Section.SectionHead = drSection["MNS_NAME"].ToString();
                            break;
                        case 2:
                            Section.SectionHead = string.IsNullOrEmpty(drSection["MNS_NAME2"].ToString()) ? drSection["MNS_NAME"].ToString() : drSection["MNS_NAME2"].ToString();
                            break;
                        case 0:
                            Section.SectionHead = drSection["MNS_NAME"].ToString() + (string.IsNullOrEmpty(drSection["MNS_NAME2"].ToString()) ? "" : "/" + drSection["MNS_NAME2"].ToString());
                            break;
                        default:
                            Section.SectionHead = drSection["MNS_NAME"].ToString();
                            break;
                    }
                    Section.SectionID = Convert.ToInt32(drSection["MNS_PK"].ToString());
                    Section.ImageUrl = drSection["MNS_IMAGE"].ToString();
                    DataRow[] drArrGroup = dtGroup.Select("MNG_SECTION" + " = " + Section.SectionID);
                    foreach (DataRow drgroup in drArrGroup)
                    {
                        groups = new BusinessObject.RightGroupNames();
                        groups.RightLinks = new List<BusinessObject.RightLinks>();
                        groups.GroupID = Convert.ToInt32(drgroup["MNG_PK"].ToString());
                        switch (menuMode)
                        {
                            case 1:
                                groups.GroupName = drgroup["MNG_NAME"].ToString();
                                break;
                            case 2:
                                groups.GroupName = string.IsNullOrEmpty(drgroup["MNG_NAME2"].ToString()) ? drgroup["MNG_NAME"].ToString() : drgroup["MNG_NAME2"].ToString();
                                break;
                            case 0:
                                groups.GroupName = drgroup["MNG_NAME"].ToString() + (string.IsNullOrEmpty(drgroup["MNG_NAME2"].ToString()) ? "" : "/" + drgroup["MNG_NAME2"].ToString());
                                break;
                            default:
                                groups.GroupName = drgroup["MNG_NAME"].ToString();
                                break;
                        }
                        groups.SectionID = Section.SectionID;
                        //DataRow[] drArrLinks = dtLinks.Select("MNG_PK" + " = " + groups.GroupID, "MNU_NAME ASC");
                        DataRow[] drArrLinks = dtLinks.Select("MNG_PK" + " = " + groups.GroupID);
                        foreach (DataRow drLinks in drArrLinks)
                        {
                            RightLinks = new BusinessObject.RightLinks();
                            RightLinks.GroupID = groups.GroupID;
                            switch (menuMode)
                            {
                                case 1:
                                    RightLinks.LinkText = drLinks["MNU_NAME"].ToString();
                                    break;
                                case 2:
                                    RightLinks.LinkText = string.IsNullOrEmpty(drLinks["MNU_NAME2"].ToString()) ? drLinks["MNU_NAME"].ToString() : drLinks["MNU_NAME2"].ToString();
                                    break;
                                case 0:
                                    RightLinks.LinkText = drLinks["MNU_NAME"].ToString() + (string.IsNullOrEmpty(drLinks["MNU_NAME2"].ToString()) ? "" : "/" + drLinks["MNU_NAME2"].ToString());
                                    break;
                                default:
                                    RightLinks.LinkText = drLinks["MNU_NAME"].ToString();
                                    break;
                            }
                            RightLinks.LinkUrl = drLinks["MNU_ACTION_URL"].ToString();
                            RightLinks.Description = drLinks["MNU_DESC"].ToString();
                            RightLinks.PostUrl = drLinks["MNU_LINK"].ToString();
                            groups.RightLinks.Add(RightLinks);
                        }
                        Section.RightGroupNames.Add(groups);

                    }
                    DataRow[] drArrIcons = dtIcons.Select("MNS_PK" + " = " + Section.SectionID);
                    foreach (DataRow drIcon in drArrIcons)
                    {
                        IconList = new BusinessObject.IconList();
                        IconList.SectionID = Section.SectionID;
                        IconList.IconImage = drIcon["MNU_IMAGE_URL"].ToString();
                        IconList.IconLink = drIcon["MNU_PAGE_URL"].ToString();
                        Section.IconList.Add(IconList);
                    }
                    Menudtls.Add(Section);
                }
            }
            return Menudtls;
        }

        /// <summary>
        /// To get search results to fill autocomplete textbox
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="SBU"></param>
        /// <param name="department"></param>
        /// <param name="userPk"></param>
        /// <returns></returns>
        public static string GetSearchMenuAuto(string searchKey, int SBU, int department, int userPk)
        {
            DataTable dtSearch;
            short module;
            Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
            dtSearch = DataAccess.Administration.Configurations.MenuManagement.GetSearchMenuAuto(searchKey, SBU, department, userPk, module);
            string virtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];
            if(virtualDirectory.Trim()!=string.Empty)
                return GTIService.CommonFunctions.GetTextValueList(dtSearch,"MNU_LINK", "MNU_PAGE_URL", "MNU_NAME");
            else
                return GTIService.CommonFunctions.GetTextValueList(dtSearch, "MNU_PAGE_URL", "MNU_NAME");
        }

        /// <summary>
        /// To Get Section list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetSectionList(int SectionPK, int Active, int bizUnit)
        {
            return DataAccess.Administration.Configurations.MenuManagement.GetSectionList(SectionPK, Active, bizUnit);
        }

        /// <summary>
        /// To Save/Update Section Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateSectionDetails(int SectionPK, string LastModDate, string SectionName = null, string SectionNameForeign = null, string ActiveStatus = null)
        {
            return DataAccess.Administration.Configurations.MenuManagement.UpdateSectionDetails(SectionPK,LastModDate, SectionName,SectionNameForeign, ActiveStatus);
        }

         /// <summary>
        /// To Get Menu Group list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMenuGroupList(int MenuPK, int Active, int SectionPK=0)
        {
            return DataAccess.Administration.Configurations.MenuManagement.GetMenuGroupList(MenuPK, Active, SectionPK);
        }
        /// <summary>
        /// To Save/Update Menu Group Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateMenuGroupDetails(int MenuGroupPK, string LastModDate, string MenuGroupName = null, string MenuGroupnameForeign=null, string ActiveStatus = null)
        {
            return DataAccess.Administration.Configurations.MenuManagement.UpdateMenuGroupDetails(MenuGroupPK, LastModDate, MenuGroupName, MenuGroupnameForeign, ActiveStatus);
        }

        /// <summary>
        /// To Get Menu Group list for Menu Edit
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="Active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMenuList(int MenuPK, int Active, int MenuGroupPK = 0)
        {
            return DataAccess.Administration.Configurations.MenuManagement.GetMenuList(MenuPK, Active, MenuGroupPK);
        }
        /// <summary>
        /// To Save/Update Menu Details
        /// </summary>
        /// <param name="SectionPK"></param>
        /// <param name="SectionName"></param>
        /// <param name="ActiveStatus"></param>
        /// <returns></returns>
        public static int UpdateMenuDetails(int MenuPK, string LastModDate, string MenuName = null, string MenuNameForeign = null, string ActiveStatus = null)
        {
            return DataAccess.Administration.Configurations.MenuManagement.UpdateMenuDetails(MenuPK, LastModDate, MenuName, MenuNameForeign, ActiveStatus);
        }

    }
}
