using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using BusinessObject.Inventory;

namespace ERPManager
{
    public interface IAdmCustomerBrandsManager
    {
        List<CRM_CUSTOMER_MST> GetCustomers();
        List<INV_ITEM_MST> GetProducts(int itemPK);
        List<CustomerBrands> GetCustomerBrands(CustomerBrands AdmCustomerBrandsMstObj, ServiceUtility utilityObj);
        int SaveCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList);
        int DeleteCustomerBrands(List<CRM_CUST_ITEM_MAP> admCustomerBrandsMstList);
    }
}
