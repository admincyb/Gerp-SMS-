using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Xml;

namespace Gti.Infrastructure.ResourceManagement
{
    /// <summary>
    /// Global Resource Helper
    /// </summary>
    public class GtiGlobalResourceHelper
    {
        public static IDictionary GetResources(string className, string cultureName,
          bool designMode, IServiceProvider serviceProvider)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            string clientSufix = String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientResourceSuffix"])
                                    ? string.Empty
                                    : "_" + ConfigurationManager.AppSettings["ClientResourceSuffix"];

            string cultureCode = string.IsNullOrEmpty(cultureName)
                                   ? string.Empty
                                   : "." + cultureName;

            string rootDirectory = HttpContext.Current.Server.MapPath("~");
            string resPath = string.Empty;

            resPath = FindResourceFileName(className, cultureCode, clientSufix);


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

        public static string FindResourceFileName(string className, string cultureCode, string clientSufix)
        {
            string resourceFilePath = string.Empty;
            string rootPath = HttpContext.Current.Request.PhysicalApplicationPath;

            if (!string.IsNullOrEmpty(className))
            {
                //Global
                if (clientSufix != null && clientSufix.StartsWith("_")) clientSufix = clientSufix.Substring(1); // remove "_" from ClientSufix for Global resource
                string globalResourceRootFolder = string.Format(@"{0}\App_GlobalResources\languages\", rootPath);

                string cultureBasedResourceRootFolder = string.IsNullOrEmpty(cultureCode)
                                                        ? globalResourceRootFolder
                                                        : string.Format(@"{0}{1}", globalResourceRootFolder, cultureCode.Remove(0, 1));

                if (Directory.Exists(cultureBasedResourceRootFolder))
                {
                    if (!string.IsNullOrEmpty(cultureCode))
                    {
                        resourceFilePath = string.Format(@"{0}\{1}{2}{3}.resx", cultureBasedResourceRootFolder, className, clientSufix, cultureCode.Replace('.', '_'));
                        if (File.Exists(resourceFilePath))
                            return resourceFilePath;
                        resourceFilePath = string.Format(@"{0}\{1}{2}.resx", cultureBasedResourceRootFolder, className, cultureCode.Replace('.', '_'));
                        if (File.Exists(resourceFilePath))
                            return resourceFilePath;
                    }
                    resourceFilePath = string.Format(@"{0}\{1}{2}.resx", cultureBasedResourceRootFolder, className, clientSufix);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    resourceFilePath = string.Format(@"{0}\{1}.resx", cultureBasedResourceRootFolder, className);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;
                }
                else
                {
                    resourceFilePath = string.Format(@"{0}FALLBACK\{1}_{2}.resx", globalResourceRootFolder, className, clientSufix);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;

                    //className
                    resourceFilePath = string.Format(@"{0}FALLBACK\{1}.resx", globalResourceRootFolder, className);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;
                }
            }

            return string.Empty;
        }
    }
}
