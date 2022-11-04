using System;
using System.Collections.Generic;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using System.Xml.Linq;

namespace CmisObjectModel.AtomPub
{
    /// <summary>
   /// Methods to convert XmlReader-nodes to specified types
   /// </summary>
   /// <remarks></remarks>
    public static class Factory
    {

        #region Helper classes
        public abstract class GenericDelegates<TParam, TResult>
        {
            public delegate TResult CreateLinkDelegate(TParam uri, string relationshipType, string mediaType, string id, string renditionKind);
        }

        /// <summary>
      /// Implements the creation of XElement-type
      /// </summary>
      /// <remarks></remarks>
        public class XElementBuilder
        {

            public XElementBuilder(XNamespace ns, string elementName)
            {
                _ns = ns;
                _elementName = elementName;
            }

            /// <summary>
         /// Creates XElement-instance specified by href, relationshipType, mediaType, id (cmisra:id) and renditionKind (cmisra:renditionKind)
         /// </summary>
         /// <param name="href"></param>
         /// <param name="relationshipType"></param>
         /// <param name="mediaType"></param>
         /// <param name="id"></param>
         /// <param name="renditionKind"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public XElement CreateXElement(string href, string relationshipType, string mediaType, string id, string renditionKind)
            {
                var contents = new List<object>() { new XAttribute("type", mediaType), new XAttribute("rel", relationshipType), new XAttribute("href", href) };

                if (!string.IsNullOrEmpty(id))
                    contents.Add(new XAttribute("{" + CmisObjectModel.Constants.Namespaces.cmisra + "}id", id));
                if (!string.IsNullOrEmpty(renditionKind))
                    contents.Add(new XAttribute("{" + CmisObjectModel.Constants.Namespaces.cmisra + "}renditionKind", id));
                contents.Add(string.Empty);

                return new XElement(_ns + _elementName, contents.ToArray());
            }

            public XElement CreateXElement(Uri uri, string relationshipType, string mediaType, string id, string renditionKind)
            {
                return CreateXElement(uri.AbsoluteUri, relationshipType, mediaType, id, renditionKind);
            }

            private XNamespace _ns;
            private string _elementName;

        }
        #endregion

        /// <summary>
      /// Evaluates the current node as a SyndicationPerson-instance
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static sss.SyndicationPerson CreateAuthor(sx.XmlReader reader)
        {
            reader.MoveToContent();
            if (reader.IsEmptyElement)
            {
                reader.ReadOuterXml();
                return new sss.SyndicationPerson();
            }
            else
            {
                string name = null;
                string email = null;
                string uri = null;

                reader.ReadStartElement();
                reader.MoveToContent();
                while (reader.IsStartElement())
                {
                    switch (reader.LocalName ?? "")
                    {
                        case "email":
                            {
                                email = reader.ReadElementString();
                                break;
                            }
                        case "name":
                            {
                                name = reader.ReadElementString();
                                break;
                            }
                        case "uri":
                            {
                                uri = reader.ReadElementString();
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
                return new sss.SyndicationPerson(email, name, uri);
            }
        }

        private static System.Text.RegularExpressions.Regex _toDateTimeRegEx = new System.Text.RegularExpressions.Regex(@"-?\d{4,}(-\d{2}){2}T\d{2}(\:\d{2}){2}(\.\d+)?([\+\-]\d{2}\:\d{2}|Z)?", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        /// <summary>
      /// Evaluates the current node as a DateTimeOffset-value
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static DateTimeOffset CreateDateTimeOffset(sx.XmlReader reader)
        {
            try
            {
                reader.MoveToContent();
                return CmisObjectModel.Common.CommonFunctions.CreateDateTimeOffset(reader.ReadElementString());
            }
            catch
            {
            }

            return default;
        }

        /// <summary>
      /// Evaluates the current node as a AtomLink-instance
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomLink CreateLink(sx.XmlReader reader)
        {
            return CreateLink(reader, (href, relationshipType, mediaType, id, renditionKind) => new AtomLink(new Uri(href), relationshipType, mediaType, id, renditionKind));
        }

        /// <summary>
      /// Evaluates the current node as a XElement-instance
      /// </summary>
      /// <param name="reader"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static XElement CreateLink(sx.XmlReader reader, XNamespace ns, string elementName)
        {
            {
                var withBlock = new XElementBuilder(ns, elementName);
                return CreateLink(reader, withBlock.CreateXElement);
            }
        }

        /// <summary>
      /// Creates a link
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static TResult CreateLink<TResult>(sx.XmlReader reader, GenericDelegates<string, TResult>.CreateLinkDelegate factory)
        {
            string href = null;
            string relationshipType = null;
            string mediaType = null;
            string id = null;
            string renditionKind = null;
            bool isEmptyElement;

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            for (int index = 0, loopTo = reader.AttributeCount - 1; index <= loopTo; index++)
            {
                reader.MoveToAttribute(index);
                switch (reader.NamespaceURI ?? "")
                {
                    case CmisObjectModel.Constants.Namespaces.atom:
                    case var @case when @case == "":
                        {
                            switch (reader.LocalName ?? "")
                            {
                                case "href":
                                case "src":
                                    {
                                        href = reader.GetAttribute(index);
                                        break;
                                    }
                                case "rel":
                                    {
                                        relationshipType = reader.GetAttribute(index);
                                        break;
                                    }
                                case "type":
                                    {
                                        mediaType = reader.GetAttribute(index);
                                        break;
                                    }
                            }

                            break;
                        }
                    case CmisObjectModel.Constants.Namespaces.cmisra:
                        {
                            switch (reader.LocalName ?? "")
                            {
                                case "id":
                                    {
                                        id = reader.GetAttribute(index);
                                        break;
                                    }
                                case "renditionKind":
                                    {
                                        renditionKind = reader.GetAttribute(index);
                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
            // read to end of link
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                CmisObjectModel.Common.CommonFunctions.ReadToEndElement(reader, true);
            }

            return factory(href, relationshipType, mediaType, id, renditionKind);
        }

    }
}