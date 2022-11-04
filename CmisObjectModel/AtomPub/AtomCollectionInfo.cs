using System;
using System.Collections.Generic;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.AtomPub
{
    /// <summary>
   /// Represents a collection of the cmis-repository workspace
   /// </summary>
   /// <remarks></remarks>
    public class AtomCollectionInfo : sss.ResourceCollectionInfo
    {

        #region Constants
        private const string csCollectionType = "collectionType";
        #endregion

        #region Constructors
        public AtomCollectionInfo() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomCollectionInfo(string title, Uri link, string collectionType, params string[] accepts) : this()
        {

            if (!string.IsNullOrEmpty(title))
                Title = new sss.TextSyndicationContent(title);
            Link = link;
            CollectionType = collectionType;
            if (accepts is not null)
            {
                foreach (string accept in accepts)
                    Accepts.Add(accept);
            }
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomCollectionInfo CreateInstance(sx.XmlReader reader)
        {
            var accepts = new List<string>();
            string collectionType = null;
            Uri link;
            string title = null;
            bool isEmptyElement;

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            link = new Uri(reader.GetAttribute("href"));
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
                                if (reader.LocalName == "accept")
                                {
                                    string accept = reader.ReadElementString();
                                    if (!string.IsNullOrEmpty(accept))
                                        accepts.Add(accept);
                                }
                                else
                                {
                                    // ignore node
                                    reader.ReadOuterXml();
                                }

                                break;
                            }
                        case CmisObjectModel.Constants.Namespaces.atom:
                            {
                                if (reader.LocalName == "title")
                                {
                                    title = reader.ReadElementString();
                                }
                                else
                                {
                                    // ignore node
                                    reader.ReadOuterXml();
                                }

                                break;
                            }
                        case CmisObjectModel.Constants.Namespaces.cmisra:
                            {
                                if ((reader.LocalName ?? "") == csCollectionType)
                                {
                                    collectionType = reader.ReadElementString();
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

            return new AtomCollectionInfo(title, link, collectionType, accepts.Count > 0 ? accepts.ToArray() : null);
        }
        #endregion

        #region AtomPub-extensions
        private sss.SyndicationElementExtension _collectionType;
        /// <summary>
      /// AtomPub extension CollectionType
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public string CollectionType
        {
            get
            {
                return _collectionType is null ? null : _collectionType.GetObject<string>();
            }
            private set
            {
                if (_collectionType is not null)
                    ElementExtensions.Remove(_collectionType);
                if (value is null)
                {
                    _collectionType = null;
                }
                else
                {
                    _collectionType = new sss.SyndicationElementExtension(csCollectionType, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_collectionType);
                }
            }
        } // CollectionType
        #endregion

    }
}