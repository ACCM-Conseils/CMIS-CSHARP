using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sn = System.Net;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using ca = CmisObjectModel.AtomPub;
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
using ccg = CmisObjectModel.Common.Generic;
using CmisObjectModel.Constants;
using cm = CmisObjectModel.Messaging;
using cmr = CmisObjectModel.Messaging.Responses;
using CmisObjectModel.Messaging;
/* TODO ERROR: Skipped IfDirectiveTrivia
#If Not xs_HttpRequestAddRange64 Then
*//* TODO ERROR: Skipped DisabledTextTrivia
#Const HttpRequestAddRangeShortened = True
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.AtomPub
{
    /// <summary>
   /// Implements the functionality of a cmis-client version 1.1
   /// </summary>
   /// <remarks>
   /// Requested Repositories will be cached in the System.Runtime.Caching.MemoryCache.Default-instance for a duration of
   /// AppSettings.CacheLeaseTime (value specified in seconds). After this duration the repository is not longer valid and
   /// has to be renewed
   /// Limitations using the AtomPub-Binding: search for 'AtomPub binding' in this document
   /// </remarks>
    public class CmisClient : Base.Generic.CmisClient<ca.AtomWorkspace>
    {

        private static readonly sxs.XmlAttributeOverrides _requestBaseAttributeOverrides = new sxs.XmlAttributeOverrides();
        /// <summary>
      /// Supported linkRelationshipTypes in the cache
      /// </summary>
      /// <remarks></remarks>
        private static HashSet<string> _supportedLinkRelationshipTypes = new HashSet<string>() { LinkRelationshipTypes.Acl, LinkRelationshipTypes.AllowableActions, LinkRelationshipTypes.ContentStream, LinkRelationshipTypes.Down, LinkRelationshipTypes.EditMedia, LinkRelationshipTypes.FolderTree, LinkRelationshipTypes.Policies, LinkRelationshipTypes.Relationships, LinkRelationshipTypes.Self, LinkRelationshipTypes.Up, LinkRelationshipTypes.VersionHistory, LinkRelationshipTypes.WorkingCopy };

        static CmisClient()
        {
            // use Namespace cmisw (instead of cmism) for Type derived from Messaging.Request.RequestBase
            var attrs = new sxs.XmlAttributes() { XmlRoot = new sxs.XmlRootAttribute() { Namespace = Namespaces.cmisw } };

            _requestBaseAttributeOverrides.Add(typeof(cm.Requests.RequestBase), attrs);
        }

        public CmisClient(Uri serviceDocUri, enumVendor vendor, AuthenticationProvider authentication, int? connectTimeout = default, int? readWriteTimeout = default) : base(serviceDocUri, vendor, authentication, connectTimeout, readWriteTimeout)
        {
        }

        #region Repository
        /// <summary>
      /// Creates a new type definition that is a subtype of an existing specified parent type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks>The client gets the information about the types-collection via the collectioninfo</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateType(string repositoryId, Core.Definitions.Types.cmisTypeDefinitionType type)
        {
            // types-collection
            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Types);
                if (withBlock.Exception is null)
                {
                    // collection found, but readonly
                    if (!CollectionAccepts(withBlock.Response, MediaTypes.Entry))
                    {
                        var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.NotFound, cm.enumServiceException.objectNotFound, "Type-collectionInfo is readonly.");
                        return cmisFault.ToFaultException();
                    }
                    else
                    {
                        // create the new type
                        var retVal = Post(withBlock.Response.Link, new sss.Atom10ItemFormatter(new ca.AtomEntry(type)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);

                        if (retVal.Exception is null)
                            WriteToCache(repositoryId, null, retVal.Response, _typeLinks);
                        return retVal;
                    }
                }
                else
                {
                    // types-collectionInfo not found
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a new type definition that is a subtype of an existing specified parent type
      /// </summary>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createTypeResponse> CreateType(cm.Requests.createType request)
        {
            var result = CreateType(request.RepositoryId, request.Type);

            if (result.Exception is null)
            {
                return new cmr.createTypeResponse() { Type = result.Response.Type };
            }
            else
            {
                return result.Exception;
            }
        }

        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Response DeleteType(string repositoryId, string typeId)
        {
            // get the self relation of specified type to delete the type using this link
            {
                var withBlock = GetLink(repositoryId, typeId, LinkRelationshipTypes.Self, MediaTypes.Entry, _typeLinks, GetTypeDefinition);
                if (withBlock.Exception is null)
                {
                    // type found
                    return Delete(new Uri(withBlock.Response));
                }
                else
                {
                    // type not found
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.deleteTypeResponse> DeleteType(cm.Requests.deleteType request)
        {
            {
                var withBlock = DeleteType(request.RepositoryId, request.TypeId);
                if (withBlock.Exception is null)
                {
                    return new cmr.deleteTypeResponse();
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Returns all repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks>_serviceDocUri used
      /// This is the only method which uses a known uri (_serviceDocUri). From this point the client gets informations about
      /// the repositories supported by the server. From the instance of a repositoryInfoType the client is able to request for
      /// types and objects via the uri-templates ObjectById and TypeById. The given collectionInfos in the repositoryInfoType
      /// allows the direct access to checkedOut-, bulkUpdate-, types-, typedescendants-, unfiled- and query-collection.
      /// All the other methods of the cmis-service uses diverse links sent with every request to objects or types</remarks>
        public Generic.ResponseType<ca.AtomServiceDocument> GetRepositories()
        {
            return Get(_serviceDocUri, ca.AtomServiceDocument.CreateInstance);
        }
        /// <summary>
      /// Returns all repositories
      /// </summary>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getRepositoriesResponse> GetRepositories(cm.Requests.getRepositories request)
        {
            var response = GetRepositories();

            if (response.Exception is null)
            {
                return new cmr.getRepositoriesResponse() { Repositories = (from workspace in response.Response.Workspaces let ws = workspace as ca.AtomWorkspace where ws is not null select ((cm.cmisRepositoryEntryType)ws.RepositoryInfo)).ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the workspace of specified repository
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="ignoreCache">If True the method ignores cached repository informations</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomWorkspace> GetRepositoryInfo(string repositoryId, bool ignoreCache = false)
        {
            // try to get the info using the cache
            var ws = ignoreCache ? null : get_RepositoryInfo(repositoryId);

            // workspace of specified repository could not be found in the cache
            if (ws is null)
            {
                // in 3.7.1  HTTP GET it is defined that a client may add the repositoryId as an optional argument to the
                // AtomPub Service Document resource
                var serviceDocUri = new Uri(ServiceURIs.GetServiceUri(_serviceDocUri.OriginalString, ServiceURIs.enumRepositoriesUri.repositoryId).ReplaceUri("repositoryId", repositoryId));
                var response = Get(serviceDocUri, ca.AtomServiceDocument.CreateInstance);

                if (response.Exception is null)
                {
                    // store response into the cache
                    foreach (ca.AtomWorkspace currentWs in response.Response.Workspaces)
                    {
                        ws = currentWs;
                        WriteToCache(ws);
                    }
                    // try to get workspace
                    ws = get_RepositoryInfo(repositoryId);
                }
                else
                {
                    return response.Exception;
                }
            }

            if (ws is null)
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.NotFound, cm.enumServiceException.objectNotFound, "Workspace not found.");
                return cmisFault.ToFaultException();
            }
            else
            {
                return ws;
            }
        }
        /// <summary>
      /// Returns the workspace of specified repository or null
      /// </summary>
      /// <param name="ignoreCache">If True the method ignores cached repository informations</param>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getRepositoryInfoResponse> GetRepositoryInfo(cm.Requests.getRepositoryInfo request, bool ignoreCache = false)
        {
            var response = GetRepositoryInfo(request.RepositoryId, ignoreCache);

            if (response.Exception is null)
            {
                return new cmr.getRepositoryInfoResponse() { RepositoryInfo = response.Response.RepositoryInfo };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId">If null this function returns all base types, otherwise the children of the speciefied type</param>
      /// <param name="includePropertyDefinitions"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetTypeChildren(string repositoryId, string typeId = null, bool includePropertyDefinitions = false, long? maxItems = default, long? skipCount = default)
        {
            string link;

            if (string.IsNullOrEmpty(typeId))
            {
                // link to all base types using types-collectionInfo
                {
                    var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Types);
                    if (withBlock.Exception is null)
                    {
                        link = withBlock.Response.Link.OriginalString;
                    }
                    else
                    {
                        return withBlock.Exception;
                    }
                }
            }
            else
            {
                // link to children of specified type
                {
                    var withBlock1 = GetLink(repositoryId, typeId, LinkRelationshipTypes.Down, MediaTypes.Feed, _typeLinks, GetTypeDefinition);
                    if (withBlock1.Exception is null)
                    {
                        link = withBlock1.Response;
                    }
                    else
                    {
                        return withBlock1.Exception;
                    }
                }
            }
            // notice: the typeId-parameter is already handled (GetLink())
            {
                var withBlock2 = new ccg.LinkUriBuilder<ServiceURIs.enumTypesUri>(link, repositoryId);
                withBlock2.Add(ServiceURIs.enumTypesUri.includePropertyDefinitions, includePropertyDefinitions);
                withBlock2.Add(ServiceURIs.enumTypesUri.maxItems, maxItems);
                withBlock2.Add(ServiceURIs.enumTypesUri.skipCount, skipCount);

                var retVal = Get(withBlock2.ToUri(), ca.AtomFeed.CreateInstance);

                if (retVal.Exception is null)
                    WriteToCache(repositoryId, retVal.Response, _typeLinks);
                return retVal;
            }
        }
        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the specified type
      /// </summary>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getTypeChildrenResponse> GetTypeChildren(cm.Requests.getTypeChildren request)
        {
            var response = GetTypeChildren(request.RepositoryId, request.TypeId, request.IncludePropertyDefinitions.HasValue && request.IncludePropertyDefinitions.Value, request.MaxItems, request.SkipCount);

            if (response.Exception is null)
            {
                return new cmr.getTypeChildrenResponse() { Types = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks>uses uritemplate: TypeById</remarks>
        public Generic.ResponseType<ca.AtomEntry> GetTypeDefinition(string repositoryId, string typeId)
        {
            Generic.ResponseType<ca.AtomEntry> retVal;

            // ensure that the called repositoryId is available
            {
                var withBlock = GetRepositoryInfo(repositoryId);
                if (withBlock.Exception is null)
                {
                    string uriTemplate = withBlock.Response.get_UriTemplate(UriTemplates.TypeById).Template;
                    var state = _vendor.BeginRequest(repositoryId, ref typeId);

                    uriTemplate = CommonFunctions.ReplaceUriTemplate("id", typeId);
                    retVal = Get(new Uri(uriTemplate), ca.AtomEntry.CreateInstance);
                    if (retVal.Exception is null)
                    {
                        _vendor.EndRequest(state, retVal.Response.Type);
                        WriteToCache(repositoryId, typeId, retVal.Response, _typeLinks);
                    }
                }
                else
                {
                    retVal = withBlock.Exception;
                }
            }

            return retVal;
        }
        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getTypeDefinitionResponse> GetTypeDefinition(cm.Requests.getTypeDefinition request)
        {
            var response = GetTypeDefinition(request.RepositoryId, request.TypeId);

            if (response.Exception is null)
            {
                return new cmr.getTypeDefinitionResponse() { Type = response.Response.Type };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId">If null this function returns all types, otherwise the descendants of the speciefied type</param>
      /// <param name="depth"></param>
      /// <param name="includePropertyDefinitions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetTypeDescendants(string repositoryId, string typeId = null, long? depth = default, bool includePropertyDefinitions = false)
        {
            string link;

            if (string.IsNullOrEmpty(typeId))
            {
                // link to all types using typedescendants-collectionInfo
                {
                    var withBlock = GetRepositoryLink(repositoryId, LinkRelationshipTypes.TypeDescendants);
                    if (withBlock.Exception is null)
                    {
                        link = withBlock.Response;
                    }
                    else
                    {
                        return withBlock.Exception;
                    }
                }
            }
            else
            {
                // typedescendants of specified type
                {
                    var withBlock1 = GetLink(repositoryId, typeId, LinkRelationshipTypes.Down, MediaTypes.Tree, _typeLinks, GetTypeDefinition);
                    if (withBlock1.Exception is null)
                    {
                        link = withBlock1.Response;
                    }
                    else
                    {
                        return withBlock1.Exception;
                    }
                }
            }
            // notice: the typeId-parameter is already handled (GetLink())
            {
                var withBlock2 = new ccg.LinkUriBuilder<ServiceURIs.enumTypeDescendantsUri>(link, repositoryId);
                if (!string.IsNullOrEmpty(typeId))
                    withBlock2.Add(ServiceURIs.enumTypeDescendantsUri.depth, depth);
                withBlock2.Add(ServiceURIs.enumTypeDescendantsUri.includePropertyDefinitions, includePropertyDefinitions);

                var retVal = Get(withBlock2.ToUri(), ca.AtomFeed.CreateInstance);

                if (retVal.Exception is null)
                    WriteToCache(repositoryId, retVal.Response, _typeLinks);
                return retVal;
            }
        }
        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getTypeDescendantsResponse> GetTypeDescendants(cm.Requests.getTypeDescendants request)
        {
            var response = GetTypeDescendants(request.RepositoryId, request.TypeId, request.Depth, request.IncludePropertyDefinitions.HasValue && request.IncludePropertyDefinitions.Value);
            if (response.Exception is null)
            {
                return new cmr.getTypeDescendantsResponse() { Types = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let typeContainer = entry where typeContainer is not null select typeContainer).Cast<cmisTypeContainer>().ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> UpdateType(string repositoryId, Core.Definitions.Types.cmisTypeDefinitionType type)
        {
            if (type is null || string.IsNullOrEmpty(type.Id))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument type MUST NOT be null and type.Id MUST be set.");
                return cmisFault.ToFaultException();
            }
            // using self-link to modify type
            {
                var withBlock = GetLink(repositoryId, type.Id, LinkRelationshipTypes.Self, MediaTypes.Entry, _typeLinks, GetTypeDefinition);
                if (withBlock.Exception is null)
                {
                    return Put(new Uri(withBlock.Response), new sss.Atom10ItemFormatter(new ca.AtomEntry(type)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.updateTypeResponse> UpdateType(cm.Requests.updateType request)
        {
            var response = UpdateType(request.RepositoryId, request.Type);

            if (response.Exception is null)
            {
                return new cmr.updateTypeResponse() { Type = response.Response.Type };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetCheckedOutDocs(string repositoryId, string folderId = null, long? maxItems = default, long? skipCount = default, string orderBy = null, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default)
        {
            // the access to checkedOut-collection is defined by checkedOut-collectionInfo
            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.CheckedOut);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumCheckedOutUri>(withBlock.Response.Link.OriginalString, repositoryId);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.folderId, folderId);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.maxItems, maxItems);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.skipCount, skipCount);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.orderBy, orderBy);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.includeRelationships, includeRelationships);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.renditionFilter, renditionFilter);
                        withBlock1.Add(ServiceURIs.enumCheckedOutUri.includeAllowableActions, includeAllowableActions);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getCheckedOutDocsResponse> GetCheckedOutDocs(cm.Requests.getCheckedOutDocs request)
        {
            var response = GetCheckedOutDocs(request.RepositoryId, request.FolderId, request.MaxItems, request.SkipCount, request.OrderBy, request.Filter, request.IncludeRelationships, request.RenditionFilter, request.IncludeAllowableActions);
            if (response.Exception is null)
            {
                return new cmr.getCheckedOutDocsResponse() { Objects = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the list of child objects contained in the specified folder
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetChildren(string repositoryId, string folderId, long? maxItems = default, long? skipCount = default, string orderBy = null, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default, bool includePathSegment = false)
        {
            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.Down, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // notice: the folderId-parameter is already handled (GetLink())
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.maxItems, maxItems);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.skipCount, skipCount);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.orderBy, orderBy);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.includeRelationships, includeRelationships);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.renditionFilter, renditionFilter);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.includeAllowableActions, includeAllowableActions);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.includePathSegment, includePathSegment);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of child objects contained in the specified folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getChildrenResponse> GetChildren(cm.Requests.getChildren request)
        {
            var response = GetChildren(request.RepositoryId, request.FolderId, request.MaxItems, request.SkipCount, request.OrderBy, request.Filter, request.IncludeRelationships, request.RenditionFilter, request.IncludeAllowableActions, request.IncludePathSegment.HasValue && request.IncludePathSegment.Value);
            if (response.Exception is null)
            {
                return new cmr.getChildrenResponse() { Objects = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the set of descendant objects containded in the specified folder or any of its child-folders
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="depth"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includePathSegment"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetDescendants(string repositoryId, string folderId, long? depth = default, string filter = null, bool? includeAllowableActions = default, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool includePathSegment = false)
        {
            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.Down, MediaTypes.Tree, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // notice: the folderId-parameter is already handled (GetLink())
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumDescendantsUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.depth, depth);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.includeAllowableActions, includeAllowableActions);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.includeRelationships, includeRelationships);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.renditionFilter, renditionFilter);
                        withBlock1.Add(ServiceURIs.enumDescendantsUri.includePathSegment, includePathSegment);


                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the set of descendant objects containded in the specified folder or any of its child-folders
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getDescendantsResponse> GetDescendants(cm.Requests.getDescendants request)
        {
            var response = GetDescendants(request.RepositoryId, request.FolderId, request.Depth, request.Filter, request.IncludeAllowableActions, request.IncludeRelationships, request.RenditionFilter, request.IncludePathSegment.HasValue && request.IncludePathSegment.Value);

            if (response.Exception is null)
            {
                return new cmr.getDescendantsResponse() { Objects = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let objectInFolderContainer = entry where objectInFolderContainer is not null select objectInFolderContainer).Cast<cmisObjectInFolderContainerType>().ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the parent folder object for the specified folder object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> GetFolderParent(string repositoryId, string folderId, string filter = null)
        {
            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.Up, MediaTypes.Entry, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // notice: the folderId-parameter is already handled (GetLink())
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumObjectUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumObjectUri.filter, filter);

                        var state = new Vendors.Vendor.State(repositoryId);
                        var response = Get(withBlock1.ToUri(), ca.AtomEntry.CreateInstance);
                        string parentFolderId = response.Exception is null ? response.Response.ObjectId : null;
                        return TransformResponse(response, state, parentFolderId, !string.IsNullOrEmpty(parentFolderId));
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the parent folder object for the specified folder object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getFolderParentResponse> GetFolderParent(cm.Requests.getFolderParent request)
        {
            var response = GetFolderParent(request.RepositoryId, request.FolderId, request.Filter);

            if (response.Exception is null)
            {
                return new cmr.getFolderParentResponse() { Object = response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the set of descendant folder objects contained in the specified folder
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="depth"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includePathSegment"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetFolderTree(string repositoryId, string folderId, long? depth = default, string filter = null, bool? includeAllowableActions = default, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool includePathSegment = false)
        {
            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.FolderTree, MediaTypes.Tree, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // notice: the folderId-parameter is already handled (GetLink())
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumFolderTreeUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.depth, depth);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.includeAllowableActions, includeAllowableActions);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.includeRelationships, includeRelationships);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.renditionFilter, renditionFilter);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.includePathSegment, includePathSegment);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the set of descendant folder objects contained in the specified folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getFolderTreeResponse> GetFolderTree(cm.Requests.getFolderTree request)
        {
            var response = GetFolderTree(request.RepositoryId, request.FolderId, request.Depth, request.Filter, request.IncludeAllowableActions, request.IncludeRelationships, request.RenditionFilter, request.IncludePathSegment.HasValue && request.IncludePathSegment.Value);
            if (response.Exception is null)
            {
                return new cmr.getFolderTreeResponse() { Objects = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let objectInFolderContainer = entry where objectInFolderContainer is not null select objectInFolderContainer).Cast<cmisObjectInFolderContainerType>().ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the parent folder(s) for the specified fileable object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetObjectParents(string repositoryId, string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default, bool? includeRelativePathSegment = default)
        {
            // non folder object may have more than one parent if the repository supports multifiling. Therefore this function returns a
            // list (feed) of entries. To get the parent of a folder it is recommend to use GetFolderParent()
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Up, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // notice: the objectId-parameter is already handled (GetLink())
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumObjectParentsUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumObjectParentsUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumObjectParentsUri.includeRelationships, includeRelationships);
                        withBlock1.Add(ServiceURIs.enumObjectParentsUri.renditionFilter, renditionFilter);
                        withBlock1.Add(ServiceURIs.enumObjectParentsUri.includeAllowableActions, includeAllowableActions);
                        withBlock1.Add(ServiceURIs.enumObjectParentsUri.includeRelativePathSegment, includeRelativePathSegment);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the parent folder(s) for the specified fileable object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getObjectParentsResponse> GetObjectParents(cm.Requests.getObjectParents request)
        {
            var response = GetObjectParents(request.RepositoryId, request.ObjectId, request.Filter, request.IncludeRelationships, request.RenditionFilter, request.IncludeAllowableActions, request.IncludeRelativePathSegment);
            if (response.Exception is null)
            {
                return new cmr.getObjectParentsResponse() { Parents = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let parent = entry where parent is not null select parent).Cast<cmisObjectParentsType>().ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the specified document object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="contentStream"></param>
      /// <param name="isLastChunk"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks>uses editmedia-link</remarks>
        public Generic.ResponseType<cmr.appendContentStreamResponse> AppendContentStream(string repositoryId, string objectId, cm.cmisContentStreamType contentStream, bool isLastChunk = false, string changeToken = null)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }
            else if (contentStream is null || contentStream.BinaryStream is null)
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument contentStream MUST NOT be null.");
                return cmisFault.ToFaultException();
            }

            Dictionary<string, string> headers;

            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.EditMedia, contentStream.MimeType, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumContentUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumContentUri.append, true);
                        withBlock1.Add(ServiceURIs.enumContentUri.changeToken, changeToken);
                        withBlock1.Add(ServiceURIs.enumContentUri.isLastChunk, isLastChunk);

                        if (string.IsNullOrEmpty(contentStream.Filename))
                        {
                            headers = null;
                        }
                        else
                        {
                            // If the client wishes to set a new filename, it MAY add a Content-Disposition header, which carries the new filename.
                            // The disposition type MUST be "attachment". The repository SHOULD use the "filename" parameter and SHOULD ignore all other parameters
                            // see 3.11.8.2 HTTP PUT
                            headers = new Dictionary<string, string>() { { RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename) } };
                        }

                        return Put(withBlock1.ToUri(), contentStream.BinaryStream, contentStream.MimeType, headers, cmr.appendContentStreamResponse.CreateInstance);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Appends to the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.appendContentStreamResponse> AppendContentStream(cm.Requests.appendContentStream request)
        {
            var response = AppendContentStream(request.RepositoryId, request.ObjectId, request.ContentStream, request.IsLastChunk.HasValue && request.IsLastChunk.Value, request.ChangeToken);
            if (response.Exception is null)
            {
                if (response.Response is null)
                {
                    return new cmr.appendContentStreamResponse() { ObjectId = request.ObjectId, ChangeToken = request.ChangeToken };
                }
                else
                {
                    return new cmr.appendContentStreamResponse() { ObjectId = response.Response.ObjectId, ChangeToken = response.Response.ChangeToken };
                }
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectIdAndChangeTokens"></param>
      /// <param name="properties"></param>
      /// <param name="addSecondaryTypeIds"></param>
      /// <param name="removeSecondaryTypeIds"></param>
      /// <returns></returns>
      /// <remarks>uses collectionInfo for bulkUpdate
      /// see 3.8.6.1 HTTP POST:
      /// The property cmis:objectId MUST be set.
      /// The value MUST be the original object id even if the repository created a new version and therefore generated a new object id.
      /// New object ids are not exposed by AtomPub binding. 
      /// The property cmis:changeToken MUST be set if the repository supports change tokens
      /// </remarks>
        public Generic.ResponseType<ca.AtomFeed> BulkUpdateProperties(string repositoryId, Core.cmisObjectIdAndChangeTokenType[] objectIdAndChangeTokens, Core.Collections.cmisPropertiesType properties = null, string[] addSecondaryTypeIds = null, string[] removeSecondaryTypeIds = null)
        {
            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Update);
                if (withBlock.Exception is null)
                {
                    var bulkUpdate = new Core.cmisBulkUpdateType()
                    {
                        AddSecondaryTypeIds = addSecondaryTypeIds,
                        ObjectIdAndChangeTokens = objectIdAndChangeTokens,
                        Properties = properties,
                        RemoveSecondaryTypeIds = removeSecondaryTypeIds
                    };
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        return TransformResponse(Post(withBlock.Response.Link, new sss.Atom10ItemFormatter(new ca.AtomEntry(bulkUpdate)), MediaTypes.Entry, null, ca.AtomFeed.CreateInstance), state, false);
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <returns></returns>
      /// <remarks>see 3.8.6.1 HTTP POST:
      /// The property cmis:objectId MUST be set.
      /// The value MUST be the original object id even if the repository created a new version and therefore generated a new object id.
      /// New object ids are not exposed by AtomPub binding. 
      /// The property cmis:changeToken MUST be set if the repository supports change tokens
      /// </remarks>
        public override Generic.ResponseType<cmr.bulkUpdatePropertiesResponse> BulkUpdateProperties(cm.Requests.bulkUpdateProperties request)
        {
            var response = BulkUpdateProperties(request.RepositoryId, request.BulkUpdateData.ObjectIdAndChangeTokens, request.BulkUpdateData.Properties, request.BulkUpdateData.AddSecondaryTypeIds, request.BulkUpdateData.RemoveSecondaryTypeIds);
            if (response.Exception is null)
            {
                return new cmr.bulkUpdatePropertiesResponse() { ObjectIdAndChangeTokens = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let objectIdAndChangeToken = entry where objectIdAndChangeToken is not null select objectIdAndChangeToken).Cast<CmisObjectModel.Core.cmisObjectIdAndChangeTokenType>().ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createDocument-instance (detected by the accepted MediaTypes.Request-contentType of the unfiled- or
      /// root-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way to reduce
      /// communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateDocument(string repositoryId, Core.Collections.cmisPropertiesType properties, string folderId = null, cm.cmisContentStreamType contentStream = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            var acceptRequest = default(bool);

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(cmisObject.ObjectTypeId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectTypeId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetChildrenOrUnfiledLink(repositoryId, folderId, ref acceptRequest);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            withBlock1.Add(ServiceURIs.enumChildrenUri.versioningState, versioningState);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createDocument()
                                {
                                    AddACEs = addACEs,
                                    ContentStream = contentStream,
                                    FolderId = folderId,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId,
                                    VersioningState = versioningState
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                RestAtom.cmisContentType content = (RestAtom.cmisContentType)contentStream;
                                var entry = new ca.AtomEntry(cmisObject, content);
                                Dictionary<string, string> headers = null;

                                // transmit ContentStreamFileName, ContentStreamLength as property
                                if (contentStream is not null)
                                {
                                    contentStream.ExtendProperties(cmisObject);
                                    if (!string.IsNullOrEmpty(contentStream.Filename))
                                    {
                                        // If the client wishes to set a new filename, it MAY add a Content-Disposition header, which carries the new filename.
                                        // The disposition type MUST be "attachment". The repository SHOULD use the "filename" parameter and SHOULD ignore all other parameters
                                        // see 3.11.8.2 HTTP PUT
                                        headers = new Dictionary<string, string>() { { RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename) } };
                                    }
                                }
                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, headers, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createDocumentResponse> CreateDocument(cm.Requests.createDocument request)
        {
            var response = CreateDocument(request.RepositoryId, request.Properties, request.FolderId, request.ContentStream, request.VersioningState, request.Policies, request.AddACEs, request.RemoveACEs);
            if (response.Exception is null)
            {
                return new cmr.createDocumentResponse() { Object = response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="sourceId"></param>
      /// <param name="properties"></param>
      /// <param name="folderId"></param>
      /// <param name="versioningState"></param>
      /// <param name="policies"></param>
      /// <param name="addACEs"></param>
      /// <param name="removeACEs"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createDocumentFromSource-instance (detected by the accepted MediaTypes.Request-contentType of the
      /// unfiled- or root-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way
      /// to reduce communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateDocumentFromSource(string repositoryId, string sourceId, Core.Collections.cmisPropertiesType properties = null, string folderId = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            var acceptRequest = default(bool);

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetChildrenOrUnfiledLink(repositoryId, folderId, ref acceptRequest);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            withBlock1.Add(ServiceURIs.enumChildrenUri.sourceId, sourceId);
                            withBlock1.Add(ServiceURIs.enumChildrenUri.versioningState, versioningState);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createDocumentFromSource()
                                {
                                    AddACEs = addACEs,
                                    FolderId = folderId,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId,
                                    SourceId = sourceId,
                                    VersioningState = versioningState
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                var entry = new ca.AtomEntry(cmisObject);

                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createDocumentFromSourceResponse> CreateDocumentFromSource(cm.Requests.createDocumentFromSource request)
        {
            var response = CreateDocumentFromSource(request.RepositoryId, request.SourceId, request.Properties, request.FolderId, request.VersioningState, request.Policies, request.AddACEs, request.RemoveACEs);
            if (response.Exception is null)
            {
                return new cmr.createDocumentFromSourceResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Creates a folder object of the specified type (given by the cmis:objectTypeId property) in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createFolder-instance (detected by the accepted MediaTypes.Request-contentType of the
      /// root-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way to reduce
      /// communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateFolder(string repositoryId, Core.Collections.cmisPropertiesType properties, string folderId, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            var acceptRequest = default(bool);

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(cmisObject.ObjectTypeId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectTypeId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(folderId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument folderId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetChildrenOrUnfiledLink(repositoryId, folderId, ref acceptRequest);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createFolder()
                                {
                                    AddACEs = addACEs,
                                    FolderId = folderId,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                var entry = new ca.AtomEntry(cmisObject);

                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a folder object of the specified type (given by the cmis:objectTypeId property) in the specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createFolderResponse> CreateFolder(cm.Requests.createFolder request)
        {
            var response = CreateFolder(request.RepositoryId, request.Properties, request.FolderId, request.Policies, request.AddACEs, request.RemoveACEs);
            if (response.Exception is null)
            {
                return new cmr.createFolderResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Creates a item object of the specified type (given by the cmis:objectTypeId property) in (optionally) the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createItem-instance (detected by the accepted MediaTypes.Request-contentType of the unfiled- or
      /// root-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way to reduce
      /// communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateItem(string repositoryId, Core.Collections.cmisPropertiesType properties, string folderId = null, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            var acceptRequest = default(bool);

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(cmisObject.ObjectTypeId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectTypeId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetChildrenOrUnfiledLink(repositoryId, folderId, ref acceptRequest);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createItem()
                                {
                                    AddACEs = addACEs,
                                    FolderId = folderId,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                var entry = new ca.AtomEntry(cmisObject);

                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a item object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createItemResponse> CreateItem(cm.Requests.createItem request)
        {
            var response = CreateItem(request.RepositoryId, request.Properties, request.FolderId, request.Policies, request.AddACEs, request.RemoveACEs);

            if (response.Exception is null)
            {
                return new cmr.createItemResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Creates a policy object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createPolicy-instance (detected by the accepted MediaTypes.Request-contentType of the unfiled- or
      /// root-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way to reduce
      /// communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreatePolicy(string repositoryId, Core.Collections.cmisPropertiesType properties, string folderId = null, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            var acceptRequest = default(bool);

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(cmisObject.ObjectTypeId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectTypeId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetChildrenOrUnfiledLink(repositoryId, folderId, ref acceptRequest);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createPolicy()
                                {
                                    AddACEs = addACEs,
                                    FolderId = folderId,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                var entry = new ca.AtomEntry(cmisObject);

                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a policy object of the specified type (given by the cmis:objectTypeId property) in (optionally) the specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createPolicyResponse> CreatePolicy(cm.Requests.createPolicy request)
        {
            var response = CreatePolicy(request.RepositoryId, request.Properties, request.FolderId, request.Policies, request.AddACEs, request.RemoveACEs);

            if (response.Exception is null)
            {
                return new cmr.createPolicyResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Creates a relationship object of the specified type (given by the cmis:objectTypeId property)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>addACEs and removeACEs must be written in a second roundtrip except the repository allows to transfer a
      /// Messaging.Request.createRelationship-instance (detected by the accepted MediaTypes.Request-contentType of the
      /// relationships-collectionInfo). If the mediatype MediaTypes.Request is allowed this function will prefer this way to reduce
      /// communication with the server.</remarks>
        public Generic.ResponseType<ca.AtomEntry> CreateRelationship(string repositoryId, Core.Collections.cmisPropertiesType properties, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties, PolicyIds = policies };
            bool acceptRequest = CollectionAccepts(repositoryId, CollectionInfos.Relationships, MediaTypes.Request);
            var sourceIdProperty = properties is null ? null : properties.FindProperty(CmisPredefinedPropertyNames.SourceId) as Core.Properties.cmisPropertyId;
            string sourceId = sourceIdProperty is null ? null : sourceIdProperty.Value;

            if (!string.IsNullOrEmpty(cmisObject.ObjectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectId + "' MUST NOT be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(cmisObject.ObjectTypeId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.ObjectTypeId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(sourceId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Property '" + CmisPredefinedPropertyNames.SourceId + "' MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, sourceId, LinkRelationshipTypes.Relationships, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    Generic.ResponseType<ca.AtomEntry> response;
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                            if (acceptRequest)
                            {
                                // the server accepts Messaging.Requests
                                var request = new cm.Requests.createRelationship()
                                {
                                    AddACEs = addACEs,
                                    Policies = policies is null ? null : policies.Ids,
                                    Properties = properties,
                                    RemoveACEs = removeACEs,
                                    RepositoryId = repositoryId
                                };
                                response = Post(withBlock1.ToUri(), request, MediaTypes.Request, null, ca.AtomEntry.CreateInstance);
                            }
                            else
                            {
                                var entry = new ca.AtomEntry(cmisObject);

                                response = Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(entry), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance);
                                // modify acl in separate roundtrip
                                if (response.Exception is null && !(addACEs is null || removeACEs is null))
                                    ApplyAcl(repositoryId, response.Response.ObjectId, addACEs, removeACEs);
                            }
                            return TransformResponse(response, state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Creates a relationship object of the specified type (given by the cmis:objectTypeId property)
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.createRelationshipResponse> CreateRelationship(cm.Requests.createRelationship request)
        {
            var response = CreateRelationship(request.RepositoryId, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs);

            if (response.Exception is null)
            {
                return new cmr.createRelationshipResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Deletes the content stream for the specified document object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks>uses editmedia-link</remarks>
        public Generic.ResponseType<cmr.deleteContentStreamResponse> DeleteContentStream(string repositoryId, string objectId, string changeToken = null)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumContentUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumContentUri.changeToken, changeToken);

                        return Delete(withBlock1.ToUri(), cmr.deleteContentStreamResponse.CreateInstance);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Deletes the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.deleteContentStreamResponse> DeleteContentStream(cm.Requests.deleteContentStream request)
        {
            return DeleteContentStream(request.RepositoryId, request.ObjectId, request.ChangeToken);
        }

        /// <summary>
      /// Deletes the specified object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="allVersions"></param>
      /// <returns></returns>
      /// <remarks>uses self-link</remarks>
        public Response DeleteObject(string repositoryId, string objectId, bool allVersions = true)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Self, MediaTypes.Entry, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumObjectUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumObjectUri.allVersions, allVersions);
                        var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, repositoryId, EventBus.enumBuiltInEvents.DeleteObject, objectId);
                        var retVal = Delete(withBlock1.ToUri());

                        if (retVal.Exception is null)
                        {
                            e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true } });
                        }
                        else
                        {
                            e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, retVal.Exception } });
                        }
                        return retVal;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Deletes the specified object
      /// </summary>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.deleteObjectResponse> DeleteObject(cm.Requests.deleteObject request)
        {
            {
                var withBlock = DeleteObject(request.RepositoryId, request.ObjectId, !request.AllVersions.HasValue || request.AllVersions.Value);
                if (withBlock.Exception is null)
                {
                    return new cmr.deleteObjectResponse();
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects
      /// </summary>
      /// <returns></returns>
      /// <remarks>In contrast to the implementation of DeleteTree in DotCMIS (Apache Chemistry) solely the
      /// Folder Tree Feed Resource is used accordingly to the cmis AtomPub-Binding specifications.</remarks>
        public Generic.ResponseType<cm.failedToDelete> DeleteTree(string repositoryId, string folderId, bool allVersions = true, Core.enumUnfileObject unfileObjects = Core.enumUnfileObject.delete, bool continueOnFailure = false)
        {
            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.FolderTree, MediaTypes.Tree, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumFolderTreeUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.allVersions, allVersions);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.unfileObjects, unfileObjects);
                        withBlock1.Add(ServiceURIs.enumFolderTreeUri.continueOnFailure, continueOnFailure);

                        {
                            var withBlock2 = Delete(withBlock1.ToUri());
                            if (withBlock2.Exception is null)
                            {
                                // no error
                                return new cm.failedToDelete();
                            }
                            else if (withBlock2.StatusCode == sn.HttpStatusCode.InternalServerError)
                            {
                                // this exception must be returned if something failed beyond this point
                                var exception = withBlock2.Exception;

                                // get a list of max. 1000 child-objects which deletion failed
                                {
                                    var withBlock3 = GetLink(repositoryId, folderId, LinkRelationshipTypes.Down, MediaTypes.Tree, _objectLinks, GetObjectLinksOnly);
                                    if (withBlock3.Exception is null)
                                    {
                                        {
                                            var withBlock4 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock3.Response, repositoryId);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.filter, CmisPredefinedPropertyNames.ObjectId);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.includeAllowableActions, false);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.includeRelationships, Core.enumIncludeRelationships.none);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.renditionFilter, "cmis:none");
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.includePathSegment, false);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.maxItems, 1000);
                                            withBlock4.Add(ServiceURIs.enumChildrenUri.skipCount, 0);

                                            {
                                                var withBlock5 = Get(withBlock4.ToUri(), ca.AtomFeed.CreateInstance);
                                                if (withBlock5.Exception is null)
                                                {
                                                    var retVal = new cm.failedToDelete();

                                                    retVal.ObjectIds = (from entry in withBlock5.Response.Entries
                                                                        where entry is not null
                                                                        select entry.ObjectId).ToArray();
                                                    return new Generic.ResponseType<cm.failedToDelete>(retVal, MediaTypes.Xml, exception);
                                                }
                                                else
                                                {
                                                    return exception;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return exception;
                                    }
                                }
                            }
                            else
                            {
                                // other error
                                return withBlock2.Exception;
                            }
                        }
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.deleteTreeResponse> DeleteTree(cm.Requests.deleteTree request)
        {
            var response = DeleteTree(request.RepositoryId, request.FolderId, !request.AllVersions.HasValue || request.AllVersions.Value, request.UnfileObjects.HasValue ? request.UnfileObjects.Value : default, request.ContinueOnFailure.HasValue && request.ContinueOnFailure.Value);
            if (response.Exception is null)
            {
                return new cmr.deleteTreeResponse() { FailedToDelete = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the list of allowable actions for an object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks>uses allowableactions-link</remarks>
        public Generic.ResponseType<Core.cmisAllowableActionsType> GetAllowableActions(string repositoryId, string objectId)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.AllowableActions, MediaTypes.AllowableActions, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumAllowableActionsUri>(withBlock.Response, repositoryId);
                        return Get(withBlock1.ToUri(), reader =>
                              {
                                  var retVal = new Core.cmisAllowableActionsType();
                                  retVal.ReadXml(reader);
                                  return retVal;
                              });
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of allowable actions for an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getAllowableActionsResponse> GetAllowableActions(cm.Requests.getAllowableActions request)
        {
            var response = GetAllowableActions(request.RepositoryId, request.ObjectId);

            if (response.Exception is null)
            {
                return new cmr.getAllowableActionsResponse() { AllowableActions = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the content stream for the specified document object, or gets a rendition stream for a specified rendition of a document or folder object.
      /// Note: Each CMIS protocol binding MAY provide a way for fetching a sub-range within a content stream, in a manner appropriate to that protocol
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="streamId"></param>
      /// <param name="offset"></param>
      /// <param name="length"></param>
      /// <returns></returns>
      /// <remarks>uses contentstream link-relationship</remarks>
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Public Overloads Function GetContentStream(repositoryId As String, objectId As String,
                                                         Optional streamId As String = Nothing,
                                                         Optional offset As Integer? = Nothing,
                                                         Optional length As Integer? = Nothing) As Response
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        public Response GetContentStream(string repositoryId, string objectId, string streamId = null, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            Generic.ResponseType<string> linkResponse;

            // first chance: content link of the entry
            linkResponse = GetLink(repositoryId, objectId, LinkRelationshipTypes.ContentStream, null, _objectLinks, GetObjectLinksOnly);
            if (linkResponse.Exception is not null)
            {
                // second chance: edit-media-link
                // see 3.4.3.1 Existing Link Relations; edit-media:
                // When used on a CMIS document resource, this link relation MUST point to the URI for content stream of the CMIS document.
                // This URI MUST be used to set or delete the content stream. This URI MAY be used to retrieve the content stream for the document.
                linkResponse = GetLink(repositoryId, objectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, null, "No content stream.");
            }
            // contentstream link-relationship, but unspecified mediatype
            if (linkResponse.Exception is null)
            {
                {
                    var withBlock = new ccg.LinkUriBuilder<ServiceURIs.enumContentUri>(linkResponse.Response, repositoryId);
                    withBlock.Add(ServiceURIs.enumContentUri.streamId, streamId);
                    return Get(withBlock.ToUri(), offset, length);
                }
            }
            else
            {
                return linkResponse.Exception;
            }
        }
        /// <summary>
      /// Gets the content stream for the specified document object, or gets a rendition stream for a specified rendition of a document or folder object.
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getContentStreamResponse> GetContentStream(cm.Requests.getContentStream request)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia
            #If HttpRequestAddRangeShortened Then
            *//* TODO ERROR: Skipped DisabledTextTrivia
                     'Range properties (offset, length) of a HttpWebRequest are defined as Integer (see System.Net.HttpWebRequest-class),
                     'therefore the xs_Integer has to be transformed
                     Dim offset As Integer?
                     Dim length As Integer?

                     If request.Offset.HasValue Then
                        offset = CInt(request.Offset.Value)
                     Else
                        offset = Nothing
                     End If
                     If request.Length.HasValue Then
                        length = CInt(request.Length.Value)
                     Else
                        length = Nothing
                     End If

                     Dim response = GetContentStream(request.RepositoryId, request.ObjectId, request.StreamId, offset, length)
            *//* TODO ERROR: Skipped ElseDirectiveTrivia
            #Else
            */
            var response = GetContentStream(request.RepositoryId, request.ObjectId, request.StreamId, request.Offset, request.Length);
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            if (response.Exception is null)
            {
                // Maybe the filename is sent via Content-Disposition
                var headers = response.WebResponse is null ? null : response.WebResponse.Headers;
                string disposition = null;
                string fileName = headers is null ? null : RFC2231Helper.DecodeContentDisposition(headers[RFC2231Helper.ContentDispositionHeaderName], ref disposition);

                return new cmr.getContentStreamResponse() { ContentStream = new cm.cmisContentStreamType(response.Stream, fileName, response.ContentType) };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the uri to get the content of a cmisDocument
      /// </summary>
        public override Generic.ResponseType<string> GetContentStreamLink(string repositoryId, string objectId, string streamId = null)
        {
            Generic.ResponseType<string> retVal;

            // first chance: content link of the entry
            retVal = GetLink(repositoryId, objectId, LinkRelationshipTypes.ContentStream, null, _objectLinks, GetObjectLinksOnly);
            if (retVal.Exception is not null)
            {
                // second chance: edit-media-link
                // see 3.4.3.1 Existing Link Relations; edit-media:
                // When used on a CMIS document resource, this link relation MUST point to the URI for content stream of the CMIS document.
                // This URI MUST be used to set or delete the content stream. This URI MAY be used to retrieve the content stream for the document.
                retVal = GetLink(repositoryId, objectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, null, "No content stream.");
            }

            if (retVal.Exception is null)
            {
                {
                    var withBlock = new ccg.LinkUriBuilder<ServiceURIs.enumContentUri>(retVal.Response, repositoryId);
                    withBlock.Add(ServiceURIs.enumContentUri.streamId, streamId);
                    return withBlock.ToUri().AbsoluteUri;
                }
            }
            else
            {
                return retVal;
            }
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> GetObject(string repositoryId, string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            return GetObjectCore(repositoryId, objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions);
        }
        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getObjectResponse> GetObject(cm.Requests.getObject request)
        {
            var response = GetObject(request.RepositoryId, request.ObjectId, request.Filter, request.IncludeRelationships, request.IncludePolicyIds, request.RenditionFilter, request.IncludeACL, request.IncludeAllowableActions);
            if (response.Exception is null)
            {
                return new cmr.getObjectResponse() { Object = response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }
        private Generic.ResponseType<ca.AtomEntry> GetObjectCore(string repositoryId, string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, RestAtom.enumReturnVersion? returnVersion = default)
        {
            // ensure that the called repositoryId is available
            {
                var withBlock = GetRepositoryInfo(repositoryId);
                if (withBlock.Exception is null)
                {
                    string uriTemplate = withBlock.Response.get_UriTemplate(UriTemplates.ObjectById).Template;
                    var state = new Vendors.Vendor.State(repositoryId);

                    uriTemplate = CommonFunctions.ReplaceUriTemplate("id", objectId, "filter", filter, "includeRelationships", includeRelationships.HasValue ? includeRelationships.Value.GetName() : null, "includePolicyIds", CommonFunctions.Convert(includePolicyIds), "renditionFilter", renditionFilter, "includeACL", CommonFunctions.Convert(includeACL), "includeAllowableActions", CommonFunctions.Convert(includeAllowableActions), "returnVersion", returnVersion.HasValue ? returnVersion.Value.GetName() : null);
                    return TransformResponse(Get(new Uri(uriTemplate), ca.AtomEntry.CreateInstance), state, objectId);
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the links for the object and the cmis:objectId-property
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<ca.AtomEntry> GetObjectLinksOnly(string repositoryId, string objectId)
        {
            return GetObjectCore(repositoryId, objectId, CmisPredefinedPropertyNames.ObjectId, Core.enumIncludeRelationships.none, false, "cmis:none", false, false);
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="path"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks>uses the ObjectByPath-uritemplate</remarks>
        public Generic.ResponseType<ca.AtomEntry> GetObjectByPath(string repositoryId, string path, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            // ensure that the called repositoryId is available
            {
                var withBlock = GetRepositoryInfo(repositoryId);
                if (withBlock.Exception is null)
                {
                    var state = new Vendors.Vendor.State(repositoryId);
                    string uriTemplate = withBlock.Response.get_UriTemplate(UriTemplates.ObjectByPath).Template;

                    uriTemplate = CommonFunctions.ReplaceUriTemplate("path", path, "filter", filter, "includeRelationships", includeRelationships.HasValue ? includeRelationships.Value.GetName() : null, "includePolicyIds", CommonFunctions.Convert(includePolicyIds), "renditionFilter", renditionFilter, "includeACL", CommonFunctions.Convert(includeACL), "includeAllowableActions", CommonFunctions.Convert(includeAllowableActions));
                    return TransformResponse(Get(new Uri(uriTemplate), ca.AtomEntry.CreateInstance), state);
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getObjectByPathResponse> GetObjectByPath(cm.Requests.getObjectByPath request)
        {
            var response = GetObjectByPath(request.RepositoryId, request.Path, request.Filter, request.IncludeRelationships, request.IncludePolicyIds, request.RenditionFilter, request.IncludeACL, request.IncludeAllowableActions);
            if (response.Exception is null)
            {
                return new cmr.getObjectByPathResponse() { Object = response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the list of properties for the object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks>A subset of GetObject</remarks>
        public Generic.ResponseType<Core.Collections.cmisPropertiesType> GetProperties(string repositoryId, string objectId, string filter = null)
        {
            {
                var withBlock = GetObjectCore(repositoryId, objectId, filter, Core.enumIncludeRelationships.none, false, "cmis:none", false, false);
                if (withBlock.Exception is null)
                {
                    var cmisraObject = withBlock.Response is null ? null : withBlock.Response.Object;

                    return cmisraObject is null ? null : cmisraObject.Properties;
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of properties for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getPropertiesResponse> GetProperties(cm.Requests.getProperties request)
        {
            var response = GetProperties(request.RepositoryId, request.ObjectId, request.Filter);

            if (response.Exception is null)
            {
                return new cmr.getPropertiesResponse() { Properties = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the list of associated renditions for the specified object. Only rendition attributes are returned, not rendition stream
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <returns></returns>
      /// <remarks>A subset of GetObject</remarks>
        public Generic.ResponseType<Core.cmisRenditionType[]> GetRenditions(string repositoryId, string objectId, string renditionFilter = null, long? maxItems = default, long? skipCount = default)
        {
            {
                var withBlock = GetObjectCore(repositoryId, objectId, CmisPredefinedPropertyNames.ObjectId, Core.enumIncludeRelationships.none, false, renditionFilter, false, false);
                if (withBlock.Exception is null)
                {
                    var cmisraObject = withBlock.Response is null ? null : withBlock.Response.Object;
                    var renditions = cmisraObject is null ? null : cmisraObject.Renditions;

                    if (renditions is not null && (maxItems.HasValue || skipCount.HasValue))
                    {
                        long nMaxItems = maxItems.HasValue ? maxItems.Value : long.MaxValue;
                        long nSkipCount = skipCount.HasValue ? skipCount.Value : 0L;
                        long length;

                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                        *//* TODO ERROR: Skipped DisabledTextTrivia
                                          length = renditions.Length
                        *//* TODO ERROR: Skipped ElseDirectiveTrivia
                        #Else
                        */
                        length = renditions.LongLength;
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        if (nSkipCount >= length)
                        {
                            renditions = new Core.cmisRenditionType[] { };
                        }
                        else
                        {
                            long newLength = Math.Min(length - nSkipCount, nMaxItems);
                            Core.cmisRenditionType[] cutout = (Core.cmisRenditionType[])Array.CreateInstance(typeof(Core.cmisRenditionType), newLength);
                            Array.Copy(renditions, nSkipCount, cutout, 0L, newLength);
                            renditions = cutout;
                        }
                    }

                    return renditions;
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of associated renditions for the specified object. Only rendition attributes are returned, not rendition stream
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getRenditionsResponse> GetRenditions(cm.Requests.getRenditions request)
        {
            var response = GetRenditions(request.RepositoryId, request.ObjectId, request.RenditionFilter, request.MaxItems, request.SkipCount);

            if (response.Exception is null)
            {
                return new cmr.getRenditionsResponse() { Renditions = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="targetFolderId"></param>
      /// <param name="sourceFolderId"></param>
      /// <returns></returns>
      /// <remarks>uses the link to the children-collection of the targetFolder</remarks>
        public Generic.ResponseType<ca.AtomEntry> MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }
            else if (string.IsNullOrEmpty(targetFolderId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument targetFolderId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, targetFolderId, LinkRelationshipTypes.Down, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.sourceFolderId, sourceFolderId);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(new ca.AtomEntry(objectId)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.moveObjectResponse> MoveObject(cm.Requests.moveObject request)
        {
            var response = MoveObject(request.RepositoryId, request.ObjectId, request.TargetFolderId, request.SourceFolderId);

            if (response.Exception is null)
            {
                return new cmr.moveObjectResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Sets the content stream for the specified document object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="contentStream"></param>
      /// <param name="overwriteFlag"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks>uses editmedia-link</remarks>
        public Generic.ResponseType<cmr.setContentStreamResponse> SetContentStream(string repositoryId, string objectId, cm.cmisContentStreamType contentStream, bool overwriteFlag = true, string changeToken = null)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }
            else if (contentStream is null || contentStream.BinaryStream is null)
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument contentStream MUST NOT be null.");
                return cmisFault.ToFaultException();
            }

            Dictionary<string, string> headers;

            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumContentUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumContentUri.changeToken, changeToken);
                        withBlock1.Add(ServiceURIs.enumContentUri.overwriteFlag, overwriteFlag);

                        if (string.IsNullOrEmpty(contentStream.Filename))
                        {
                            headers = null;
                        }
                        else
                        {
                            // If the client wishes to set a new filename, it MAY add a Content-Disposition header, which carries the new filename.
                            // The disposition type MUST be "attachment". The repository SHOULD use the "filename" parameter and SHOULD ignore all other parameters
                            // see 3.11.8.2 HTTP PUT
                            headers = new Dictionary<string, string>() { { RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename) } };
                        }
                        return Put(withBlock1.ToUri(), contentStream.BinaryStream, contentStream.MimeType, headers, cmr.setContentStreamResponse.CreateInstance);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Sets the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.setContentStreamResponse> SetContentStream(cm.Requests.setContentStream request)
        {
            return SetContentStream(request.RepositoryId, request.ObjectId, request.ContentStream, !request.OverwriteFlag.HasValue || request.OverwriteFlag.Value, request.ChangeToken);
        }

        /// <summary>
      /// Updates properties and secondary types of the specified object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="properties"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> UpdateProperties(string repositoryId, string objectId, Core.Collections.cmisPropertiesType properties, string changeToken = null)
        {
            var cmisObject = new Core.cmisObjectType() { Properties = properties };

            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Self, MediaTypes.Entry, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    var state = TransformRequest(repositoryId, properties);

                    try
                    {
                        {
                            var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumObjectUri>(withBlock.Response, repositoryId);
                            withBlock1.Add(ServiceURIs.enumObjectUri.changeToken, changeToken);
                            return TransformResponse(Put(withBlock1.ToUri(), new sss.Atom10ItemFormatter(new ca.AtomEntry(cmisObject)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state);
                        }
                    }
                    finally
                    {
                        if (state is not null)
                            state.Rollback();
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Updates properties and secondary types of the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.updatePropertiesResponse> UpdateProperties(cm.Requests.updateProperties request)
        {
            var response = UpdateProperties(request.RepositoryId, request.ObjectId, request.Properties, request.ChangeToken);

            if (response.Exception is null)
            {
                return new cmr.updatePropertiesResponse() { ObjectId = response.Response.ObjectId, ChangeToken = response.Response.ChangeToken };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="folderId"></param>
      /// <param name="allVersions"></param>
      /// <returns></returns>
      /// <remarks>uses children-collection of specified folder</remarks>
        public Generic.ResponseType<ca.AtomEntry> AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions = true)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, folderId, LinkRelationshipTypes.Down, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChildrenUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumChildrenUri.allVersions, allVersions);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(new ca.AtomEntry(objectId)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state, writeToCache: false);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.addObjectToFolderResponse> AddObjectToFolder(cm.Requests.addObjectToFolder request)
        {
            var response = AddObjectToFolder(request.RepositoryId, request.ObjectId, request.FolderId, !request.AllVersions.HasValue || request.AllVersions.Value);

            if (response.Exception is null)
            {
                return new cmr.addObjectToFolderResponse() { Object = response.Response is null ? null : response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="folderId"></param>
      /// <returns></returns>
      /// <remarks>uses unfiled-collectionInfo</remarks>
        public Generic.ResponseType<ca.AtomEntry> RemoveObjectFromFolder(string repositoryId, string objectId, string folderId = null)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Unfiled);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumUnfiledUri>(withBlock.Response.Link.OriginalString, repositoryId);
                        withBlock1.Add(ServiceURIs.enumUnfiledUri.folderId, folderId);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Post(withBlock1.ToUri(), new sss.Atom10ItemFormatter(new ca.AtomEntry(objectId)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state, writeToCache: false);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Removes an existing fileable non-folder object from a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.removeObjectFromFolderResponse> RemoveObjectFromFolder(cm.Requests.removeObjectFromFolder request)
        {
            var response = RemoveObjectFromFolder(request.RepositoryId, request.ObjectId, request.FolderId);

            if (response.Exception is null)
            {
                return new cmr.removeObjectFromFolderResponse() { Object = response.Response is null ? null : response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Discovery
        /// <summary>
      /// Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to
      /// efficiently understand what has changed in the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks>uses changes-link of the specified repository</remarks>
        public Generic.ResponseType<ca.AtomFeed> GetContentChanges(string repositoryId, string filter = null, string changeLogToken = null, bool includeProperties = false, bool includePolicyIds = false, bool? includeACL = default, long? maxItems = default)
        {
            {
                var withBlock = GetRepositoryLink(repositoryId, LinkRelationshipTypes.Changes);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumChangesUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumChangesUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumChangesUri.changeLogToken, changeLogToken);
                        withBlock1.Add(ServiceURIs.enumChangesUri.includeProperties, includeProperties);
                        withBlock1.Add(ServiceURIs.enumChangesUri.includePolicyIds, includePolicyIds);
                        withBlock1.Add(ServiceURIs.enumChangesUri.includeACL, includeACL);
                        withBlock1.Add(ServiceURIs.enumChangesUri.maxItems, maxItems);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state, false);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to
      /// efficiently understand what has changed in the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks>Notice:
      /// The ChangeLog Token is specified in the URI specified by the paging link notations.
      /// Through the AtomPub binding it is not possible to retrieve the ChangeLog Token from the URIs</remarks>
        public override Generic.ResponseType<cmr.getContentChangesResponse> GetContentChanges(cm.Requests.getContentChanges request)
        {
            var response = GetContentChanges(request.RepositoryId, request.Filter, request.ChangeLogToken, request.IncludeProperties.HasValue && request.IncludeProperties.Value, request.IncludePolicyIds.HasValue && request.IncludePolicyIds.Value, request.IncludeACL.HasValue && request.IncludeACL.Value, request.MaxItems);
            if (response.Exception is null)
            {
                // through this binding it is not possible to retrieve the ChangeLog Token from the URIs
                return new cmr.getContentChangesResponse() { Objects = response.Response, ChangeLogToken = null };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Executes a CMIS query statement against the contents of the repository
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="statement"></param>
      /// <param name="searchAllVersions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <returns></returns>
      /// <remarks>uses query-collectionInfo; another way to implement this method is to use
      /// the query-uritemplate as described in 3.7.1.4 Query</remarks>
        public Generic.ResponseType<ca.AtomFeed> Query(string repositoryId, string statement, bool? searchAllVersions = default, Core.enumIncludeRelationships includeRelationships = Core.enumIncludeRelationships.none, string renditionFilter = null, bool includeAllowableActions = false, long? maxItems = default, long? skipCount = default)
        {
            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Query);
                if (withBlock.Exception is null)
                {
                    var uri = withBlock.Response.Link;

                    {
                        var withBlock1 = new Core.cmisQueryType()
                        {
                            Statement = statement,
                            SearchAllVersions = searchAllVersions,
                            IncludeRelationships = includeRelationships,
                            RenditionFilter = renditionFilter,
                            IncludeAllowableActions = includeAllowableActions,
                            MaxItems = maxItems,
                            SkipCount = skipCount
                        };
                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Post(uri, withBlock1.Self(), MediaTypes.Query, null, ca.AtomFeed.CreateInstance), state, false);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Executes a CMIS query statement against the contents of the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.queryResponse> Query(cm.Requests.query request)
        {
            var response = Query(request.RepositoryId, request.Statement, request.SearchAllVersions, request.IncludeRelationships.HasValue ? request.IncludeRelationships.Value : Core.enumIncludeRelationships.none, request.RenditionFilter, request.IncludeRelationships.HasValue && request.IncludeAllowableActions.Value, request.MaxItems, request.SkipCount);
            if (response.Exception is null)
            {
                return new cmr.queryResponse() { Objects = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents
      /// in the version series to be checked out again. If the private working copy has been created by createDocument, cancelCheckOut MUST
      /// delete the created document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="pwcLinkRequired">If False this method uses the self-link if no workingcopy-link exists. Caution: this behaviour is
      /// equivalent to a DeleteObject(repositoryId, objectId)-call and should be used for non-compliant repositories only. If the given
      /// objectId-parameter contains a private working copy id this parameter should be False or better: call DeleteObject().</param>
      /// <returns></returns>
      /// <remarks>Following the implementation in DotCMIS (Apache Chemistry) this method prefers the usage of the workingcopy-link, but
      /// also supports the deletion of the object if neither a workingcopy is found nor is required</remarks>
        public Response CancelCheckOut(string repositoryId, string objectId, bool pwcLinkRequired = true)
        {
            string link;

            // object must exists!
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Self, MediaTypes.Entry, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is not null)
                {
                    return withBlock.Exception;
                }
                else if (!pwcLinkRequired)
                {
                    link = withBlock.Response;
                }
                else
                {
                    link = null;
                }
            }
            // prefer working copy link if available (workaround for non-compliant repositories)
            {
                var withBlock1 = GetLink(repositoryId, objectId, LinkRelationshipTypes.WorkingCopy, MediaTypes.Entry, _objectLinks, null);
                if (withBlock1.Exception is null)
                {
                    link = withBlock1.Response;
                }
                else if (link is null)
                {
                    return withBlock1.Exception;
                }
            }

            var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, repositoryId, EventBus.enumBuiltInEvents.CancelCheckout, objectId);
            var retVal = Delete(new Uri(link));

            if (retVal.Exception is null)
            {
                e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true } });
            }
            else
            {
                e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, retVal.Exception } });
            }
            return retVal;
        }
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents
      /// in the version series to be checked out again. If the private working copy has been created by createDocument, cancelCheckOut MUST
      /// delete the created document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.cancelCheckOutResponse> CancelCheckOut(cm.Requests.cancelCheckOut request)
        {
            var response = CancelCheckOut(request.RepositoryId, request.ObjectId, request.PWCLinkRequired);

            if (response.Exception is null)
            {
                return new cmr.cancelCheckOutResponse();
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Checks-in the Private Working Copy document
      /// </summary>
      /// <param name="pwcLinkRequired">If False this method uses the self-link if no workingcopy-link exists. This is basically
      /// equivalent to an UpdateProperties()-call.</param>
      /// <returns></returns>
      /// <remarks>Following the implementation in DotCMIS (Apache Chemistry) this method prefers the usage of the workingcopy-link, but
      /// also supports update the properties of the object if neither a workingcopy is found nor is required</remarks>
        public Generic.ResponseType<ca.AtomEntry> CheckIn(string repositoryId, string objectId, bool major = true, Core.Collections.cmisPropertiesType properties = null, cm.cmisContentStreamType contentStream = null, string checkinComment = null, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null, bool pwcLinkRequired = false)
        {
            string link;

            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            // object must exists!
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Self, MediaTypes.Entry, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is not null)
                {
                    return withBlock.Exception;
                }
                else if (!pwcLinkRequired)
                {
                    link = withBlock.Response;
                }
                else
                {
                    link = null;
                }
            }
            // prefer working copy link if available (workaround for non-compliant repositories)
            {
                var withBlock1 = GetLink(repositoryId, objectId, LinkRelationshipTypes.WorkingCopy, MediaTypes.Entry, _objectLinks, null);
                if (withBlock1.Exception is null)
                    link = withBlock1.Response;
            }

            {
                var withBlock2 = new ccg.LinkUriBuilder<ServiceURIs.enumObjectUri>(link, repositoryId);
                withBlock2.Add(ServiceURIs.enumObjectUri.major, major);
                withBlock2.Add(ServiceURIs.enumObjectUri.checkin, true);
                withBlock2.Add(ServiceURIs.enumObjectUri.checkinComment, checkinComment);

                var cmisraObject = new Core.cmisObjectType() { PolicyIds = policies, Properties = properties };
                Dictionary<string, string> headers = null;
                RestAtom.cmisContentType content;
                var state = TransformRequest(repositoryId, properties);

                try
                {
                    if (contentStream is not null && contentStream.BinaryStream is not null && !string.IsNullOrEmpty(contentStream.MimeType))
                    {
                        // transmit ContentStreamFileName, ContentStreamLength as property
                        if (properties is not null)
                            contentStream.ExtendProperties(properties);
                        content = (RestAtom.cmisContentType)contentStream;
                        if (!string.IsNullOrEmpty(contentStream.Filename))
                        {
                            // If the client wishes to set a new filename, it MAY add a Content-Disposition header, which carries the new filename.
                            // The disposition type MUST be "attachment". The repository SHOULD use the "filename" parameter and SHOULD ignore all other parameters
                            // see 3.11.8.2 HTTP PUT
                            headers = new Dictionary<string, string>() { { RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename) } };
                        }
                    }
                    else
                    {
                        content = null;
                    }

                    var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, repositoryId, EventBus.enumBuiltInEvents.CheckIn, objectId);
                    var retVal = Put(withBlock2.ToUri(), new sss.Atom10ItemFormatter(new ca.AtomEntry(cmisraObject, content)), MediaTypes.Entry, headers, ca.AtomEntry.CreateInstance);
                    if (retVal.Exception is null)
                    {
                        TransformResponse(retVal, state);
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true }, { EventBus.EventArgs.PredefinedPropertyNames.NewObjectId, retVal.Response.ObjectId } });
                        if (!(addACEs is null || removeACEs is null))
                            ApplyAcl(repositoryId, retVal.Response.ObjectId, addACEs, removeACEs);
                    }
                    else
                    {
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, retVal.Exception } });
                    }
                    return retVal;
                }
                finally
                {
                    if (state is not null)
                        state.Rollback();
                }
            }
        }
        /// <summary>
      /// Checks-in the Private Working Copy document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.checkInResponse> CheckIn(cm.Requests.checkIn request)
        {
            var response = CheckIn(request.RepositoryId, request.ObjectId, !request.Major.HasValue || request.Major.Value, request.Properties, request.ContentStream, request.CheckinComment, request.Policies, request.AddACEs, request.RemoveACEs, request.PWCLinkRequired);
            if (response.Exception is null)
            {
                return new cmr.checkInResponse() { ObjectId = response.Response.ObjectId };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Create a private working copy (PWC) of the document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks>uses checkedOut-collectionInfo</remarks>
        public Generic.ResponseType<ca.AtomEntry> CheckOut(string repositoryId, string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument objectId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.CheckedOut);
                if (withBlock.Exception is null)
                {
                    var state = new Vendors.Vendor.State(repositoryId);
                    return TransformResponse(Post(withBlock.Response.Link, new sss.Atom10ItemFormatter(new ca.AtomEntry(objectId)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state);
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Create a private working copy (PWC) of the document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.checkOutResponse> CheckOut(cm.Requests.checkOut request)
        {
            var response = CheckOut(request.RepositoryId, request.ObjectId);

            if (response.Exception is null)
            {
                string originalContentLink;
                string pwcContentLink;

                // get the contentLink of the document
                {
                    var withBlock = GetLink(request.RepositoryId, request.ObjectId, LinkRelationshipTypes.ContentStream, null, _objectLinks, GetObjectLinksOnly);
                    if (withBlock.Exception is null)
                    {
                        originalContentLink = withBlock.Response;
                    }
                    else
                    {
                        originalContentLink = GetLink(request.RepositoryId, request.ObjectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, null).Response;
                    }
                }
                // get the contentLink of the pwc
                {
                    var withBlock1 = GetLink(request.RepositoryId, response.Response.ObjectId, LinkRelationshipTypes.ContentStream, null, _objectLinks, GetObjectLinksOnly);
                    if (withBlock1.Exception is null)
                    {
                        pwcContentLink = withBlock1.Response;
                    }
                    else
                    {
                        pwcContentLink = GetLink(request.RepositoryId, response.Response.ObjectId, LinkRelationshipTypes.EditMedia, null, _objectLinks, null).Response;
                    }
                }

                return new cmr.checkOutResponse() { ObjectId = response.Response.ObjectId, ContentCopied = (originalContentLink ?? "") != (pwcContentLink ?? "") };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the list of all document objects in the specified version series, sorted by cmis:creationDate descending
      /// </summary>
      /// <returns></returns>
      /// <remarks>If a Private Working Copy exists for the version series and the caller has permissions to access it,
      /// then it MUST be returned as the first object in the result list</remarks>
        public Generic.ResponseType<ca.AtomFeed> GetAllVersions(string repositoryId, string objectId, string versionSeriesId, string filter = null, bool? includeAllowableActions = default)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.VersionHistory, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumAllVersionsUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumAllVersionsUri.versionSeriesId, versionSeriesId);
                        withBlock1.Add(ServiceURIs.enumAllVersionsUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumAllVersionsUri.includeAllowableActions, includeAllowableActions);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Returns the list of all document objects in the specified version series, sorted by cmis:creationDate descending
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getAllVersionsResponse> GetAllVersions(cm.Requests.getAllVersions request)
        {
            var response = GetAllVersions(request.RepositoryId, request.ObjectId, null, request.Filter, request.IncludeAllowableActions);

            if (response.Exception is null)
            {
                return new cmr.getAllVersionsResponse() { Objects = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() let cmisObject = entry.Object where cmisObject is not null select cmisObject).ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> GetObjectOfLatestVersion(string repositoryId, string objectIdOrVersionSeriesId, bool major = false, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            return GetObjectCore(repositoryId, objectIdOrVersionSeriesId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, major ? RestAtom.enumReturnVersion.latestmajor : RestAtom.enumReturnVersion.latest);
        }
        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getObjectOfLatestVersionResponse> GetObjectOfLatestVersion(cm.Requests.getObjectOfLatestVersion request)
        {
            var response = GetObjectOfLatestVersion(request.RepositoryId, request.ObjectId, request.Major.HasValue && request.Major.Value, request.Filter, request.IncludeRelationships, request.IncludePolicyIds, request.RenditionFilter, request.IncludeACL, request.IncludeAllowableActions);
            if (response.Exception is null)
            {
                return new cmr.getObjectOfLatestVersionResponse() { Object = response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Get a subset of the properties for the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks>A subset of GetObjectOfLatestVersion()</remarks>
        public Generic.ResponseType<Core.Collections.cmisPropertiesType> GetPropertiesOfLatestVersion(string repositoryId, string objectIdOrVersionSeriesId, bool major = false, string filter = null)
        {
            {
                var withBlock = GetObjectOfLatestVersion(repositoryId, objectIdOrVersionSeriesId, major, filter);
                if (withBlock.Exception is null)
                {
                    var cmisraObject = withBlock.Response is null ? null : withBlock.Response.Object;
                    return cmisraObject is null ? null : cmisraObject.Properties;
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Get a subset of the properties for the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getPropertiesOfLatestVersionResponse> GetPropertiesOfLatestVersion(cm.Requests.getPropertiesOfLatestVersion request)
        {
            var response = GetPropertiesOfLatestVersion(request.RepositoryId, request.ObjectId, request.Major.HasValue && request.Major.Value, request.Filter);

            if (response.Exception is null)
            {
                return new cmr.getPropertiesOfLatestVersionResponse() { Properties = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Relationship
        /// <summary>
      /// Gets all or a subset of relationships associated with an independent object
      /// </summary>
      /// <returns></returns>
      /// <remarks>uses relationships-link</remarks>
        public Generic.ResponseType<ca.AtomFeed> GetObjectRelationships(string repositoryId, string objectId, bool includeSubRelationshipTypes = false, Core.enumRelationshipDirection relationshipDirection = Core.enumRelationshipDirection.source, string typeId = null, long? maxItems = default, long? skipCount = default, string filter = null, bool? includeAllowableActions = default)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Relationships, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumRelationshipsUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.includeSubRelationshipTypes, includeSubRelationshipTypes);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.relationshipDirection, relationshipDirection);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.typeId, typeId);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.maxItems, maxItems);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.skipCount, skipCount);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.filter, filter);
                        withBlock1.Add(ServiceURIs.enumRelationshipsUri.includeAllowableActions, includeAllowableActions);

                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets all or a subset of relationships associated with an independent object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getObjectRelationshipsResponse> GetObjectRelationships(cm.Requests.getObjectRelationships request)
        {
            var response = GetObjectRelationships(request.RepositoryId, request.ObjectId, request.IncludeSubRelationshipTypes.HasValue && request.IncludeSubRelationshipTypes.Value, request.RelationshipDirection.HasValue ? request.RelationshipDirection.Value : Core.enumRelationshipDirection.source, request.TypeId, request.MaxItems, request.SkipCount, request.Filter, request.IncludeAllowableActions);
            if (response.Exception is null)
            {
                return new cmr.getObjectRelationshipsResponse() { Objects = response.Response };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Policy
        /// <summary>
      /// Applies a specified policy to an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomEntry> ApplyPolicy(string repositoryId, string policyId, string objectId)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Policies, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    var state = new Vendors.Vendor.State(repositoryId);
                    return TransformResponse(Post(new Uri(withBlock.Response), new sss.Atom10ItemFormatter(new ca.AtomEntry(policyId)), MediaTypes.Entry, null, ca.AtomEntry.CreateInstance), state, writeToCache: false);
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Applies a specified policy to an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.applyPolicyResponse> ApplyPolicy(cm.Requests.applyPolicy request)
        {
            var response = ApplyPolicy(request.RepositoryId, request.PolicyId, request.ObjectId);

            if (response.Exception is null)
            {
                return new cmr.applyPolicyResponse() { Object = response.Response is null ? null : response.Response.Object };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Gets the list of policies currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ResponseType<ca.AtomFeed> GetAppliedPolicies(string repositoryId, string objectId, string filter = null)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Policies, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumPoliciesUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumPoliciesUri.filter, filter);
                        var state = new Vendors.Vendor.State(repositoryId);
                        return TransformResponse(Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance), state, false);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Gets the list of policies currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getAppliedPoliciesResponse> GetAppliedPolicies(cm.Requests.getAppliedPolicies request)
        {
            var response = GetAppliedPolicies(request.RepositoryId, request.ObjectId, request.Filter);

            if (response.Exception is null)
            {
                return new cmr.getAppliedPoliciesResponse() { Objects = (from entry in response.Response.Entries ?? new List<ca.AtomEntry>() where entry is not null select entry.Object).ToArray() };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Removes a specified policy from an object
      /// </summary>
      /// <returns></returns>
      /// <remarks>This is the only collection where the URI’s of the objects in the collection MUST be specific to that collection.
      /// A DELETE on the policy object in the collection is a removal of the policy from the object NOT a deletion of the policy object itself</remarks>
        public Response RemovePolicy(string repositoryId, string policyId, string objectId)
        {
            Uri policiesUri;

            if (string.IsNullOrEmpty(policyId))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.invalidArgument, "Argument policyId MUST be set.");
                return cmisFault.ToFaultException();
            }

            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Policies, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    // store the uri to the policies collection
                    policiesUri = new Uri(withBlock.Response);

                    // get the current list of policies
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumPoliciesUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumPoliciesUri.filter, CmisPredefinedPropertyNames.ObjectId);
                        {
                            var withBlock2 = Get(withBlock1.ToUri(), ca.AtomFeed.CreateInstance);
                            if (withBlock2.Exception is null)
                            {
                                var feed = withBlock2.Response;
                                var entry = feed is null ? null : feed.get_Entry(policyId);
                                var link = entry is null ? null : entry.get_Link(LinkRelationshipTypes.Self);

                                // the self-link of the policy-object can be used if the link belongs to the policies collection
                                if (link is not null && link.Uri is not null && string.Compare(link.Uri.Authority, policiesUri.Authority, true) == 0 && string.Compare(link.Uri.LocalPath, policiesUri.LocalPath, true) == 0)
                                {
                                    return Delete(link.Uri);
                                }
                            }
                            else
                            {
                                return withBlock2.Exception;
                            }
                        }
                    }

                    // second chance: use the policies-collection-link
                    {
                        var withBlock3 = new ccg.LinkUriBuilder<ServiceURIs.enumPoliciesUri>(withBlock.Response, repositoryId);
                        withBlock3.Add(ServiceURIs.enumPoliciesUri.policyId, policyId);
                        return Delete(withBlock3.ToUri());
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Removes a specified policy from an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.removePolicyResponse> RemovePolicy(cm.Requests.removePolicy request)
        {
            var response = RemovePolicy(request.RepositoryId, request.PolicyId, request.ObjectId);

            if (response.Exception is null)
            {
                return new cmr.removePolicyResponse();
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Acl
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of an object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="addACEs"></param>
      /// <param name="removeACEs"></param>
      /// <param name="aclPropagation"></param>
      /// <returns></returns>
      /// <remarks>uses acl-link</remarks>
        public Generic.ResponseType<Core.Security.cmisAccessControlListType> ApplyAcl(string repositoryId, string objectId, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs, Core.enumACLPropagation aclPropagation = Core.enumACLPropagation.repositorydetermined)
        {
            // get the current acl
            {
                var withBlock = GetAcl(repositoryId, objectId, false);
                if (withBlock.Exception is null)
                {
                    var currentAcl = withBlock.Response;

                    {
                        var withBlock1 = GetLink(repositoryId, objectId, LinkRelationshipTypes.Acl, MediaTypes.Acl, _objectLinks, GetObjectLinksOnly);
                        if (withBlock1.Exception is null)
                        {
                            {
                                var withBlock2 = new ccg.LinkUriBuilder<ServiceURIs.enumACLUri>(withBlock1.Response, repositoryId);
                                withBlock2.Add(ServiceURIs.enumACLUri.aclPropagation, aclPropagation);
                                // modify acl
                                return Put(withBlock2.ToUri(), currentAcl.Join(addACEs, removeACEs), MediaTypes.Acl, null, Serialization.XmlSerializable.GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
                            }
                        }
                        else
                        {
                            return withBlock1.Exception;
                        }
                    }
                }
                else
                {
                    return withBlock.Self();
                }
            }
        }
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.applyACLResponse> ApplyAcl(cm.Requests.applyACL request)
        {
            var response = ApplyAcl(request.RepositoryId, request.ObjectId, request.AddACEs, request.RemoveACEs, request.ACLPropagation.HasValue ? request.ACLPropagation.Value : Core.enumACLPropagation.repositorydetermined);
            if (response.Exception is null)
            {
                // 2.2.10.1.3 Outputs (applyACL)
                // exact: An indicator that the ACL returned fully describes the permission for this object.
                // That is, there are no other security constraints applied to this object. Not provided defaults to FALSE. 
                return new cmr.applyACLResponse()
                {
                    ACL = new cm.cmisACLType()
                    {
                        ACL = response.Response,
                        Exact = response.Response is null ? default : response.Response.IsExact
                    }
                };
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Get the ACL currently applied to the specified object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="onlyBasicPermissions"></param>
      /// <returns></returns>
      /// <remarks>uses acl-link</remarks>
        public Generic.ResponseType<Core.Security.cmisAccessControlListType> GetAcl(string repositoryId, string objectId, bool onlyBasicPermissions = true)
        {
            {
                var withBlock = GetLink(repositoryId, objectId, LinkRelationshipTypes.Acl, MediaTypes.Acl, _objectLinks, GetObjectLinksOnly);
                if (withBlock.Exception is null)
                {
                    {
                        var withBlock1 = new ccg.LinkUriBuilder<ServiceURIs.enumACLUri>(withBlock.Response, repositoryId);
                        withBlock1.Add(ServiceURIs.enumACLUri.onlyBasicPermissions, onlyBasicPermissions);
                        return Get(withBlock1.ToUri(), Serialization.XmlSerializable.GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        /// <summary>
      /// Get the ACL currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.getACLResponse> GetAcl(cm.Requests.getACL request)
        {
            var response = GetAcl(request.RepositoryId, request.ObjectId, !request.OnlyBasicPermissions.HasValue || request.OnlyBasicPermissions.Value);

            if (response.Exception is null)
            {
                // 2.2.10.2.2 Outputs (getACL)
                // exact: An indicator that the ACL returned fully describes the permission for this object.
                // That is, there are no other security constraints applied to this object. Not provided defaults to FALSE. 
                return new cmr.getACLResponse()
                {
                    ACL = new cm.cmisACLType()
                    {
                        ACL = response.Response,
                        Exact = response.Response is null ? default : response.Response.IsExact
                    }
                };
            }
            else
            {
                return response.Exception;
            }
        }
        #endregion

        #region Miscellaneous (ICmisClient)
        public override enumClientType ClientType
        {
            get
            {
                return enumClientType.AtomPub;
            }
        }

        /// <summary>
      /// Logs out from repository
      /// </summary>
      /// <remarks></remarks>
        public override void Logout(string repositoryId)
        {
            var uri = new Uri(ServiceURIs.GetServiceUri(_serviceDocUri.OriginalString, ServiceURIs.enumRepositoriesUri.repositoryId | ServiceURIs.enumRepositoriesUri.logout).ReplaceUri("repositoryId", repositoryId, "logout", "true"));
            Get(uri);
            set_RepositoryInfo(repositoryId, null);
        }

        /// <summary>
      /// Tells the server, that this client is still alive
      /// </summary>
      /// <remarks></remarks>
        public override void Ping(string repositoryId)
        {
            var uri = new Uri(ServiceURIs.GetServiceUri(_serviceDocUri.OriginalString, ServiceURIs.enumRepositoriesUri.repositoryId | ServiceURIs.enumRepositoriesUri.ping).ReplaceUri("repositoryId", repositoryId, "ping", "true"));
            Get(uri);
        }

        /// <summary>
      /// There is no succinct representation of properties defined in AtomPub Binding
      /// </summary>
        public override bool SupportsSuccinct
        {
            get
            {
                return false;
            }
        }
        /// <summary>
      /// There is no support of token parameters in AtomPub Binding
      /// </summary>
        public override bool SupportsToken
        {
            get
            {
                return false;
            }
        }

        /// <summary>
      /// UserAgent-name of current instance
      /// </summary>
        protected override string UserAgent
        {
            get
            {
                return "Brügmann Software CmisObjectModel.Client.AtomPub.CmisClient";
            }
        }
        #endregion

        #region Caches
        /// <summary>
      /// If folderId is set the function returns the link to the Children-collection
      /// otherwise it returns the link to the Unfiled-collection
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="acceptRequest"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<string> GetChildrenOrUnfiledLink(string repositoryId, string folderId, ref bool acceptRequest)
        {
            if (string.IsNullOrEmpty(folderId))
            {
                // Unfiled-collection
                {
                    var withBlock = GetCollectionInfo(repositoryId, CollectionInfos.Unfiled);
                    if (withBlock.Exception is null)
                    {
                        // check for mediatype Request
                        acceptRequest = CollectionAccepts(withBlock.Response, MediaTypes.Request);
                        return withBlock.Response.Link.OriginalString;
                    }
                    else
                    {
                        return withBlock.Exception;
                    }
                }
            }
            else
            {
                // Children-collection
                {
                    var withBlock1 = GetLink(repositoryId, folderId, LinkRelationshipTypes.Down, MediaTypes.Feed, _objectLinks, GetObjectLinksOnly);
                    if (withBlock1.Exception is null)
                    {
                        // check for mediatype Request
                        acceptRequest = CollectionAccepts(repositoryId, CollectionInfos.Root, MediaTypes.Request);
                        return withBlock1.Self();
                    }
                    else
                    {
                        return withBlock1.Self();
                    }
                }
            }
        }

        /// <summary>
      /// Access to specified collectioninfo of specified repository
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="collectionType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<ca.AtomCollectionInfo> GetCollectionInfo(string repositoryId, string collectionType)
        {
            {
                var withBlock = GetRepositoryInfo(repositoryId);
                if (withBlock.Exception is null)
                {
                    var retVal = withBlock.Response.get_CollectionInfo(collectionType);

                    if (retVal is null)
                    {
                        var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.BadRequest, cm.enumServiceException.notSupported, "CollectionInfo '" + collectionType + "' not supported.");
                        return cmisFault.ToFaultException();
                    }
                    else
                    {
                        return retVal;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Access to links of an object or type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="relationshipType"></param>
      /// <param name="mediaType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<string> GetLink(string repositoryId, string id, string relationshipType, string mediaType, Collections.Generic.Cache<string, string> linkCache, Func<string, string, Generic.ResponseType<ca.AtomEntry>> getObjectOrType, string defaultErrorMessage = "Nothing wrong. Either this is a bug or threading issue.")
        {
            lock (linkCache)
            {
                // first chance
                string link = linkCache.get_Item(repositoryId ?? "", id ?? "", relationshipType, mediaType);

                if (string.IsNullOrEmpty(link) && getObjectOrType is not null)
                {
                    // try to get the object and fill the link-cache with object-links
                    {
                        var withBlock = getObjectOrType.Invoke(repositoryId, id);
                        if (withBlock.Exception is null)
                        {
                            // second chance
                            link = linkCache.get_Item(repositoryId ?? "", id ?? "", relationshipType ?? "", mediaType ?? "");
                        }
                        else
                        {
                            return withBlock.Exception;
                        }
                    }
                }

                if (string.IsNullOrEmpty(link))
                {
                    var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.NotFound, cm.enumServiceException.objectNotFound, null);

                    switch (linkCache.get_ValidPathDepth(repositoryId ?? "", id ?? "", relationshipType, mediaType))
                    {
                        case 0:
                            {
                                cmisFault.Message = "Unknown repository.";
                                break;
                            }
                        case 1:
                            {
                                cmisFault.Message = "Unknown " + (ReferenceEquals(linkCache, _objectLinks) ? "object." : "type.");
                                break;
                            }
                        case 2:
                            {
                                cmisFault.Message = "Relationship not supported by the repository for this " + (ReferenceEquals(linkCache, _objectLinks) ? "object." : "type.");
                                break;
                            }
                        case 3:
                            {
                                cmisFault.Message = "No link with matching media type.";
                                break;
                            }

                        default:
                            {
                                cmisFault.Message = defaultErrorMessage;
                                break;
                            }
                    }

                    return cmisFault.ToFaultException();
                }
                else
                {
                    return link;
                }
            }
        }

        /// <summary>
      /// Access to repository-links
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="relationshipType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<string> GetRepositoryLink(string repositoryId, string relationshipType)
        {
            string link;

            // try to get the object and fill the link-cache with object-links
            {
                var withBlock = GetRepositoryInfo(repositoryId, ignoreCache: true);
                if (withBlock.Exception is null)
                {
                    link = withBlock.Response.get_Link(relationshipType);
                }
                else
                {
                    return withBlock.Exception;
                }
            }

            if (string.IsNullOrEmpty(link))
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.NotFound, cm.enumServiceException.objectNotFound, "Relationship not supported by the repository");
                return cmisFault.ToFaultException();
            }
            else
            {
                return link;
            }
        }

        private Collections.Generic.Cache<string, string> _objectLinks = new Collections.Generic.Cache<string, string>(AppSettings.CacheSizeObjects << 4, AppSettings.CacheLeaseTime, true);


        private static Collections.Generic.Cache<string, string> _typeLinks = new Collections.Generic.Cache<string, string>(AppSettings.CacheSizeTypes << 4, AppSettings.CacheLeaseTime, true);

        /// <summary>
      /// Stores the links of objects and typedefinitions
      /// </summary>
      /// <remarks></remarks>
        private void WriteToCache(string repositoryId, string id, ca.AtomEntry entry, Collections.Generic.Cache<string, string> linkCache)
        {
            // precedence: response if cmisProperty cmis:objectId is returned
            if (entry is not null)
                id = entry.ObjectId.NVL(id);
            if (!(entry is null || string.IsNullOrEmpty(repositoryId) || string.IsNullOrEmpty(id)))
            {
                lock (linkCache)
                {
                    ca.AtomLink link;

                    // remove all cache-entries with a path starting with <repositoryId, id>
                    linkCache.RemoveAll(repositoryId, id);
                    foreach (ca.AtomLink currentLink in entry.Links)
                    {
                        link = currentLink;
                        linkCache.set_Item(new string[] { repositoryId, id, link.RelationshipType ?? "", link.MediaType ?? "" }, value: link.Uri.AbsoluteUri);
                    }
                    link = entry.ContentLink;
                    if (link is not null)
                        linkCache.set_Item(new string[] { repositoryId, id, link.RelationshipType ?? "", link.MediaType ?? "" }, value: link.Uri.AbsoluteUri);

                    // recursive
                    var children = entry.Children;
                    if (children is not null)
                        WriteToCache(repositoryId, children, linkCache);
                }
            }
        }
        /// <summary>
      /// Stores the links of each object in feed
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="feed"></param>
      /// <param name="linkCache"></param>
      /// <remarks></remarks>
        private void WriteToCache(string repositoryId, ca.AtomFeed feed, Collections.Generic.Cache<string, string> linkCache)
        {
            if (!(feed is null || string.IsNullOrEmpty(repositoryId)))
            {
                var entries = feed.Entries;

                if (entries is not null)
                {
                    lock (linkCache)
                    {
                        foreach (ca.AtomEntry entry in entries)
                        {
                            string objectId = entry.ObjectId;

                            if (!string.IsNullOrEmpty(objectId))
                            {
                                WriteToCache(repositoryId, objectId, entry, linkCache);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
      /// Stores often used data from the workspace in the cache
      /// </summary>
      /// <param name="ws"></param>
      /// <remarks></remarks>
        private void WriteToCache(ca.AtomWorkspace ws)
        {
            // repository itself
            set_RepositoryInfo(ws.RepositoryInfo.RepositoryId ?? "", ws);
        }
        #endregion

        #region Requests

        #region Vendor specific and value mapping
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values on a successful response
      /// </summary>
      /// <param name="response"></param>
      /// <param name="state"></param>
      /// <param name="cacheId"></param>
      /// <param name="writeToCache"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<ca.AtomEntry> TransformResponse(Generic.ResponseType<ca.AtomEntry> response, Vendors.Vendor.State state, string cacheId = null, bool writeToCache = true)
        {
            if (response.Exception is null)
            {
                string repositoryId = state.RepositoryId;
                var collector = new ca.AtomEntryCollector();
                var propertyCollections = (from entry in collector.Collect(response)
                                           let cmisObject = entry is null ? null : entry.Object
                                           let propertyCollection = cmisObject is null ? null : cmisObject.Properties
                                           where propertyCollection is not null
                                           select propertyCollection).ToArray();
                // vendor specifics, value mapping
                TransformResponse(state, propertyCollections);
                if (writeToCache)
                    WriteToCache(repositoryId, cacheId, response.Response, _objectLinks);
            }

            return response;
        }
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values on a successful response
      /// </summary>
      /// <param name="response"></param>
      /// <param name="state"></param>
      /// <param name="writeToCache"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private Generic.ResponseType<ca.AtomFeed> TransformResponse(Generic.ResponseType<ca.AtomFeed> response, Vendors.Vendor.State state, bool writeToCache = true)
        {
            if (response.Exception is null)
            {
                string repositoryId = state.RepositoryId;
                var collector = new ca.AtomEntryCollector();
                var propertyCollections = (from entry in collector.Collect(response)
                                           let cmisObject = entry is null ? null : entry.Object
                                           let propertyCollection = cmisObject is null ? null : cmisObject.Properties
                                           where propertyCollection is not null
                                           select propertyCollection).ToArray();
                // vendor specifics, value mapping
                TransformResponse(state, propertyCollections);
                if (writeToCache)
                    WriteToCache(repositoryId, response.Response, _objectLinks);
            }

            return response;
        }
        #endregion

        #region Delete-Requests
        private Response Delete(Uri uri)
        {
            return Request(uri, "DELETE", null, null, null);
        }

        private Generic.ResponseType<TResponse> Delete<TResponse>(Uri uri, Func<sx.XmlReader, TResponse> responseFactory)
        {
            return Request(uri, "DELETE", default, null, null, responseFactory);
        }
        #endregion

        #region Get-Requests
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function [Get](uri As Uri,
                                     Optional offset As Integer? = Nothing, Optional length As Integer? = Nothing) As Response
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Response Get(Uri uri, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            return Request(uri, "GET", null, null, null, offset, length);
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function [Get](Of TResponse)(uri As Uri, responseFactory As Func(Of sx.XmlReader, TResponse),
                                                   Optional offset As Integer? = Nothing, Optional length As Integer? = Nothing) As Generic.Response(Of TResponse)
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Generic.ResponseType<TResponse> Get<TResponse>(Uri uri, Func<sx.XmlReader, TResponse> responseFactory, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            return Request(uri, "GET", default, null, null, responseFactory, offset, length);
        }
        #endregion

        #region Post-Requests
        private Generic.ResponseType<TResponse> Post<TContent, TResponse>(Uri uri, TContent content, string contentType, Dictionary<string, string> headers, Func<sx.XmlReader, TResponse> responseFactory) where TContent : sxs.IXmlSerializable, new()
        {
            return Request(uri, "POST", content, contentType, headers, responseFactory);
        }
        #endregion

        #region Put-Requests
        private Generic.ResponseType<TResponse> Put<TResponse>(Uri uri, System.IO.Stream content, string contentType, Dictionary<string, string> headers, Func<sx.XmlReader, TResponse> responseFactory)
        {
            return Request(uri, "PUT", content, contentType, headers, responseFactory);
        }

        private Generic.ResponseType<TResponse> Put<TContent, TResponse>(Uri uri, TContent content, string contentType, Dictionary<string, string> headers, Func<sx.XmlReader, TResponse> responseFactory) where TContent : sxs.IXmlSerializable, new()
        {
            return Request(uri, "PUT", content, contentType, headers, responseFactory);
        }
        #endregion

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function Request(uri As Uri, method As String, content As IO.Stream, contentType As String, headers As Dictionary(Of String, String),
                                       Optional offset As Integer? = Nothing, Optional length As Integer? = Nothing) As Response
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Response Request(Uri uri, string method, System.IO.Stream content, string contentType, Dictionary<string, string> headers, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            var contentWriter = content is null ? null : new Action<System.IO.Stream>((requestStream) => content.CopyTo(requestStream));
            try
            {
                return new Response(GetResponse(uri, method, contentWriter, contentType, headers, offset, length));
            }
            catch (sn.WebException ex)
            {
                return new Response(ex);
            }
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function Request(Of TResponse)(uri As Uri, method As String, content As IO.Stream, contentType As String, headers As Dictionary(Of String, String),
                                                     responseFactory As Func(Of sx.XmlReader, TResponse),
                                                     Optional offset As Integer? = Nothing, Optional length As Integer? = Nothing) As Generic.Response(Of TResponse)
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Generic.ResponseType<TResponse> Request<TResponse>(Uri uri, string method, System.IO.Stream content, string contentType, Dictionary<string, string> headers, Func<sx.XmlReader, TResponse> responseFactory, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            var contentWriter = content is null ? null : new Action<System.IO.Stream>((requestStream) => content.CopyTo(requestStream));
            try
            {
                return new Generic.ResponseType<TResponse>(GetResponse(uri, method, contentWriter, contentType, headers, offset, length), responseFactory);
            }
            catch (sn.WebException ex)
            {
                return new Generic.ResponseType<TResponse>(ex);
            }
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function Request(Of TContent As {New, sxs.IXmlSerializable}, TResponse)(uri As Uri, method As String, content As TContent,
                                                                                              contentType As String, headers As Dictionary(Of String, String),
                                                                                              responseFactory As Func(Of sx.XmlReader, TResponse),
                                                                                              Optional offset As Integer? = Nothing,
                                                                                              Optional length As Integer? = Nothing) As Generic.Response(Of TResponse)
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Generic.ResponseType<TResponse> Request<TContent, TResponse>(Uri uri, string method, TContent content, string contentType, Dictionary<string, string> headers, Func<sx.XmlReader, TResponse> responseFactory, long? offset = default, long? length = default) where TContent : sxs.IXmlSerializable, new()
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            Action<System.IO.Stream> contentWriter;

            if (content is null)
            {
                contentWriter = null;
            }
            else
            {
                contentWriter = new Action<System.IO.Stream>((requestStream) =>
                    {
                        sx.XmlDocument xmlDoc;
                        var serializer = new sxs.XmlSerializer(typeof(sx.XmlDocument));

                        if (typeof(cm.Requests.RequestBase).IsAssignableFrom(typeof(TContent)))
                        {
                            // the root namespace of Messaging.Requests.RequestBase instances must be changed from cmism to cmisw
                            xmlDoc = Serialization.SerializationHelper.ToXmlDocument(content, _requestBaseAttributeOverrides);
                        }
                        else
                        {
                            xmlDoc = Serialization.SerializationHelper.ToXmlDocument(content);
                        }

                        using (var writer = sx.XmlWriter.Create(requestStream, new sx.XmlWriterSettings() { Encoding = new System.Text.UTF8Encoding(false) }))
                        {
                            serializer.Serialize(writer, xmlDoc);
                        }
                    });
            }

            try
            {
                return new Generic.ResponseType<TResponse>(GetResponse(uri, method, contentWriter, contentType, headers, offset, length), responseFactory);
            }
            catch (sn.WebException ex)
            {
                return new Generic.ResponseType<TResponse>(ex);
            }
        }

        #endregion

        /// <summary>
      /// Returns True if the collectionInfo specified by repositoryId and collectionType exists and accepts the specified mediaType
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="collectionType"></param>
      /// <param name="mediaType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private bool CollectionAccepts(string repositoryId, string collectionType, string mediaType)
        {
            // check for mediatype
            {
                var withBlock = GetCollectionInfo(repositoryId, collectionType);
                return withBlock.Exception is null ? CollectionAccepts(withBlock.Response, mediaType) : false;
            }
        }
        /// <summary>
      /// Returns True if the collectionInfo defines mediaType as accepted mediatype
      /// </summary>
      /// <param name="collectionInfo"></param>
      /// <param name="mediaType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private bool CollectionAccepts(ca.AtomCollectionInfo collectionInfo, string mediaType)
        {
            if (collectionInfo is not null)
            {
                foreach (string acceptedMediaType in collectionInfo.Accepts)
                {
                    if (string.Compare(acceptedMediaType, mediaType, true) == 0)
                        return true;
                }
            }

            return false;
        }

    }
}