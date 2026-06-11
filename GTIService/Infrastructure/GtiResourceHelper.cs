using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Web;
using System.Configuration;
using System.Resources;

namespace GTIService.Infrastructure
{
    public class GtiResourceHelper
    {
        public static IDictionary GetResources(string virtualPath, string className, string cultureName,
            bool designMode, IServiceProvider serviceProvider)
        {
            Dictionary<string, object> result = null;

            string resFileName = HttpContext.Current.Server.MapPath(virtualPath);
            
            string clientSufix = String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClientResourceSuffix"])
                                    ? string.Empty
                                    : "." + ConfigurationManager.AppSettings["ClientResourceSuffix"];

            string cultureCode = string.IsNullOrEmpty(cultureName)
                                   ? string.Empty
                                   : "." + cultureName;

            string rootDirectory = !string.IsNullOrEmpty(className)
                                ? HttpContext.Current.Server.MapPath("~")
                                : Path.GetDirectoryName(resFileName);
            string pageName = Path.GetFileName(resFileName);
            string resPath = string.Empty;

            //Test Code
            //============================
            resPath =  FindResourceFileName(virtualPath, className, cultureCode,clientSufix);

            bool useOldCode = false;

            #region Old Code
            if (useOldCode)
            {
                if (string.IsNullOrEmpty(className))
                {
                    #region Local Resource
                    //Default.aspx.Gti.fr.resx
                    resPath = string.Format("{0}\\App_LocalResources\\{1}{2}{3}.resx",
                                                   rootDirectory,
                                                   pageName,
                                                   clientSufix,
                                                   cultureCode
                                                   );

                    if (!File.Exists(resPath))
                    {
                        //Default.aspx.Gti.resx
                        resPath = string.Format("{0}\\App_LocalResources\\{1}{2}.resx",
                                        rootDirectory,
                                        pageName,
                                        clientSufix
                                        );
                        if (!File.Exists(resPath))
                        {
                            //Default.aspx.fr.resx
                            resPath = string.Format("{0}\\App_LocalResources\\{1}{2}.resx",
                                       rootDirectory,
                                       pageName,
                                       cultureCode
                                       );
                            if (!File.Exists(resPath))
                            {
                                //Default.aspx.resx
                                resPath = string.Format("{0}\\App_LocalResources\\{1}.resx",
                                        rootDirectory,
                                        pageName
                                        );
                                if (!File.Exists(resPath))
                                {
                                    resPath = string.Format("{0}\\UserControls\\App_LocalResources\\{1}.resx",
                                       rootDirectory,
                                       pageName
                                       );
                                    if (!File.Exists(resPath))
                                    {
                                        resPath = string.Format("{0}\\UserControls\\App_LocalResources\\{1}.resx",
                                                  HttpContext.Current.Server.MapPath("~"),
                                                  pageName
                                                  );
                                        if (!File.Exists(resPath))
                                        {
                                            resPath = string.Format("{0}\\Journalize\\UserControls\\App_LocalResources\\{1}.resx",
                                                HttpContext.Current.Server.MapPath("~"),
                                                pageName
                                                );
                                            if (!File.Exists(resPath))
                                                return result;
                                            //throw new FileNotFoundException(string.Format("'{0}' is not a found.", resPath));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Global Resource
                    resPath = string.Format("{0}\\App_GlobalResources\\languages\\FALLBACK\\{1}{2}{3}.resx",
                                                          rootDirectory,
                                                          className,
                                                          clientSufix,
                                                          cultureCode
                                                          );

                    if (!File.Exists(resPath))
                    {
                        resPath = string.Format("{0}\\App_GlobalResources\\languages\\FALLBACK\\{1}{2}.resx",
                                                  rootDirectory,
                                                  className,
                                                  clientSufix
                                                  );
                        if (!File.Exists(resPath))
                        {
                            resPath = string.Format("{0}\\App_GlobalResources\\languages\\FALLBACK\\{1}.resx",
                                                      rootDirectory,
                                                      className
                                                      );
                            if (!File.Exists(resPath))
                                return result;
                            //throw new FileNotFoundException("Resource file not found");
                        }
                    }
                    #endregion
                }
            } 
            #endregion

            if (string.IsNullOrEmpty(resPath)) return result;

            XmlDocument xmlSettingDoc = new XmlDocument();

            xmlSettingDoc.Load(resPath);

            string XPathQuery = "//data/value";

            XmlNodeList xList = xmlSettingDoc.SelectNodes(XPathQuery);

            result = new Dictionary<string, object>();
            foreach (XmlNode node in xList)
            {
                var keyName = node.ParentNode.Attributes["name"].Value.ToLower();
                result.Add(keyName, node.InnerText);
            }

            return result;
        }

        public static string FindResourceFileName(string virtualPath, string className, string cultureCode, string clientSufix)
        {
            /*
            string p1 = HttpContext.Current.Request.PhysicalApplicationPath;
            string p2 = HttpContext.Current.Request.PhysicalPath;
            string p3 = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            */

            string resourceFilePath = string.Empty;
            string rootPath = HttpContext.Current.Request.PhysicalApplicationPath;

            if (string.IsNullOrEmpty(className))
            {
                //Local
                string requestedPagePath = HttpContext.Current.Request.PhysicalPath;
                string requestedPageName = requestedPagePath.Substring(requestedPagePath.LastIndexOf('\\')+1);
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
                        resourceFilePath =  string.Format("{0}{1}{2}{3}.resx", path, virtualPath, clientSufix, cultureCode);
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
                    resourceFilePath = string.Format(@"{0}\App_LocalResources\{1}{2}{3}.resx", requestedPageFolder,virtualPath,clientSufix, cultureCode);
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
            }
            else
            {
                //Global
                if(clientSufix != null && clientSufix.StartsWith(".")) clientSufix = clientSufix.Substring(1); // remove "." from ClientSufix for Global resource
                string globalResourceRootFolder = string.Format(@"{0}App_GlobalResources\languages\", rootPath);
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
                    resourceFilePath = string.Format(@"{0}FALLBACK\{1}.resx", globalResourceRootFolder, className);
                    if (File.Exists(resourceFilePath))
                        return resourceFilePath;
                }
                else{
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
