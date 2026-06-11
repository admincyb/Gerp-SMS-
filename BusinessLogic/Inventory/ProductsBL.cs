using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using DataAccess;
using BusinessObject.Inventory;
using GTIService;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Inventory
{
    public class ProductsBL
    {

        /// <summary>
        /// Methode used for get the property and grade
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="productPK"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPropertyGradeXML(int groupType, int productPK)
        {
            XmlDocument xmlDocument;
            DataSet dsXml = new DataSet();
            DataTable dtXml = new DataTable();
            xmlDocument = new XmlDocument();
            dtXml = DataAccess.Inventory.ProductsDA.GetPropertyGradeXML(groupType, productPK);
            if (dtXml != null && dtXml.Rows.Count > 0)
            {
                string strXml = string.Empty;
                foreach (DataRow drPlan in dtXml.Rows)//addding each row to string
                {
                    strXml = strXml + Convert.ToString(drPlan[0]);
                }
                xmlDocument.LoadXml(strXml);//reading string to xml
                dsXml.ReadXml(new XmlNodeReader(xmlDocument));
            }

            return dsXml;
        }
        /// <summary>
        /// Get Item Category
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetItemCategory(string itemCode, int active, int value = 0)
        {
            return DataAccess.Inventory.ProductsDA.GetItemCategory(itemCode, active, value);
        }


        /// <summary>
        /// Save Product Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveProductGroup(string pXML)
        {
            return DataAccess.Inventory.ProductsDA.SaveProductGroup(pXML);
        }
        /// <summary>
        /// Save Store Mapping
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveStoreMap(string pXML)
        {
            return DataAccess.Inventory.ProductsDA.SaveStoreMap(pXML);
        }

        /// <summary>
        /// To Get Product Categories
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="categoryPK" value="0 for all"></param>
        /// <returns>Category DataTable</returns>
        public static DataTable GetCategory(int bizUnit, int categoryPK)
        {
            return DataAccess.Inventory.ProductsDA.GetCategory(bizUnit, categoryPK);
        }
        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanGroupPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetProductPlanGroups(int PlanGroupPK, int Active, int Bizunit)
        {
            return DataAccess.Inventory.ProductsDA.GetProductPlanGroups(PlanGroupPK, Active, Bizunit);
        }

        #region Planning Groups Master
        /// <summary>
        /// Save Planning Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SavePlanningGroup(string pXML)
        {
            return DataAccess.Inventory.ProductsDA.SavePlanningGroup(pXML);
        }

        /// <summary>
        /// To Fill Product Items Tree view
        /// </summary>
        /// <param name="PlanGroupPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetProductItems(int CurrPK)
        {
            return DataAccess.Inventory.ProductsDA.GetProductItems(CurrPK);
        }

        /// <summary>
        /// To Fill Product Items Tree view
        /// </summary>
        /// <param name="PlanGroupPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetPlanningItemGroupList(string pageNum, int pageSize, int CurrPK, string PlanningGroupCode, string PlanningGroupName)
        {
            return DataAccess.Inventory.ProductsDA.GetPlanningItemGroupList(pageNum, pageSize, CurrPK, PlanningGroupCode, PlanningGroupName);
        }
        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="PlanGroupPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataSet GetPlanningItemEdit(int CurrPK)
        {
            return DataAccess.Inventory.ProductsDA.GetPlanningItemEdit(CurrPK);
        }


        /// <summary>
        /// Delete Planning Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int DeleteInvItemPlanningMst(int CurrPK)
        {
            return DataAccess.Inventory.ProductsDA.DeleteInvItemPlanningMst(CurrPK);
        }
        /// <summary>
        /// To Fill Product Items Autocomplete
        /// </summary>
        /// <param name="PlanGroupPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetPlanningGroupAutocomplete(string searchKey, string SearchBy)
        {
            return DataAccess.Inventory.ProductsDA.GetPlanningGroupAutocomplete(searchKey, SearchBy);
        }

        #endregion

        /// <summary>
        /// Methode used for get the mapped lines against product
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="productPK"></param>
        /// <returns>DataSet</returns>
        public static ProductLineBO GetProductLines(int productPK)
        {
            XmlDocument xmlDocument;
            DataSet dsXml = new DataSet();
            xmlDocument = new XmlDocument();
            ProductLineBO planLineObj = new ProductLineBO();
            string strXml = DataAccess.Inventory.ProductsDA.GetProductLines(productPK);
            if (strXml != string.Empty)
                planLineObj = (ProductLineBO)CommonFunctions.DeserializeObject(strXml, planLineObj);
            return planLineObj;
        }

        public static int SaveLineItemMap(string pXml)
        {
            return DataAccess.Inventory.ProductsDA.SaveLineItemMap(pXml);
        }

        /// <summary>
        /// To get uploaded file for the group of product
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static DataTable GetProductGroupDoc(int Product, int Task)
        {
            return DataAccess.Inventory.ProductsDA.GetProductGroupDoc(Product, Task);
        }
         /// <summary>
        /// To get GST class list
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static DataTable GetGSTclassificationList(int PK, int active, int bizunit)
        {
            return DataAccess.Inventory.ProductsDA.GetGSTclassificationList(PK, active, bizunit);
        }

        /// <summary>
        /// To Save Brand Product
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static int BrandProductSave(int ItemPK,int UserPk)
        {
            return DataAccess.Inventory.ProductsDA.BrandProductSave(ItemPK, UserPk);
        }
    }
}
