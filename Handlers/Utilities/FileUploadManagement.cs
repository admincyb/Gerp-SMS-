using System;

using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Text;
using System.Reflection;
using System.ComponentModel;


namespace Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void UploadFiles(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "UPLOADFILE":
                        Handlers.UploadFile(context);
                        break;
                    case "DELETEFILE":
                        Handlers.DeleteUploadedFile(context);
                        break;
                    case "SAVEFILE":
                        Handlers.SaveFile(context);
                        break;

                    case "DOWNLOADFILE":

                        Handlers.DownloadFile(context);
                        break;
                }
            }
        }
        

        /// <summary>
        /// Upload File 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveFile(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string fileDetails = GetRequestString(context);
                string status = BusinessLogic.CommonManagement.CommonManagement.SaveFileDtls(fileDetails);
                //GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                //Response.Write(status);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("File Upload");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Upload File 
        /// </summary>
        /// <param name="context"></param>
        private static void UploadFile(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                if (Request.Files.Count > 0)
                {

                    GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                    // get Uploded Folder Name
                    string uploadFolder = GTIService.Constants.Common.FileUpload.TEMPFOLDER;
                    string uploadPath = Request.MapPath("~/") + GTIService.Constants.Common.FileUpload.UPLOADURL + uploadFolder;
                    // Get Name Of the Uploaded File , without Extension
                    FileInfo file = new FileInfo(Request.Files[0].FileName);
                    string title = file.Name.Split('.')[0];
                    // Name of the Title Control / Div Id For Listing
                    string titleCntrl = Request.QueryString["TitleCntrl"];
                    // name of the Id Capturing Control / Div Storage Name
                    string fileCntrl = Request.QueryString["FileIDCntrl"];

                    string page = Request.QueryString["Page"];

                    // Get Extension of the File
                    string ext = "." + Request.Files[0].FileName.Split('.')[Request.Files[0].FileName.Split('.').Length - 1].ToString();
                    string fileStatus = GTIService.CommonFunctions.checkValidFileType(ext);
                    // Set New Name For uploaded File
                    string actName = Guid.NewGuid().ToString();
                    GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                    if (fileStatus != "Invalid")
                    {
                        StringBuilder sbScript = new StringBuilder();
                        sbScript.Clear();
                        sbScript.Append("<script type='text/javascript'>");
                        //parent to refer the iframe's parent window 
                        sbScript.Append("window.top.window.GrandScriptUtils.ShowUploadStatus(\"");
                        sbScript.Append(Request.Files[0].FileName.Substring(Request.Files[0].FileName.LastIndexOf("\\") + 1) + "\",\"");
                        sbScript.Append(Request.QueryString["Type"] + "\",\"");
                        sbScript.Append(actName + ext + "\",\"");
                        sbScript.Append(title + "\" ,\"" + fileCntrl + "\" ,\"" + titleCntrl + "\" ,\"" + ext + "\" , \"" + GetDownLoadPath(page) + "\" );");
                        sbScript.Append("</script>");
                        // Check Folder Already Exists Or Not
                        if (BusinessLogic.CommonManagement.CommonManagement.CheckFolderExists(uploadPath))
                        {
                            // Save File
                            Request.Files[0].SaveAs(uploadPath + "//" + actName + ext);
                            Response.Write(sbScript);

                        }
                        else
                        {
                            // if Folder Does not Exists - Create Folder 
                            if (BusinessLogic.CommonManagement.CommonManagement.CreateFolder(Request.MapPath("~/") + GTIService.Constants.Common.FileUpload.UPLOADURL, uploadFolder))
                            {
                                // Save File
                                Request.Files[0].SaveAs(uploadPath + "//" + actName + ext);
                                Response.Write(sbScript);

                            }
                            else
                            {
                                // No Permission to Create Folder
                                Response.Write(FileErrorMsg("No Permission to create folder"));

                            }
                        }
                    }
                    else
                    {
                        // For Invalid Files
                        Response.Write(FileErrorMsg("Invalid File"));

                    }

                }
                else
                {
                    // Please Select File
                    Response.Write(FileErrorMsg("Please Select File"));

                }
            }
            catch (Exception ex)
            {
                 NLog.Logger logger = NLog.LogManager.GetLogger("File Upload");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            
            }

        }

        private static string GetDownLoadPath(string page)
        {
            string path = string.Empty;
            if (page == "Machine" || page == "Material")
            {
                path = "../../DwnloadFile.aspx?fPath=";
            }
            else if (page == "Vendor")
            {
                path = "../DwnloadFile.aspx?fPath=";
            }
            else if (page == "PO")
            {
                path = "../DwnloadFile.aspx?fPath=";
            }
            else if (page == "GIN")
            {
                path = "../DwnloadFile.aspx?fPath=";
            }
            else if (page == "GRN")
            {
                path = "../DwnloadFile.aspx?fPath=";
            }
            else if (page == "PR")
            {
                path = "../DwnloadFile.aspx?fPath=";
            }
            return path;
        }

        /// <summary>
        /// To Set Error Message When File Upload
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static StringBuilder FileErrorMsg(string msg)
        {
            StringBuilder sbScript = new StringBuilder();

            sbScript.Clear();
            sbScript.Append("<script type='text/javascript'>");
            sbScript.Append("window.top.window.GrandScriptUtils.FileMsg(\"" + msg + "\");");
            sbScript.Append("</script>");
            return sbScript;

        }

        /// <summary>
        /// Delete File
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteUploadedFile(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            string FileName = Request.QueryString["FileName"];
            string FileID = Request.QueryString["FileID"];

            BusinessLogic.CommonManagement.CommonManagement.DeleteFileFromFolder(FileID);
            GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
            context.Response.Write("{\"FileName\":\"" + Request.QueryString["FileName"] + "\",\"Type\":\"" + Request.QueryString["Type"] + "\"}");
        }
       
        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="context"></param>
        private static void DownloadFile(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            string FileID = Request.QueryString["FileID"];

            string FullFilePath = context.Server.MapPath("~/" + FileID);
            FileInfo file = new FileInfo(FullFilePath);
            if (file.Exists)
            {
                context.Response.ContentType = "image/pjpeg";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.WriteFile(file.FullName);
                context.Response.End();
            }

        }
    

    }
}