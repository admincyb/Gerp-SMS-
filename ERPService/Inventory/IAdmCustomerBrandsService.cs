using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using BusinessObject.Inventory;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdmCustomerBrandsService" in both code and config file together.
    [ServiceContract]
    public interface IAdmCustomerBrandsService
    {
        #region CustomerBrands Master Functions
        [OperationContract]
        List<CustomerBrands> GetCustomerBrands(CustomerBrands AdmCustomerBrandsMstObj, ServiceUtility utilityObj);
        [OperationContract]
        List<CRM_CUSTOMER_MST> GetCustomers();
        [OperationContract]
        List<INV_ITEM_MST> GetProducts(int itemPK);
        [OperationContract]
        int SaveCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList);
        [OperationContract]
        int DeleteCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList);
        #endregion
    }
}
