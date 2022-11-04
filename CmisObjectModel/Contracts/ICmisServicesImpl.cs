using System;
using sss = System.ServiceModel.Syndication;
using ccg = CmisObjectModel.Common.Generic;
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
using cc = CmisObjectModel.Core;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cm = CmisObjectModel.Messaging;
using cs = CmisObjectModel.ServiceModel;
using CmisObjectModel.Common.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Contracts
{
    /// <summary>
   /// Defines the custom implementation of cmis
   /// </summary>
   /// <remarks></remarks>
    public interface ICmisServicesImpl
    {

        #region Repository
        /// <summary>
      /// Creates a new type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<ccdt.cmisTypeDefinitionType> CreateType(string repositoryId, ccdt.cmisTypeDefinitionType newType);

        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="typeId">TypeId</param>
      /// <remarks></remarks>
        Exception DeleteType(string repositoryId, string typeId);

        /// <summary>
      /// Returns a list of workspaces for all available repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cc.cmisRepositoryInfoType[]> GetRepositories();
        /// <summary>
        /// Returns a workspace for the specified repository
        /// </summary>
        /// <param name="repositoryId">The identifier for the repository</param>
        /// <returns></returns>
        /// <remarks>log in into specified respository</remarks>
        ccg.Result<cc.cmisRepositoryInfoType> GetRepositoryInfo(string repositoryId);

        /// <summary>
        /// Returns all child types of the specified type, if defined, otherwise the basetypes of the repository.
        /// </summary>
        /// <param name="repositoryId">The identifier for the repository</param>
        /// <param name="typeId">TypeId; optional
        /// If specified, then the repository MUST return all of child types of the specified type
        /// If not specified, then the repository MUST return all base object-types</param>
        /// <param name="includePropertyDefinitions">If TRUE, then the repository MUST return the property deﬁnitions for each object-type.
        /// If FALSE (default), the repository MUST return only the attributes for each object-type</param>
        /// <param name="maxItems">optional</param>
        /// <param name="skipCount">optional</param>
        /// <returns></returns>
        /// <remarks></remarks>
        ccg.Result<cm.cmisTypeDefinitionListType> GetTypeChildren(string repositoryId, string typeId, bool includePropertyDefinitions, long? maxItems, long? skipCount);

        /// <summary>
      /// Returns the descendant object-types under the specified type.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="typeId">TypeId; optional
      /// If speciﬁed, then the repository MUST return all of descendant types of the speciﬁed type
      /// If not speciﬁed, then the Repository MUST return all types and MUST ignore the value of the depth parameter</param>
      /// <param name="includePropertyDefinitions">If TRUE, then the repository MUST return the property deﬁnitions for each object-type.
      /// If FALSE (default), the repository MUST return only the attributes for each object-type</param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cm.cmisTypeContainer> GetTypeDescendants(string repositoryId, string typeId, bool includePropertyDefinitions, long? depth);

        /// <summary>
      /// Returns the type-definition of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>

        /// <summary>
      /// Explicit log in into repository
      /// </summary>
        ccg.Result<System.Net.HttpStatusCode> Login(string repositoryId, string authorization);

        /// <summary>
      /// Log out from repository
      /// </summary>
      /// <remarks>Not defined in the CMIS-specification</remarks>
        ccg.Result<System.Net.HttpStatusCode> Logout(string repositoryId);

        /// <summary>
      /// Tell server that the client is alive
      /// </summary>
      /// <remarks>Not defined in the CMIS-specification</remarks>
        ccg.Result<System.Net.HttpStatusCode> Ping(string repositoryId);

        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="modifiedType">A type definition object with the property definitions that are to change.
      /// Repositories MUST ignore all fields in the type definition except for the type id and the list of properties.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<ccdt.cmisTypeDefinitionType> UpdateType(string repositoryId, ccdt.cmisTypeDefinitionType modifiedType);
        #endregion

        #region Navigation
        /// <summary>
      /// Returns a list of check out object the user has access to.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="filter"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <returns></returns>
      /// <remarks>
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// </remarks>
        ccg.Result<cs.cmisObjectListType> GetCheckedOutDocs(string repositoryId, string folderId, string filter, long? maxItems, long? skipCount, string renditionFilter, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships);

        /// <summary>
      /// Returns all children of the specified CMIS object.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId">The identifier for the folder</param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="orderBy"></param>
      /// <param name="includePathSegment">If TRUE, returns a PathSegment for each child object for use in constructing that object’s path.
      /// Defaults to FALSE</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectInFolderListType> GetChildren(string repositoryId, string folderId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy, bool includePathSegment);

        /// <summary>
      /// Returns the descendant objects contained in the specified folder or any of its child-folders.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId">The identifier for the folder</param>
      /// <param name="filter"></param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includePathSegment"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectInFolderContainerType> GetDescendants(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includePathSegment);

        /// <summary>
      /// Returns the descendant folders contained in the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId">The identifier for the folder</param>
      /// <param name="filter"></param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePathSegment">If TRUE, returns a PathSegment for each child object for use in constructing that object’s path.
      /// Defaults to FALSE</param>
      /// <param name="renditionFilter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectInFolderContainerType> GetFolderTree(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships, bool includePathSegment, string renditionFilter);

        /// <summary>
      /// Returns the parent folder-object of the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> GetFolderParent(string repositoryId, string folderId, string filter);

        /// <summary>
      /// Returns the parent folders for the specified object
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeRelativePathSegment"></param>
      /// <returns></returns>
      /// <remarks>
      /// This feed contains a set of atom entries for each parent of the object that MUST contain: 
      /// cmisra:object inside atom:entry 
      /// cmisra:relativePathSegment 
      /// </remarks>
        ccg.Result<cs.cmisObjectParentsType[]> GetObjectParents(string repositoryId, string objectId, string filter, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeRelativePathSegment);

        /// <summary>
      /// Returns a list of check out object the user has access to.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="orderBy"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> GetUnfiledObjects(string repositoryId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy);
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the specified document object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="contentStream"></param>
      /// <param name="isLastChunk"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cm.Responses.setContentStreamResponse> AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool isLastChunk, string changeToken);

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> BulkUpdateProperties(string repositoryId, cc.cmisBulkUpdateType data);

        /// <summary>
      /// Creates a new document in the specified folder or as unfiled document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newDocument"></param>
      /// <param name="folderId">If specified, the identifier for the folder that MUST be the parent folder for the newly-created document object.
      /// This parameter MUST be specified if the repository does NOT support the optional "unfiling" capability.</param>
      /// <param name="versioningState"></param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if specified, or being applied if no folderId is specified.</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if specified, or being ignored if no folderId is specified.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreateDocument(string repositoryId, cc.cmisObjectType newDocument, string folderId, cm.cmisContentStreamType content, cc.enumVersioningState? versioningState, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) speciﬁed location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="sourceId"></param>
      /// <param name="properties">The property values that MUST be applied to the object. This list of properties SHOULD only contain properties whose values differ from the source document</param>
      /// <param name="folderId">If speciﬁed, the identifier for the folder that MUST be the parent folder for the newly-created document object.
      /// This parameter MUST be specified if the repository does NOT support the optional "unfiling" capability.</param>
      /// <param name="versioningState"></param>
      /// <param name="policies">A list of policy ids that MUST be applied to the newly-created document object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if specified, or being ignored if no folderId is specified.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreateDocumentFromSource(string repositoryId, string sourceId, cc.Collections.cmisPropertiesType properties, string folderId, cc.enumVersioningState? versioningState, string[] policies, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Creates a folder object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newFolder"></param>
      /// <param name="parentFolderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created folder object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created folder object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreateFolder(string repositoryId, cc.cmisObjectType newFolder, string parentFolderId, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Creates an item object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newItem"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreateItem(string repositoryId, cc.cmisObjectType newItem, string folderId, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Creates a policy object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newPolicy"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreatePolicy(string repositoryId, cc.cmisObjectType newPolicy, string folderId, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Creates a relationship object of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="newRelationship"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CreateRelationship(string repositoryId, cc.cmisObjectType newRelationship, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs);

        /// <summary>
      /// Deletes the content stream for the specified document object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks>
      /// A repository MAY automatically create new document versions as part of this service method. Therefore, the obejctId output NEED NOT be identical to the objectId input.
      /// </remarks>
        ccg.Result<cm.Responses.deleteContentStreamResponse> DeleteContentStream(string repositoryId, string objectId, string changeToken);

        /// <summary>
      /// Returns True, if the submitted document was successfully removed
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="allVersions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        Exception DeleteObject(string repositoryId, string objectId, bool allVersions);

        /// <summary>
      /// Deletes the speciﬁed folder object and all of its child- and descendant-objects.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId"></param>
      /// <param name="allVersions">If TRUE (default), then delete all versions of all documents. If FALSE, delete only the document versions referenced in the tree. The repository MUST ignore the value of this parameter when this service is invoked on any non-document objects or non-versionable document objects.</param>
      /// <param name="unfileObjects"></param>
      /// <param name="continueOnFailure">If TRUE, then the repository SHOULD continue attempting to perform this operation even if deletion of a child- or descendant-object in the specified folder cannot be deleted. Default: False</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cm.Responses.deleteTreeResponse> DeleteTree(string repositoryId, string folderId, bool allVersions, cc.enumUnfileObject? unfileObjects, bool continueOnFailure);

        /// <summary>
      /// Returns the allowable actions for the specified document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cc.cmisAllowableActionsType> GetAllowableActions(string repositoryId, string objectId);

        /// <summary>
      /// Returns the content stream of the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="streamId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cm.cmisContentStreamType> GetContentStream(string repositoryId, string objectId, string streamId);

        /// <summary>
      /// Gets the specified information for the object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="returnVersion"></param>
      /// <param name="privateWorkingCopy">If True the private working copy of the document specified by objectId is requested</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> GetObject(string repositoryId, string objectId, string filter, cc.enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions, RestAtom.enumReturnVersion? returnVersion, bool? privateWorkingCopy);

        ccg.Result<cs.cmisObjectType> GetObjectByPath(string repositoryId, string path, string filter, bool? includeAllowableActions, bool? includePolicyIds, cc.enumIncludeRelationships? includeRelationships, bool? includeACL, string renditionFilter);

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="targetFolderId"></param>
      /// <param name="sourceFolderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId);

        /// <summary>
      /// Sets the content stream for the speciﬁed document object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="contentStream"></param>
      /// <param name="overwriteFlag"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cm.Responses.setContentStreamResponse> SetContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool overwriteFlag, string changeToken);

        /// <summary>
      /// Updates the submitted cmis-object
      /// </summary>
        ccg.Result<cs.cmisObjectType> UpdateProperties(string repositoryId, string objectId, cc.Collections.cmisPropertiesType properties, string changeToken);
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="folderId"></param>
      /// <param name="allVersions">Add all versions of the object to the folder if the repository supports version-specific filing. Defaults to TRUE.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions);

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="folderId">The folder from which the object is to be removed.
      /// If no value is specified, then the repository MUST remove the object from all folders in which it is currently filed.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> RemoveObjectFromFolder(string repositoryId, string objectId, string folderId);
        #endregion

        #region Discovery
        /// <summary>
      /// Returns a list of content changes
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="filter"></param>
      /// <param name="maxItems"></param>
      /// <param name="includeACL"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="includeProperties"></param>
      /// <param name="changeLogToken"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.getContentChanges> GetContentChanges(string repositoryId, string filter, long? maxItems, bool? includeACL, bool includePolicyIds, bool includeProperties, ref string changeLogToken);

        /// <summary>
      /// Returns the data described by the specified CMIS query.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="q"></param>
      /// <param name="searchAllVersions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> Query(string repositoryId, string q, bool searchAllVersions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includeAllowableActions, long? maxItems, long? skipCount);
        #endregion

        #region Versioning
        /// <summary>
      /// Rollback a CheckOut-action
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId">Id of a private working copy object</param>
      /// <returns></returns>
      /// <remarks></remarks>
        Exception CancelCheckOut(string repositoryId, string objectId);

        /// <summary>
      /// Checks-in the Private Working Copy document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="major">TRUE (default) if the checked-in document object MUST be a major version.</param>
      /// <param name="checkInComment"></param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object.</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object.</param>
      /// <returns></returns>
      /// <remarks>
      /// For repositories that do NOT support the optional capabilityPWCUpdatable capability, the properties and contentStream input parameters MUST be
      /// provided on the checkIn service for updates to happen as part of checkIn.
      /// Each CMIS protocol binding MUST specify whether the checkin service MUST always include all updatable properties, or only those properties
      /// whose values are different than the original value of the object.
      /// </remarks>
        ccg.Result<cs.cmisObjectType> CheckIn(string repositoryId, string objectId, cc.Collections.cmisPropertiesType properties, string[] policies, cm.cmisContentStreamType content, bool major, string checkInComment, cc.Security.cmisAccessControlListType addACEs = null, cc.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Checks out the specified CMIS object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> CheckOut(string repositoryId, string objectId);

        /// <summary>
      /// Returns all Documents in the specified version series.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="versionSeriesId"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> GetAllVersions(string repositoryId, string objectId, string versionSeriesId, string filter, bool? includeAllowableActions);
        #endregion

        #region Relationships
        /// <summary>
      /// Returns the relationships for the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="includeSubRelationshipTypes"></param>
      /// <param name="relationshipDirection"></param>
      /// <param name="typeId"></param>
      /// <param name="maxItems"></param>
      /// <param name="skipCount"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> GetObjectRelationships(string repositoryId, string objectId, bool includeSubRelationshipTypes, cc.enumRelationshipDirection? relationshipDirection, string typeId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions);
        #endregion

        #region Policy
        /// <summary>
      /// Applies a policy to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="policyId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectType> ApplyPolicy(string repositoryId, string objectId, string policyId);

        /// <summary>
      /// Returns a list of policies applied to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cs.cmisObjectListType> GetAppliedPolicies(string repositoryId, string objectId, string filter);

        /// <summary>
      /// Removes a policy from the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="policyId"></param>
      /// <remarks></remarks>
        Exception RemovePolicy(string repositoryId, string objectId, string policyId);
        #endregion

        #region ACL
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="addACEs"></param>
      /// <param name="removeACEs"></param>
      /// <param name="aclPropagation"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cc.Security.cmisAccessControlListType> ApplyACL(string repositoryId, string objectId, cc.Security.cmisAccessControlListType addACEs, cc.Security.cmisAccessControlListType removeACEs, cc.enumACLPropagation aclPropagation);

        /// <summary>
      /// Get the ACL currently applied to the specified document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="onlyBasicPermissions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        ccg.Result<cc.Security.cmisAccessControlListType> GetACL(string repositoryId, string objectId, bool onlyBasicPermissions);
        #endregion

        /// <summary>
      /// Returns the baseUri of the service
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        Uri BaseUri { get; }

        /// <summary>
        /// Returns True if the objectId exists in the repository
        /// </summary>
        /// <param name="objectId"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        bool get_Exists(string repositoryId, string objectId);

        CmisObjectModel.Core.cmisRepositoryInfoType get_RepositoryInfo(string repositoryId);
        Result<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> get_TypeDefinition(string repositoryId, string typeId);

        /// <summary>
        /// Returns the BaseObjectType of cmisObject specified by objectId
        /// </summary>
        cc.enumBaseObjectTypeIds GetBaseObjectType(string repositoryId, string objectId);

        /// <summary>
      /// Returns the objectId of the object specified by path.
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        string GetObjectId(string repositoryId, string path);

        /// <summary>
      /// Returns the parent-typeId of the specified type. If the specified type is a
      /// base type, the function returns null.
      /// </summary>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        string GetParentTypeId(string repositoryId, string typeId);

        /// <summary>
      /// Returns the cookie for the sessionId
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        string GetSessionIdCookieName();

        /// <summary>
      /// Returns the author for lists of types or objects
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        sss.SyndicationPerson GetSystemAuthor();

        /// <summary>
      /// Log exception called before the cmisService throws an exception
      /// </summary>
      /// <param name="ex"></param>
      /// <remarks>Compiler constant EnableExceptionLogging must be set to 'True'</remarks>
        void LogException(Exception ex);

        /// <summary>
      /// Returns True if userName describes a known user and the password is valid
      /// </summary>
      /// <param name="userName"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        bool ValidateUserNamePassword(string userName, string password);
    }
}