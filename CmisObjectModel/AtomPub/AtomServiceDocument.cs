using System.Collections.Generic;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;

namespace CmisObjectModel.AtomPub
{
    /// <summary>
   /// Represents a cmis service document (response on a repositoryInfo-request)
   /// </summary>
   /// <remarks></remarks>
    public class AtomServiceDocument : sss.ServiceDocument
    {

        public AtomServiceDocument() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomServiceDocument(params AtomWorkspace[] workspaces) : this()
        {

            if (workspaces is not null)
            {
                foreach (AtomWorkspace workspace in workspaces)
                {
                    // omit duplicate namespace definitions
                    foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                        workspace.AttributeExtensions.Remove(de.Key);
                    Workspaces.Add(workspace);
                }
            }
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomServiceDocument CreateInstance(sx.XmlReader reader)
        {
            bool isEmptyElement;
            var retVal = new AtomServiceDocument();

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                reader.MoveToContent();
                while (reader.IsStartElement())
                {
                    switch (reader.NamespaceURI ?? "")
                    {
                        case CmisObjectModel.Constants.Namespaces.app:
                            {
                                if (string.Compare(reader.LocalName, "workspace", true) == 0)
                                {
                                    var workspace = AtomWorkspace.CreateInstance(reader);
                                    if (workspace is not null)
                                        retVal.Workspaces.Add(workspace);
                                }
                                else
                                {
                                    // ignore node
                                    reader.ReadOuterXml();
                                }

                                break;
                            }

                        default:
                            {
                                // ignore node
                                reader.ReadOuterXml();
                                break;
                            }
                    }
                    reader.MoveToContent();
                }

                reader.ReadEndElement();
            }

            return retVal;
        }

    }
}