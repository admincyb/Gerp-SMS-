using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Configurations;
using System.Data;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class RoleActionBL
    {
        /// <summary>
        /// method for  Get Role Actions Details
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataSet GetRoleActions(int roleActionPK, DbActiveStatus status, int sbu)
        {
            return RoleActionDA.GetRoleActions(roleActionPK, status, sbu);
        }
        /// <summary>
        /// Methode used for get role actions
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static XmlDocument GetRoleActionsXml(int roleActionPK, DbActiveStatus status, int sbu)
        {
            XmlDocument xmlDocument;
            xmlDocument = new XmlDocument();
            string xmlRoleActions;
            xmlRoleActions = RoleActionDA.GetRoleActionsXml(roleActionPK, status, sbu);
            xmlDocument.LoadXml(xmlRoleActions);
            return xmlDocument;
        }
        /// <summary>
        /// method for saving roleaction details.
        /// </summary>
        /// <param name="objRoleAction"></param>
        /// <param name="xmlCostCenter"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int SaveRoleActions(RoleActionBO objRoleAction, string xmlCostCenter, int user)
        {
            return RoleActionDA.SaveRoleActions(objRoleAction, xmlCostCenter, user);
        }
    }
}
