using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ERPSMS_v01.Handlers
{
    /// <summary>
    /// Summary description for DownloadHandler
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            var request = context.Request;
            Stream stream = null;
            var buffer = new byte[2048];
            var fileName = request["FileName"];

            if (string.IsNullOrEmpty(fileName))
                throw new Exception("File name is Empty");

            var fullFileName = Path.Combine(string.Format("{0}",
                request.PhysicalApplicationPath), "DownloadFiles", fileName);
            try
            {
                stream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var dataToRead = stream.Length;
                while (dataToRead > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        var length = stream.Read(buffer, 0, 2048);
                        context.Response.OutputStream.Write(buffer, 0, length);
                        context.Response.Flush();
                        buffer = new byte[2048];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}