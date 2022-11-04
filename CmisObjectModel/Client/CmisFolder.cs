using System.Collections.Generic;
using ccg = CmisObjectModel.Common.Generic;
using cmr = CmisObjectModel.Messaging.Requests;
/* TODO ERROR: Skipped IfDirectiveTrivia
#If Not xs_HttpRequestAddRange64 Then
*//* TODO ERROR: Skipped DisabledTextTrivia
#Const HttpRequestAddRangeShortened = True
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Simplifies requests to cmis folder
   /// </summary>
   /// <remarks></remarks>
    public class CmisFolder : CmisObject
    {

        #region Constructors
        public CmisFolder(Core.cmisObjectType cmisObject, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(cmisObject, client, repositoryInfo)
        {
        }
        #endregion

        #region Predefined properties
        public virtual ccg.Nullable<string[]> AllowedChildObjectTypeIds
        {
            get
            {
                return _cmisObject.AllowedChildObjectTypeIds;
            }
            set
            {
                _cmisObject.AllowedChildObjectTypeIds = value;
            }
        }

        public virtual ccg.Nullable<string> ParentId
        {
            get
            {
                return _cmisObject.ParentId;
            }
            set
            {
                _cmisObject.ParentId = value;
            }
        }

        public virtual ccg.Nullable<string> Path
        {
            get
            {
                return _cmisObject.Path;
            }
            set
            {
                _cmisObject.Path = value;
            }
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out in the current folder that the user has access to
      /// </summary>
      /// <remarks></remarks>
        public new Generic.ItemList<CmisObject> GetCheckedOutDocs(long? maxItems = default, long? skipCount = default, string orderBy = null, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default)
        {
            return GetCheckedOutDocs(_cmisObject.ObjectId, maxItems, skipCount, orderBy, filter, includeRelationships, renditionFilter, includeAllowableActions);
        }

        /// <summary>
      /// Gets the list of child objects contained in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemList<CmisObject> GetChildren(long? maxItems = default, long? skipCount = default, string orderBy = null, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default, bool includePathSegment = false)
        {
            {
                var withBlock = _client.GetChildren(new cmr.getChildren()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
                    MaxItems = maxItems,
                    SkipCount = skipCount,
                    OrderBy = orderBy,
                    Filter = filter,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
                    IncludeAllowableActions = includeAllowableActions,
                    IncludePathSegment = includePathSegment
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

        /// <summary>
      /// Gets the set of descendant objects containded in the current folder or any of its child-folders
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemContainer<CmisObject>[] GetDescendants(long? depth = default, string filter = null, bool? includeAllowableActions = default, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool includePathSegment = false)
        {
            {
                var withBlock = _client.GetDescendants(new cmr.getDescendants()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
                    Depth = depth,
                    Filter = filter,
                    IncludeAllowableActions = includeAllowableActions,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
                    IncludePathSegment = includePathSegment
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return Transform(withBlock.Response.Objects, new List<Generic.ItemContainer<CmisObject>>()).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets the parent folder object for the current folder object
      /// </summary>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisFolder GetParent(string filter = null)
        {
            {
                var withBlock = _client.GetFolderParent(new cmr.getFolderParent() { RepositoryId = _repositoryInfo.RepositoryId, FolderId = _cmisObject.ObjectId, Filter = filter });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return CreateCmisObject(withBlock.Response.Object) as CmisFolder;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets the set of descendant folder objects contained in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemContainer<CmisObject>[] GetTree(long? depth = default, string filter = null, bool? includeAllowableActions = default, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool includePathSegment = false)
        {
            {
                var withBlock = _client.GetFolderTree(new cmr.getFolderTree()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
                    Depth = depth,
                    Filter = filter,
                    IncludeAllowableActions = includeAllowableActions,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
                    IncludePathSegment = includePathSegment
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return Transform(withBlock.Response.Objects, new List<Generic.ItemContainer<CmisObject>>()).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Object
        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument CreateDocument(Core.Collections.cmisPropertiesType properties, Messaging.cmisContentStreamType contentStream = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            return CreateDocument(properties, _cmisObject.ObjectId, contentStream, versioningState, policies, addACEs, removeACEs);
        }

        /// <summary>
      /// Creates a document object as a copy of the given source document in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument CreateDocumentFromSource(string sourceId, Core.Collections.cmisPropertiesType properties = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            return CreateDocumentFromSource(sourceId, properties, _cmisObject.ObjectId, versioningState, policies, addACEs, removeACEs);
        }

        /// <summary>
      /// Creates a folder object of the specified type (given by the cmis:objectTypeId property) in the current folder
      /// </summary>
      /// <remarks></remarks>
        public CmisFolder CreateFolder(Core.Collections.cmisPropertiesType properties, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = _client.CreateFolder(new cmr.createFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
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
      /// Creates a item object of the specified type (given by the cmis:objectTypeId property) in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObject CreateItem(Core.Collections.cmisPropertiesType properties, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = _client.CreateItem(new cmr.createItem()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
                    Properties = properties,
                    Policies = policies,
                    AddACEs = addACEs,
                    RemoveACEs = removeACEs
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return GetObject(withBlock.Response.ObjectId);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Creates a policy object of the specified type (given by the cmis:objectTypeId property) in the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisPolicy CreatePolicy(Core.Collections.cmisPropertiesType properties, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = _client.CreatePolicy(new cmr.createPolicy()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    Properties = properties,
                    FolderId = _cmisObject.ObjectId,
                    Policies = policies,
                    AddACEs = addACEs,
                    RemoveACEs = removeACEs
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return GetObject(withBlock.Response.ObjectId) as CmisPolicy;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Deletes the current folder object and all of its child- and descendant-objects
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Messaging.failedToDelete DeleteTree(bool allVersions = true, Core.enumUnfileObject unfileObjects = Core.enumUnfileObject.delete, bool continueOnFailure = false)
        {
            {
                var withBlock = _client.DeleteTree(new cmr.deleteTree()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = _cmisObject.ObjectId,
                    AllVersions = allVersions,
                    UnfileObjects = unfileObjects,
                    ContinueOnFailure = continueOnFailure
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response.FailedToDelete;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets a rendition stream for a specified rendition of the current folder object
      /// </summary>
      /// <param name="streamId"></param>
      /// <param name="offset"></param>
      /// <param name="length"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Public Shadows Function GetContentStream(streamId As String,
                                                       Optional offset As Integer? = Nothing,
                                                       Optional length As Integer? = Nothing) As Messaging.cmisContentStreamType
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        public new Messaging.cmisContentStreamType GetContentStream(string streamId, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            return base.GetContentStream(streamId, offset, length);
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to the current folder
      /// </summary>
      /// <remarks></remarks>
        public new CmisObject MoveObjectFrom(string objectId, string sourceFolderId)
        {
            return MoveObject(objectId, _cmisObject.ObjectId, sourceFolderId);
        }

        /// <summary>
      /// Moves the specified file-able object from the current folder to another folder
      /// </summary>
      /// <remarks></remarks>
        public new CmisObject MoveObjectTo(string objectId, string targetFolderId)
        {
            return MoveObject(objectId, targetFolderId, _cmisObject.ObjectId);
        }
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to the current folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool AddObject(string objectId, bool allVersions = true)
        {
            {
                var withBlock = _client.AddObjectToFolder(new cmr.addObjectToFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    FolderId = _cmisObject.ObjectId,
                    AllVersions = allVersions
                });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Folders are non-filable objects, therefore this method should not be used
      /// </summary>
      /// <param name="folderId"></param>
      /// <param name="allVersions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool AddObjectToFolder(string folderId, bool allVersions = true)
        {
            return false;
        }

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool RemoveObject(string objectId)
        {
            {
                var withBlock = _client.RemoveObjectFromFolder(new cmr.removeObjectFromFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    FolderId = _cmisObject.ObjectId
                });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Folders are non-filable objects, therefore this method should not be used
      /// </summary>
      /// <param name="folderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool RemoveObjectFromFolder(string folderId = null)
        {
            return false;
        }
        #endregion

        /// <summary>
      /// Transforms the cmisObjectInFolderContainerType()-structure into a List(Of ItemContainer(Of CmisObject))-structure
      /// </summary>
      /// <param name="source"></param>
      /// <param name="result"></param>
      /// <remarks></remarks>
        private List<Generic.ItemContainer<CmisObject>> Transform(Messaging.cmisObjectInFolderContainerType[] source, List<Generic.ItemContainer<CmisObject>> result)
        {
            result.Clear();
            if (source is not null)
            {
                foreach (Messaging.cmisObjectInFolderContainerType objectInFolderContainer in source)
                {
                    var objectInFolder = objectInFolderContainer is null ? null : objectInFolderContainer.ObjectInFolder;

                    if (objectInFolder is not null)
                    {
                        var cmisObject = CreateCmisObject(objectInFolder.Object);
                        var container = new Generic.ItemContainer<CmisObject>(cmisObject);

                        cmisObject.PathSegment = objectInFolder.PathSegment;
                        result.Add(container);
                        Transform(objectInFolderContainer.Children, container.Children);
                    }
                }
            }

            return result;
        }

    }
}