
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
using cmr = CmisObjectModel.Messaging.Requests;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Simplifies requests to a cmis repository
   /// </summary>
   /// <remarks></remarks>
    public class CmisRepository : CmisDataModelObject
    {

        #region Constructors
        public CmisRepository(Core.cmisRepositoryInfoType repositoryInfo, Contracts.ICmisClient client) : base(client, repositoryInfo)
        {
        }
        #endregion

        #region Repository
        /// <summary>
      /// Creates a new type definition that is a subtype of an existing specified parent type
      /// </summary>
      /// <remarks></remarks>
        public CmisType CreateType(Core.Definitions.Types.cmisTypeDefinitionType type)
        {
            {
                var withBlock = _client.CreateType(new cmr.createType() { RepositoryId = _repositoryInfo.RepositoryId, Type = type });
                _lastException = withBlock.Exception;
                return _lastException is null ? CreateCmisType(withBlock.Response.Type) : null;
            }
        }

        /// <summary>
      /// Returns the list of object-base-types defined for the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemList<CmisType> GetBaseTypes(bool includePropertyDefinitions = false, long? maxItems = default, long? skipCount = default)
        {
            return GetTypeChildren(null, includePropertyDefinitions, maxItems, skipCount);
        }

        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
      /// <remarks></remarks>
        public new CmisType GetTypeDefinition(string typeId)
        {
            return base.GetTypeDefinition(typeId);
        }

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
      /// <remarks></remarks>
        public new Generic.ItemContainer<CmisType>[] GetTypeDescendants(string typeId = null, long? depth = default, bool includePropertyDefinitions = false)
        {
            return base.GetTypeDescendants(typeId, depth, includePropertyDefinitions);
        }

        /// <summary>
      /// Returns the set of the object-types defined for the Repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemContainer<CmisType>[] GetTypes(bool includePropertyDefinitions = false)
        {
            return base.GetTypeDescendants(null, default, includePropertyDefinitions);
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
      /// <remarks></remarks>
        public new Generic.ItemList<CmisObject> GetCheckedOutDocs(long? maxItems = default, long? skipCount = default, string orderBy = null, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default)
        {
            return GetCheckedOutDocs(null, maxItems, skipCount, orderBy, filter, includeRelationships, renditionFilter, includeAllowableActions);
        }
        #endregion

        #region Object
        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <returns></returns>
      /// <remarks>Notice: using the AtomPub-Binding new object ids are not exposed</remarks>
        public Core.cmisObjectIdAndChangeTokenType[] BulkUpdateProperties(Core.cmisObjectIdAndChangeTokenType[] objectIdAndChangeTokens, Core.Collections.cmisPropertiesType properties = null, string[] addSecondaryTypeIds = null, string[] removeSecondaryTypeIds = null)
        {
            var bulkUpdateData = new Core.cmisBulkUpdateType()
            {
                ObjectIdAndChangeTokens = objectIdAndChangeTokens,
                Properties = properties,
                AddSecondaryTypeIds = addSecondaryTypeIds,
                RemoveSecondaryTypeIds = removeSecondaryTypeIds
            };
            {
                var withBlock = _client.BulkUpdateProperties(new cmr.bulkUpdateProperties()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    BulkUpdateData = bulkUpdateData
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response.ObjectIdAndChangeTokens;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument CreateDocument(Core.Collections.cmisPropertiesType properties, string folderId = null, Messaging.cmisContentStreamType contentStream = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            return base.CreateDocument(properties, folderId, contentStream, versioningState, policies, addACEs, removeACEs);
        }

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument CreateDocumentFromSource(string sourceId, Core.Collections.cmisPropertiesType properties = null, string folderId = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            return base.CreateDocumentFromSource(sourceId, properties, folderId, versioningState, policies, addACEs, removeACEs);
        }

        /// <summary>
      /// Creates a folder object of the specified type (given by the cmis:objectTypeId property)
      /// </summary>
      /// <remarks></remarks>
        public CmisFolder CreateFolder(Core.Collections.cmisPropertiesType properties, string folderId = null, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = _client.CreateFolder(new cmr.createFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = string.IsNullOrEmpty(folderId) ? RootFolderId : folderId,
                    Properties = properties,
                    Policies = policies,
                    AddACEs = addACEs,
                    RemoveACEs = removeACEs
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return GetObject(withBlock.Response.ObjectId) as CmisFolder;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Creates a relationship object of the specified type (given by the cmis:objectTypeId property)
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisRelationship CreateRelationship(Core.Collections.cmisPropertiesType properties, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = _client.CreateRelationship(new cmr.createRelationship()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    Properties = properties,
                    Policies = policies,
                    AddACEs = addACEs,
                    RemoveACEs = removeACEs
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return GetObject(withBlock.Response.ObjectId) as CmisRelationship;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisObject GetObject(string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            return base.GetObject(objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions);
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new TResult GetObject<TResult>(string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default) where TResult : CmisObject
        {
            return base.GetObject(objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as TResult;
        }

        /// <summary>
      /// Gets the specified information for the latest object in the version series
      /// </summary>
        public new CmisDocument GetObjectOfLatestVersion(string objectId, bool major = false, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, enumCheckedOutState acceptPWC = enumCheckedOutState.notCheckedOut)
        {
            return base.GetObjectOfLatestVersion(objectId, major, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, acceptPWC);
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObject GetObjectByPath(string path, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.GetObjectByPath(new cmr.getObjectByPath()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    Path = path,
                    Filter = filter,
                    IncludeRelationships = includeRelationships,
                    IncludePolicyIds = includePolicyIds,
                    RenditionFilter = renditionFilter,
                    IncludeACL = includeACL,
                    IncludeAllowableActions = includeAllowableActions
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return CreateCmisObject(withBlock.Response.Object);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public TResult GetObjectByPath<TResult>(string path, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default) where TResult : CmisObject
        {
            return GetObjectByPath(path, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as TResult;
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <remarks></remarks>
        public new CmisObject MoveObject(string objectId, string targetFolderId, string sourceFolderId)
        {
            return base.MoveObject(objectId, targetFolderId, sourceFolderId);
        }
        #endregion

        #region Discovery
        /// <summary>
      /// Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to
      /// efficiently understand what has changed in the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks>Caution: Through the AtomPub binding it is not possible to retrieve the ChangeLog Token</remarks>
        public Generic.ItemList<CmisObject> GetContentChanges(ref string changeLogToken, bool includeProperties = false, bool includePolicyIds = false, bool? includeACL = default, long? maxItems = default)
        {
            {
                var withBlock = _client.GetContentChanges(new cmr.getContentChanges()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ChangeLogToken = changeLogToken,
                    IncludeProperties = includeProperties,
                    IncludePolicyIds = includePolicyIds,
                    IncludeACL = includeACL,
                    MaxItems = maxItems
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    changeLogToken = withBlock.Response.ChangeLogToken;
                    return Convert(withBlock.Response.Objects);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Executes a CMIS query statement against the contents of the repository
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemList<CmisObject> Query(string statement, bool? searchAllVersions = default, Core.enumIncludeRelationships includeRelationships = Core.enumIncludeRelationships.none, string renditionFilter = null, bool includeAllowableActions = false, long? maxItems = default, long? skipCount = default)
        {
            {
                var withBlock = _client.Query(new cmr.query()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    Statement = statement,
                    SearchAllVersions = searchAllVersions,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
                    IncludeAllowableActions = includeAllowableActions,
                    MaxItems = maxItems,
                    SkipCount = skipCount
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return Convert(withBlock.Response.Objects);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents
      /// in the version series to be checked out again. If the private working copy has been created by createDocument, cancelCheckOut MUST
      /// delete the created document
      /// </summary>
        public CmisDocument CancelCheckOut(string objectId, bool major = false, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            var document = GetObject<CmisDocument>(objectId, string.Join(",", CmisPredefinedPropertyNames.VersionSeriesId, CmisPredefinedPropertyNames.IsPrivateWorkingCopy));

            // GetObject mit Filterangabe führt bei AGORUM zu keinem Ergebnis. Deshalb hier nochmal ohne Filter anfragen, wenn document Nothing ist. Siehe #7522.
            if (document is null)
            {
                document = GetObject<CmisDocument>(objectId);
            }

            var isPrivateWorkingCopy = document is null ? default : document.IsPrivateWorkingCopy;

            {
                var withBlock = _client.CancelCheckOut(new cmr.cancelCheckOut()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    PWCLinkRequired = !(isPrivateWorkingCopy.HasValue && isPrivateWorkingCopy.Value)
                });
                _lastException = withBlock.Exception;
                return _lastException is null ? GetObjectOfLatestVersion(document.VersionSeriesId.Value ?? objectId, major, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, enumCheckedOutState.notCheckedOut) : null;
            }
        }

        /// <summary>
      /// Checks-in the Private Working Copy document
      /// </summary>
        public CmisDocument CheckIn(string objectId, Core.Collections.cmisPropertiesType properties = null, Messaging.cmisContentStreamType contentStream = null, string checkinComment = null, bool major = true, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.CheckIn(new cmr.checkIn()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    Major = major,
                    Properties = properties,
                    ContentStream = contentStream,
                    CheckinComment = checkinComment
                });
                _lastException = withBlock.Exception;
                return _lastException is null ? GetObject(withBlock.Response.ObjectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as CmisDocument : null;
            }
        }

        /// <summary>
      /// Create a private working copy (PWC) of the document
      /// </summary>
        public CmisDocument CheckOut(string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.CheckOut(new cmr.checkOut() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = objectId });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    CmisDocument retVal = GetObject(withBlock.Response.ObjectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as CmisDocument;
                    if (retVal is not null)
                        retVal.CancelCheckOutFallbackId = objectId;
                    return retVal;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region pass-through properties
        public Core.Security.cmisACLCapabilityType AclCapabilities
        {
            get
            {
                return _repositoryInfo.AclCapability;
            }
        }

        public Core.cmisRepositoryCapabilitiesType Capabilities
        {
            get
            {
                return _repositoryInfo.Capabilities;
            }
        }

        public bool? ChangesIncomplete
        {
            get
            {
                return _repositoryInfo.ChangesIncomplete;
            }
        }

        public Core.enumBaseObjectTypeIds[] ChangesOnTypes
        {
            get
            {
                return _repositoryInfo.ChangesOnTypes;
            }
        }

        public string CmisVersionSupported
        {
            get
            {
                return _repositoryInfo.CmisVersionSupported;
            }
        }

        public string Description
        {
            get
            {
                return _repositoryInfo.RepositoryDescription;
            }
        }

        public string Id
        {
            get
            {
                return _repositoryInfo.RepositoryId;
            }
        }

        public string LatestChangeLogToken
        {
            get
            {
                return _repositoryInfo.LatestChangeLogToken;
            }
        }

        public string Name
        {
            get
            {
                return _repositoryInfo.RepositoryName;
            }
        }

        public string PrincipalAnonymus
        {
            get
            {
                return _repositoryInfo.PrincipalAnonymous;
            }
        }

        public string PrincipalAnyone
        {
            get
            {
                return _repositoryInfo.PrincipalAnyone;
            }
        }

        public string ProductName
        {
            get
            {
                return _repositoryInfo.ProductName;
            }
        }

        public string ProductVersion
        {
            get
            {
                return _repositoryInfo.ProductVersion;
            }
        }

        public string RootFolderId
        {
            get
            {
                return _repositoryInfo.RootFolderId;
            }
        }

        public string ThinClientUri
        {
            get
            {
                return _repositoryInfo.ThinClientURI;
            }
        }

        public string VendorName
        {
            get
            {
                return _repositoryInfo.VendorName;
            }
        }
        #endregion

        /// <summary>
      /// Returns the root folder object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisFolder GetRootFolder(string filter = null, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            return GetObject(_repositoryInfo.RootFolderId, filter, default, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as CmisFolder;
        }

        /// <summary>
      /// Returns true if the repository supports holds
      /// </summary>
        public new bool HoldCapability
        {
            get
            {
                return base.HoldCapability;
            }
        }

        /// <summary>
      /// Logs out from repository
      /// </summary>
      /// <remarks></remarks>
        public void Logout()
        {
            _client.Logout(_repositoryInfo.RepositoryId);
        }

        /// <summary>
      /// Tells the server, that this client is still alive
      /// </summary>
      /// <remarks></remarks>
        public void Ping()
        {
            _client.Ping(_repositoryInfo.RepositoryId);
        }

        /// <summary>
      /// Returns the retentions supported by the repository
      /// </summary>
        public new enumRetentionCapability RetentionCapability
        {
            get
            {
                return base.RetentionCapability;
            }
        }

    }
}