using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.SessionState;

namespace Handlers
{
    public partial class Handlers : IHttpHandler,IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Method Used to Handle all the process request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            { 
                string ReqeustPath = Request.Path.Substring(Request.Path.LastIndexOf("/")+1);
               switch (ReqeustPath)
                {

                    case "UserManagement.do":
                        Handlers.UserManagement(context);
                        break;
                    case "ProjectSite.do":
                        Handlers.ProjectSiteManagement(context);
                        break;
                    case "MachineryManagement.do":
                        Handlers.MachineryManagement(context);
                        break;
                    case "MaterialCategory.do":
                        Handlers.MaterialCategoryManagement(context);
                        break;
                    case "MaterialConsumption.do":
                        Handlers.MaterialConsumption(context);
                        break;
                    case "ExternalMaterialIssue.do":
                        Handlers.ExternalMaterialIssue(context);
                        break;

                    case "UOMManagement.do":
                        Handlers.UOMMaster(context);
                        break;
                    case "DispersionManagement.do":
                        Handlers.DispersionManagement(context);
                        break;
                    case "MaterialManagement.do":
                        Handlers.MaterialManagement(context);
                        break;
                    case "StoreManagement.do":
                        Handlers.StoreManagement(context);
                        break;
                    case "NewItemRequest.do":
                        Handlers.NewItemRequest(context);
                        break; 
                    case "VendorManagement.do":
                        Handlers.VendorManagement(context);
                        break;
                    case "VendorEvaluationManagement.do":
                        Handlers.VendorEvaluationManagement(context);
                        break;
                    case "VendorTermsManagement.do":
                        Handlers.VendorTermsManagement(context);
                        break;
                    case "InboxManagement.do":
                        Handlers.InboxManagement(context);
                        break;
                    case "FileUploadHandler.do":
                        Handlers.UploadFiles(context);
                        break;
                    case "AdvanceSearch.do":
                        Handlers.AdvanceSearch(context);
                        break;
                    case "MenuManagement.do":
                        Handlers.MenuManagement(context);
                        break;
                    case "CommonManagement.do":
                        Handlers.CommonMaster(context);
                        break;
                    case "GetType.do":
                         Handlers.CommonMaster(context);
                        break;
                    case "StoreRequisitionSlip.do":
                        Handlers.StoreRequisitionSlip(context);
                        break;
                    case "SubDepartmentManagement.do":
                        Handlers.SubDepartmentManagement(context);
                        break;
                    case "SBUConfiguration.do":
                        Handlers.SBUConfiguartion(context);
                        break;
                    case "VendorRegistration.do":
                        Handlers.VendorRegistration(context);
                        break;
                    case "DepartmentConfig.do":
                        Handlers.DepartmentConfig(context);
                        break;
                    case "GeneralTemplateMaster.do":
                        Handlers.GeneralTemplateMaster(context);
                        break;
                    case "DefaultValueConfig.do":
                        Handlers.DefaultValueConfig(context);
                        break;
                    case "DepartmentSettingsConfig.do":
                        Handlers.DepartmentSettings(context);
                        break;
                    case "SubDepartment.do":
                        Handlers.SubDepartmentManagement(context);
                        break;
                    //Vineeth For Tax Settings 04/Apr/2011
                    case "TaxSettings.do":
                        Handlers.TaxSettings(context);
                        break;
                    case "UserGroupDepartment.do":
                        Handlers.UserGroupDepartment(context);
                        break;
                    case "POGeneration.do":
                        Handlers.CreatePurchaseOrder(context);
                        break;

                    case "PurchaseRequest.do":
                        Handlers.PurcahseRequestManagement(context);
                        break;
                    case "GoodsReceiptNote.do":
                        Handlers.GoodsReceiptNoteManagement(context);
                        break;
                    case "TankManagement.do":
                        Handlers.TankManagement(context);
                        break;
                    case "StoreMaterialMapping.do":
                        Handlers.StoreMaterialMapping(context);
                        break;
                    case "BinCardGeneration.do":
                        Handlers.BinCardGeneration(context);
                        break;
                    case "CompoundMaster.do":
                        Handlers.CompoundMaster(context);
                        break;
                    case "MaterialIssue.do":
                        Handlers.MaterialIssue(context);
                        break;
                    case "StoreAuditManagement.do":
                        Handlers.StoreAuditManagement(context);
                        break;
                    case "DispersionPreparation.do":
                        Handlers.DispersionPreparation(context);
                        break;
                    case "CompoundPreparation.do":
                        Handlers.CompoundPreparation(context);
                        break;

                    case "StoreAdjustment.do":
                        Handlers.CompoundPreparation(context);
                        break;

                    case "GoodsInspectionNote.do":
                        Handlers.GoodsInspectionNoteManagement(context);
                        break;
                    case "StockAdjustmentManagement.do":
                        Handlers.StockAdjustmentManagement(context);
                        break;
                    case "CurrencyManagement.do":
                        Handlers.CurrencyManagement(context);
                        break;
                    case "TopUpManagement.do":
                        Handlers.TopUpManagement(context);
                        break;
                    case "MaterialAccept.do":
                        Handlers.MaterialAcceptManagement(context);
                        break;

                    case "PurchaseOrderGenerate.do":
                        Handlers.PurchaseOrderManagement(context);
                        break;
                    case "StockTransfer.do":
                        Handlers.StockTranferManagement(context);
                        break;
                    case "AgentRegistration.do":
                        Handlers.AgentRegistration(context);
                        break;
                    case "DirectStockTransfer.do":
                        Handlers.DirectStockTransferManagement(context);
                        break;
                    case "StoreLocationMaster.do":
                        Handlers.StoreLocationManagement(context);
                        break;
                        
                }
            }
        }
       

        /// <summary>
        /// Method Used to get the Request string
        /// </summary>
        /// <param name="context"></param>
        private static string GetRequestString(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            //HttpFileCollection hfc = Request.Files;
            //for (int i = 0; i < hfc.Count; i++)
            //{
            //    HttpPostedFile hpf = hfc[i];
            //    if (hpf.ContentLength > 0)
            //    {
            //        hpf.SaveAs(Request.MapPath("Test") + "\\" +
            //          System.IO.Path.GetFileName(hpf.FileName));
            //        Response.Write("<b>File: </b>" + hpf.FileName + " <b>Size:</b> " +
            //            hpf.ContentLength + " <b>Type:</b> " + hpf.ContentType + " Uploaded Successfully <br/>");
            //    }
            //}


            var bytes = new byte[Request.InputStream.Length];
            Request.InputStream.Read(bytes, 0, bytes.Length);
            string reqString =  HttpUtility.UrlDecode(System.Text.Encoding.UTF8.GetString(bytes));
            reqString = reqString.Replace("'[", "[");
            reqString = reqString.Replace("]'", "]");
            return reqString;

        }
    }
}