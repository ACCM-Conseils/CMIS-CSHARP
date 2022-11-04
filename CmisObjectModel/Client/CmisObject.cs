using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
   /// Simplifies requests to cmis document, cmis folder, cmis policy, cmis relationship
   /// </summary>
   /// <remarks></remarks>
    public class CmisObject : CmisDataModelObject
    {

        #region Constructors
        public CmisObject(Core.cmisObjectType cmisObject, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(client, repositoryInfo)
        {
            _cmisObject = cmisObject;
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Creates the CmisType-Instance
      /// </summary>
      /// <remarks></remarks>
        public class PreStage
        {
            public PreStage(Contracts.ICmisClient client, Core.cmisObjectType cmisObject)
            {
                _client = client;
                _cmisObject = cmisObject;
            }

            private Contracts.ICmisClient _client;
            private Core.cmisObjectType _cmisObject;

            public static CmisObject operator +(PreStage arg1, Core.cmisRepositoryInfoType arg2)
            {
                var baseTypeId = arg1._cmisObject is null ? default : arg1._cmisObject.BaseTypeId;

                switch (baseTypeId.HasValue ? baseTypeId.Value : string.Empty ?? "")
                {
                    case var @case when @case == (Core.enumBaseObjectTypeIds.cmisDocument.GetName() ?? ""):
                        {
                            return new CmisDocument(arg1._cmisObject, arg1._client, arg2);
                        }
                    case var case1 when case1 == (Core.enumBaseObjectTypeIds.cmisFolder.GetName() ?? ""):
                        {
                            return new CmisFolder(arg1._cmisObject, arg1._client, arg2);
                        }
                    case var case2 when case2 == (Core.enumBaseObjectTypeIds.cmisPolicy.GetName() ?? ""):
                        {
                            return new CmisPolicy(arg1._cmisObject, arg1._client, arg2);
                        }
                    case var case3 when case3 == (Core.enumBaseObjectTypeIds.cmisRelationship.GetName() ?? ""):
                        {
                            return new CmisRelationship(arg1._cmisObject, arg1._client, arg2);
                        }

                    default:
                        {
                            return new CmisObject(arg1._cmisObject, arg1._client, arg2);
                        }
                }
            }
        }
        #endregion

        #region Predefined properties
        public virtual ccg.Nullable<string> BaseTypeId
        {
            get
            {
                return _cmisObject.BaseTypeId;
            }
            set
            {
                _cmisObject.BaseTypeId = value;
            }
        }

        public virtual ccg.Nullable<string> ChangeToken
        {
            get
            {
                return _cmisObject.ChangeToken;
            }
            set
            {
                _cmisObject.ChangeToken = value;
            }
        }

        public virtual ccg.Nullable<string> CreatedBy
        {
            get
            {
                return _cmisObject.CreatedBy;
            }
            set
            {
                _cmisObject.CreatedBy = value;
            }
        }

        public virtual DateTimeOffset? CreationDate
        {
            get
            {
                return _cmisObject.CreationDate;
            }
            set
            {
                _cmisObject.CreationDate = value;
            }
        }

        public virtual ccg.Nullable<string> Description
        {
            get
            {
                return _cmisObject.Description;
            }
            set
            {
                _cmisObject.Description = value;
            }
        }

        public virtual DateTimeOffset? LastModificationDate
        {
            get
            {
                return _cmisObject.LastModificationDate;
            }
            set
            {
                _cmisObject.LastModificationDate = value;
            }
        }

        public virtual ccg.Nullable<string> LastModifiedBy
        {
            get
            {
                return _cmisObject.LastModifiedBy;
            }
            set
            {
                _cmisObject.LastModifiedBy = value;
            }
        }

        public virtual ccg.Nullable<string> Name
        {
            get
            {
                return _cmisObject.Name;
            }
            set
            {
                _cmisObject.Name = value;
            }
        }

        public virtual ccg.Nullable<string> ObjectId
        {
            get
            {
                return _cmisObject.ObjectId;
            }
            set
            {
                _cmisObject.ObjectId = value;
            }
        }

        public virtual ccg.Nullable<string> ObjectTypeId
        {
            get
            {
                return _cmisObject.ObjectTypeId;
            }
            set
            {
                _cmisObject.ObjectTypeId = value;
            }
        }

        public virtual ccg.Nullable<string[]> SecondaryObjectTypeIds
        {
            get
            {
                return _cmisObject.SecondaryObjectTypeIds;
            }
            set
            {
                _cmisObject.SecondaryObjectTypeIds = value;
            }
        }
        #endregion

        #region Pass-through-methods
        /// <summary>
      /// Returns the objectTypeId followed by the secondaryObjectTypeIds separated by comma
      /// </summary>
        public virtual string GetCompositeObjectTypeId()
        {
            return _cmisObject.GetCompositeObjectTypeId();
        }

        /// <summary>
      /// Returns as first element the objectTypeId of the current object followed by the secondaryTypeIds if defined
      /// </summary>
        public virtual IEnumerable<string> GetObjectTypeIds()
        {
            return _cmisObject.GetObjectTypeIds();
        }
        #endregion

        #region Pass-through-properties
        public virtual Core.Security.cmisAccessControlListType Acl
        {
            get
            {
                return _cmisObject.Acl;
            }
            set
            {
                _cmisObject.Acl = value;
            }
        }

        public virtual Core.cmisAllowableActionsType AllowableActions
        {
            get
            {
                return _cmisObject.AllowableActions;
            }
            set
            {
                _cmisObject.AllowableActions = value;
            }
        }

        public virtual Core.cmisChangeEventType ChangeEventInfo
        {
            get
            {
                return _cmisObject.ChangeEventInfo;
            }
            set
            {
                _cmisObject.ChangeEventInfo = value;
            }
        }

        public virtual bool? ExactAcl
        {
            get
            {
                return _cmisObject.ExactACL;
            }
            set
            {
                _cmisObject.ExactACL = value;
            }
        }

        public virtual Core.Collections.cmisListOfIdsType PolicyIds
        {
            get
            {
                return _cmisObject.PolicyIds;
            }
            set
            {
                _cmisObject.PolicyIds = value;
            }
        }

        public virtual Core.Collections.cmisPropertiesType Properties
        {
            get
            {
                return _cmisObject.Properties;
            }
            set
            {
                _cmisObject.Properties = value;
            }
        }

        public virtual CmisRelationship[] Relationships
        {
            get
            {
                if (_cmisObject.Relationships is null)
                {
                    return null;
                }
                else
                {
                    return (from relationship in _cmisObject.Relationships
                            select new CmisRelationship(relationship, _client, _repositoryInfo)).ToArray();
                }
            }
            set
            {
                if (value is null)
                {
                    _cmisObject.Relationships = null;
                }
                else
                {
                    _cmisObject.Relationships = (from relationship in value
                                                 select relationship is null ? null : relationship._cmisObject).ToArray();
                }
            }
        }

        public virtual Core.cmisRenditionType[] Renditions
        {
            get
            {
                return _cmisObject.Renditions;
            }
            set
            {
                _cmisObject.Renditions = value;
            }
        }
        #endregion

        #region Repository
        /// <summary>
      /// Returns BaseType for the current object or null, if BaseTypeId is not specified
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisType GetObjectBaseType()
        {
            return GetTypeDefinition(BaseTypeId);
        }

        /// <summary>
      /// Returns ObjectType for the current object or null, if ObjectTypeId is not specified
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisType GetObjectType()
        {
            return GetTypeDefinition(ObjectTypeId);
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the parent folder(s) for the current fileable object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObject[] GetParents(string filter = null, Core.enumIncludeRelationships? includeRelationships = default, string renditionFilter = null, bool? includeAllowableActions = default, bool? includeRelativePathSegment = default)
        {
            {
                var withBlock = _client.GetObjectParents(new cmr.getObjectParents()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    Filter = filter,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
                    IncludeAllowableActions = includeAllowableActions,
                    IncludeRelativePathSegment = includeRelativePathSegment
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    var result = new List<CmisObject>();

                    if (withBlock.Response.Parents is not null)
                    {
                        foreach (Messaging.cmisObjectParentsType parent in withBlock.Response.Parents)
                        {
                            var cmisObject = CreateCmisObject(parent.Object);

                            cmisObject.RelativePathSegment = parent.RelativePathSegment;
                            result.Add(cmisObject);
                        }
                    }

                    return result.ToArray();
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
      /// Deletes the current object
      /// </summary>
        public virtual bool DeleteObject(bool allVersions = true)
        {
            {
                var withBlock = _client.DeleteObject(new cmr.deleteObject() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, AllVersions = allVersions });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Gets the list of allowable actions for an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.cmisAllowableActionsType GetAllowableActions()
        {
            {
                var withBlock = _client.GetAllowableActions(new cmr.getAllowableActions() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId });
                _lastException = withBlock.Exception;
                return _lastException is null ? withBlock.Response.AllowableActions : null;
            }
        }

        /// <summary>
      /// Gets the content stream for the specified document object, or gets a rendition stream for a specified rendition of a document or folder object.
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Protected Function GetContentStream(Optional streamId As String = Nothing,
                                                  Optional offset As Integer? = Nothing,
                                                  Optional length As Integer? = Nothing) As Messaging.cmisContentStreamType
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        protected Messaging.cmisContentStreamType GetContentStream(string streamId = null, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            {
                var withBlock = _client.GetContentStream(new cmr.getContentStream()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    StreamId = streamId,
                    Offset = offset,
                    Length = length
                });
                _lastException = withBlock.Exception;
                return _lastException is null ? withBlock.Response.ContentStream : null;
            }
        }

        /// <summary>
      /// Gets the list of properties for the current object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.Collections.cmisPropertiesType GetProperties(string filter = null)
        {
            {
                var withBlock = _client.GetProperties(new cmr.getProperties() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, Filter = filter });
                _lastException = withBlock.Exception;
                return _lastException is null ? withBlock.Response.Properties : null;
            }
        }

        /// <summary>
      /// Gets the list of associated renditions for the specified object. Only rendition attributes are returned, not rendition stream
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.cmisRenditionType[] GetRenditions(string renditionFilter = null, long? maxItems = default, long? skipCount = default)
        {
            {
                var withBlock = _client.GetRenditions(new cmr.getRenditions()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    RenditionFilter = renditionFilter,
                    MaxItems = maxItems,
                    SkipCount = skipCount
                });
                _lastException = withBlock.Exception;
                return _lastException is null ? withBlock.Response.Renditions : null;
            }
        }

        /// <summary>
      /// Moves the current file-able object from one folder to another
      /// </summary>
      /// <remarks></remarks>
        public new CmisObject Move(string targetFolderId, string sourceFolderId)
        {
            return MoveObject(_cmisObject.ObjectId, targetFolderId, sourceFolderId);
        }

        /// <summary>
      /// Updates properties and secondary types of the current object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool UpdateProperties(Core.Collections.cmisPropertiesType properties)
        {
            {
                var withBlock = _client.UpdateProperties(new cmr.updateProperties()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    Properties = properties,
                    ChangeToken = _cmisObject.ChangeToken
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    string objectId = withBlock.Response.ObjectId;
                    string changeToken = withBlock.Response.ChangeToken;

                    if (!string.IsNullOrEmpty(objectId))
                        _cmisObject.ObjectId = objectId;
                    if (!string.IsNullOrEmpty(changeToken))
                        _cmisObject.ChangeToken = changeToken;
                    // update property values
                    if (!(properties is null || properties.Properties is null))
                    {
                        if (_cmisObject.Properties is null)
                        {
                            _cmisObject.Properties = properties;
                        }
                        else
                        {
                            var currentProperties = _cmisObject.GetProperties(true);

                            foreach (Core.Properties.cmisProperty property in properties.Properties)
                            {
                                string propertyDefinitionId = (property.PropertyDefinitionId ?? "").ToLowerInvariant();
                                if (currentProperties.ContainsKey(propertyDefinitionId))
                                {
                                    currentProperties[propertyDefinitionId].Values = property.Values;
                                }
                                else
                                {
                                    _cmisObject.Properties.Append(property);
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Multi
        /// <summary>
      /// Adds the current fileable non-folder object to a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual bool AddObjectToFolder(string folderId, bool allVersions = true)
        {
            {
                var withBlock = _client.AddObjectToFolder(new cmr.addObjectToFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    FolderId = folderId,
                    AllVersions = allVersions
                });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual bool RemoveObjectFromFolder(string folderId = null)
        {
            {
                var withBlock = _client.RemoveObjectFromFolder(new cmr.removeObjectFromFolder()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    FolderId = folderId
                });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }
        #endregion

        #region Relationship
        /// <summary>
      /// Gets all or a subset of relationships associated with an independent object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Generic.ItemList<CmisObject> GetObjectRelationships(bool includeSubRelationshipTypes = false, Core.enumRelationshipDirection relationshipDirection = Core.enumRelationshipDirection.source, string typeId = null, long? maxItems = default, long? skipCount = default, string filter = null, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.GetObjectRelationships(new cmr.getObjectRelationships()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    IncludeSubRelationshipTypes = includeSubRelationshipTypes,
                    RelationshipDirection = relationshipDirection,
                    TypeId = typeId,
                    MaxItems = maxItems,
                    SkipCount = skipCount,
                    Filter = filter,
                    IncludeAllowableActions = includeAllowableActions
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

        #region Policy
        /// <summary>
      /// Applies a specified policy to the current object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool ApplyPolicy(string policyId)
        {
            {
                var withBlock = _client.ApplyPolicy(new cmr.applyPolicy() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, PolicyId = policyId });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Gets the list of policies currently applied to the current object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisPolicy[] GetAppliedPolicies(string filter = null)
        {
            {
                var withBlock = _client.GetAppliedPolicies(new cmr.getAppliedPolicies() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, Filter = filter });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return (from @object in withBlock.Response.Objects
                            let policy = CreateCmisObject(@object) as CmisPolicy
                            where policy is not null
                            select policy).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Removes a specified policy from the current object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool RemovePolicy(string policyId)
        {
            {
                var withBlock = _client.RemovePolicy(new cmr.removePolicy() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, PolicyId = policyId });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }
        #endregion

        #region Acl
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of an object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Messaging.cmisACLType ApplyAcl(Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs, Core.enumACLPropagation aclPropagation = Core.enumACLPropagation.repositorydetermined)
        {
            {
                var withBlock = _client.ApplyAcl(new cmr.applyACL()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    AddACEs = addACEs,
                    RemoveACEs = removeACEs,
                    ACLPropagation = aclPropagation
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response.ACL;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Get the ACL currently applied to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Messaging.cmisACLType GetAcl(bool onlyBasicPermissions = true)
        {
            {
                var withBlock = _client.GetAcl(new cmr.getACL() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId, OnlyBasicPermissions = onlyBasicPermissions });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response.ACL;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region AllowableActions
        public bool CanAddObjectToFolder
        {
            get
            {
                if (_cmisObject.AllowableActions is null)
                {
                    return false;
                }
                else
                {
                    {
                        var withBlock = _cmisObject.AllowableActions.CanAddObjectToFolder;

                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanAddObjectToFolder = value;
            }
        }
        #endregion

        protected Core.cmisObjectType _cmisObject;
        public Core.cmisObjectType Object
        {
            get
            {
                return _cmisObject;
            }
        }

        /// <summary>
      /// Access to properties via index or PropertyDefinitionId
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public Collections.Generic.ArrayMapper<Core.Collections.cmisPropertiesType, Core.Properties.cmisProperty> PropertiesAsReadOnly
        {
            get
            {
                return _cmisObject.PropertiesAsReadOnly;
            }
        }

        public string PathSegment { get; set; }
        public string RelativePathSegment { get; set; }

        /// <summary>
      /// Returns the CmisType for specified typeId
      /// </summary>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected override CmisType GetTypeDefinition(string typeId)
        {
            return string.IsNullOrEmpty(typeId) ? null : base.GetTypeDefinition(typeId);
        }

    }
}