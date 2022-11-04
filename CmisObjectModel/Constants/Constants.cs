
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// *
// * Contact: opensource<at>patorg.de
// * 
// * CmisObjectModelLibrary is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
// *
// * This file is part of CmisObjectModelLibrary.
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 3.0 of the License, or (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// * Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public
// * License along with this library (lgpl.txt).
// * If not, see <http://www.gnu.org/licenses/lgpl.txt>.
// ***********************************************************************************************************************

namespace CmisObjectModel.Constants
{
    public abstract class CmisPredefinedPropertyNames
    {
        private CmisPredefinedPropertyNames()
        {
        }

        public const string AllowedChildObjectTypeIds = "cmis:allowedChildObjectTypeIds";
        public const string BaseTypeId = "cmis:baseTypeId";
        public const string ChangeToken = "cmis:changeToken";
        public const string CheckinComment = "cmis:checkinComment";
        public const string ContentStreamFileName = "cmis:contentStreamFileName";
        public const string ContentStreamId = "cmis:contentStreamId";
        public const string ContentStreamLength = "cmis:contentStreamLength";
        public const string ContentStreamMimeType = "cmis:contentStreamMimeType";
        public const string CreatedBy = "cmis:createdBy";
        public const string CreationDate = "cmis:creationDate";
        public const string Description = "cmis:description";
        public const string IsImmutable = "cmis:isImmutable";
        public const string IsLatestMajorVersion = "cmis:isLatestMajorVersion";
        public const string IsLatestVersion = "cmis:isLatestVersion";
        public const string IsMajorVersion = "cmis:isMajorVersion";
        public const string IsPrivateWorkingCopy = "cmis:isPrivateWorkingCopy";
        public const string IsVersionSeriesCheckedOut = "cmis:isVersionSeriesCheckedOut";
        public const string LastModificationDate = "cmis:lastModificationDate";
        public const string LastModifiedBy = "cmis:lastModifiedBy";
        public const string Name = "cmis:name";
        public const string ObjectId = "cmis:objectId";
        public const string ObjectTypeId = "cmis:objectTypeId";
        public const string ParentId = "cmis:parentId";
        public const string Path = "cmis:path";
        public const string PolicyText = "cmis:policyText";
        public const string SecondaryObjectTypeIds = "cmis:secondaryObjectTypeIds";
        public const string SourceId = "cmis:sourceId";
        public const string TargetId = "cmis:targetId";
        public const string VersionLabel = "cmis:versionLabel";
        public const string VersionSeriesCheckedOutBy = "cmis:versionSeriesCheckedOutBy";
        public const string VersionSeriesCheckedOutId = "cmis:versionSeriesCheckedOutId";
        public const string VersionSeriesId = "cmis:versionSeriesId";

        public const string RM_DestructionDate = "cmis:rm_destructionDate";
        public const string RM_ExpirationDate = "cmis:rm_expirationDate";
        public const string RM_HoldIds = "cmis:rm_holdIds";
        public const string RM_StartOfRetention = "cmis:rm_startOfRetention";

        /// <summary>
      /// Extensions defined in this implementation
      /// </summary>
      /// <remarks></remarks>
        public abstract class Extensions
        {
            private Extensions()
            {
            }

            public const string ForeignChangeToken = "com:foreignChangeToken";
            public const string ForeignObjectId = "com:foreignObjectId";
            public const string LastSyncError = "com:lastSyncError";
            public const string SyncRequired = "com:syncRequired";
        }
    }

    public abstract class CollectionInfos
    {
        private CollectionInfos()
        {
        }

        public const string CheckedOut = "checkedout";
        public const string Policies = "policies";
        public const string Query = "query";
        public const string Relationships = "relationships";
        public const string Root = "root";
        public const string Types = "types";
        public const string Unfiled = "unfiled";
        public const string Update = "update";
    }

    /// <summary>
   /// CmisObjectModel specific extended properties of XmlSerializable-classes
   /// </summary>
   /// <remarks></remarks>
    public abstract class ExtendedProperties
    {
        public const string Cardinality = "{53c6a1e8-eca2-48cb-aa7e-dc843ccb682f}";
        public const string DeclaringType = "{d2d3cd23-1ce8-4f7c-b6f4-bd99305b04eb}";
    }

    public abstract class MediaTypes
    {
        private MediaTypes()
        {
        }

        public const string Acl = "application/cmisacl+xml";
        public const string AllowableActions = "application/cmisallowableactions+xml";
        public const string Entry = "application/atom+xml;type=entry";
        public const string Feed = "application/atom+xml;type=feed";
        public const string Html = "text/html";
        public const string JavaScript = "application/javascript";
        public const string Json = "application/json";
        public const string JsonText = "text/json";
        public const string MultipartFormData = "multipart/form-data";
        public const string PlainText = "text/plain";
        public const string Query = "application/cmisquery+xml";
        public const string Request = "application/cmisatom+xml;type=request";
        public const string Service = "application/atomsvc+xml";
        public const string Stream = "application/octet-stream";
        public const string Tree = "application/cmistree+xml";
        public const string UrlEncoded = "application/x-www-form-urlencoded";
        public const string UrlEncodedUTF8 = "application/x-www-form-urlencoded; charset=UTF-8";
        public const string Xml = "application/cmisatom+xml";
        public const string XmlApplication = "application/xml";
        public const string XmlText = "text/xml";
    }

    public abstract class Namespaces
    {
        private Namespaces()
        {
        }

        public const string alf = "http://www.alfresco.org";
        public const string app = "http://www.w3.org/2007/app";
        public const string atom = "http://www.w3.org/2005/Atom";
        public const string atompub = app;
        public const string browser = "http://docs.oasis-open.org/ns/cmis/browser/201103/";
        public const string cmis = "http://docs.oasis-open.org/ns/cmis/core/200908/";
        public const string com = "http://bruegmann-software.de/cmisObjectModel";
        public const string cmisl = "http://docs.oasis-open.org/ns/cmis/link/200908/";
        public const string cmism = "http://docs.oasis-open.org/ns/cmis/messaging/200908/";
        public const string cmisra = "http://docs.oasis-open.org/ns/cmis/restatom/200908/";
        public const string cmisw = "http://docs.oasis-open.org/ns/cmis/ws/200908/";
        public const string w3instance = "http://www.w3.org/2001/XMLSchema-instance";
        public const string xmlns = "http://www.w3.org/2000/xmlns/";
    }

    public abstract class NamespacesLowerInvariant
    {
        private NamespacesLowerInvariant()
        {
        }

        public static readonly string alf = Namespaces.alf.ToLowerInvariant();
        public static readonly string app = Namespaces.app.ToLowerInvariant();
        public static readonly string atom = Namespaces.atom.ToLowerInvariant();
        public static readonly string atompub = Namespaces.atompub.ToLowerInvariant();
        public static readonly string browser = Namespaces.browser.ToLowerInvariant();
        public static readonly string cmis = Namespaces.cmis.ToLowerInvariant();
        public static readonly string com = Namespaces.com.ToLowerInvariant();
        public static readonly string cmisl = Namespaces.cmisl.ToLowerInvariant();
        public static readonly string cmism = Namespaces.cmism.ToLowerInvariant();
        public static readonly string cmisra = Namespaces.cmisra.ToLowerInvariant();
        public static readonly string cmisw = Namespaces.cmisw.ToLowerInvariant();
        public static readonly string w3instance = Namespaces.w3instance.ToLowerInvariant();
        public static readonly string xmlns = Namespaces.xmlns.ToLowerInvariant();

    }

    public abstract class UriTemplates
    {
        private UriTemplates()
        {
        }

        public const string ObjectById = "objectbyid";
        public const string ObjectByPath = "objectbypath";
        public const string Query = "query";
        public const string TypeById = "typebyid";
    }
}