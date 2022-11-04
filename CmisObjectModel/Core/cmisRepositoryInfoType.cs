using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
using CmisObjectModel.Common;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Core
{
    public partial class cmisRepositoryInfoType
    {

        protected override void InitClass()
        {
            base.InitClass();
            _capabilities = new cmisRepositoryCapabilitiesType();
            _cmisVersionSupported = "1.1";
        }

        #region links
        /// <summary>
      /// Creates a list of links for a cmisRepositoryInfoType-instance
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks>
      /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
      /// 3.7.1 HTTP GET
      /// </remarks>
        protected List<TLink> GetLinks<TLink>(Uri baseUri, AtomPub.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            var retVal = new List<TLink>() { elementFactory(new Uri(baseUri, ServiceURIs.get_TypeDescendantsUri(ServiceURIs.enumTypeDescendantsUri.none).ReplaceUri("repositoryId", _repositoryId)), LinkRelationshipTypes.TypeDescendants, MediaTypes.Feed, null, null) };

            if (_capabilities is not null)
            {
                if (_capabilities.CapabilityGetFolderTree)
                {
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_FolderTreeUri(ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _rootFolderId)), LinkRelationshipTypes.FolderTree, MediaTypes.Tree, null, null));
                }
                if (_capabilities.CapabilityGetDescendants)
                {
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_DescendantsUri(ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _rootFolderId)), LinkRelationshipTypes.RootDescendants, MediaTypes.Tree, _repositoryId, null));
                }
                if (_capabilities.CapabilityChanges != enumCapabilityChanges.none)
                {
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ChangesUri(ServiceURIs.enumChangesUri.none).ReplaceUri("repositoryId", _repositoryId)), LinkRelationshipTypes.Changes, MediaTypes.Feed, null, null));
                }
            }

            return retVal;
        }

        /// <summary>
      /// Creates a list of links for a cmisRepositoryInfoType-instance
      /// </summary>
      /// <param name="baseUri"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<AtomPub.AtomLink> GetLinks(Uri baseUri)
        {
            return GetLinks(baseUri, (uri, relationshipType, mediaType, id, renditionKind) => new AtomPub.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }
        /// <summary>
      /// Creates a list of links for a cmisRepositoryInfoType-instance
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetLinks(Uri baseUri, XNamespace ns, string elementName)
        {
            {
                var withBlock = new AtomPub.Factory.XElementBuilder(ns, elementName);
                return GetLinks(baseUri, withBlock.CreateXElement);
            }
        }
        #endregion

        /// <summary>
      /// Returns the uritemplates to
      /// - get an object by id
      /// - get an object by path
      /// - get a type by id
      /// - request a query (if the repository supports queries)
      /// </summary>
      /// <param name="baseUri"></param>
      /// <returns></returns>
      /// <remarks>
      /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
      /// 3.7.1.1 URI Templates
      /// </remarks>
        public List<RestAtom.cmisUriTemplateType> GetUriTemplates(Uri baseUri)
        {
            var retVal = new List<RestAtom.cmisUriTemplateType>() { new RestAtom.cmisUriTemplateType(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.getObjectById).ReplaceUri("repositoryId", RepositoryId)), UriTemplates.ObjectById, MediaTypes.Entry), new RestAtom.cmisUriTemplateType(new Uri(baseUri, ServiceURIs.GetObjectByPath.ReplaceUri("repositoryId", RepositoryId)), UriTemplates.ObjectByPath, MediaTypes.Entry), new RestAtom.cmisUriTemplateType(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", RepositoryId)), UriTemplates.TypeById, MediaTypes.Entry) };

            if (_capabilities is not null && _capabilities.CapabilityQuery != enumCapabilityQuery.none)
            {
                retVal.Add(new RestAtom.cmisUriTemplateType(new Uri(baseUri, ServiceURIs.get_QueryUri(ServiceURIs.enumQueryUri.query).ReplaceUri("repositoryId", RepositoryId)), UriTemplates.Query, MediaTypes.Feed));
            }

            return retVal;
        }

        /// <summary>
      /// Returns the collections supported by the repository
      /// </summary>
      /// <param name="baseUri"></param>
      /// <returns></returns>
      /// <remarks>
      /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
      /// 3.7.1 HTTP GET
      /// Accepts defined in 3.8
      /// </remarks>
        public List<AtomPub.AtomCollectionInfo> GetCollectionInfos(Uri baseUri)
        {
            var retVal = new List<AtomPub.AtomCollectionInfo>();
            AtomPub.AtomCollectionInfo collectionInfo;
            string[] accepts;

            // root
            collectionInfo = new AtomPub.AtomCollectionInfo("Root Collection", new Uri(baseUri, ServiceURIs.get_ChildrenUri(ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _rootFolderId)), CollectionInfos.Root, MediaTypes.Entry, MediaTypes.Request);
            retVal.Add(collectionInfo);
            // query
            if (_capabilities is not null && _capabilities.CapabilityQuery != enumCapabilityQuery.none)
            {
                collectionInfo = new AtomPub.AtomCollectionInfo("Query Collection", new Uri(baseUri, ServiceURIs.get_QueryUri(ServiceURIs.enumQueryUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Query, MediaTypes.Query, MediaTypes.Request);
                retVal.Add(collectionInfo);
            }
            // checkedOut
            if (_capabilities is not null && _capabilities.CapabilityPWCUpdatable)
            {
                collectionInfo = new AtomPub.AtomCollectionInfo("Checked Out Collection", new Uri(baseUri, ServiceURIs.get_CheckedOutUri(ServiceURIs.enumCheckedOutUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.CheckedOut, MediaTypes.Entry, MediaTypes.Request);
                retVal.Add(collectionInfo);
            }
            // unfiling
            if (_capabilities is not null && _capabilities.CapabilityUnfiling)
            {
                collectionInfo = new AtomPub.AtomCollectionInfo("Unfiled collection", new Uri(baseUri, ServiceURIs.get_UnfiledUri(ServiceURIs.enumUnfiledUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Unfiled, MediaTypes.Entry, MediaTypes.Request);
                retVal.Add(collectionInfo);
            }
            // type children
            accepts = _capabilities is not null && _capabilities.CapabilityNewTypeSettableAttributes is not null ? (new string[] { MediaTypes.Entry }) : null;
            collectionInfo = new AtomPub.AtomCollectionInfo("Types Collection", new Uri(baseUri, ServiceURIs.get_TypesUri(ServiceURIs.enumTypesUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Types, accepts);
            retVal.Add(collectionInfo);
            // BulkUpdates
            if (_capabilities is not null && _capabilities.CapabilityBulkUpdatable)
            {
                collectionInfo = new AtomPub.AtomCollectionInfo("Updates Collection", new Uri(baseUri, ServiceURIs.BulkUpdateProperties.ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Update, MediaTypes.Entry);
            }
            // relationships (not directly defined in 3.7.1, but listed in 3.9.1)
            collectionInfo = new AtomPub.AtomCollectionInfo("Relationships Collection", new Uri(baseUri, ServiceURIs.get_RelationshipsUri(ServiceURIs.enumRelationshipsUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Relationships, MediaTypes.Entry, MediaTypes.Request);
            // policies (not directly defined in 3.7.1, but listed in 3.9.3)
            collectionInfo = new AtomPub.AtomCollectionInfo("Policies Collection", new Uri(baseUri, ServiceURIs.get_PoliciesUri(ServiceURIs.enumPoliciesUri.none).ReplaceUri("repositoryId", _repositoryId)), CollectionInfos.Policies, MediaTypes.Entry);

            return retVal;
        }

        public static Client.CmisRepository operator +(cmisRepositoryInfoType arg1, Contracts.ICmisClient arg2)
        {
            return new Client.CmisRepository(arg1, arg2);
        }
        public static Client.CmisRepository operator +(Contracts.ICmisClient arg1, cmisRepositoryInfoType arg2)
        {
            return new Client.CmisRepository(arg2, arg1);
        }

    }
}