using System;
using cc = CmisObjectModel.Client;
using ccg = CmisObjectModel.Client.Generic;
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
using requests = CmisObjectModel.Messaging.Requests;
using responses = CmisObjectModel.Messaging.Responses;

namespace CmisObjectModel.Contracts
{
    public interface ICmisClient
    {

        #region Repository
        /// <summary>
      /// Creates a new type definition that is a subtype of an existing specified parent type
      /// </summary>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createTypeResponse> CreateType(requests.createType request);

        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.deleteTypeResponse> DeleteType(requests.deleteType request);

        /// <summary>
      /// Returns all repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getRepositoriesResponse> GetRepositories(requests.getRepositories request);

        /// <summary>
      /// Returns the workspace of specified repository or null
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getRepositoryInfoResponse> GetRepositoryInfo(requests.getRepositoryInfo request, bool ignoreCache = false);

        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the specified type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getTypeChildrenResponse> GetTypeChildren(requests.getTypeChildren request);

        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getTypeDefinitionResponse> GetTypeDefinition(requests.getTypeDefinition request);

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getTypeDescendantsResponse> GetTypeDescendants(requests.getTypeDescendants request);

        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.updateTypeResponse> UpdateType(requests.updateType request);
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getCheckedOutDocsResponse> GetCheckedOutDocs(requests.getCheckedOutDocs request);

        /// <summary>
      /// Gets the list of child objects contained in the specified folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getChildrenResponse> GetChildren(requests.getChildren request);

        /// <summary>
      /// Gets the set of descendant objects containded in the specified folder or any of its child-folders
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getDescendantsResponse> GetDescendants(requests.getDescendants request);

        /// <summary>
      /// Gets the parent folder object for the specified folder object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getFolderParentResponse> GetFolderParent(requests.getFolderParent request);

        /// <summary>
      /// Gets the set of descendant folder objects contained in the specified folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getFolderTreeResponse> GetFolderTree(requests.getFolderTree request);

        /// <summary>
      /// Gets the parent folder(s) for the specified fileable object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getObjectParentsResponse> GetObjectParents(requests.getObjectParents request);
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.appendContentStreamResponse> AppendContentStream(requests.appendContentStream request);

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.bulkUpdatePropertiesResponse> BulkUpdateProperties(requests.bulkUpdateProperties request);

        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createDocumentResponse> CreateDocument(requests.createDocument request);

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createDocumentFromSourceResponse> CreateDocumentFromSource(requests.createDocumentFromSource request);

        /// <summary>
      /// Creates a folder object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createFolderResponse> CreateFolder(requests.createFolder request);

        /// <summary>
      /// Creates a item object of the specified type (given by the cmis:objectTypeId property) in the specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createItemResponse> CreateItem(requests.createItem request);

        /// <summary>
      /// Creates a policy object of the specified type (given by the cmis:objectTypeId property) in the specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createPolicyResponse> CreatePolicy(requests.createPolicy request);

        /// <summary>
      /// Creates a relationship object of the specified type (given by the cmis:objectTypeId property)
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.createRelationshipResponse> CreateRelationship(requests.createRelationship request);

        /// <summary>
      /// Deletes the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.deleteContentStreamResponse> DeleteContentStream(requests.deleteContentStream request);

        /// <summary>
      /// Deletes the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.deleteObjectResponse> DeleteObject(requests.deleteObject request);

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.deleteTreeResponse> DeleteTree(requests.deleteTree request);

        /// <summary>
      /// Gets the list of allowable actions for an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getAllowableActionsResponse> GetAllowableActions(requests.getAllowableActions request);

        /// <summary>
      /// Gets the content stream for the specified document object, or gets a rendition stream for a specified rendition of a document or folder object.
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getContentStreamResponse> GetContentStream(requests.getContentStream request);

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getObjectResponse> GetObject(requests.getObject request);

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getObjectByPathResponse> GetObjectByPath(requests.getObjectByPath request);

        /// <summary>
      /// Gets the list of properties for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getPropertiesResponse> GetProperties(requests.getProperties request);

        /// <summary>
      /// Gets the list of associated renditions for the specified object. Only rendition attributes are returned, not rendition stream
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getRenditionsResponse> GetRenditions(requests.getRenditions request);

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.moveObjectResponse> MoveObject(requests.moveObject request);

        /// <summary>
      /// Sets the content stream for the specified document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.setContentStreamResponse> SetContentStream(requests.setContentStream request);

        /// <summary>
      /// Updates properties and secondary types of the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.updatePropertiesResponse> UpdateProperties(requests.updateProperties request);
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.addObjectToFolderResponse> AddObjectToFolder(requests.addObjectToFolder request);

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.removeObjectFromFolderResponse> RemoveObjectFromFolder(requests.removeObjectFromFolder request);
        #endregion

        #region Discovery
        /// <summary>
      /// Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to
      /// efficiently understand what has changed in the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getContentChangesResponse> GetContentChanges(requests.getContentChanges request);

        /// <summary>
      /// Executes a CMIS query statement against the contents of the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.queryResponse> Query(requests.query request);
        #endregion

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents
      /// in the version series to be checked out again. If the private working copy has been created by createDocument, cancelCheckOut MUST
      /// delete the created document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.cancelCheckOutResponse> CancelCheckOut(requests.cancelCheckOut request);

        /// <summary>
      /// Checks-in the Private Working Copy document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.checkInResponse> CheckIn(requests.checkIn request);

        /// <summary>
      /// Create a private working copy (PWC) of the document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.checkOutResponse> CheckOut(requests.checkOut request);

        /// <summary>
      /// Returns the list of all document objects in the specified version series, sorted by cmis:creationDate descending
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getAllVersionsResponse> GetAllVersions(requests.getAllVersions request);

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getObjectOfLatestVersionResponse> GetObjectOfLatestVersion(requests.getObjectOfLatestVersion request);

        /// <summary>
      /// Get a subset of the properties for the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getPropertiesOfLatestVersionResponse> GetPropertiesOfLatestVersion(requests.getPropertiesOfLatestVersion request);
        #endregion

        #region Relationship
        /// <summary>
      /// Gets all or a subset of relationships associated with an independent object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getObjectRelationshipsResponse> GetObjectRelationships(requests.getObjectRelationships request);
        #endregion

        #region Policy
        /// <summary>
      /// Applies a specified policy to an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.applyPolicyResponse> ApplyPolicy(requests.applyPolicy request);

        /// <summary>
      /// Gets the list of policies currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getAppliedPoliciesResponse> GetAppliedPolicies(requests.getAppliedPolicies request);

        /// <summary>
      /// Removes a specified policy from an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.removePolicyResponse> RemovePolicy(requests.removePolicy request);
        #endregion

        #region Acl
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.applyACLResponse> ApplyAcl(requests.applyACL request);

        /// <summary>
      /// Get the ACL currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.ResponseType<responses.getACLResponse> GetAcl(requests.getACL request);
        #endregion

        #region Mapping
        /// <summary>
      /// Adds mapping information to the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="mapper"></param>
      /// <remarks></remarks>
        void AddMapper(string repositoryId, Data.Mapper mapper);

        /// <summary>
      /// Removes mapping information from the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        void RemoveMapper(string repositoryId);
        #endregion

        /// <summary>
      /// Authentication information used for all requests
      /// </summary>
      /// <returns></returns>
        AuthenticationInfo Authentication { get; }

        /// <summary>
      /// ClientType (AtomPubBinding, BrowserBinding)
      /// </summary>
      /// <returns></returns>
        enumClientType ClientType { get; }

        /// <summary>
      /// Timeout HttpWebRequest.Timeout. If not set default is used.
      /// </summary>
      /// <returns></returns>
        int? ConnectTimeout { get; }

        /// <summary>
      /// Returns the uri to get the content of a cmisDocument
      /// </summary>
        ccg.ResponseType<string> GetContentStreamLink(string repositoryId, string objectId, string streamId = null);

        void Logout(string repositoryId);
        void Ping(string repositoryId);

        /// <summary>
      /// Timeout read or write operations (HttpWebRequest.ReadWriteTimeout). If not set default is used.
      /// </summary>
      /// <returns></returns>
        int? ReadWriteTimeout { get; }

        /// <summary>
      /// Installs an automatic ping to tell the server that the client is still alive
      /// </summary>
      /// <param name="interval">Time-interval in seconds</param>
      /// <remarks></remarks>
        void RegisterPing(string repositoryId, double interval);

        /// <summary>
      /// The base address of the cmis-service the client is connected with
      /// </summary>
      /// <returns></returns>
        Uri ServiceDocUri { get; }

        /// <summary>
      /// Returns True if the binding supports a succinct representation of properties
      /// </summary>
        bool SupportsSuccinct { get; }
        /// <summary>
      /// Returns True if the binding supports token parameters
      /// </summary>
        bool SupportsToken { get; }

        /// <summary>
      /// Timeout of request in milliseconds; if not defined, the default value of System.Net.HttpWebRequest.Timeout is used (100s)
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        int? Timeout { get; set; }

        /// <summary>
      /// Removes the automatic ping
      /// </summary>
        void UnregisterPing(string repositoryId);

        string User { get; }

        cc.Vendors.Vendor Vendor { get; }

    }
}