using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace CustomActions
{
    public partial class CustomActions
    {
        [CustomAction]
        public static ActionResult UpdateRevitManifestFiles(Session session)
        {
            string[] versions = new string[] { "2017", "2018", "2019", "2020", "2021", "2022", "2023" };

            string mainManifestFileDirectory = Modify.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Autodesk", "Revit", "Addins");

            //MessageBox.Show(string.Join(", ", session.CustomActionData.Keys));

            if (!session.CustomActionData.TryGetValue("INSTALLDIR", out string installDirectory) || string.IsNullOrEmpty(installDirectory) || !System.IO.Directory.Exists(installDirectory))
            {
                return ActionResult.Failure;
            }

            //MessageBox.Show(installDirectory);

            if (!session.CustomActionData.TryGetValue("MANIFESTFILENAME", out string manifestFileName) || string.IsNullOrEmpty(manifestFileName))
            {
                return ActionResult.Failure;
            }

            //MessageBox.Show(manifestFileName);

            if (!session.CustomActionData.TryGetValue("ASSEMBLYNAME", out string assemblyName) || string.IsNullOrEmpty(assemblyName))
            {
                return ActionResult.Failure;
            }

            if(session.CustomActionData.TryGetValue("VERSIONS", out string versions_Custom) && !string.IsNullOrWhiteSpace(versions_Custom))
            {
                List<string> versionList = versions_Custom.Split(',')?.ToList();
                if(versionList != null)
                {
                    for (int i = versionList.Count - 1; i >= 0; i--)
                    {
                        string version = versionList[i]?.Trim();

                        if(string.IsNullOrWhiteSpace(version))
                        {
                            versionList.RemoveAt(i);
                            continue;
                        }

                        versionList[i] = version;
                    }

                    if(versionList != null && versionList.Count != 0)
                    {
                        versions = versionList.ToArray();
                    }
                }
            }

            //MessageBox.Show(string.Join(", ", versions));

            foreach (string version in versions)
            {
                string manifestFilePath = System.IO.Path.Combine(mainManifestFileDirectory, version, manifestFileName);
                if (!System.IO.File.Exists(manifestFilePath))
                {
                    continue;
                }

                string assemblyPath = System.IO.Path.Combine(installDirectory, string.Format("Revit {0}", version), assemblyName);
                if (!System.IO.File.Exists(assemblyPath))
                {
                    continue;
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(manifestFilePath);

                XmlNode xmlNode = xmlDocument.SelectSingleNode("/RevitAddIns/AddIn/Assembly");
                if(xmlNode == null)
                {
                    continue;
                }

                xmlNode.InnerText = assemblyPath;
                xmlDocument.Save(manifestFilePath);
            }

            return ActionResult.Success;
        }
    }
}
