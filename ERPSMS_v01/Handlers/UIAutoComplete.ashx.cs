using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject.Common;
using ERPService.Inventory;
using BusinessLogic.CommonManagement;



namespace ERPSMS_v01.Handlers
{
    /// <summary>
    /// Summary description for UIAutoComplete
    /// </summary>
    public class UIAutoComplete : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Resusable Flag
        /// don't delete
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        BusinessObject.User currentUser;
        HttpRequest request;
        HttpResponse response;
        private bool IsSBUVendor = false;
        /// <summary>
        /// Request Processing Event
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                GetFieldValues(context);
            }
            catch
            {
            }
        }

        #region GetFieldValues
        /// <summary>
        /// get field values
        /// </summary>
        /// <param name="context"></param>
        private void GetFieldValues(HttpContext context)
        {
            string searchKey;
            string searchType;
            string filterType;
            string basisType;
            string autoFilterType;
            string serviceType;
            string searchBy;
            string excDate;
            int processPK;
            int CustomerID;
            int BrandID;
            int itemPK;
            int sale;
            int categ;
            int PackSpec;
            int SubType;
            int ItemCategPk;
            int ItemTypePk;
            string Category;
            int IsProductRequired;
            string DeptPK;
            //int ItemPk;
            string fieldName;
            short? Role = null;
            int? MenuType=0;
            AutoEnum currAutoEnum;
            try
            {
                searchKey = string.Empty;
                searchType = string.Empty;
                filterType = string.Empty;
                basisType = string.Empty;
                autoFilterType = string.Empty;
                serviceType = string.Empty;
                searchBy = string.Empty;
                excDate = string.Empty;
                processPK = 0;
                CustomerID = 0;
                BrandID = 0;
                itemPK = 0;
                categ = 0;
                PackSpec = 0;
                SubType = 0;
                ItemCategPk = 0;
                ItemTypePk = 0;
                //ItemPk = 0;
                request = context.Request;
                response = context.Response;
                fieldName = string.Empty;
                string pageURL = string.Empty;
                Category = string.Empty;
                IsProductRequired = 0;
                DeptPK = string.Empty;
                int scID = 0;
                //fetch Request Parameters to variables
                if (request.Params[RequestParameters.SearchValue] != null)
                {
                    searchKey = HttpUtility.HtmlDecode(request.Params[RequestParameters.SearchValue].Trim().ToString());
                }
                if (request.Params[RequestParameters.Category] != null)
                {
                    Category = request.Params[RequestParameters.Category].Trim();
                }

                if (request.Params[RequestParameters.IsProductRequired] != null)
                {
                    IsProductRequired = Convert.ToInt32(request.Params[RequestParameters.IsProductRequired].Trim()); ;
                }

                if (request.Params[RequestParameters.Role] != null)
                {
                    Role = Convert.ToInt16(request.Params[RequestParameters.Role].Trim());
                }
                if (request.Params[RequestParameters.MenuType] != null)
                {
                    MenuType = Convert.ToInt16(request.Params[RequestParameters.MenuType].Trim());
                }
                if (request.Params[RequestParameters.Dep] != null)
                {
                    DeptPK = request.Params[RequestParameters.Dep].Trim();
                }
                if (request.Params[RequestParameters.CustomerID] != null)
                {
                    CustomerID = Convert.ToInt32(request.Params[RequestParameters.CustomerID].Trim());
                }
                if (request.Params[RequestParameters.BrandID] != null)
                {
                    BrandID = Convert.ToInt32(request.Params[RequestParameters.BrandID].Trim());
                }
                if (request.Params[RequestParameters.itemPK] != null)
                {
                    itemPK = Convert.ToInt32(request.Params[RequestParameters.itemPK].Trim());
                }
                if (request.Params[RequestParameters.SearchBy] != null)
                {
                    searchBy = request.Params[RequestParameters.SearchBy].Trim().ToString();
                }
                if (request.Params[RequestParameters.SearchType] != null)
                {
                    searchType = request.Params[RequestParameters.SearchType].Trim().ToString();
                }
                if (request.Params[RequestParameters.Type] != null)
                {
                    filterType = request.Params[RequestParameters.Type].Trim().ToString();
                }
                if (request.Params[RequestParameters.BasisType] != null)
                {
                    basisType = request.Params[RequestParameters.BasisType].Trim().ToString();
                }
                if (request.Params[RequestParameters.FilterType] != null)
                {
                    autoFilterType = request.Params[RequestParameters.FilterType].Trim().ToString();
                }
                if (request.Params[RequestParameters.ServiceType] != null)
                {
                    serviceType = request.Params[RequestParameters.ServiceType].Trim().ToString();
                }
                if (request.Params[RequestParameters.ProcessPK] != null)
                {
                    processPK = Convert.ToInt32(request.Params[RequestParameters.ProcessPK].Trim());
                }
                if (request.Params[RequestParameters.ExcDate] != null)
                {
                    excDate = request.Params[RequestParameters.ExcDate].ToString();
                }
                if (request.Params[RequestParameters.FieldName] != null)
                {
                    fieldName = HttpUtility.HtmlDecode(request.Params[RequestParameters.FieldName].Trim().ToString());
                }
                if (request.Params[RequestParameters.SCID] != null)
                {
                    scID = Convert.ToInt32(request.Params[RequestParameters.SCID].Trim());
                }
                if (request.Params[RequestParameters.BrandID] != null)
                {
                    BrandID = Convert.ToInt32(request.Params[RequestParameters.BrandID].Trim());
                }
                if (request.Params[RequestParameters.ItmCategoryPK] != null)
                {
                    ItemCategPk = Convert.ToInt32(request.Params[RequestParameters.ItmCategoryPK].Trim());
                }
                //if (request.Params[RequestParameters.ItemPK] != null)
                //{
                //    ItemPk = Convert.ToInt32(request.Params[RequestParameters.ItmCategoryPK].Trim());
                //}
                if (request.Params[RequestParameters.ItmCategory] != null)
                {
                    categ = Convert.ToInt32(request.Params[RequestParameters.ItmCategory].Trim());
                }
                if (request.Params[RequestParameters.PackSpec] != null)
                {
                    PackSpec = Convert.ToInt32(request.Params[RequestParameters.PackSpec].Trim());
                }
                if (request.Params[RequestParameters.SubType] != null)
                {
                    SubType = Convert.ToInt32(request.Params[RequestParameters.SubType].Trim());
                }
                if (request.Params[RequestParameters.PAGE_URL] != null)
                {
                    pageURL = request.Params[RequestParameters.PAGE_URL].Trim().ToString();
                }
                if (request.Params[RequestParameters.Category] != null)
                {
                    ItemCategPk =Convert.ToInt32(request.Params[RequestParameters.Category].Trim());
                }
                if (request.Params[RequestParameters.Type] != null)
                {
                    int.TryParse(request.Params[RequestParameters.Type].Trim(), out ItemTypePk);
                }
                if (request.Params[RequestParameters.CustomerID] != null)
                {
                    CustomerID = Convert.ToInt32(request.Params[RequestParameters.CustomerID].Trim());
                }
                if (request.Params[RequestParameters.IsSBUVendor] != null)
                {
                    if (request.Params[RequestParameters.IsSBUVendor].Trim().ToString() != string.Empty)
                        IsSBUVendor = Convert.ToBoolean(request.Params[RequestParameters.IsSBUVendor].Trim().ToString());
                }

                //switch the selected autocomplete method
                currAutoEnum = (AutoEnum)Enum.Parse(typeof(AutoEnum), searchType.ToUpper());
                switch (currAutoEnum)
                {
                    case AutoEnum.GETINVLOC:
                        GetInventoryLocation(searchKey, filterType, Category, DeptPK);
                        break;
                    case AutoEnum.VENDOR:
                        GetVendor(searchKey, Role);
                        break;
                    case AutoEnum.SERVICEVENDOR:
                        GetServiceVendor(searchKey);
                        break;
                    case AutoEnum.PRODUCTREQUEST:
                        GetProductRequest(searchKey, processPK);
                        break;
                    case AutoEnum.MAILTYPE:
                        GetMailType(searchKey);
                        break;
                    case AutoEnum.GETTRXTYPE:
                        GetTrxType(searchKey);
                        break;
                    case AutoEnum.CATEGORY:
                        GetItemCategory(searchKey);
                        break;
                    case AutoEnum.ITEM:
                        GetItem(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType));
                        break;
                    case AutoEnum.ITEMTYPE:
                        GetItemType(searchKey, filterType, processPK);
                        break;
                    case AutoEnum.RFQPRSEARCH:
                        GetRFQItemSearch(searchKey, searchBy);
                        break;
                    case AutoEnum.VENDORCURRENCY:
                        GetVendorCurrency(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), excDate);
                        break;
                    case AutoEnum.CUTOMERBRAND:
                        GetCustomerBrand(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType));
                        break;
                    case AutoEnum.CUTOMERBRANDWITHSPEC:
                        GetCustomerBrand(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), true);
                        break;
                    case AutoEnum.CUSTOMERBRANDCODEWITHSPEC:
                        GetCustomerBrand(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), true, searchFor: "CIM_BRAND_CODE");
                        break;
                    case AutoEnum.BRANDBYSC:
                        GetBrandBySC(searchKey, string.IsNullOrEmpty(serviceType) ? 0 : Convert.ToInt32(serviceType), string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), true, searchFor: "CIM_BRAND_TEXT");
                        break;
                    case AutoEnum.SELECTLOTNO:
                        GetLotNoBySphpPlanPk(string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), searchKey, true, searchFor: "SOD_LOT_NO");
                        break;
                    case AutoEnum.SELECTLOTNOBYBRAND:
                        GetLotNoByBrandPK(string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), searchKey, true, searchFor: "SOD_LOT_NO");
                        break;
                    //case AutoEnum.SELECTLOTNO:
                    //    GetLotNoByLoadPk(string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), searchKey, true, searchFor: "LPD_LOT_NO");
                    //    break;
                    case AutoEnum.CUSTOMERBRANDCODENAMEWITHSPEC:
                        GetCustomerBrand(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType), true, searchFor: "CIM_BRAND_TEXT");
                        break;
                    case AutoEnum.CUTOMERPRODUCT:
                        GetCustomerProduct(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType));
                        break;
                    case AutoEnum.GETPARTY:
                        GetMailQParty(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType));
                        break;
                    case AutoEnum.CUSTOMER:
                        GetCustomer(searchKey);
                        break;
                    case AutoEnum.ENQUIRYNO:
                        GetEnquiryNo(searchKey, (int)DirectOrderStatus.Enquiry);
                        break;
                    case AutoEnum.QUOTATIONNO:
                        GetEnquiryNo(searchKey, (int)DirectOrderStatus.Quotation);
                        break;
                    case AutoEnum.DIRECTORDERNO:
                        GetEnquiryNo(searchKey, (int)DirectOrderStatus.DirectOrder);
                        break;
                    case AutoEnum.CUSTOMERORDERNO:
                        GetCustOrderNo(searchKey, (int)DirectOrderStatus.DirectOrder);
                        break;
                    case AutoEnum.SALEORDERNO:
                        GetCustOrderNo(searchKey, (int)DirectOrderStatus.SalesOrder);
                        break;
                    case AutoEnum.ARTWORK:
                        GetArtwork(searchKey, string.IsNullOrEmpty(filterType) ? 0 : Convert.ToInt32(filterType));
                        break;
                    case AutoEnum.CURRENCY:
                        GetCurrency(searchKey, null);
                        break;
                    case AutoEnum.GETAPPCONFIG:
                        GetAppConfig(searchKey, filterType);
                        break;
                    case AutoEnum.PRODUCTMASTER:
                        GetProductMaster(searchKey, filterType);
                        break;
                    case AutoEnum.UOM:
                        GetUOM(searchKey);
                        break;
                    case AutoEnum.BRANDCUSTOMER:
                        BrandList(searchKey, CustomerID, BrandID);
                        break;
                    case AutoEnum.BRANDPRODUCT:
                        BrandProduct(searchKey, itemPK, BrandID);
                        break;
                    case AutoEnum.STOCKADMISSION:
                        GetStockAdmission(searchKey, processPK, fieldName);
                        break;
                    case AutoEnum.DIRECTSTOCKADMISSIONNO:
                        GetDirectStockAdmissionNo(searchKey, fieldName,MenuType);
                        break;
                    case AutoEnum.PALLETE:
                        GetPalleteAutoComplete(fieldName, searchKey, scID, BrandID);
                        break;
                    //case AutoEnum.PONO:
                    //    GetDirectStockAdmissionNo(searchKey, processPK, fieldName);
                    //    break;
                    case AutoEnum.PMAUTO:
                        GetPMAutoComplete(fieldName, searchKey, CustomerID, itemPK);
                        break;
                    case AutoEnum.GETITEMCATEGORYSALE:
                        GetItemCategorySale(searchKey, ItemCategPk);
                        break;
                    case AutoEnum.GETITEMSALE:
                        GetItemSale(searchKey, itemPK, categ, PackSpec, SubType);
                        break;
                    case AutoEnum.GETDIRECTSONOAUTO:
                        GetDirectSONoAuto(searchKey, filterType, pageURL);
                        break;
                    case AutoEnum.PACKINGSPEC:
                        GetBrandPackSpec(searchKey);
                        break;
                    case AutoEnum.PACKINGSPECCATEGORY:
                        GetPackingSpecCategory(searchKey,IsProductRequired);
                        break;
                    case AutoEnum.PACKINGSPECTYPE:
                        GetPackingSpecType(searchKey, IsProductRequired);
                        break;
                    case AutoEnum.GETPACKINGSPEC:
                        GetPackingSpec(searchKey, ItemTypePk,ItemCategPk, CustomerID, IsProductRequired);
                        break;

                    #region PORT DETAILS
                    case AutoEnum.FILLPORTDETAILS:
                        GetPortDetails(searchKey);
                        break;
                    #endregion
                    #region PURRFQAUTO
                    case AutoEnum.PURRFQAUTO:
                        GetPURRFQAuto(searchKey, pageURL, fieldName);
                        break;
                    #endregion
                    case AutoEnum.USEDCUSTOMER:
                        GetUsedCustomer(searchKey);
                        break;
                    case AutoEnum.CONSULTANT:
                        GetConsultant(searchKey);
                        break;
                    case AutoEnum.HSNNO:
                        GetHSNNO(searchKey);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Helper Methods

        /// <summary>
        /// Get Inventory Location
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="deptPK"></param>
        private void GetInventoryLocation(string searchKey, string type, string category, string deptPK)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.StoreManagement.StoreMaster.GetInventoryLocationList(deptPK, type, category, searchValue);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("DPT_PK"),
                    Name = row.Field<string>("DPT_NAME")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Application Configuration
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="searchType"></param>
        private void GetAppConfig(string searchKey, string searchType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {

                DataTable dtSearch = CommonBL.GetAppConfig(currentUser.SBUID, searchType);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<byte>("CFG_VALUE"),

                    Name = row.Field<string>("CFG_DATA")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                serializer = new JavaScriptSerializer();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// GetProductNature
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="filterType"></param>
        private void GetProductMaster(string searchKey, string filterType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.FilterBy = filterType;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                INV_ITEM_MSTList = CommonServiceClient.GetInvItemMstAutoCompleteList(Convert.ToByte(DbActiveStatus.ACTIVE), serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                if (filterType == Resources.DataFieldRes.ItemName)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK);
                }
                else if (filterType == Resources.DataFieldRes.ItemCode)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                }

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                INV_ITEM_MSTList = null;
            }
        }

        /// <summary>
        /// Get Mail Type
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="role"></param>
        private void GetMailType(string searchKey, short? role = null)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {
                DataTable dtSearch = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "CRM MAIL TYPE");
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<byte>("CFG_VALUE"),

                    Name = row.Field<string>("CFG_DATA")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                serializer = new JavaScriptSerializer();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        /// <summary>
        /// Get Transaction Type
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="role"></param>
        private void GetTrxType(string searchKey, short? role = null)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            try
            {

                DataTable dtSearch = BusinessLogic.CommonManagement.CommonBL.GetTrxTypeList(0, null, Convert.ToByte(CommonConstants.ACTIVE), currentUser.SBUID, ApplicationType.MailQueue);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int16>("APT_PK"),

                    Name = row.Field<string>("APT_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                serializer = new JavaScriptSerializer();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Product Request
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetProductRequest(string searchKey, int processPK)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> purchaseRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                purchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestAutocomplete("POH_NO", searchValue, processPK, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purchaseRequest, "Key", "Name");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Item Category
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetItemCategory(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> category;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                category = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryAuto(objUser.SBUID, searchValue);

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(category, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Item Category
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetItem(string searchKey, int category)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> item;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                item = BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialByCategoryAuto(searchValue, category, 0);

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(item, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetItemType(string searchKey, string filterType, int type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            try
            {
                CommonServiceClient = new CommonService();
                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.FilterBy = filterType;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                INV_ITEM_MSTList = CommonServiceClient.GetInvItemMstTypeAutoCompleteList(Convert.ToByte(DbActiveStatus.ACTIVE), type, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                if (filterType == Resources.DataFieldRes.ItemName)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK);
                }
                else if (filterType == Resources.DataFieldRes.ItemCode)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemPK);
                }
                else if (filterType == string.Empty)
                {
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(INV_ITEM_MSTList, Resources.DataFieldRes.ItemCode, Resources.DataFieldRes.ItemName, Resources.DataFieldRes.ItemPK, true, Resources.Constants.VendorTextFormat);
                }

                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                CommonServiceClient = null;
                serviceUtilityObj = null;
                INV_ITEM_MSTList = null;
            }
        }

        /// <summary>
        /// For auto Search RFQ Item Search
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetRFQItemSearch(string searchKey, string searchBy)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> item;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                item = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetItemPRSearchValues(searchBy, searchValue, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(item, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor Currency
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetVendorCurrency(string searchKey, int vendor, string excDate)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> currencies;
            JavaScriptSerializer serializer;
            DateTime date;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (DateTime.TryParse(excDate, out date))
                {
                    currencies = BusinessLogic.Administration.Masters.CurrencyMaster.GetVendorExchangeCurrencyAuto(searchValue, vendor, date);
                }
                else
                {
                    currencies = BusinessLogic.Administration.Masters.CurrencyMaster.GetVendorExchangeCurrencyAuto(searchValue, vendor, DateTime.Now);
                }

                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(currencies, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetCustomerProduct(string item, int customer)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                item = !string.IsNullOrEmpty(item) ? "%" + item + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(0, 0, customer, "%%", item, objUser.SBUID, 1).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CIM_ITEM"),
                    Name = row.Field<string>("CIM_ITEM_TEXT")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetBrandBySC(string brand, int shpPK, int scPk, bool? hasSpec = false, string searchFor = "CIM_BRAND_TEXT")
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                brand = !string.IsNullOrEmpty(brand) ? "%" + brand + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetBrandBySC(shpPK, scPk, brand, objUser.SBUID, 1, hasSpec).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CIM_SOD_PK"),
                    //Key = row.Field<int>("CIM_PK"),
                    Name = row.Field<string>(searchFor)
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetLotNoByLoadPk(int pk, string lotNo, bool? hasSpec = false, string searchFor = "LPD_LOT_NO")
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                lotNo = !string.IsNullOrEmpty(lotNo) ? "%" + lotNo + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetLotNoByLoadPk(pk, lotNo, objUser.SBUID, 2, hasSpec).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("LPD_PK"),
                    Name = row.Field<string>(searchFor)
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetLotNoBySphpPlanPk(int pk, string lotNo, bool? hasSpec = false, string searchFor = "SOD_LOT_NO")
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                lotNo = !string.IsNullOrEmpty(lotNo) ? "%" + lotNo + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetLotNoBySphpPlanPk(pk, lotNo, objUser.SBUID, 2, hasSpec).Tables[0].DefaultView.ToTable(true, Resources.DataFieldRes.SODLotNo);
                DataTable dtSearchNew = new DataTable();
                dtSearchNew.Columns.Add("tempPk", typeof(int));
                dtSearchNew.Columns.Add(searchFor, typeof(string));
                int tempPk = 1;
                for (int i = 0; i < dtSearch.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dtSearch.Rows[i][searchFor].ToString()))
                    {
                        dtSearchNew.Rows.Add(tempPk, dtSearch.Rows[i][searchFor].ToString());
                        tempPk++;
                    }
                }
                result = dtSearchNew.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("tempPk"),
                    Name = row.Field<string>(searchFor)
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetLotNoByBrandPK(int pk, string lotNo, bool? hasSpec = false, string searchFor = "SOD_LOT_NO")
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                lotNo = !string.IsNullOrEmpty(lotNo) ? "%" + lotNo + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetLotNoByBrandPK(pk, lotNo, objUser.SBUID, 2, hasSpec).Tables[0].DefaultView.ToTable(true, Resources.DataFieldRes.SODLotNo);
                DataTable dtSearchNew = new DataTable();
                dtSearchNew.Columns.Add("tempPk", typeof(int));
                dtSearchNew.Columns.Add(searchFor, typeof(string));
                int tempPk = 1;
                for (int i = 0; i < dtSearch.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dtSearch.Rows[i][searchFor].ToString()))
                    {
                        dtSearchNew.Rows.Add(tempPk, dtSearch.Rows[i][searchFor].ToString());
                        tempPk++;
                    }
                }
                result = dtSearchNew.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("tempPk"),
                    Name = row.Field<string>(searchFor)
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetCustomerBrand(string brand, int customer, bool? hasSpec = false, string searchFor = "CIM_BRAND_TEXT")
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                brand = !string.IsNullOrEmpty(brand) ? "%" + brand + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(0, 0, customer, brand, "%%", objUser.SBUID, 1, hasSpec).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CIM_PK"),
                    Name = row.Field<string>(searchFor)
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        //
        private void GetMailQParty(string searchKey, int Type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetMailQParty(Type).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("PARTY_PK"),
                    Name = row.Field<string>("PARTY_NAME")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        private void GetCustomer(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetCustomer(0, HttpUtility.HtmlEncode(searchKey), objUser.SBUID, 1).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CUS_PK"),
                    Name = row.Field<string>("CUS_TEXT")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void BrandList(string searchKey, int CustomerID, int BrandID)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetBrandList(searchKey, CustomerID, BrandID).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CUS_PK"),
                    Name = row.Field<string>("CUS_TEXT")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void BrandProduct(string searchKey, int itemPK, int BrandID)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetBrandProduct(searchKey, itemPK, BrandID).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("ITM_PK"),
                    Name = row.Field<string>("ITM_NAME")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetEnquiryNo(string searchKey, int directOrderStatus)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string field = "CEH_ENQ_NO";
                if (directOrderStatus == (int)DirectOrderStatus.Enquiry)
                    field = "CEH_ENQ_NO";
                else if (directOrderStatus == (int)DirectOrderStatus.Quotation)
                    field = "CEH_QTN_NO";
                else if (directOrderStatus == (int)DirectOrderStatus.DirectOrder)
                    field = "CEH_DOR_NO";
                DataTable dtSearch = BusinessLogic.Sales.Enquiry.GetEnquiryNoAuto(field, searchKey, objUser.SBUID, objUser.PKUser).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetCustOrderNo(string searchKey, int directOrderStatus)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string field = "CEH_COR_NO";
                if (directOrderStatus == (int)DirectOrderStatus.DirectOrder)
                    field = "CEH_COR_NO";
                else if (directOrderStatus == (int)DirectOrderStatus.SalesOrder)
                    field = "CEH_SOR_NO";
                DataTable dtSearch = BusinessLogic.Sales.Enquiry.GetEnquiryNoAuto(field, searchKey, objUser.SBUID, objUser.PKUser).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        private void GetArtwork(string searchKey, int brandPK)
        {
            List<AutoCompletePairedBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtSearch = BusinessLogic.Sales.CustomerProduct.GetArtWork(0, brandPK, 1, searchKey);
                dtSearch = CommonFunctions.HtmlDecodeDataTable(dtSearch, "CIA_ART_WORK");
                result = dtSearch.AsEnumerable().Select(row => new AutoCompletePairedBO()
                {
                    Key = row.Field<int>("CIA_PK"),
                    Name = row.Field<string>("CIA_ART_WORK"),
                    PairText = row.Field<string>("CIA_FILE_PATH"),
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key", "PairText");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Currency
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetCurrency(string searchKey, string filterType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            CurrencyMstService currencyMstServiceClient;

            ADM_CURRENCY_MST admCurrencyMstObj;
            List<ADM_CURRENCY_MST> admCurrencyMstList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            currencyMstServiceClient = null;
            try
            {
                currencyMstServiceClient = new CurrencyMstService();
                currencyMstServiceClient = CommonFunctions.InitiateClient(currencyMstServiceClient);
                admCurrencyMstObj = CommonFunctions.Initilize<ERPData.ADM_CURRENCY_MST>();
                admCurrencyMstObj.CUR_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                admCurrencyMstObj.CUR_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.CurrencyCode;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                admCurrencyMstList = currencyMstServiceClient.GetCurrencyListAutoCompleteList(admCurrencyMstObj, serviceUtilityObj);
                var searchResult = (Object)null;
                if (string.IsNullOrEmpty(filterType))
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(admCurrencyMstList, Resources.DataFieldRes.CurrencyCode, Resources.DataFieldRes.CurrencyName, Resources.DataFieldRes.CurrencyPK, true);
                else
                    searchResult = CommonFunctions.GetFormatedAutoCompleteList(admCurrencyMstList, Resources.DataFieldRes.CurrencyCode, Resources.DataFieldRes.CurrencyPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                currencyMstServiceClient = null;
                admCurrencyMstObj = null;
                serviceUtilityObj = null;
                admCurrencyMstList = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetVendor(string searchKey, short? role = null)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                serviceUtilityObj.IsSBUSpecific = IsSBUVendor;
                if (role.HasValue)
                {
                    purVendorObj.PUR_VENDOR_ROLE_MAP = new System.Data.Objects.DataClasses.EntityCollection<PUR_VENDOR_ROLE_MAP>();
                    purVendorObj.PUR_VENDOR_ROLE_MAP.Add(
                        new PUR_VENDOR_ROLE_MAP()
                        {
                            VRM_ROLE = role.Value
                        });
                }
                purVendorList = VendorMstServiceClient.GetVendorListAutoCompleteList(purVendorObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purVendorList, Resources.DataFieldRes.VendorName, Resources.DataFieldRes.VendorPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
            }
        }

        /// <summary>
        /// For auto Search Vendor
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetServiceVendor(string searchKey, short? role = null)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            VendorMstService VendorMstServiceClient;

            PUR_VENDOR_MST purVendorObj;
            List<PUR_VENDOR_MST> purVendorList;
            ServiceUtility serviceUtilityObj;
            JavaScriptSerializer serializer;
            VendorMstServiceClient = null;
            try
            {
                VendorMstServiceClient = new VendorMstService();
                VendorMstServiceClient = CommonFunctions.InitiateClient(VendorMstServiceClient);
                purVendorObj = CommonFunctions.Initilize<ERPData.PUR_VENDOR_MST>();
                purVendorObj.VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                purVendorObj.VEN_ACTIVE = 1;
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.VendorName;
                serviceUtilityObj.FilterValue = searchKey.Trim();
                //if (role.HasValue)
                //{
                //    purVendorObj.PUR_VENDOR_ROLE_MAP = new System.Data.Objects.DataClasses.EntityCollection<PUR_VENDOR_ROLE_MAP>();
                //    purVendorObj.PUR_VENDOR_ROLE_MAP.Add(
                //        new PUR_VENDOR_ROLE_MAP()
                //        {
                //            VRM_ROLE = role.Value
                //        });
                //}
                purVendorList = VendorMstServiceClient.GetServiceVendorListAutoCompleteList(purVendorObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(purVendorList, Resources.DataFieldRes.VendorName, Resources.DataFieldRes.VendorPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                VendorMstServiceClient = null;
                purVendorObj = null;
                serviceUtilityObj = null;
                purVendorList = null;
            }
        }

        private void GetUOM(string searchKey)
        {
            InvItemMstService invItemMstServiceClient;
            ServiceUtility serviceUtilityObj;
            INV_UOM_MST invUomMstObj;
            List<INV_UOM_MST> invUomMstList;
            JavaScriptSerializer serializer;
            try
            {
                invItemMstServiceClient = new InvItemMstService();
                invItemMstServiceClient = CommonFunctions.InitiateClient(invItemMstServiceClient);
                serviceUtilityObj = new ServiceUtility();
                serializer = new JavaScriptSerializer();
                serviceUtilityObj.CurrentPage = -1;
                serviceUtilityObj.PageSize = -1;
                serviceUtilityObj.FilterBy = Resources.DataFieldRes.UomCode;
                serviceUtilityObj.FilterValue = searchKey;
                invUomMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_UOM_MST>();
                invUomMstObj.UOM_PK = 0;
                invUomMstObj.UOM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                invUomMstList = invItemMstServiceClient.GetUomMstAutoCompleteList(invUomMstObj, serviceUtilityObj);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(invUomMstList, Resources.DataFieldRes.UomCode, Resources.DataFieldRes.UomPK);
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
                invItemMstServiceClient = null;
                serviceUtilityObj = null;
                invUomMstObj = null;
                invUomMstList = null;
            }
        }

        /// <summary>
        /// For auto Search Product Request
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetStockAdmission(string searchKey, int processPK, string fieldName)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.StoreManagement.StockTransferBL.GetStockAdmissionAutocomplete(fieldName, searchValue, processPK, objUser);
                //stockAdmissionRequest = BusinessLogic.StoreManagement.StockTransferBL.GetAutoCompleteSearch("SFH_NO", searchValue, objUser, processPK);
                // var a= BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestAutocomplete("POH_NO", searchValue, processPK, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Product Request
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetDirectStockAdmissionNo(string searchKey, string fieldName,int? MenuType=0)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> stockAdmissionRequest;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                stockAdmissionRequest = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockAdmissionAutocomplete(fieldName, searchValue, objUser,MenuType);
                //stockAdmissionRequest = BusinessLogic.StoreManagement.StockTransferBL.GetAutoCompleteSearch("SFH_NO", searchValue, objUser, processPK);
                // var a= BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestAutocomplete("POH_NO", searchValue, processPK, objUser);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(stockAdmissionRequest, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Pallet BinCard
        /// </summary>
        /// <param name="fieldName">string</param>
        /// <param name="searchKey">string</param>
        /// <param name="scID">int</param>
        /// <param name="BrandID">int</param>
        /// </param>
        private void GetPalleteAutoComplete(string fieldName, string searchKey, int scID, int BrandID)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> palletNo;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                palletNo = BusinessLogic.Shipping.ContainerReleaseBL.GetPalleteAutoComplete(fieldName, searchValue, scID, BrandID, objUser.SBUID);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(palletNo, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search PMAUTO
        /// </summary>
        /// <param name="fieldName">string</param>
        /// <param name="searchKey">string</param>
        /// <param name="scID">int</param>
        /// <param name="BrandID">int</param>
        /// </param>
        private void GetPMAutoComplete(string fieldName, string searchKey, int customerPK, int packMatAutoValue)
        {

            JavaScriptSerializer serializer;
            List<AutoCompleteBO> result;
            DataTable dtAsset;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtAsset = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetailsAuto(0, 0, packMatAutoValue, customerPK, 1, currentUser.SBUID, searchValue);
                result = dtAsset.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>(Resources.DataFieldRes.IPDITEM),
                    Name = row.Field<string>(Resources.DataFieldRes.ItemName)

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Item Category with Sale
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetItemCategorySale(string searchKey, int ItemCategPk)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtItemCategorySaleList;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtItemCategorySaleList = CommonBL.GetItemCategorySale(searchValue, ItemCategPk, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE));
                result = dtItemCategorySaleList.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ITC_PK"),

                    Name = row.Field<string>("ITC_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Item BrandPackingSpec
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetBrandPackSpec(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtItemCategorySaleList;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtItemCategorySaleList = CommonBL.GetBrandPackSpec(searchValue, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK);
                result = dtItemCategorySaleList.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("KEY"),

                    Name = row.Field<string>("VALUE")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }



        /// <summary>
        /// For auto Search Item BrandPackingSpecCategory
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetPackingSpecCategory(string searchKey,int IsProductRequired)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtPackSpecCategory;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            int Categorytype = 3; //for packing material
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                if(IsProductRequired==1)
                {
                    Categorytype = 2; //for product 
                }

                dtPackSpecCategory = CommonBL.GetPackingSpecCategory(objUser.SBUID, searchValue, Categorytype);
                result = dtPackSpecCategory.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ITC_PK"),

                    Name = row.Field<string>("ITC_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }




        /// <summary>
        /// For auto Search Item BrandPackingSpecCategory
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetPackingSpecType(string searchKey,int IsProductRequired)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtPackSpecType;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtPackSpecType = CommonBL.GetConstMstValuesAuto(0, 0, ConstGroupType.Packing, (int)PackingType.PakingMaterialType, searchValue, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK, IsProductRequired);
                //dtPackSpecType = CommonBL.GetPackingSpecType(searchValue,0,Convert.ToInt32(DbActiveStatus.ACTIVE),11,currentUser.CurrentSBUPK);
                result = dtPackSpecType.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CON_PK"),

                    Name = row.Field<string>("CON_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Get Used Customer
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetUsedCustomer(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtSearch = BusinessLogic.CommonManagement.CommonBL.GetUsedCustomer(0, HttpUtility.HtmlEncode(searchKey), currentUser.SBUID, 1).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("CUS_PK"),
                    Name = row.Field<string>("CUS_TEXT")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }
        /// <summary>
        /// Get Consultant
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetConsultant(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtSearch = BusinessLogic.CommonManagement.CommonBL.GetCustomer(0, HttpUtility.HtmlEncode(searchKey), currentUser.SBUID, 1, 1).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("CUS_PK"),
                    Name = row.Field<string>("CUS_TEXT")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }


        /// <summary>
        /// Get HSNNO
        /// </summary>
        /// <param name="searchKey"></param>
        private void GetHSNNO(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                DataTable dtSearch = BusinessLogic.CommonManagement.CommonBL.GetHSNNO(0, HttpUtility.HtmlEncode(searchKey), currentUser.SBUID, 1, 1).Tables[0];
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>("PK"),
                    Name = row.Field<string>("VALUE")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Item BrandPackingSpecCategory
        /// </summary>
        /// <param name="searchKey">
        /// </param>(searchKey, ItemCategPk, ,ItemTypePk,CustomerID
        private void GetPackingSpec(string searchKey, int Con_Pk, int Itc_Pk, int Cus_Pk,int IsProductRequired)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtPackSpecType;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtPackSpecType = CommonBL.GetPackingSpec(searchValue, Con_Pk, Itc_Pk, Cus_Pk, IsProductRequired);
                result = dtPackSpecType.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ITM_PK"),

                    Name = row.Field<string>("ITM_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }






        /// <summary>
        /// For auto Search Item under sale category 
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetItemSale(string searchKey, int ItemPK, int categ, int PackSpec, int SubType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtItemSaleList;
            List<AutoCompleteBO> result;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                dtItemSaleList = CommonBL.GetItemSale(searchValue, ItemPK, currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), categ, PackSpec, SubType);
                result = dtItemSaleList.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("ITM_PK"),

                    Name = row.Field<string>("ITM_NAME")

                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// For auto Search Direct Sale Order Number
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetDirectSONoAuto(string searchKey, string fieldName, string pagURL)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> directSO;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                directSO = BusinessLogic.Sales.DirectSaleOrderBL.GetDirectSONoAutocomplete(fieldName, searchValue, objUser, pagURL);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(directSO, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        /// <summary>
        /// Get Port Details
        /// </summary>
        /// <param name="searchKey"></param> 
        private void GetPortDetails(string searchKey)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                JavaScriptSerializer serializer;
                List<AutoCompleteBO> result;
                DataTable dtData;
                int SaleFromPort = 0;
                int SaleToPort = 0;
                int PurFromPort = 0;
                int PurToPort = 0;
                int SIType = 0;
                int PrmPK = 0;
                if (request.Params[RequestParameters.SIType] != null)
                {
                    SIType = Convert.ToInt32(request.Params[RequestParameters.SIType].ToString());
                }
                if (request.Params[RequestParameters.SaleFromPort] != null)
                {
                    SaleFromPort = Convert.ToInt32(request.Params[RequestParameters.SaleFromPort].ToString());
                }
                if (request.Params[RequestParameters.SaleToPort] != null)
                {
                    SaleToPort = Convert.ToInt32(request.Params[RequestParameters.SaleToPort].ToString());
                }
                if (request.Params[RequestParameters.PurFromPort] != null)
                {
                    PurFromPort = Convert.ToInt32(request.Params[RequestParameters.PurFromPort].ToString());
                }
                if (request.Params[RequestParameters.PurToPort] != null)
                {
                    PurToPort = Convert.ToInt32(request.Params[RequestParameters.PurToPort].ToString());
                }

                if (SIType != 1 && SIType != 2) // 1 = Domestic, 2 = Export
                    SIType = 0;

                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                dtData = BusinessLogic.CommonManagement.CommonBL.GetPortDetails(searchValue, PrmPK, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID,
                                  SIType, SaleFromPort, SaleToPort, PurFromPort, PurToPort);

                result = dtData.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<Int32>("PRM_PK"),
                    Name = row.Field<string>("PRM_NAME")
                }).ToList();
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(result, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));

            }
            catch (Exception ex) { throw ex; }
            finally { }

        }

        /// <summary>
        /// For auto Search RFQ Item Search
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private void GetPURRFQAuto(string searchKey, string pageURL, string searchBy)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            List<AutoCompleteBO> item;
            JavaScriptSerializer serializer;
            try
            {
                serializer = new JavaScriptSerializer();
                string searchValue = !string.IsNullOrEmpty(searchKey) ? "%" + searchKey + "%" : "%%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                item = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetPURRFQAuto(searchBy, searchValue, objUser, pageURL);
                var searchResult = CommonFunctions.GetFormatedAutoCompleteList(item, "Name", "Key");
                response.ClearContent();
                response.Write(serializer.Serialize(searchResult));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                serializer = null;
            }
        }

        #endregion
        #region AutoEnum
        /// <summary>
        /// Auto Complete Enum
        /// </summary>
        private enum AutoEnum
        {
            VENDOR,
            SERVICEVENDOR,
            PRODUCTREQUEST,
            CATEGORY,
            ITEM,
            ITEMTYPE,
            RFQPRSEARCH,
            VENDORCURRENCY,
            CUTOMERBRAND,
            CUTOMERPRODUCT,
            CUSTOMER,
            CUTOMERBRANDWITHSPEC,
            CUSTOMERBRANDCODEWITHSPEC,
            ENQUIRYNO,
            QUOTATIONNO,
            DIRECTORDERNO,
            CUSTOMERORDERNO,
            SALEORDERNO,
            ARTWORK,
            CURRENCY,
            PRODUCTMASTER,
            MAILTYPE,
            UOM,
            GETTRXTYPE,
            GETAPPCONFIG,
            GETPARTY,
            BRANDCUSTOMER,
            BRANDPRODUCT,
            CUSTOMERBRANDCODENAMEWITHSPEC,
            STOCKADMISSION,
            BRANDBYSC,
            SELECTLOTNO,
            SELECTLOTNOBYBRAND,
            DIRECTSTOCKADMISSIONNO,
            PALLETE,
            PMAUTO,
            GETITEMCATEGORYSALE,
            GETITEMSALE,
            GETDIRECTSONOAUTO,
            FILLPORTDETAILS,
            GETINVLOC,
            PURRFQAUTO,
            PACKINGSPEC,
            PACKINGSPECCATEGORY,
            PACKINGSPECTYPE,
            GETPACKINGSPEC,
            USEDCUSTOMER,
            CONSULTANT,
            HSNNO,
        }
        #endregion
    }
}