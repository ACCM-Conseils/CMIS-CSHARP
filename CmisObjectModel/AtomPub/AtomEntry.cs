using CmisObjectModel.Common;
using System;
using System.Collections.Generic;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.AtomPub
{
    /// <summary>
   /// Represents a cmisObject (for example: cmisdocument)
   /// </summary>
   /// <remarks></remarks>
    public class AtomEntry : sss.SyndicationItem
    {

        #region Constants
        private const string csBulkUpdate = "bulkUpdate";
        private const string csChildren = "children";
        private const string csContent = "content";
        private const string csObject = "object";
        private const string csPathSegment = "pathSegment";
        private const string csRelativePathSegment = "relativePathSegment";
        private const string csType = "type";
        #endregion

        #region Constructors
        public AtomEntry() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        internal AtomEntry(CmisObjectModel.Core.cmisBulkUpdateType bulkUpdate, List<AtomLink> links = null) : this()
        {

            var currentTime = DateTimeOffset.UtcNow;
            InitClass("urn:bulkupdate", null, null, currentTime, currentTime, links);
            BulkUpdate = bulkUpdate;
        }

        internal AtomEntry(string objectId) : this(new CmisObjectModel.Core.cmisObjectType() { ObjectId = objectId })
        {
        }

        internal AtomEntry(CmisObjectModel.Core.cmisObjectType cmisraObject, CmisObjectModel.RestAtom.cmisContentType content = null) : this()
        {

            var currentTime = DateTimeOffset.UtcNow;
            InitClass(cmisraObject, "", "", "", currentTime, currentTime, null);

            if (content is not null)
                Content = content;
        }

        internal AtomEntry(CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType cmisraType) : this()
        {

            var currentTime = DateTimeOffset.UtcNow;
            InitClass(cmisraType, "", "", "", currentTime, currentTime, null);
        }

        public AtomEntry(CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType cmisraType, List<AtomLink> links, params sss.SyndicationPerson[] authors) : this()
        {

            var currentTime = DateTimeOffset.UtcNow;
            InitClass(cmisraType, "", "", "", currentTime, currentTime, links, authors);
        }

        public AtomEntry(CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType cmisraType, AtomFeed children, List<AtomLink> links, params sss.SyndicationPerson[] authors) : this()
        {

            var currentTime = DateTimeOffset.UtcNow;
            InitClass(cmisraType, "", "", "", currentTime, currentTime, links, authors);
            Children = children;
        }

        public AtomEntry(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, List<AtomLink> links, params sss.SyndicationPerson[] authors) : this()
        {
            InitClass(id, title, summary, publishDate, lastUpdatedTime, links, authors);
        }

        protected AtomEntry(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, CmisObjectModel.Core.cmisObjectType cmisraObject, AtomLink contentLink, CmisObjectModel.RestAtom.cmisContentType content, AtomFeed children, List<AtomLink> links, string relativePathSegment, string pathSegment, params sss.SyndicationPerson[] authors) : this()
        {
            InitClass(cmisraObject, id, title, summary, publishDate, lastUpdatedTime, links, authors);

            if (contentLink is null)
            {
                Content = content;
            }
            else
            {
                ContentLink = contentLink;
            }
            Children = children;
            PathSegment = pathSegment;
            RelativePathSegment = relativePathSegment;
        }
        public AtomEntry(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, CmisObjectModel.Core.cmisObjectType cmisraObject, AtomLink contentLink, AtomFeed children, List<AtomLink> links, string relativePathSegment, string pathSegment, params sss.SyndicationPerson[] authors) : this(id, title, summary, publishDate, lastUpdatedTime, cmisraObject, contentLink, null, children, links, relativePathSegment, pathSegment, authors)
        {
        }
        public AtomEntry(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, CmisObjectModel.Core.cmisObjectType cmisraObject, CmisObjectModel.RestAtom.cmisContentType content, AtomFeed children, List<AtomLink> links, string relativePathSegment, string pathSegment, params sss.SyndicationPerson[] authors) : this(id, title, summary, publishDate, lastUpdatedTime, cmisraObject, null, content, children, links, relativePathSegment, pathSegment, authors)
        {
        }

        public AtomEntry(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType cmisraType, AtomFeed children, List<AtomLink> links, params sss.SyndicationPerson[] authors) : this()
        {
            InitClass(cmisraType, id, title, summary, publishDate, lastUpdatedTime, links, authors);

            Children = children;
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomEntry CreateInstance(sx.XmlReader reader)
        {
            var authors = new List<sss.SyndicationPerson>();
            CmisObjectModel.Core.cmisBulkUpdateType bulkUpdate = null;
            AtomFeed children = null;
            CmisObjectModel.RestAtom.cmisContentType content = null;
            AtomLink contentLink = null;
            string id = null;
            var lastUpdatedTime = default(DateTimeOffset);
            var links = new List<AtomLink>();
            CmisObjectModel.Core.cmisObjectType @object = null;
            string pathSegment = null;
            var publishDate = default(DateTimeOffset);
            string relativePathSegment = null;
            string summary = null;
            string title = null;
            CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType type = null;
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
                        case CmisObjectModel.Constants.Namespaces.atom:
                            {
                                switch (reader.LocalName ?? "")
                                {
                                    case "author":
                                    case "contributor":
                                        {
                                            authors.Add(Factory.CreateAuthor(reader));
                                            break;
                                        }
                                    case csContent:
                                        {
                                            contentLink = Factory.CreateLink(reader);
                                            break;
                                        }
                                    case "id":
                                        {
                                            id = reader.ReadElementString();
                                            break;
                                        }
                                    case "link":
                                        {
                                            links.Add(Factory.CreateLink(reader));
                                            break;
                                        }
                                    case "published":
                                        {
                                            publishDate = Factory.CreateDateTimeOffset(reader);
                                            break;
                                        }
                                    case "summary":
                                        {
                                            summary = reader.ReadElementString();
                                            break;
                                        }
                                    case "title":
                                        {
                                            title = reader.ReadElementString();
                                            break;
                                        }
                                    case "updated":
                                        {
                                            lastUpdatedTime = Factory.CreateDateTimeOffset(reader);
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
                        case CmisObjectModel.Constants.Namespaces.app:
                            {
                                switch (reader.LocalName ?? "")
                                {
                                    case "edited":
                                        {
                                            // same as 'updated'
                                            reader.ReadElementString();
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
                                    case csBulkUpdate:
                                        {
                                            bulkUpdate = new CmisObjectModel.Core.cmisBulkUpdateType();
                                            bulkUpdate.ReadXml(reader);
                                            break;
                                        }
                                    case csChildren:
                                        {
                                            if (reader.IsEmptyElement)
                                            {
                                                // ignore node
                                                reader.ReadOuterXml();
                                            }
                                            else
                                            {
                                                reader.ReadStartElement();
                                                reader.MoveToContent();
                                                while (reader.IsStartElement())
                                                {
                                                    if ((reader.NamespaceURI ?? "") == CmisObjectModel.Constants.Namespaces.atom && reader.LocalName == "feed")
                                                    {
                                                        children = AtomFeed.CreateInstance(reader);
                                                    }
                                                    else
                                                    {
                                                        // ignore node
                                                        reader.ReadOuterXml();
                                                    }
                                                    reader.MoveToContent();
                                                }
                                                reader.ReadEndElement();
                                            }

                                            break;
                                        }
                                    case csContent:
                                        {
                                            content = new CmisObjectModel.RestAtom.cmisContentType();
                                            content.ReadXml(reader);
                                            break;
                                        }
                                    case csObject:
                                        {
                                            @object = new CmisObjectModel.Core.cmisObjectType();
                                            @object.ReadXml(reader);
                                            break;
                                        }
                                    case csPathSegment:
                                        {
                                            pathSegment = reader.ReadElementString();
                                            break;
                                        }
                                    case csRelativePathSegment:
                                        {
                                            relativePathSegment = reader.ReadElementString();
                                            break;
                                        }
                                    case csType:
                                        {
                                            type = CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType.CreateInstance(reader);
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

            if (type is null)
            {
                return new AtomEntry(id, title, summary, publishDate, lastUpdatedTime, @object, contentLink, content, children, links.Count > 0 ? links : null, relativePathSegment, pathSegment, authors.Count > 0 ? authors.ToArray() : null) { BulkUpdate = bulkUpdate };
            }
            else
            {
                return new AtomEntry(id, title, summary, publishDate, lastUpdatedTime, type, children, links.Count > 0 ? links : null, authors.Count > 0 ? authors.ToArray() : null);
            }
        }

        private void InitClass(string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, List<AtomLink> links, params sss.SyndicationPerson[] authors)
        {
            Id = id;
            if (!string.IsNullOrEmpty(title))
                Title = new sss.TextSyndicationContent(title);
            if (!string.IsNullOrEmpty(summary))
                Summary = new sss.TextSyndicationContent(summary);
            PublishDate = publishDate;
            LastUpdatedTime = lastUpdatedTime;
            // 3.5.2 Entries
            // app:edited MUST be lastModificationDate
            ElementExtensions.Add("edited", CmisObjectModel.Constants.Namespaces.app, sx.XmlConvert.ToString(lastUpdatedTime));
            if (authors is not null)
            {
                foreach (sss.SyndicationPerson author in authors)
                    Authors.Add(author);
            }
            if (links is not null)
            {
                foreach (AtomLink link in links)
                    Links.Add(link);
            }
        }

        /// <summary>
      /// Initializes this instance and ensures, that the AtomEntry is conform to the guidelines
      /// performed in 3.5.2 Entries
      /// </summary>
      /// <param name="cmisraObject"></param>
      /// <param name="id"></param>
      /// <param name="title"></param>
      /// <param name="summary"></param>
      /// <param name="publishDate"></param>
      /// <param name="lastUpdatedTime"></param>
      /// <param name="links"></param>
      /// <param name="authors"></param>
      /// <remarks></remarks>
        private void InitClass(CmisObjectModel.Core.cmisObjectType cmisraObject, string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, List<AtomLink> links, params sss.SyndicationPerson[] authors)
        {
            Object = cmisraObject;
            // guidelines
            if (cmisraObject is not null && cmisraObject.Properties is not null)
            {
                string author;
                var properties = cmisraObject.Properties.FindProperties(true, CmisObjectModel.Constants.CmisPredefinedPropertyNames.Name, CmisObjectModel.Constants.CmisPredefinedPropertyNames.LastModificationDate, CmisObjectModel.Constants.CmisPredefinedPropertyNames.CreationDate, CmisObjectModel.Constants.CmisPredefinedPropertyNames.CreatedBy, CmisObjectModel.Constants.CmisPredefinedPropertyNames.Description, CmisObjectModel.Constants.CmisPredefinedPropertyNames.ObjectId);
                // atom:title MUST be the cmis:name property
                title = this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.Name], title);
                // atom:updated MUST be cmis:lastModificationDate
                lastUpdatedTime = this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.LastModificationDate], lastUpdatedTime);
                // atom:published MUST be cmis:creationDate
                publishDate = this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.CreationDate], publishDate);
                // atom:author/atom:name MUST be cmis:createdBy
                author = this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.CreatedBy], "");
                if (!string.IsNullOrEmpty(author))
                {
                    if (authors is null || authors.Length == 0 || string.Compare(authors[0].Name, author, true) != 0)
                    {
                        authors = new sss.SyndicationPerson[] { new sss.SyndicationPerson(null, author, null) };
                    }
                }
                // atom:summary SHOULD be cmis:description
                if (string.IsNullOrEmpty(summary))
                    summary = this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.Description], "");
                // atom:id SHOULD be derived from cmis:objectId
                if (string.IsNullOrEmpty(id))
                    id = "urn:objects:" + this.ReadProperty(properties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.ObjectId], "id");
            }

            InitClass(id, title, summary, publishDate, lastUpdatedTime, links, authors);
        }

        /// <summary>
      /// Initializes this instance and ensures, that the AtomEntry is conform to the guidelines
      /// performed in 3.5.2 Entries
      /// </summary>
      /// <param name="cmisraType"></param>
      /// <param name="id"></param>
      /// <param name="title"></param>
      /// <param name="summary"></param>
      /// <param name="publishDate"></param>
      /// <param name="lastUpdatedTime"></param>
      /// <param name="links"></param>
      /// <param name="authors"></param>
      /// <remarks></remarks>
        private void InitClass(CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType cmisraType, string id, string title, string summary, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, List<AtomLink> links, params sss.SyndicationPerson[] authors)
        {
            Type = cmisraType;
            if (cmisraType is not null)
            {
                // atom:title MUST be the cmis:displayName
                title = cmisraType.DisplayName;
                // The repository SHOULD populate the atom:summary tag with text that best represents a summary of the object. For example, the type description if available
                if (string.IsNullOrEmpty(summary))
                    summary = CommonFunctions.NVL(title, cmisraType.Id);
                if (string.IsNullOrEmpty(id))
                    id = "urn:types:" + cmisraType.Id;
            }

            InitClass(id, title, summary, publishDate, lastUpdatedTime, links, authors);
        }

        /// <summary>
      /// Returns [property].Value or defaultValue if [property].Value equals to null
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="property"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private TResult ReadProperty<TResult>(CmisObjectModel.Core.Properties.cmisProperty property, TResult defaultValue)
        {
            var value = property is CmisObjectModel.Core.Properties.Generic.cmisProperty<TResult> ? ((CmisObjectModel.Core.Properties.Generic.cmisProperty<TResult>)property).Value : default(TResult);
            return value is null || value.Equals(null) ? defaultValue : value;
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Implements the children - AtomPub extension defined in
      /// http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-RestAtom.xsd
      /// </summary>
      /// <remarks></remarks>
        public class CmisChildrenType : CmisObjectModel.Serialization.XmlSerializable
        {

            public CmisChildrenType()
            {
            }
            public CmisChildrenType(AtomFeed children)
            {
                feedFormatter = new sss.Atom10FeedFormatter(children);
            }

            public sss.Atom10FeedFormatter feedFormatter { get; set; }

            protected override void ReadAttributes(sx.XmlReader reader)
            {
                // nicht benötigt
            }

            protected override void ReadXmlCore(sx.XmlReader reader, CmisObjectModel.Serialization.XmlAttributeOverrides attributeOverrides)
            {
                // nicht benötigt (siehe Methode AtomEntry.CreateInstance(), in der AtomFeed.CreateInstance() aufgerufen wird)
            }

            protected override void WriteXmlCore(sx.XmlWriter writer, CmisObjectModel.Serialization.XmlAttributeOverrides attributeOverrides)
            {
                if (feedFormatter is not null)
                {
                    var xmlDoc = CmisObjectModel.Serialization.SerializationHelper.ToXmlDocument(feedFormatter, attributeOverrides);
                    writer.WriteRaw(xmlDoc.DocumentElement.OuterXml);
                }
            }
        }
        #endregion

        #region AtomPub-extensions
        private sss.SyndicationElementExtension _bulkUpdate;
        /// <summary>
      /// AtomPub extension BulkUpdate
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public CmisObjectModel.Core.cmisBulkUpdateType BulkUpdate
        {
            get
            {
                return _bulkUpdate is null ? null : _bulkUpdate.GetObject<CmisObjectModel.Core.cmisBulkUpdateType>();
            }
            private set
            {
                if (_bulkUpdate is not null)
                    ElementExtensions.Remove(_bulkUpdate);
                if (value is null)
                {
                    _bulkUpdate = null;
                }
                else
                {
                    _bulkUpdate = new sss.SyndicationElementExtension(csBulkUpdate, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_bulkUpdate);
                }
            }
        } // BulkUpdate

        private sss.SyndicationElementExtension _children;
        /// <summary>
      /// AtomPub extension Children
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public AtomFeed Children
        {
            get
            {
                return _children is null ? null : (AtomFeed)_children.GetObject<CmisChildrenType>().feedFormatter.Feed;
            }
            private set
            {
                if (_children is not null)
                    ElementExtensions.Remove(_children);
                if (value is null)
                {
                    _children = null;
                }
                else
                {
                    // omit duplicate namespace definitions
                    foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                        value.AttributeExtensions.Remove(de.Key);
                    _children = new sss.SyndicationElementExtension(csChildren, CmisObjectModel.Constants.Namespaces.cmisra, new CmisChildrenType(value));
                    ElementExtensions.Add(_children);
                }
            }
        } // Children

        private sss.SyndicationElementExtension _content;
        /// <summary>
      /// AtomPub extension Content
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public new CmisObjectModel.RestAtom.cmisContentType Content
        {
            get
            {
                return _content is null ? null : _content.GetObject<CmisObjectModel.RestAtom.cmisContentType>();
            }
            private set
            {
                if (_content is not null)
                    ElementExtensions.Remove(_content);
                if (value is null)
                {
                    _content = null;
                }
                else
                {
                    _content = new sss.SyndicationElementExtension(value);
                    ElementExtensions.Add(_content);
                }
            }
        } // Content

        private sss.SyndicationElementExtension _object;
        /// <summary>
      /// AtomPub extension Object
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public CmisObjectModel.Core.cmisObjectType Object
        {
            get
            {
                return _object is null ? null : _object.GetObject<CmisObjectModel.Core.cmisObjectType>();
            }
            private set
            {
                if (_object is not null)
                    ElementExtensions.Remove(_object);
                if (value is null)
                {
                    _object = null;
                }
                else
                {
                    _object = new sss.SyndicationElementExtension(csObject, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_object);
                }
            }
        } // [Object]

        private sss.SyndicationElementExtension _pathSegment;
        /// <summary>
      /// AtomPub extension PathSegment
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public string PathSegment
        {
            get
            {
                return _pathSegment is null ? null : _pathSegment.GetObject<string>();
            }
            private set
            {
                if (_pathSegment is not null)
                    ElementExtensions.Remove(_pathSegment);
                if (string.IsNullOrEmpty(value))
                {
                    _pathSegment = null;
                }
                else
                {
                    _pathSegment = new sss.SyndicationElementExtension(csPathSegment, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_pathSegment);
                }
            }
        } // PathSegment

        private sss.SyndicationElementExtension _relativePathSegment;
        /// <summary>
      /// AtomPub - extension RelativePathSegment
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public string RelativePathSegment
        {
            get
            {
                return _relativePathSegment is null ? null : _relativePathSegment.GetObject<string>();
            }
            private set
            {
                if (_relativePathSegment is not null)
                    ElementExtensions.Remove(_relativePathSegment);
                if (string.IsNullOrEmpty(value))
                {
                    _relativePathSegment = null;
                }
                else
                {
                    _relativePathSegment = new sss.SyndicationElementExtension(csRelativePathSegment, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_relativePathSegment);
                }
            }
        } // RelativePathSegment

        private sss.SyndicationElementExtension _type;
        /// <summary>
      /// AtomPub extension Type
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType Type
        {
            get
            {
                return _type is null ? null : _type.GetObject<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType>();
            }
            private set
            {
                if (_type is not null)
                    ElementExtensions.Remove(_type);
                if (value is null)
                {
                    _type = null;
                }
                else
                {
                    _type = new sss.SyndicationElementExtension(csType, CmisObjectModel.Constants.Namespaces.cmisra, value);
                    ElementExtensions.Add(_type);
                }
            }
        } // Type
        #endregion

        [sxs.XmlIgnore()]
        public string BaseTypeId
        {
            get
            {
                var cmisraObject = Object;
                return cmisraObject is null ? default(CmisObjectModel.Common.Generic.Nullable<string>) : cmisraObject.BaseTypeId;
            }
        }

        [sxs.XmlIgnore()]
        public AtomLink ContentLink
        {
            get
            {
                if (base.Content is sss.UrlSyndicationContent)
                {
                    {
                        var withBlock = (sss.UrlSyndicationContent)base.Content;
                        return new AtomLink(withBlock.Url, CmisObjectModel.Constants.LinkRelationshipTypes.ContentStream, withBlock.Type);
                    }
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                if (value is null)
                {
                    base.Content = null;
                }
                else
                {
                    base.Content = new sss.UrlSyndicationContent(value.Uri, value.MediaType);
                }
            }
        } // ContentUri

        /// <summary>
      /// Atom elements take precedence over the corresponding writable CMIS property.
      /// </summary>
      /// <remarks>
      /// When POSTing an Atom Document, the Atom elements MUST take precedence over the corresponding writable CMIS property. For example, atom:title will overwrite cmis:name.
      /// This is conform to the guidelines performed in 3.5.2 Entries
      /// </remarks>
        public void EnsurePOSTRuleOfPrecedence()
        {
            var cmisraObject = Object;
            var cmisraType = Type;

            if (cmisraObject is not null)
            {
                CmisObjectModel.Core.Properties.cmisProperty prop;
                var propertyList = new List<CmisObjectModel.Core.Properties.cmisProperty>();
                var verifyProperties = new Dictionary<string, CmisObjectModel.Core.Properties.cmisProperty>();
                bool changes = false;

                // enlist the current properties
                if (cmisraObject.Properties is not null && cmisraObject.Properties.Properties is not null)
                {
                    foreach (var currentProp in cmisraObject.Properties.Properties)
                    {
                        prop = currentProp;
                        if (!verifyProperties.ContainsKey(prop.PropertyDefinitionId ?? ""))
                        {
                            verifyProperties.Add(prop.PropertyDefinitionId ?? "", prop);
                            propertyList.Add(prop);
                        }
                    }
                }

                // check, if a property is missed
                {
                    var withBlock = new CmisObjectModel.Common.PredefinedPropertyDefinitionFactory(null);
                    // atom:title MUST be the cmis:name property
                    if (Title is not null)
                    {
                        if (verifyProperties.ContainsKey(CmisObjectModel.Constants.CmisPredefinedPropertyNames.Name))
                        {
                            prop = verifyProperties[CmisObjectModel.Constants.CmisPredefinedPropertyNames.Name];
                        }
                        else
                        {
                            prop = withBlock.Name().CreateProperty();
                            verifyProperties.Add(CmisObjectModel.Constants.CmisPredefinedPropertyNames.Name, prop);
                            propertyList.Add(prop);
                            changes = true;
                        }
                        prop.Value = Title.Text;
                    }
                    if (changes)
                        cmisraObject.Properties = new CmisObjectModel.Core.Collections.cmisPropertiesType(propertyList.ToArray());
                }
            }
            else if (cmisraType is not null)
            {
                if (Title is not null)
                    cmisraType.DisplayName = Title.Text;
            }
        }

        [sxs.XmlIgnore()]
        public string ChangeToken
        {
            get
            {
                var cmisraObject = Object;
                return cmisraObject is null ? default(CmisObjectModel.Common.Generic.Nullable<string>) : cmisraObject.ChangeToken;
            }
        }

        /// <summary>
      /// Return the first matching link
      /// </summary>
      /// <param name="relationshipType"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public sss.SyndicationLink get_Link(string relationshipType, string mediaType = null)
        {
            foreach (sss.SyndicationLink retVal in Links)
            {
                if (string.Compare(relationshipType ?? "", retVal.RelationshipType ?? "", true) == 0 && (string.IsNullOrEmpty(mediaType) || string.Compare(mediaType, retVal.MediaType ?? "", true) == 0))
                {
                    return retVal;
                }
            }
            // not found
            return null;
        }

        [sxs.XmlIgnore()]
        public string Name
        {
            get
            {
                var cmisraObject = Object;
                return cmisraObject is null ? default(CmisObjectModel.Common.Generic.Nullable<string>) : cmisraObject.Name;
            }
        }

        [sxs.XmlIgnore()]
        public string ObjectId
        {
            get
            {
                var cmisraObject = Object;
                return cmisraObject is null ? default(CmisObjectModel.Common.Generic.Nullable<string>) : cmisraObject.ObjectId;
            }
        }

        [sxs.XmlIgnore()]
        public string TypeId
        {
            get
            {
                var cmisraObject = Object;
                var cmisType = Type;

                if (cmisType is not null)
                {
                    return cmisType.Id;
                }
                else if (cmisraObject is null)
                {
                    return null;
                }
                else
                {
                    return cmisraObject.ObjectTypeId;
                }
            }
        }

    }
}