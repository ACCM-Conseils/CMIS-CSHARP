using System;
using System.Collections.Generic;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.AtomPub
{
    public class AtomLink : sss.SyndicationLink
    {

        #region Constants
        private const string csId = "id";
        private const string csRenditionKind = "renditionKind";
        #endregion

        #region Constructors
        public AtomLink() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomLink(Uri uri) : base(uri)
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomLink(Uri uri, string relationshipType, string mediaType) : this(uri)
        {

            RelationshipType = relationshipType;
            MediaType = mediaType;
        }

        public AtomLink(Uri uri, string relationshipType, string mediaType, string id, string renditionKind) : this(uri, relationshipType, mediaType)
        {

            Id = id;
            RenditionKind = renditionKind;
        }
        #endregion

        #region AtomPub-extensions
        private sx.XmlQualifiedName _id;
        /// <summary>
      /// AtomPub extension cmisra:Id
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public string Id
        {
            get
            {
                return _id is null ? null : AttributeExtensions[_id];
            }
            private set
            {
                if (_id is not null)
                    AttributeExtensions.Remove(_id);
                if (value is null)
                {
                    _id = null;
                }
                else
                {
                    _id = new sx.XmlQualifiedName(csId, CmisObjectModel.Constants.Namespaces.cmisra);
                    AttributeExtensions.Add(_id, value);
                }
            }
        } // Id

        private sx.XmlQualifiedName _renditionKind;
        /// <summary>
      /// AtomPub extension cmisra:RenditionKind
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public string RenditionKind
        {
            get
            {
                return _renditionKind is null ? null : AttributeExtensions[_renditionKind];
            }
            set
            {
                if (_renditionKind is not null)
                    AttributeExtensions.Remove(_renditionKind);
                if (value is null)
                {
                    _renditionKind = null;
                }
                else
                {
                    _renditionKind = new sx.XmlQualifiedName(csRenditionKind, CmisObjectModel.Constants.Namespaces.cmisra);
                    AttributeExtensions.Add(_renditionKind, value);
                }
            }
        } // RenditionKind
        #endregion

    }
}