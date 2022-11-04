using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using System.Xml.Linq;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.AtomPub
{
    public class AtomWorkspace : sss.Workspace
    {

        #region Constants
        private const string csLink = "link";
        private const string csRepositoryInfo = "repositoryInfo";
        private const string csUriTemplate = "uritemplate";
        #endregion

        #region Constructors
        public AtomWorkspace() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomWorkspace(string title, CmisObjectModel.Core.cmisRepositoryInfoType repositoryInfo, List<AtomCollectionInfo> collections, List<XElement> links, List<CmisObjectModel.RestAtom.cmisUriTemplateType> uriTemplates) : this()
        {

            if (!string.IsNullOrEmpty(title))
                Title = new sss.TextSyndicationContent(title);
            if (collections is not null)
            {
                foreach (AtomCollectionInfo collection in collections)
                {
                    // omit duplicate namespace definitions
                    foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                        collection.AttributeExtensions.Remove(de.Key);
                    Collections.Add(collection);
                }
            }
            Links = links is null || links.Count == 0 ? null : links.ToArray();
            RepositoryInfo = repositoryInfo;
            UriTemplates = uriTemplates is null || uriTemplates.Count == 0 ? null : uriTemplates.ToArray();
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomWorkspace CreateInstance(sx.XmlReader reader)
        {
            var collections = new List<AtomCollectionInfo>();
            var links = new List<XElement>();
            CmisObjectModel.Core.cmisRepositoryInfoType repositoryInfo = null;
            string title = null;
            var uriTemplates = new List<CmisObjectModel.RestAtom.cmisUriTemplateType>();
            bool isEmptyElement;

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
                                if (reader.LocalName == "collection")
                                {
                                    collections.Add(AtomCollectionInfo.CreateInstance(reader));
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
                                switch (reader.LocalName ?? "")
                                {
                                    case "link":
                                        {
                                            links.Add(Factory.CreateLink(reader, CmisObjectModel.Constants.Namespaces.atom, "link"));
                                            break;
                                        }
                                    case "title":
                                        {
                                            title = reader.ReadElementString();
                                            break;
                                        }

                                    default:
                                        {
                                            // ignore node
                                            reader.ReadOuterXml();
                                            break;
                                        }
                                }

                                break;
                            }
                        case CmisObjectModel.Constants.Namespaces.cmisra:
                            {
                                switch (reader.LocalName ?? "")
                                {
                                    case csRepositoryInfo:
                                        {
                                            repositoryInfo = new CmisObjectModel.Core.cmisRepositoryInfoType();
                                            repositoryInfo.ReadXml(reader);
                                            break;
                                        }
                                    case csUriTemplate:
                                        {
                                            var uriTemplate = new CmisObjectModel.RestAtom.cmisUriTemplateType();
                                            uriTemplate.ReadXml(reader);
                                            uriTemplates.Add(uriTemplate);
                                            break;
                                        }

                                    default:
                                        {
                                            // ignore node
                                            reader.ReadOuterXml();
                                            break;
                                        }
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

            return new AtomWorkspace(title, repositoryInfo, collections, links, uriTemplates);
        }
        #endregion

        #region AtomPub-extensions
        private List<sss.SyndicationElementExtension> _links;
        /// <summary>
      /// AtomPub extension Links
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public XElement[] Links
        {
            get
            {
                return _links is null ? null : (from link in _links
                                                select link.GetObject<XElement>()).ToArray();
            }
            private set
            {
                // discard quick access
                _linkCache = null;

                if (_links is not null)
                {
                    foreach (sss.SyndicationElementExtension link in _links)
                        ElementExtensions.Remove(link);
                }
                if (value is null)
                {
                    _links = null;
                }
                else
                {
                    _links = new List<sss.SyndicationElementExtension>();
                    foreach (XElement link in value)
                    {
                        var linkExtension = new sss.SyndicationElementExtension(link);
                        _links.Add(linkExtension);
                        ElementExtensions.Add(linkExtension);
                    }
                }
            }
        } // Links

        private sss.SyndicationElementExtension _repositoryInfo;
        /// <summary>
      /// AtomPub extension RepositoryInfo
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public CmisObjectModel.Core.cmisRepositoryInfoType RepositoryInfo
        {
            get
            {
                return _repositoryInfo is null ? null : _repositoryInfo.GetObject<CmisObjectModel.Core.cmisRepositoryInfoType>();
            }
            private set
            {
                if (_repositoryInfo is not null)
                    ElementExtensions.Remove(_repositoryInfo);
                if (value is null)
                {
                    _repositoryInfo = null;
                }
                else
                {
                    _repositoryInfo = new sss.SyndicationElementExtension(csRepositoryInfo, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_repositoryInfo);
                }
            }
        } // RepositoryInfo

        private List<sss.SyndicationElementExtension> _uriTemplates;
        /// <summary>
      /// AtomPub extension UriTemplates
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public CmisObjectModel.RestAtom.cmisUriTemplateType[] UriTemplates
        {
            get
            {
                return _uriTemplates is null ? null : (from uriTemplate in _uriTemplates
                                                       select uriTemplate.GetObject<CmisObjectModel.RestAtom.cmisUriTemplateType>()).ToArray();
            }
            private set
            {
                // discard quick access
                _uriTemplateCache = null;

                if (_uriTemplates is not null)
                {
                    foreach (sss.SyndicationElementExtension uriTemplate in _uriTemplates)
                        ElementExtensions.Remove(uriTemplate);
                }
                if (value is null)
                {
                    _uriTemplates = null;
                }
                else
                {
                    _uriTemplates = new List<sss.SyndicationElementExtension>();
                    foreach (CmisObjectModel.RestAtom.cmisUriTemplateType uriTemplate in value)
                    {
                        var uriTemplateExtension = new sss.SyndicationElementExtension(csUriTemplate, CmisObjectModel.Constants.Namespaces.cmisra, uriTemplate);
                        _uriTemplates.Add(uriTemplateExtension);
                        ElementExtensions.Add(uriTemplateExtension);
                    }
                }
            }
        } // UriTemplates
        #endregion

        #region quick access to specific collectionInfo, repositoryLink or uriTemplate
        private CmisObjectModel.Collections.Generic.Cache<string, AtomCollectionInfo> _collectionInfoCache;
        /// <summary>
      /// Quick access to CollectionInfo
      /// </summary>
      /// <param name="collectionType"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public AtomCollectionInfo get_CollectionInfo(string collectionType)
        {
            lock (_syncObject)
            {
                string repositoryId = RepositoryInfo.RepositoryId ?? "";

                if (_collectionInfoCache is null)
                {
                    _collectionInfoCache = new CmisObjectModel.Collections.Generic.Cache<string, AtomCollectionInfo>(1000, double.PositiveInfinity, false);
                    foreach (sss.ResourceCollectionInfo collection in Collections)
                    {
                        try
                        {
                            AtomCollectionInfo atomCollection = collection as AtomCollectionInfo;

                            if (atomCollection is not null)
                            {
                                _collectionInfoCache.set_Item(new string[] { repositoryId, atomCollection.CollectionType ?? "" }, value : atomCollection);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                return _collectionInfoCache.get_Item(repositoryId, collectionType ?? "");
            }
        }

        private CmisObjectModel.Collections.Generic.Cache<string, string> _linkCache;
        /// <summary>
      /// Quick access to specified link
      /// </summary>
      /// <param name="relationshipType"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string get_Link(string relationshipType)
        {
            lock (_syncObject)
            {
                string repositoryId = RepositoryInfo.RepositoryId ?? "";

                // build the cache
                if (_linkCache is null)
                {
                    var links = Links;

                    _linkCache = new CmisObjectModel.Collections.Generic.Cache<string, string>(1000, double.PositiveInfinity, false);
                    if (links is not null)
                    {
                        foreach (XElement item in links)
                        {
                            try
                            {
                                _linkCache.set_Item(new string[] { repositoryId, item.Attribute("rel").Value ?? "" }, value: item.Attribute("href").Value);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                return _linkCache.get_Item(repositoryId, relationshipType ?? "");
            }
        }

        private CmisObjectModel.Collections.Generic.Cache<string, CmisObjectModel.RestAtom.cmisUriTemplateType> _uriTemplateCache;
        /// <summary>
      /// Quick access to UriTemplate
      /// </summary>
      /// <param name="templateType"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObjectModel.RestAtom.cmisUriTemplateType get_UriTemplate(string templateType)
        {
            lock (_syncObject)
            {
                string repositoryId = RepositoryInfo.RepositoryId ?? "";

                // build the cache
                if (_uriTemplateCache is null)
                {
                    var uriTemplates = UriTemplates;

                    _uriTemplateCache = new CmisObjectModel.Collections.Generic.Cache<string, CmisObjectModel.RestAtom.cmisUriTemplateType>(1000, double.PositiveInfinity, false);
                    if (uriTemplates is not null)
                    {
                        foreach (CmisObjectModel.RestAtom.cmisUriTemplateType item in uriTemplates)
                        {
                            // Using Alfresco the parameter returnVersion is missed in template ObjectById
                            if ((item.Type ?? "").ToLowerInvariant() == "objectbyid")
                            {
                                try
                                {
                                    var ut = new UriTemplate(item.Template);
                                    var cmisDefinedParameter = new Dictionary<string, string>() { { "id", "id" }, { "filter", "filter" }, { "includeallowableactions", "includeAllowableActions" }, { "includepolicyids", "includePolicyIds" }, { "includerelationships", "includeRelationships" }, { "includeacl", "includeACL" }, { "renditionfilter", "renditionFilter" }, { "returnversion", "returnVersion" } };
                                    var missedParameters = cmisDefinedParameter.Keys.Except(from queryValueVariableName in ut.QueryValueVariableNames
                                                                                            select queryValueVariableName.ToLowerInvariant()).ToArray();
                                    if (missedParameters.Length > 0)
                                    {
                                        item.Template += (ut.QueryValueVariableNames.Count == 0 ? "?" : "&") + string.Join("&", (from missedParameterName in missedParameters
                                                                                                                                 select (cmisDefinedParameter[missedParameterName] + "={" + cmisDefinedParameter[missedParameterName] + "}")).ToArray());
                                    }
                                }
                                catch
                                {
                                }
                            }
                            _uriTemplateCache.set_Item(new string[] { repositoryId, item.Type ?? "" }, value : item);
                        }
                    }
                }

                return _uriTemplateCache.get_Item(repositoryId, templateType ?? "");
            }
        }
        #endregion

        private object _syncObject = new object();

    }
}