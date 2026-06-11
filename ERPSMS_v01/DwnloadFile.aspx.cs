using System;
using System.IO;

namespace ERPSMS_v01
{
    public partial class DwnloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["fPath"] != null)
                {
                    string path = Server.MapPath("~\\" + Request.QueryString["fPath"]);
                    string fileName = string.Empty;
                    if (Request.QueryString["Title"] != null)
                    {
                        fileName = Request.QueryString["Title"].ToString();
                    }
                    else
                    {
                        fileName = "Latexerp";
                    }
                    FileInfo finfo = new FileInfo(path);
                    if (finfo.Exists)
                    {
                        //Response.Redirect(path);
                        Response.ContentType = GTIService.CommonFunctions.GetContentType(finfo.Extension);// "application/ms-word";
                        Response.AddHeader("content-disposition", "attachment; filename=" + fileName + finfo.Extension);
                        FileStream sourceFile = new FileStream(path, FileMode.Open);
                        long FileSize;
                        FileSize = sourceFile.Length;
                        byte[] getContent = new byte[(int)FileSize];
                        sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                        sourceFile.Close();

                        Response.BinaryWrite(getContent);
                    }
                    else
                    {
                        Response.Write("File Not Found");

                    }

                }
            }
            catch (Exception ex)
            {

            }
  
        }
    }
}