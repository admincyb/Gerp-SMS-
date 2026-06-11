using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Gti.Infrastructure.ResourceManagement
{
    /// <summary>
    /// Local Resource Helper
    /// </summary>
    public class GtiLocalResourceHelper
    {
        public static IDictionary GetResources(string virtualPath, string cultureName,
           bool designMode, IServiceProvider serviceProvider)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            string resFileName = HttpContext.Current.Server.MapPath(virtualPath);

            string clientSufix = String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientResourceSuffix"])
                                    ? string.Empty
                                    : "." + ConfigurationManager.AppSettings["ClientResourceSuffix"];

            string cultureCode = string.IsNullOrEmpty(cultureName)
                                   ? string.Empty
                                   : "." + cultureName;

            string rootDirectory = Path.GetDirectoryName(resFileName);
            string pageName = Path.GetFileName(resFileName);
            string resPath = string.Empty;

            resPath = FindResourceFileName(virtualPath, cultureCode, clientSufix);

            if (string.IsNullOrEmpty(resPath)) return result;

            XmlDocument xmlSettingDoc = new XmlDocument();

            xmlSettingDoc.Load(resPath);

            string XPathQuery = "//data/value";

            XmlNodeList xList = xmlSettingDoc.SelectNodes(XPathQuery);

            foreach (XmlNode node in xList)
            {
                var keyName = node.ParentNode.Attributes["name"].Value.ToLower();
                result.Add(keyName, node.InnerText);
            }

            return result;

        }

        public static string FindResourceFileName(string virtualPath, string cultureCode, string clientSufix)
        {
            /*
            string p1 = HttpContext.Current.Request.PhysicalApplicationPath;
            string p2 = HttpContext.Current.Request.PhysicalPath;
            string p3 = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            */

            string resourceFilePath = string.Empty;
            string rootPath = HttpContext.Current.Request.PhysicalApplicationPath;
            string requestedPagePath = HttpContext.Current.Request.PhysicalPath;
            string requestedPageName = requestedPagePath.Substring(requestedPagePath.LastIndexOf('\\') + 1);
            string requestedPageFolder = requestedPagePath.Substring(0, requestedPagePath.LastIndexOf('\\'));

            bool isUserControl = virtualPath.EndsWith(".ascx");

            if (isUserControl)
            {
                #region UserControl Resource Finding
                List<string> userControlLocations = new List<string>();

                userControlLocations.Add(string.Format(@"{0}\UserControls\App_LocalResources\", requestedPageFolder));
                userControlLocations.Add(string.Format(@"{0}\UserControls\App_LocalResources\", rootPath));
                userControlLocations.Add(string.Format(@"{0}\Journalize\UserControls\App_LocalResources\", rootPath));


                foreach (string path in userControlLocations)
                {
                    if (!string.IsNullOrEmpty(cultureCode))
                    {
                        resourceFilePath = string.Format(@"{0}{1}\{2}{3}{4}.resx", path, cultureCode.Remove(0, 1), virtualPath, clientSufix, cultureCode);
                        if (File.Exists(resourceFilePath))
                            return resourceFilePath;

                        resourceFilePath = string.Format(@"{0}{1}\{2}{3}.resx", path, cultureCode.Remove(0, 1), virtualPath, cultureCode);
                        if (File.Exists(resourceFilePath))
                            return resourceFilePath;
                    }
                    resourceFilePath = string.Format("{0}{1}{2}{3}.resx", path, virtualPath, clientSufix, cultureCode);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    resourceFilePath = string.Format("{0}{1}{2}.resx", path, virtualPath, clientSufix);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    resourceFilePath = string.Format("{0}{1}{2}.resx", path, virtualPath, cultureCode);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    resourceFilePath = string.Format("{0}{1}.resx", path, virtualPath);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                }
                #endregion
            }
            else
            {
                #region Page Resource Finding
                if (!string.IsNullOrEmpty(cultureCode))
                {
                    resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}\{2}{3}{4}.resx", requestedPageFolder, cultureCode.Remove(0, 1), virtualPath, clientSufix, cultureCode);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}\{2}{3}.resx", requestedPageFolder, cultureCode.Remove(0, 1), virtualPath, cultureCode);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;
                }
                resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}{2}{3}.resx", requestedPageFolder, virtualPath, clientSufix, cultureCode);
                if (File.Exists(resourceFilePath))
                    return resourceFilePath;

                resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}{2}.resx", requestedPageFolder, virtualPath, clientSufix);
                if (File.Exists(resourceFilePath))
                    return resourceFilePath;

                resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}{2}.resx", requestedPageFolder, virtualPath, cultureCode);
                if (File.Exists(resourceFilePath))
                    return resourceFilePath;

                resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}.resx", requestedPageFolder, virtualPath);
                if (File.Exists(resourceFilePath))
                    return resourceFilePath;
                #endregion
            }
            return string.Empty;
        }
    }
}
