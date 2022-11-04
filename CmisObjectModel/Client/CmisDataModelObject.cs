using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sn = System.Net;
using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
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
using CmisObjectModel.Client.Generic;
using CmisObjectModel.Common;
using CmisObjectModel.Constants;
using cmr = CmisObjectModel.Messaging.Requests;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Simplifies requests to cmis document, cmis folder, cmis policy, cmis relationship, cmis TypeDefinition, cmis repository
   /// </summary>
   /// <remarks></remarks>
    public abstract class CmisDataModelObject
    {

        protected const string VersioningStateCheckedOutPrefix = "{6cb02a6d-eccb-4126-a6ac-2a27cf76210e}";

        #region Constructors
        protected CmisDataModelObject(Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo)
        {
            _client = client;
            _repositoryInfo = repositoryInfo;
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Some cmis repositories do not fulfil the cmis-specification specified in
      /// http://docs.oasis-open.org/cmis/CMIS/v1.1/errata01/os/CMIS-v1.1-errata01-os-complete.html#x1-3280002
      /// When creating a new document with the versioningState checkedout the document must be deleted completely if
      /// the checkedout state of the document is terminated by a CancelCheckOut()-call. To make the client be able
      /// to detect documents created by
      ///   CreateDocument(versioningState:=checkedout)
      /// the description of the document (cmis:description) is marked with a predefined GUID
      /// </summary>
        private class CreateDocumentService
        {

            public CreateDocumentService(CmisDataModelObject owner, Core.Collections.cmisPropertiesType properties, string folderId, Messaging.cmisContentStreamType contentStream, Core.enumVersioningState? versioningState, Core.Collections.cmisListOfIdsType policies, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs)
            {
                _owner = owner;
                if (properties is null && versioningState.HasValue && versioningState.Value == Core.enumVersioningState.checkedout)
                {
                    _properties = new Core.Collections.cmisPropertiesType();
                }
                else
                {
                    _properties = properties;
                }
                _folderId = folderId;
                _contentStream = contentStream;
                _versioningState = versioningState;
                _policies = policies;
                _addACEs = addACEs;
                _removeACEs = removeACEs;
            }

            protected Core.Security.cmisAccessControlListType _addACEs;
            private Messaging.cmisContentStreamType _contentStream;
            protected string _description;
            protected string _folderId;

            public CmisDocument Invoke()
            {
                bool checkedOutRequired = _versioningState.HasValue && _versioningState.Value == Core.enumVersioningState.checkedout;

                _owner._lastException = null;
                try
                {
                    // make sure the client is able to detect a newly created document with versioning state with checkedout
                    if (checkedOutRequired)
                        LabelDescription();
                    {
                        var withBlock = InvokeCore();
                        _owner._lastException = withBlock.Exception;
                        if (_owner._lastException is null)
                        {
                            CmisDocument retVal = _owner.GetObject(withBlock.Response) as CmisDocument;
                            // if the return document is valid,
                            if (retVal is not null && checkedOutRequired)
                            {
                                // ... but is not the requested pwc-version
                                if (!(retVal.IsPrivateWorkingCopy.HasValue && retVal.IsPrivateWorkingCopy.Value))
                                {
                                    // ... we have to request for the pwc; in case of repositories not supporting the versionstate-parameter we have to check out the
                                    // newly created document
                                    if (retVal.IsVersionSeriesCheckedOut.HasValue && retVal.IsVersionSeriesCheckedOut.Value || !retVal.CheckOut())
                                    {
                                        retVal = retVal.GetObjectOfLatestVersion(acceptPWC: enumCheckedOutState.checkedOutByMe);
                                    }
                                }
                                // reset description property in pwc
                                retVal.Description = _description;
                                var descriptionProperty = retVal.Object.Properties.FindProperty<Core.Properties.cmisPropertyString>(CmisPredefinedPropertyNames.Description);
                                if (descriptionProperty is not null)
                                    retVal.UpdateProperties(new Core.Collections.cmisPropertiesType(descriptionProperty));
                                // bind to the PWCRemovedListeners
                                retVal.AddPWCRemovedListeners();
                            }

                            return retVal;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _owner._lastException = new ssw.WebFaultException<Exception>(ex, sn.HttpStatusCode.BadRequest);
                }

                return null;
            }

            /// <summary>
         /// CreateDocument()
         /// </summary>
            protected virtual ResponseType<string> InvokeCore()
            {
                {
                    var withBlock = _owner._client.CreateDocument(new cmr.createDocument()
                    {
                        RepositoryId = _owner._repositoryInfo.RepositoryId,
                        Properties = _properties,
                        FolderId = _folderId,
                        ContentStream = _contentStream,
                        VersioningState = _versioningState,
                        Policies = _policies,
                        AddACEs = _addACEs,
                        RemoveACEs = _removeACEs
                    });
                    if (withBlock.Exception is not null)
                    {
                        return withBlock.Exception;
                    }
                    else
                    {
                        return withBlock.Response.ObjectId;
                    }
                }
            }

            /// <summary>
         /// Mark description property
         /// </summary>
            protected virtual void LabelDescription()
            {
                Core.Properties.cmisProperty descriptionProperty = null;
                var properties = _properties.GetProperties(enumKeySyntax.original, CmisPredefinedPropertyNames.Description, CmisPredefinedPropertyNames.ObjectTypeId);
                string objectTypeId;

                if (properties.ContainsKey(CmisPredefinedPropertyNames.Description))
                {
                    descriptionProperty = properties[CmisPredefinedPropertyNames.Description];
                }
                else
                {
                    CmisType cmisType;

                    objectTypeId = properties.ContainsKey(CmisPredefinedPropertyNames.ObjectTypeId) ? Conversions.ToString(properties[CmisPredefinedPropertyNames.ObjectTypeId].Value) : null;
                    if (string.IsNullOrEmpty(objectTypeId))
                        objectTypeId = Core.Definitions.Types.cmisTypeDocumentDefinitionType.TargetTypeName;
                    cmisType = _owner.GetTypeDefinition(objectTypeId);
                    if (cmisType is not null)
                    {
                        var pdc = cmisType.Type.GetPropertyDefinitions(CmisPredefinedPropertyNames.Description);
                        if (pdc.Count == 1)
                        {
                            descriptionProperty = pdc[CmisPredefinedPropertyNames.Description].CreateProperty();
                            _properties.Append(descriptionProperty);
                        }
                    }
                }
                if (descriptionProperty is not null)
                {
                    _description = Conversions.ToString(descriptionProperty.Value);
                    descriptionProperty.Value = VersioningStateCheckedOutPrefix + _description;
                }
            }

            protected CmisDataModelObject _owner;
            protected Core.Collections.cmisListOfIdsType _policies;
            protected Core.Collections.cmisPropertiesType _properties;
            protected Core.Security.cmisAccessControlListType _removeACEs;
            protected Core.enumVersioningState? _versioningState;

        }

        /// <summary>
      /// Some cmis repositories do not fulfil the cmis-specification specified in
      /// http://docs.oasis-open.org/cmis/CMIS/v1.1/errata01/os/CMIS-v1.1-errata01-os-complete.html#x1-3280002
      /// When creating a new document with the versioningState checkedout the document must be deleted completely if
      /// the checkedout state of the document is terminated by a CancelCheckOut()-call. To make the client be able
      /// to detect documents created by
      ///   CreateDocumentFromSource(versioningState:=checkedout)
      /// the description of the document (cmis:description) is marked with a predefined GUID
      /// </summary>
        private class CreateDocumentFromSourceService : CreateDocumentService
        {

            public CreateDocumentFromSourceService(CmisDataModelObject owner, string sourceId, Core.Collections.cmisPropertiesType properties, string folderId, Core.enumVersioningState? versioningState, Core.Collections.cmisListOfIdsType policies, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) : base(owner, properties, folderId, null, versioningState, policies, addACEs, removeACEs)
            {
                _sourceId = sourceId;
            }

            /// <summary>
         /// CreateDocumentFromSource
         /// </summary>
            protected override ResponseType<string> InvokeCore()
            {
                {
                    var withBlock = _owner._client.CreateDocumentFromSource(new cmr.createDocumentFromSource()
                    {
                        RepositoryId = _owner._repositoryInfo.RepositoryId,
                        SourceId = _sourceId,
                        Properties = _properties,
                        FolderId = _folderId,
                        VersioningState = _versioningState,
                        Policies = _policies,
                        AddACEs = _addACEs,
                        RemoveACEs = _removeACEs
                    });
                    if (withBlock.Exception is not null)
                    {
                        return withBlock.Exception;
                    }
                    else
                    {
                        return withBlock.Response.ObjectId;
                    }
                }
            }

            /// <summary>
         /// Mark description property
         /// </summary>
            protected override void LabelDescription()
            {
                Core.Properties.cmisProperty descriptionProperty = null;
                var properties = _properties.GetProperties(enumKeySyntax.original, CmisPredefinedPropertyNames.Description, CmisPredefinedPropertyNames.ObjectTypeId);

                if (properties.ContainsKey(CmisPredefinedPropertyNames.Description))
                {
                    descriptionProperty = properties[CmisPredefinedPropertyNames.Description];
                }
                else
                {
                    var source = _owner.GetObject(_sourceId, CmisPredefinedPropertyNames.Description);
                    if (source is not null && source.Properties is not null)
                    {
                        descriptionProperty = source.Properties.FindProperty(CmisPredefinedPropertyNames.Description);
                        if (descriptionProperty is not null)
                            _properties.Append(descriptionProperty);
                    }
                }
                if (descriptionProperty is not null)
                {
                    _description = Conversions.ToString(descriptionProperty.Value);
                    descriptionProperty.Value = VersioningStateCheckedOutPrefix + _description;
                }
            }

            private string _sourceId;
        }
        #endregion

        #region Repository
        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the specified type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected ItemList<CmisType> GetTypeChildren(string typeId, bool includePropertyDefinitions, long? maxItems, long? skipCount)
        {
            {
                var withBlock = _client.GetTypeChildren(new cmr.getTypeChildren()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    TypeId = typeId,
                    IncludePropertyDefinitions = includePropertyDefinitions,
                    MaxItems = maxItems,
                    SkipCount = skipCount
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    var types = withBlock.Response.Types;
                    bool hasMoreItems = false;
                    long? numItems = default;
                    var result = new CmisType[] { };

                    if (types is not null)
                    {
                        hasMoreItems = types.HasMoreItems;
                        numItems = types.NumItems;
                        if (types.Types is not null)
                        {
                            result = (from type in types.Types
                                      select CreateCmisType(type)).ToArray();
                        }
                    }

                    return new ItemList<CmisType>(result, hasMoreItems, numItems);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
      /// <remarks></remarks>
        protected virtual CmisType GetTypeDefinition(string typeId)
        {
            {
                var withBlock = _client.GetTypeDefinition(new cmr.getTypeDefinition() { RepositoryId = _repositoryInfo.RepositoryId, TypeId = typeId });
                _lastException = withBlock.Exception;
                return _lastException is null ? CreateCmisType(withBlock.Response.Type) : null;
            }
        }

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected ItemContainer<CmisType>[] GetTypeDescendants(string typeId, long? depth, bool includePropertyDefinitions)
        {
            {
                var withBlock = _client.GetTypeDescendants(new cmr.getTypeDescendants()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    TypeId = typeId,
                    Depth = depth,
                    IncludePropertyDefinitions = includePropertyDefinitions
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return Transform(withBlock.Response.Types, new List<ItemContainer<CmisType>>()).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
      /// <remarks></remarks>
        protected ItemList<CmisObject> GetCheckedOutDocs(string folderId, long? maxItems, long? skipCount, string orderBy, string filter, Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeAllowableActions)
        {
            {
                var withBlock = _client.GetCheckedOutDocs(new cmr.getCheckedOutDocs()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    FolderId = folderId,
                    MaxItems = maxItems,
                    SkipCount = skipCount,
                    OrderBy = orderBy,
                    Filter = filter,
                    IncludeRelationships = includeRelationships,
                    RenditionFilter = renditionFilter,
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

        #region Object
        /// <summary>
      /// Creates a document object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisDocument CreateDocument(Core.Collections.cmisPropertiesType properties, string folderId = null, Messaging.cmisContentStreamType contentStream = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = new CreateDocumentService(this, properties, folderId, contentStream, versioningState, policies, addACEs, removeACEs);
                return withBlock.Invoke();
            }
        }

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisDocument CreateDocumentFromSource(string sourceId, Core.Collections.cmisPropertiesType properties = null, string folderId = null, Core.enumVersioningState? versioningState = default, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            {
                var withBlock = new CreateDocumentFromSourceService(this, sourceId, properties, folderId, versioningState, policies, addACEs, removeACEs);
                return withBlock.Invoke();
            }
        }

        /// <summary>
      /// Gets the specified information for the object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisObject GetObject(string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.GetObject(new cmr.getObject()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
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
      /// Gets the specified information for the latest object in the version series
      /// </summary>
      /// <param name="objectId"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisDocument GetObjectOfLatestVersion(string objectId, bool major = false, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, enumCheckedOutState acceptPWC = enumCheckedOutState.notCheckedOut)
        {
            // returns the private working copy if exists and is accepted ...
            if (acceptPWC != enumCheckedOutState.notCheckedOut)
            {
                var retVal = GetPrivateWorkingCopy(objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, acceptPWC);
                if (retVal is not null)
                    return retVal;
            }
            // ... otherwise returns ObjectOfLatestVersion
            {
                var withBlock = _client.GetObjectOfLatestVersion(new cmr.getObjectOfLatestVersion()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    Major = major,
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
                    return CreateCmisObject(withBlock.Response.Object) as CmisDocument;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Get the private working copy if the current document is checked out by current user (acceptPWC=enumCheckedOutState.checkedOutByMe)
      /// or by any user (acceptPWC=enumCheckedOutState.checkedOut)
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisDocument GetPrivateWorkingCopy(string objectId, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, enumCheckedOutState acceptPWC = enumCheckedOutState.checkedOutByMe)
        {
            if (acceptPWC == enumCheckedOutState.notCheckedOut)
            {
                return null;
            }
            else
            {
                var propertyNames = new string[] { CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut, CmisPredefinedPropertyNames.ObjectId, CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy, CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, CmisPredefinedPropertyNames.VersionSeriesId };
                var response = _client.GetObject(new cmr.getObject()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    Filter = string.Join(Conversions.ToString(','), propertyNames)
                });
                var cmisObject = response.Exception is null ? response.Response.Object : null;

                if (cmisObject is not null && cmisObject.IsVersionSeriesCheckedOut.HasValue && cmisObject.IsVersionSeriesCheckedOut.Value && (GetCheckedOut(cmisObject, _client) & acceptPWC) == acceptPWC && !string.IsNullOrEmpty(cmisObject.VersionSeriesCheckedOutId))
                {
                    // the document is checked out
                    return GetObject(response.Response.Object.VersionSeriesCheckedOutId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions) as CmisDocument;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        protected CmisObject MoveObject(string objectId, string targetFolderId, string sourceFolderId)
        {
            {
                var withBlock = _client.MoveObject(new cmr.moveObject()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = objectId,
                    TargetFolderId = targetFolderId,
                    SourceFolderId = sourceFolderId
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
        #endregion

        #region Browser Binding support
        public virtual void BeginSuccinct(bool succinct)
        {
            if (_client.SupportsSuccinct)
                Browser.SuccinctSupport.BeginSuccinct(succinct);
        }
        public virtual void BeginToken(Browser.TokenGenerator token)
        {
            if (_client.SupportsToken)
                Browser.TokenGenerator.BeginToken(token);
        }
        public virtual bool CurrentSuccinct
        {
            get
            {
                return _client.SupportsSuccinct && Browser.SuccinctSupport.Current;
            }
        }
        public virtual Browser.TokenGenerator CurrentToken
        {
            get
            {
                return _client.SupportsToken ? Browser.TokenGenerator.Current : null;
            }
        }
        public virtual bool EndSuccinct()
        {
            return _client.SupportsSuccinct && Browser.SuccinctSupport.EndSuccinct();
        }
        public virtual Browser.TokenGenerator EndToken()
        {
            return _client.SupportsToken ? Browser.TokenGenerator.EndToken() : null;
        }
        #endregion

        protected Contracts.ICmisClient _client;
        public Contracts.ICmisClient Client
        {
            get
            {
                return _client;
            }
        }

        public virtual CmisService CmisService
        {
            get
            {
                return new CmisService(_client);
            }
        }

        /// <summary>
      /// Converts cmisObjectListType into ItemList(Of CmisObject)
      /// </summary>
      /// <param name="objects"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected ItemList<CmisObject> Convert(Messaging.cmisObjectListType objects)
        {
            var result = new List<CmisObject>();
            bool hasMoreItems = false;
            long? numItems = default;

            if (objects is not null)
            {
                hasMoreItems = objects.HasMoreItems;
                numItems = objects.NumItems;
                if (objects.Objects is not null)
                {
                    foreach (Core.cmisObjectType @object in objects.Objects)
                        result.Add(CreateCmisObject(@object));
                }
            }

            return new ItemList<CmisObject>(result.ToArray(), hasMoreItems, numItems);
        }

        /// <summary>
      /// Converts cmisObjectInFolderListType into ItemList(Of CmisObject)
      /// </summary>
      /// <param name="objects"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected ItemList<CmisObject> Convert(Messaging.cmisObjectInFolderListType objects)
        {
            bool hasMoreItems = false;
            long? numItems = default;
            var result = new List<CmisObject>();

            if (objects is not null)
            {
                hasMoreItems = objects.HasMoreItems;
                numItems = objects.NumItems;
                if (objects.Objects is not null)
                {
                    foreach (Messaging.cmisObjectInFolderType @object in objects.Objects)
                    {
                        var cmisObject = CreateCmisObject(@object.Object);
                        cmisObject.PathSegment = @object.PathSegment;
                        result.Add(cmisObject);
                    }
                }
            }

            return new ItemList<CmisObject>(result.ToArray(), hasMoreItems, numItems);
        }

        protected virtual CmisObject CreateCmisObject(Core.cmisObjectType cmisObject)
        {
            _client.Vendor.PatchProperties(_repositoryInfo, cmisObject);
            return cmisObject + _client + _repositoryInfo;
        }

        protected virtual CmisType CreateCmisType(Core.Definitions.Types.cmisTypeDefinitionType type)
        {
            return type + _client + _repositoryInfo;
        }

        protected static enumCheckedOutState GetCheckedOut(Core.cmisObjectType cmisObject, Contracts.ICmisClient client)
        {
            if (cmisObject is null)
            {
                return enumCheckedOutState.notCheckedOut;
            }
            else
            {
                string versionSeriesCheckedOutBy = cmisObject.VersionSeriesCheckedOutBy;

                if (string.IsNullOrEmpty(versionSeriesCheckedOutBy))
                {
                    return cmisObject.IsVersionSeriesCheckedOut == true ? enumCheckedOutState.checkedOut : enumCheckedOutState.notCheckedOut;
                }
                else if (string.Compare(cmisObject.VersionSeriesCheckedOutBy, client.User, true) == 0)
                {
                    return enumCheckedOutState.checkedOutByMe;
                }
                else
                {
                    return enumCheckedOutState.checkedOut;
                }
            }
        }

        private bool? _holdCapability;
        /// <summary>
      /// Returns true if the repository supports holds
      /// </summary>
        protected bool HoldCapability
        {
            get
            {
                if (!_holdCapability.HasValue)
                {
                    var td = GetTypeDefinition(Core.Definitions.Types.cmisTypeRM_HoldDefinitionType.TargetTypeName);
                    _holdCapability = td is not null;
                }
                return _holdCapability.Value;
            }
        }

        protected ss.FaultException _lastException;
        public ss.FaultException LastException
        {
            get
            {
                return _lastException;
            }
        }

        protected Core.cmisRepositoryInfoType _repositoryInfo;
        public Core.cmisRepositoryInfoType RepositoryInfo
        {
            get
            {
                return _repositoryInfo;
            }
        }

        private enumRetentionCapability? _retentionCapability;
        /// <summary>
      /// Returns the retentions supported by the repository
      /// </summary>
        protected enumRetentionCapability RetentionCapability
        {
            get
            {
                if (!_retentionCapability.HasValue)
                {
                    var retVal = enumRetentionCapability.none;

                    if (GetTypeDefinition(Core.Definitions.Types.cmisTypeRM_ClientMgtRetentionDefinitionType.TargetTypeName) is not null)
                    {
                        retVal = enumRetentionCapability.clientMgt;
                    }
                    if (GetTypeDefinition(Core.Definitions.Types.cmisTypeRM_RepMgtRetentionDefinitionType.TargetTypeName) is not null)
                    {
                        retVal = retVal | enumRetentionCapability.repositoryMgt;
                    }
                    _retentionCapability = retVal;

                    return retVal;
                }
                else
                {
                    return _retentionCapability.Value;
                }
            }
        }

        /// <summary>
      /// Transforms the cmisTypeContainer()-structure into a List(Of ItemContainer(Of CmisType))-structure
      /// </summary>
      /// <param name="source"></param>
      /// <param name="result"></param>
      /// <remarks></remarks>
        private List<ItemContainer<CmisType>> Transform(Messaging.cmisTypeContainer[] source, List<ItemContainer<CmisType>> result)
        {
            result.Clear();
            if (source is not null)
            {
                foreach (Messaging.cmisTypeContainer typeContainer in source)
                {
                    if (typeContainer is not null)
                    {
                        var container = new ItemContainer<CmisType>(CreateCmisType(typeContainer.Type));

                        result.Add(container);
                        Transform(typeContainer.Children, container.Children);
                    }
                }
            }

            return result;
        }

    }
}