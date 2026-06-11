using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class ShowFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["fPath"] != null)
                {
                    string path = Server.MapPath("~\\" + Request.QueryString["fPath"]);
                    FileInfo finfo = new FileInfo(path);
                    if (finfo.Exists)
                    {
                        //Response.Redirect(path);
                        Response.ContentType = GTIService.CommonFunctions.GetContentType(finfo.Extension);// "application/ms-word";
                        Response.AddHeader("content-disposition", "attachment; filename=erp" + finfo.Extension);
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
                        Response.Write("File No Found");

                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}