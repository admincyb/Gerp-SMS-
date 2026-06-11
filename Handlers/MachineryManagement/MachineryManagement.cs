using System;
using System.Web;
using System.Web.SessionState;
using GTIService;


namespace Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods
        /// <summary>
        /// handiling Machinery management handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void MachineryManagement(HttpContext context)
        {
            /// <summary>
            /// Handles all the Machine Master  Requests
            /// </summary>
            /// <param name="context"></param>
            /// 
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                  //=========================== Machine Type =====================================//
                    case "GetMachineType":
                        Handlers.GetMachineType(context);
                        break;
                    case "SaveMachineType":
                        Handlers.SaveMachineType(context);
                        break;
                    case "DeleteMachineType":
                        Handlers.DeleteMachineType(context);
                        break;
                    case "GetMachineTypeDetails":
                        Handlers.GetMachineTypeDetails(context);
                        break;

                    //===========================Location ==========================================//


                    case "GetLocation":
                        Handlers.GetLocation(context);
                        break;
                    case "GetLocationList":
                        Handlers.GetLocationList(context);
                        break;
                    case "SaveLocation":
                        Handlers.SaveLocation(context);
                        break;
                    
                    case "DeleteLocation":
                        Handlers.DeleteLocation(context);
                        break;
                    case "GetLocationDetails":
                        Handlers.GetLocationDetails(context);
                        break;

                    //===========================DropDown  ==========================================//
                 
                    case "GetRunByDtls":
                        Handlers.GetRunByDtls(context);
                        break;
                    case "GetMainteanceType":
                        Handlers.GetMainteanceType(context);
                        break;
                    case "GetFreequencyType":
                        Handlers.GetFrequencyType(context);
                        break;
                    case "GetMeasuringType":
                        Handlers.GetMeasuringType(context);
                        break;

                    //========== Machine Section ==================================

                    case "SavePage":
                        Handlers.SaveMachineDetails(context);
                        break;
                   
                    case "GetMachineList":
                        Handlers.GetMachineList(context);
                        break;
                 
                    case "DeleteMachineDtls":
                        Handlers.DeleteMachineDtls(context);
                        break;
                    case "GetSearchMachineValue":
                        Handlers.GetSearchMachineValue(context);
                        break;

                    case "GetMachineDtls":
                        Handlers.GetMachineDtls(context);
                        break;


                    case "GetMachineNameByType":
                        Handlers.GetMachineNameByType(context);
                        break;
                }
            }
        }

        //============================================================ Machine Type ======================================================================
        /// <summary>
        /// Method Used to get the MachineType Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetMachineType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMachineType(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///  Save Machine Type Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMachineType(HttpContext context)
        {
            string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                string requestData = GetRequestString(context);
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.SaveMachineType(requestData, Convert.ToInt32(user), sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }

        /// <summary>
        /// Delete Machine Type Details By Machine Type ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMachineType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int machineTypeID = 0;
            try
            {
                if (Request.Params["MachineTypeID"] != null)
                {
                    machineTypeID = Convert.ToInt32((Request.Params["MachineTypeID"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.DeleteMachineTypeDtls(machineTypeID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get MachineType Details - For Listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetMachineTypeDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMachineTypeDtls(CommonFunctions.GetGridParams(Request), sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        //============================================================ Location ======================================================================
        /// <summary>
        /// Get Location Details - For Listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetLocationDetails(HttpContext context)
        {
            
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetLocationDtls(CommonFunctions.GetGridParams(Request)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



       
       
        /// <summary>
        /// Method Used to get the Location Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetLocation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPK = 0;
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPK = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetLocation(sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to get the Location Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetLocationList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPK = 0;
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPK = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetLocationList(CommonFunctions.GetGridParams(Request),sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
         /// Save Location Details 
         /// </summary>
         /// <param name="context"></param>
        private static void SaveLocation(HttpContext context)
        {
            string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.SaveLocation(requestData, Convert.ToInt32(user)));
                  
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }
       
        /// <summary>
        /// Delete Location Details By LocationID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteLocation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int locID = 0;
            try
            {
                if (Request.Params["locID"] != null)
                {
                    locID = Convert.ToInt32((Request.Params["locID"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.DeleteLocationDtls(locID));
            }
            catch (Exception ex)
            {
                
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        
        /// <summary>
        /// Get Maintenance Type Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetMainteanceType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMainteanceType(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get RunBy Details 
        /// </summary>
        /// <param name="context"></param>
        private static void GetRunByDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetRunByDtls(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
       
        /// <summary>
        /// Get FreequncyType
        /// </summary>
        /// <param name="context"></param>
        private static void GetFrequencyType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetFrequencyType(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetMeasuringType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            int Active = 0;
            string CfgType = string.Empty;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                if (Request.Params["Active"] != null)
                {
                    Active = Convert.ToInt32(Request.Params["Active"]);
                }
                if (Request.Params["CfgType"] != null)
                {
                    CfgType = Request.Params["CfgType"].ToString();
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMeasuringType(sBU, Active,  CfgType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

       
       
        // ================================================== Machine =========================================================================
        /// <summary>
        /// Save Machine Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMachineDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string machineDetails = GetRequestString(context);
                string machinePK = BusinessLogic.MachineryManagement.MachineryMaster.SaveMachineDetails(machineDetails);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(machinePK);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }

        }
        
        /// <summary>
        /// Get Machine List - For Listing All Details uin grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetMachineList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            int StatusPK=1;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                if (Request.Params["StatusPK"] != null)
                {
                    StatusPK = Convert.ToInt32(Request.Params["StatusPK"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMachineList(CommonFunctions.GetGridParams(Request), sBU, StatusPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Seqarch Machine Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchMachineValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sBU = 0;
            try
            {

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBU"] != null)
                {
                    sBU = Convert.ToInt32(Request.Params["SBU"]);
                }
                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetSearchMachineValues(searchBy, searchValue,sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Machine Details By Machine ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetMachineDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int machinePK = 0;
            try
            {
                if (Request.Params["machinePK"] != null)
                {
                    machinePK = Convert.ToInt32((Request.Params["machinePK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMachineDetails(machinePK));
            }
            catch (Exception ex)
            {
               
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Delete Machine Details By MachineID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMachineDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int machineID = 0;
            try
            {
                if (Request.Params["MachineID"] != null)
                {
                    machineID = Convert.ToInt32((Request.Params["MachineID"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.DeleteMachineDtls(machineID));
            }
            catch (Exception ex)
            {
                
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }



        private static void GetMachineNameByType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sBU = 0;
            int machType = 0;
            int? processID = null;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Request.Params["MachType"] != null)
                {
                    machType = Convert.ToInt32(Request.Params["MachType"]);
                }
                if (Request.Params["ProcessID"] != null)
                {
                    processID = Convert.ToInt32(Request.Params["ProcessID"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MachineryManagement.MachineryMaster.GetMachineNameByMachineType(objUser.SBUID, machType,processID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
       
        #endregion
       
    }
}