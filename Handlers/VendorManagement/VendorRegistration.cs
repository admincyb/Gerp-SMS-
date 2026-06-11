using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void VendorRegistration(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    //Used for Machinery Vendor Management
                    case "SaveVendorDetails":
                        Handlers.RegisterVendor(context);
                        break;
                    case "GetTaxDiscountDetails":
                        Handlers.GetTaxDiscountDetails(context);
                        break;
                    case "SaveVenMaterial":
                        Handlers.SaveVenMaterial(context);
                        break;
                    case "TaxDiscountApply":
                        Handlers.TaxDiscountApply(context);
                        break;
                    case "DeleteVndMaterial":
                        Handlers.DeleteVndMaterial(context);
                        break;
                    case "GetRole":
                        Handlers.GetVendorRole(context);
                        break;
                    case "GetVendorDetails":
                        Handlers.GetVendorDetails(context);
                        break;
                    case "GetVndMaterials":
                        Handlers.GetVndMaterials(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetVendorSearchValue(context);
                        break;
                    case "DeleteVendorDetails":
                        Handlers.DeleteVendorDetails(context);
                        break;
                    case "GetVendorAddress":
                        Handlers.GetVendorAddress(context);
                        break;

                    //Used For Vendor Management
                    case "GetVendors":
                        Handlers.GetVendors(context);
                        break;
                    case "GetPOVendorsAuto":
                        GetPOVendorAutoSearch(context);
                        break;
                    case "GetActiveVendors":
                        Handlers.GetActiveVendors(context);
                        break;
                    case "GetActiveVendorsOnly":
                        Handlers.GetActiveVendorsOnly(context);//vendors with Role :Dealer,PM Manufacturer
                        break;
                    case "GetVendorDtls":
                        Handlers.GetVendorDtls(context);
                        break;
                    case "GetVendorDtlsStatus":
                        Handlers.GetVendorDtlsStatus(context);
                        break;
                    case "GetVendorMaterials":
                        Handlers.GetVendorMaterials(context);
                        break;
                    case "GetVendorStoreMaterials":
                        Handlers.GetVendorStoreMaterials(context);
                        break;
                    case "GetVendorMaterialsCode":
                        Handlers.GetVendorMaterialsCode(context);
                        break;
                    case "GetVendorMaterialDetails":
                        Handlers.GetVendorMaterialDetails(context);
                        break;
                    case "GetMaterialUOM":
                        Handlers.GetVendorMaterialUOM(context);
                        break;
                    //Used For Vendor Management
                    case "GetPurchaseOrderVendors":
                        Handlers.GetPurchaseOrderVendors(context);
                        break;
                    //Used For Vendor Adddress Book Type
                    //////////////////////////////////////////////////////////////////////////////New
                    case "GetAddressTypeList":
                        Handlers.GetAddressTypeList(context);
                        break;

                    case "GetVendorPOType":
                        Handlers.GetVendorPOType(context);
                        break;

                    case "SaveVenBank":
                        Handlers.SaveVendorBank(context);
                        break;
                    case "GetAccountTypesBank":
                        Handlers.GetAccountTypesBank(context);
                        break;
                    case "GetVndBanks":
                        Handlers.GetVendorBanks(context);
                        break;
                    case "GetBankDetaislById":
                        Handlers.GetBankDetailsById(context);
                        break;
                    case "DeleteVndBank":
                        Handlers.DeleteVndBank(context);
                        break;
                    case "GetVendorAutoSearch":
                        Handlers.GetVendorAutoSearch(context);
                        break;
                    case "SaveVenLocalAddress":
                        Handlers.SaveVenLocalAddress(context);
                        break;
                    case "GetVendorLocalAddress":
                        Handlers.GetVendorLocalAddress(context);
                        break;
                    case "GetVendorLocalAddressById":
                        Handlers.GetVendorLocalAddressById(context);
                        break;
                    case "DeleteVenLocalAddress":
                        Handlers.DeleteVenLocalAddress(context);
                        break;

                }
            }
        }


        /// <summary>
        /// Set Tax And Discount for Vendor Item
        /// </summary>
        /// <param name="context"></param>
        private static void TaxDiscountApply(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string strXML = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.TaxDiscountApply(strXML));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Save Vendor Material
        /// </summary>
        /// <param name="context"></param>
        private static void SaveVenMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.SaveVenMaterial(requestData, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Add order Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorRole(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // int menuPK = Request.Params["deptID"] != null ? Convert.ToInt32(Request.Params["deptID"]) : 0;
                // int mapParentPK = Request.Params["MapParentID"] != null ? Convert.ToInt32(Request.Params["MapParentID"]) : 0;
                int bizUnit = Request.Params["SBU"] != "null" ? Convert.ToInt32(Request.Params["SBU"]) : 0;
                int venPK = Request.Params["venPK"] != null ? Convert.ToInt32(Request.Params["venPK"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetUserRole(venPK, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Add order Details
        /// </summary>
        /// <param name="context"></param>
        private static void RegisterVendor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string vendorDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string vendorPk = BusinessLogic.VendorManagement.VendorRegistration.RegisterVendor(vendorDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(vendorPk);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }
        /// <summary>
        /// Get Vendor Materials
        /// </summary>
        /// <param name="context"></param>
        private static void GetVndMaterials(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            string PageUrl = "";
            try
            {
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"]);
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVndMaterials(CommonFunctions.GetGridParams(Request), vendorID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// methord used to get venodr details
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procId = 0, venType=0, venActive = 1;
            string PageUrl = "";
            string venCode = "", venName = "", venPhone = "";
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procId = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["venCode"] != null)
                {                  
                    venCode = Request.Params["venCode"].ToString();                                     
                    if (venCode.Contains("ampersand"))
                    {
                        venCode = venCode.Replace("ampersand", "&");
                    }
                }
                if (Request.Params["venName"] != null)
                {                  
                    venName = Request.Params["venName"].ToString();
                    if (venName.Contains("ampersand"))
                    {
                        venName = venName.Replace("ampersand", "&");
                    }
                }
                if (Request.Params["venType"] != null && Request.Params["venType"] != "null")
                {
                    venType = Convert.ToInt32(Request.Params["venType"]);
                }
                if (Request.Params["venPhone"] != null)
                {
                    venPhone = Request.Params["venPhone"];
                }
                if (Request.Params["venActive"] != null && Request.Params["venActive"] != string.Empty)
                {
                    venActive = Convert.ToInt32(Request.Params["venActive"]);
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorDetails(CommonFunctions.GetGridParams(Request), objUser.SBUID, procId, PageUrl,venCode,venName,venType,venPhone,venActive));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// To get vendors for listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendors(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            try
            {
                //BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                //BizUnitPk = objUser.SBUID;
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendors(BizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// To get Active vendors for listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetActiveVendors(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            try
            {             
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetActiveVendors(BizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// To get Active vendors(Dealer & PM Manufacturer only) for listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetActiveVendorsOnly(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            try
            {
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetActiveVendorsOnly(BizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Vendor based on type
        /// </summary>
        /// <param name="context"></param>
        private static void GetTypeVendors(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            int type = 1; // 1 For Material
            try
            {
                //BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                //BizUnitPk = objUser.SBUID;
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["type"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetTypeVendors(BizUnitPk, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Active Vendor based on type
        /// </summary>
        /// <param name="context"></param>
        private static void GetTypeVendorsActive(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            int type = 1; // 1 For Material
            int VenPK = 0;
            try
            {                
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["type"]);
                }
                if (Request.Params["VenPK"] != null)
                {
                    VenPK = Convert.ToInt32(Request.Params["VenPK"]);
                }
                
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetTypeVendorsActive(BizUnitPk, type,VenPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Filter values for filling the search value
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int procId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.QueryString["ProcId"] != null)
                {
                    procId = Convert.ToInt32(Request.QueryString["ProcId"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));


                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetSearchValues(searchBy, searchValue, objUser, procId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Delete Vendor material
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteVndMaterial(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32((Request.Params["MaterialID"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.DeleteVndMaterial(materialID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Handler used to Delete vendor details by passing vendor ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteVendorDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["vendorID"] != null)
                {
                    vendorID = Convert.ToInt32((Request.Params["vendorID"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.DeleteVendorDetails(vendorID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method to get vendor details
        /// </summary>
        /// <param name="context"></param>
        public static void GetVendorAddress(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int VendorId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorId"] != null)
                {
                    VendorId = Convert.ToInt32((Request.Params["VendorId"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorAddress(VendorId));
            }
            catch (Exception ex)
            {
                Response.Write("-1");
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to get vendor details by pasing VendorID as Details in string Used in Po Generation
        /// </summary>
        /// <param name="context"></param>
        public static void GetVendorDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int VendorId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorId"] != null)
                {
                    VendorId = Convert.ToInt32((Request.Params["VendorId"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorDtls(VendorId));
            }
            catch (Exception ex)
            {
                Response.Write("-1");
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to get vendor details by pasing VendorID and Status(Active/Inactive) as Details in string Used in Po Generation
        /// For Resolving Bug ID:  2220 
        /// </summary>
        /// <param name="context"></param>
        public static void GetVendorDtlsStatus(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int VendorId = 0;
            int Status = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorId"] != null)
                {
                    VendorId = Convert.ToInt32((Request.Params["VendorId"].Trim()));
                }
                if (Request.Params["Status"] != null)
                {
                    Status = Convert.ToInt32((Request.Params["Status"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorDtlsStatus(VendorId,Status));
            }
            catch (Exception ex)
            {
                Response.Write("-1");
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Vendor Mapped Materials
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material Corresponding to Vendor selected</For>
        /// <Used In>Pomaterial Filling </Used>
        /// <param name="context"></param>
        private static void GetVendorMaterials(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorMaterials(vendorID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///  Function Used To Get Vendor store Mapped Materials
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorStoreMaterials(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            int storeID = 0,itemPk=0;
            int amend = 0;          

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }
                if (Request.Params["StoreID"] != "null")
                {
                    storeID = Convert.ToInt32(Request.Params["StoreID"].ToString());
                }
                if (Request.Params["Amend"] != "null")
                {
                    amend = Convert.ToInt32(Request.Params["Amend"].ToString());
                }
                if (Request.Params["ItemPk"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemPk"]);
                }
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorStoreMaterials(vendorID, storeID, amend == 1 ? 0 : objUser.PKUser,searchValue,itemPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Vendor Mapped Materials
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material Corresponding to Vendor selected</For>
        /// <Used In>Pomaterial Filling </Used>
        /// <param name="context"></param>
        private static void GetVendorMaterialsCode(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorMaterialsCode(vendorID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Materials Details Corrresponding to a material ID
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material details Corresponding to material Selection</For>
        /// <Used In>Po material details Filling </Used>
        /// <param name="context"></param>
        private static void GetVendorMaterialDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            int materialID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorId"].ToString());
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialId"].ToString());
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorMaterialDetails(vendorID, materialID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Materials Details Corrresponding to a material ID
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material details Corresponding to material Selection</For>
        /// <Used In>Po material details Filling </Used>
        /// <param name="context"></param>
        private static void GetVendorMaterialUOM(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            int materialID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorId"].ToString());
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialId"].ToString());
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetMaterialUOM(vendorID, materialID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Methode used to get the purhase order vendor 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderVendors(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int BizUnitPk = 0;
            int grhPK = 0;
            int shipDeptID = 0;
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                grhPK = Request.Params["GRHPK"] != null ? int.Parse(Request.Params["GRHPK"]) : 0;
                shipDeptID = Request.Params["ShipDeptID"] != null ? int.Parse(Request.Params["ShipDeptID"]) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetPurchaseOrderVendors(BizUnitPk, grhPK,shipDeptID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Tax Discount Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxDiscountDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemPk = 0;
            int category = 1;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.Params["ItemPk"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemPk"]);
                }
                if (Request.Params["Category"] != null)
                {
                    category = Convert.ToInt32(Request.Params["Category"]);
                }

                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorItemTaxDiscount(itemPk, category));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        //////////////////////////////////////////////////////////////////////////////New
        /// <summary>
        /// Method to fill Address Type list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetAddressTypeList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.Params["SBUPk"] != null)
                {
                    bizUnitPK = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetAddressTypeList(bizUnitPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorPOType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string CfgType = Request.Params["CfgType"] != null ? Request.Params["CfgType"].ToString() : string.Empty;
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 1;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorPOType(objUser, CfgType, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Save Vendor Bank
        /// </summary>
        /// <param name="context"></param>
        private static void SaveVendorBank(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.SaveVendorBank(requestData, objUser));

                ////
                //string vendorPk = BusinessLogic.VendorManagement.VendorMaster. .RegisterVendor(vendorDetails, objUser);
                //GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //Response.Write(vendorPk);
                ////
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// save local address
        /// </summary>
        private static void SaveVenLocalAddress(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.SaveVenLocalAddress(requestData, objUser));

                ////
                //string vendorPk = BusinessLogic.VendorManagement.VendorMaster. .RegisterVendor(vendorDetails, objUser);
                //GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //Response.Write(vendorPk);
                ////
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetAccountTypesBank(HttpContext context)
        {           
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetAccountTypesBank());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Refistration, Bank Details Tab");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Vendor Mapped Banks
        /// </summary>
        /// <Created By>Shihab</Created>
        /// <param name="context"></param>
        private static void GetVendorBanks(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int vendorID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }

                // Get the list in json string format from BL
                //Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorMaterials(vendorID));
                DataTable dtBanks = BusinessLogic.VendorManagement.VendorMaster.GetVendorBanks(objUser, 0, vendorID, 1);
                string jString = ConvertDataTabletoString(dtBanks);
                //string jString = GTIService.CommonFunctions.GetTextValueList(dtBanks, GTIService.Constants.Vendor.Fields.BANKNAME, GTIService.Constants.Vendor.Fields.BANKID);
                Response.Write(jString);

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        private static void GetVendorLocalAddress(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int vendorID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }

                // Get the list in json string format from BL
                //Response.Write(BusinessLogic.VendorManagement.VendorRegistration.GetVendorMaterials(vendorID));
                DataTable dtLocalAddress = BusinessLogic.VendorManagement.VendorMaster.GetVendorLocalAddress(objUser, 0, vendorID, 1);
                string jString = ConvertDataTabletoString(dtLocalAddress);
                //string jString = GTIService.CommonFunctions.GetTextValueList(dtBanks, GTIService.Constants.Vendor.Fields.BANKNAME, GTIService.Constants.Vendor.Fields.BANKID);
                Response.Write(jString);

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Bank By P_VBD_PK
        /// </summary>
        /// <Created By>Shihab</Created>
        /// <param name="context"></param>
        private static void GetBankDetailsById(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int vendorID = 0;
            int vbdPk = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }
                if (Request.Params["P_VBD_PK"] != null)
                {
                    vbdPk = Convert.ToInt32(Request.Params["P_VBD_PK"].ToString());
                }                
                DataTable dtBank = BusinessLogic.VendorManagement.VendorMaster.GetVendorBanks(objUser, vbdPk, vendorID, 2);
                string jString = ConvertDataTabletoString(dtBank);                
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        private static void GetVendorLocalAddressById(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int vendorID = 0;
            int vnlcpk = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].ToString());
                }
                if (Request.Params["P_VNC_LC_PK"] != null)
                {
                    vnlcpk = Convert.ToInt32(Request.Params["P_VNC_LC_PK"].ToString());
                }                
                DataTable dtBank = BusinessLogic.VendorManagement.VendorMaster.GetVendorLocalAddress(objUser, vnlcpk, vendorID, 2);
                string jString = ConvertDataTabletoString(dtBank);                
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// Delete Vendor Bank
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteVndBank(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bankId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["P_VBD_PK"] != null)
                {
                    bankId = Convert.ToInt32((Request.Params["P_VBD_PK"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.DeleteVndBank(bankId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        private static void DeleteVenLocalAddress(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vnclcId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["P_VNC_LC_PK"] != null)
                {
                    vnclcId = Convert.ToInt32((Request.Params["P_VNC_LC_PK"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorRegistration.DeleteVenLocalAddress(vnclcId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        public static string ObjectToJsonString(object obj)
        {
            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(obj);
            return jsonString;
        }

        public static string ConvertDataTabletoString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        /// <summary>
        /// Get Filter values for filling the search dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorAutoSearch(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int BizUnitPk=0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }             
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorAutoSearch(searchBy, searchValue,BizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("PO Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Filter values for filling the search dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetPOVendorAutoSearch(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int BizUnitPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                if (Request.Params["SBUPk"] != null)
                {
                    BizUnitPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetPOVendorAutoSearch(searchValue, BizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("PO Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
