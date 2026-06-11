using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GTIService;
using System.Web.SessionState;
namespace Handlers
{

    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
     private static void StoreMaterialMapping(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveMaterialMappingDetails":
                        Handlers.SaveMaterialMappingDetails(context);
                        break;
                    case "GetMaterialMappingDetails":
                        Handlers.GetMaterialMappingDtls(context);
                        break;
                    case "GetMaterialMappingList":
                        Handlers.GetMaterialMappingList(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetMaterialMappingSearchValues(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Menu Group Details
        /// </summary>
        /// <param name="context"></param>
     private static void GetMaterialMappingDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
               // int menuPK = Request.Params["deptID"] != null ? Convert.ToInt32(Request.Params["deptID"]) : 0;
                int mapParentPK = Request.Params["MapParentID"] != "null" ? Convert.ToInt32(Request.Params["MapParentID"]) : 0;
                int bizUnit = Request.Params["SBUPk"] != "null" ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int store = Request.Params["StoreID"] != null ? Convert.ToInt32(Request.Params["StoreID"]) : 0;
                string pVal = Request.Params["pVal"] != null ? Request.Params["pVal"] : "null";
                int type = Request.Params["Type"] != null ? Convert.ToInt32(Request.Params["Type"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.StoreMaterialMapping.GetMaterialMappingDtls(mapParentPK, bizUnit, store, pVal, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
     private static void SaveMaterialMappingDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.StoreMaterialMapping.SaveMaterialMappingDetails(GetRequestString(context)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
     /// <summary>
     ///  Get Material Mapping List List based search results
     /// </summary>
     /// <param name="context"></param>
     private static void GetMaterialMappingList(HttpContext context)
     {
         // Create the request and response objects from context
         HttpRequest Request = context.Request;
         HttpResponse Response = context.Response;
         int sbuPk = 0;
         try
         {
             // clear all the response
             GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
             if (Request.Params["SBUPk"] != null)
             {
                 sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
             }
             // Get the list in json string format from BL
             Response.Write(BusinessLogic.Administration.Masters.StoreMaterialMapping.GetMaterialMappingList(CommonFunctions.GetGridParams(Request), sbuPk));
         }
         catch (Exception ex)
         {
             NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
             logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
         }
     }
     /// <summary>
     /// Search Material Details , and Get Data Related to Search Criteria
     /// </summary>
     /// <param name="context"></param>
     private static void GetMaterialMappingSearchValues(HttpContext context)
     {
         // Create the request and response objects from context
         HttpRequest Request = context.Request;
         HttpResponse Response = context.Response;
         string searchBy = string.Empty;
         string searchValue = string.Empty;
         int sbuPk = 0;
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
             if (Request.Params["SBUPk"] != null)
             {
                 sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
             }
             Response.Write(BusinessLogic.Administration.Masters.StoreMaterialMapping.GetMaterialMapSearchValues(searchBy, searchValue, sbuPk));
         }
         catch (Exception ex)
         {
             NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
             logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
         }
     }
    }
}
