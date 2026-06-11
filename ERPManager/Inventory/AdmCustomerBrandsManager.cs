using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;
using BusinessObject.Inventory;

namespace ERPManager.Inventory
{
    public class AdmCustomerBrandsManager : IAdmCustomerBrandsManager
    {
        #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion

        #region Manager Methods
        /// <summary>
        /// Bank AdmProductProperties Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public AdmCustomerBrandsManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<CustomerBrands> GetCustomerBrands(CustomerBrands AdmCustomerBrandsMstObj, ServiceUtility utilityObj)
        {
            List<CustomerBrands> Obj_CustomerBrands_List;
            IQueryable<CustomerBrands> Obj_CustomerBrands_Qry;
            try
            {
                Obj_CustomerBrands_Qry = (from pac in this.currentEntity.CRM_CUST_ITEM_MAP
                                          where (pac.CIM_CUSTOMER == (AdmCustomerBrandsMstObj.CIM_CUSTOMER > 0 ? AdmCustomerBrandsMstObj.CIM_CUSTOMER : pac.CIM_CUSTOMER)
                                               && pac.CIM_ITEM == (AdmCustomerBrandsMstObj.CIM_ITEM > 0 ? AdmCustomerBrandsMstObj.CIM_ITEM : pac.CIM_ITEM))
                                               && pac.CIM_PK == (AdmCustomerBrandsMstObj.CIM_PK > 0 ? AdmCustomerBrandsMstObj.CIM_PK : pac.CIM_PK)
                                          select new CustomerBrands
                                          {
                                              CIM_PK=pac.CIM_PK,
                                              CIM_CUSTOMER=pac.CIM_CUSTOMER ,
                                              CIM_BRAND_CODE =pac.CIM_BRAND_CODE ,
                                              CIM_BRAND_NAME =pac.CIM_BRAND_NAME ,
                                              CIM_ITEM =pac.CIM_ITEM ,
                                              ITM_CODE = pac.INV_ITEM_MST.ITM_CODE,
                                              ITM_NAME =pac.INV_ITEM_MST.ITM_NAME ,
                                              CUS_CODE = pac.CRM_CUSTOMER_MST.CUS_CODE,
                                              CUS_NAME = pac.CRM_CUSTOMER_MST.CUS_NAME,
                                              CIM_ACTIVE =pac.CIM_ACTIVE ,
                                              CIM_STATUS =pac.CIM_STATUS ,
                                              CIM_MOD_BY = pac.CIM_MOD_BY,
                                              CIM_MOD_DT = pac.CIM_MOD_DT
                                          });


                //Set page size one if not given
                utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                utilityObj.TotalRecords = Obj_CustomerBrands_Qry.Count();

                //Filter Query
                //paymentHdrQuery = FilterEntity(finPaymentVndHdrListObj, paymentHdrQuery, serviceUtilityObj);

                //Apply Paging And Sorting for grid Purpose
                Obj_CustomerBrands_List = Obj_CustomerBrands_Qry.SortRecords<CustomerBrands>(utilityObj).ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_CustomerBrands_List;

        }

        public List<CRM_CUSTOMER_MST> GetCustomers()
        {
            List<CRM_CUSTOMER_MST> Obj_CRM_CUSTOMER_MST_List;
            IQueryable<CRM_CUSTOMER_MST> Obj_CRM_CUSTOMER_MST_Qry;
            try
            {
                Obj_CRM_CUSTOMER_MST_Qry = (from pac in this.currentEntity.CRM_CUSTOMER_MST
                                         where  pac.CUS_ACTIVE == (byte)1
                                         select pac);

                Obj_CRM_CUSTOMER_MST_List = Obj_CRM_CUSTOMER_MST_Qry.ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_CRM_CUSTOMER_MST_List;
        }

        public List<INV_ITEM_MST> GetProducts(int itemPK)
        {
            List<INV_ITEM_MST> Obj_INV_ITEM_MST_List;
            IQueryable<INV_ITEM_MST> Obj_INV_ITEM_MST_Qry;
            try
            {
                Obj_INV_ITEM_MST_Qry = (from pac in this.currentEntity.INV_ITEM_MST
                                        where pac.ITM_ACTIVE == (byte)1
                                        && pac.ITM_PK == (itemPK > 0 ? itemPK : pac.ITM_PK)
                                        select pac);

                Obj_INV_ITEM_MST_List = Obj_INV_ITEM_MST_Qry.ToList();
            }
            catch (Exception ex)
            {

                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            return Obj_INV_ITEM_MST_List;
        }

        public int SaveCustomerBrands(List<CRM_CUST_ITEM_MAP> AdmCustomerBrandsMstObj)
        {
            int retval = 0;
            int? maxPK;

            CRM_CUST_ITEM_MAP Old_CRM_CUST_ITEM_MAP_Obj;

            try
            {
                foreach (CRM_CUST_ITEM_MAP CRM_CUST_ITEM_MAP_Obj in AdmCustomerBrandsMstObj)
                {
                    if (CRM_CUST_ITEM_MAP_Obj.CIM_PK == 0)   //SAVE
                    {
                        // Gets last pk
                        maxPK = this.currentEntity.CRM_CUST_ITEM_MAP.Max(a => (int?)a.CIM_PK);

                        // Sets return value as next pk
                        retval = (maxPK.HasValue ? maxPK.Value + 1 : 1);
                        CRM_CUST_ITEM_MAP_Obj.CIM_PK = retval;

                        CRM_CUST_ITEM_MAP_Obj.CIM_MOD_DT = DateTime.Now;

                        this.currentEntity.CRM_CUST_ITEM_MAP.AddObject(CRM_CUST_ITEM_MAP_Obj);
                    }
                    else //UPDATION
                    {
                        Old_CRM_CUST_ITEM_MAP_Obj = this.currentEntity.CRM_CUST_ITEM_MAP.SingleOrDefault(a => a.CIM_PK == CRM_CUST_ITEM_MAP_Obj.CIM_PK && a.CIM_MOD_DT == CRM_CUST_ITEM_MAP_Obj.CIM_MOD_DT);

                        if (Old_CRM_CUST_ITEM_MAP_Obj != null)
                        {
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_CUSTOMER = CRM_CUST_ITEM_MAP_Obj.CIM_CUSTOMER;
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_BRAND_CODE = CRM_CUST_ITEM_MAP_Obj.CIM_BRAND_CODE;
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_BRAND_NAME = CRM_CUST_ITEM_MAP_Obj.CIM_BRAND_NAME;
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_ITEM = CRM_CUST_ITEM_MAP_Obj.CIM_ITEM;
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_ACTIVE = CRM_CUST_ITEM_MAP_Obj.CIM_ACTIVE;
                            Old_CRM_CUST_ITEM_MAP_Obj.CIM_MOD_DT = DateTime.Now;

                            retval = CRM_CUST_ITEM_MAP_Obj.CIM_PK;
                        }
                        else
                        {
                            // throws exception already deleted or modified by other user
                            throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                        }
                    }

                }

                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

            throw new NotImplementedException();
        }

        public int DeleteCustomerBrands(List<CRM_CUST_ITEM_MAP> AdmCustomerBrandsMstObj)
        {
            int retval = 0;
            CRM_CUST_ITEM_MAP Old_CRM_CUST_ITEM_MAP_Obj;

            try
            {
                foreach (CRM_CUST_ITEM_MAP CRM_CUST_ITEM_MAP_Obj in AdmCustomerBrandsMstObj)
                {
                    Old_CRM_CUST_ITEM_MAP_Obj = currentEntity.CRM_CUST_ITEM_MAP.SingleOrDefault(sah => sah.CIM_PK == CRM_CUST_ITEM_MAP_Obj.CIM_PK);
                    if (Old_CRM_CUST_ITEM_MAP_Obj != null)
                    {
                        this.currentEntity.CRM_CUST_ITEM_MAP.DeleteObject(Old_CRM_CUST_ITEM_MAP_Obj);
                    }
                    else
                    {
                        // throws exception already deleted or modified by other user
                        throw new OptimisticConcurrencyException(ERPManagerRes.EditConcurrencyException);
                    }
                    retval = 1;
                }

                return retval;
            }
            catch (OptimisticConcurrencyException ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        #endregion
    }
}
