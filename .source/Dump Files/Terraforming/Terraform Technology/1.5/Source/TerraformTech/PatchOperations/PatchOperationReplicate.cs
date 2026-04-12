using System.Xml;
using UnityEngine;
using Verse;

namespace TerraformTech
{
    public class PatchOperationReplicate : PatchOperationPathed
    {
        private string xpathFrom = string.Empty;
        private string xpathTo = string.Empty;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            XmlNode sourceNode, targetNode, tempNode;
            string[] partsOfTargetXPath = xpathTo.Trim('/').Split('/');

            bool result = false;
            foreach (object item in xml.SelectNodes(xpath))
            {
                result = true;
                XmlNode xmlNode = item as XmlNode;

                if (xmlNode != null)
                {
                    sourceNode = xmlNode.SelectSingleNode(xpathFrom);
                    
                    if (sourceNode != null)
                    {
                        targetNode = xmlNode.SelectSingleNode(xpathTo);
                        if (targetNode == null)
                        {
                            targetNode = xmlNode;
                            foreach (string part in partsOfTargetXPath)
                            {
                                tempNode = targetNode.SelectSingleNode(part);
                                
                                if (tempNode == null)
                                {
                                    tempNode = xml.CreateElement(part);

                                    targetNode = targetNode.AppendChild(tempNode);
                                }
                                else
                                {
                                    targetNode = tempNode;
                                }
                            }
                        }
                        
                        targetNode.InnerXml = sourceNode.InnerXml;
                    }

                    //Log.Warning(xmlNode.OuterXml);
                }
            }
            return result;
        }
    }
}
