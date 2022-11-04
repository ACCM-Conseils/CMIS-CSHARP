using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using srs = System.Runtime.Serialization;
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
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Constants
{
    /// <summary>
   /// ServiceUri for the CMIS-implementation
   /// </summary>
   /// <remarks>
   /// Required parameters are received through the UriTemplate of the WebGetAttribute/WebInvokeAttribute attribute.
   /// Optional parameters are sent in the query string or through the HttpContext class.
   /// The reason for this is that when using the same UriTemplate for both WebGet and WebInvoke, with optional parameters,
   /// the optional parameters will not be optional and they will be case sensitive. This due to an issue in WCF REST.
   /// 
   /// Supported UriTemplates in this file:
   /// /
   /// /DebugHelpPage
   /// /MetadataExchange
   /// /{repositoryId}
   /// /{repositoryId}/acl
   /// /{repositoryId}/allowableactions
   /// /{repositoryId}/allversions
   /// /{repositoryId}/checkedout
   /// /{repositoryId}/changes
   /// /{repositoryId}/children
   /// /{repositoryId}/content
   /// /{repositoryId}/descendants
   /// /{repositoryId}/foldertree
   /// /{repositoryId}/object
   /// /{repositoryId}/objectbypath
   /// /{repositoryId}/objectparents
   /// /{repositoryId}/policies
   /// /{repositoryId}/query
   /// /{repositoryId}/relationships
   /// /{repositoryId}/root
   /// /{repositoryId}/root/{*path}
   /// /{repositoryId}/type
   /// /{repositoryId}/typedescendants
   /// /{repositoryId}/types
   /// /{repositoryId}/unfiled
   /// /{repositoryId}/updates
   /// </remarks>
    public abstract class ServiceURIs
    {
        private ServiceURIs()
        {
        }

        #region Build service uri
        /// <summary>
      /// Extends the given serviceUri with queryStringParameters
      /// </summary>
      /// <typeparam name="TEnum">MUST be an enum-type based on integer</typeparam>
      /// <param name="baseServiceUri"></param>
      /// <param name="queryStringParameters"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetServiceUri<TEnum>(string baseServiceUri, TEnum queryStringParameters) where TEnum : struct
        {
            var parameters = new List<string>(8);
            int currentFlag = 1;
            var values = GetValues(typeof(TEnum));
            int linkParametersValue = Conversions.ToInteger(queryStringParameters);

            // check each bit
            while (linkParametersValue != 0)
            {
                if ((currentFlag & linkParametersValue) != 0)
                {
                    linkParametersValue = linkParametersValue ^ currentFlag;
                    if (values.ContainsKey(currentFlag))
                    {
                        parameters.Add(values[currentFlag].Item1 + "={" + values[currentFlag].Item2.GetName() + "}");
                    }
                }
                currentFlag <<= 1;
            }

            switch (parameters.Count)
            {
                case 0:
                    {
                        return baseServiceUri;
                    }
                case 1:
                    {
                        return baseServiceUri + (baseServiceUri.Contains("?") ? "&" : "?") + parameters[0];
                    }

                default:
                    {
                        return baseServiceUri + (baseServiceUri.Contains("?") ? "&" : "?") + string.Join("&", parameters.ToArray());
                    }
            }
        }

        private static Dictionary<Type, Dictionary<int, Tuple<string, Enum>>> _getValuesResults = new Dictionary<Type, Dictionary<int, Tuple<string, Enum>>>();
        /// <summary>
      /// Returns the values of an enumType based on the integer type
      /// </summary>
      /// <param name="enumType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Dictionary<int, Tuple<string, Enum>> GetValues(Type enumType)
        {
            lock (_getValuesResults)
            {
                if (_getValuesResults.ContainsKey(enumType))
                {
                    return _getValuesResults[enumType];
                }
                else
                {
                    var retVal = new Dictionary<int, Tuple<string, Enum>>();
                    var members = (from fi in enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                   select fi).ToList();

                    foreach (System.Reflection.FieldInfo member in members)
                    {
                        Enum enumValue = (Enum)member.GetValue(null);
                        int value = Conversions.ToInteger(member.GetValue(null));
                        if (!retVal.ContainsKey(value))
                            retVal.Add(value, new Tuple<string, Enum>(member.Name, enumValue));
                    }
                    _getValuesResults.Add(enumType, retVal);

                    return retVal;
                }
            }
        }
        #endregion

        #region Enums
        [Flags()]
        public enum enumAbsoluteObjectUri : int
        {
            // predefined
            callback = JSON.Enums.enumJSONPredefinedParameter.callback,
            cmisaction = JSON.Enums.enumJSONPredefinedParameter.cmisaction,
            cmisselector = JSON.Enums.enumJSONPredefinedParameter.cmisselector,
            succinct = JSON.Enums.enumJSONPredefinedParameter.succinct,
            suppressResponseCodes = JSON.Enums.enumJSONPredefinedParameter.suppressResponseCodes,
            token = JSON.Enums.enumJSONPredefinedParameter.token
        }

        [Flags()]
        public enum enumACLUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            aclPropagation = 2,
            onlyBasicPermissions = 4,

            // special
            applyACL = objectId | aclPropagation,
            getACL = objectId | onlyBasicPermissions
        }

        [Flags()]
        public enum enumAllowableActionsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,

            // special
            getAllowableActions = objectId
        }

        [Flags()]
        public enum enumAllVersionsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            versionSeriesId = 2,
            filter = 4,
            includeAllowableActions = 8,

            // special
            getAllVersions = objectId | versionSeriesId | filter | includeAllowableActions
        }

        [Flags()]
        public enum enumChangesUri : int
        {
            none = 0,
            filter = 1,
            maxItems = 2,
            includeACL = 4,
            includePolicyIds = 8,
            includeProperties = 0x10,
            changeLogToken = 0x20,

            // special
            getContentChanges = filter | maxItems | includeACL | includePolicyIds | includeProperties | changeLogToken
        }

        [Flags()]
        public enum enumCheckedOutUri : int
        {
            none = 0,
            filter = 1,
            [srs.EnumMember(Value = "id")]
            folderId = 2,
            maxItems = 4,
            skipCount = 8,
            renditionFilter = 0x10,
            includeAllowableActions = 0x20,
            includeRelationships = 0x40,
            orderBy = 0x80,
            objectId = 0x100,

            // special
            getCheckedOutDocs = filter | folderId | maxItems | skipCount | renditionFilter | includeAllowableActions | includeRelationships | orderBy
        }

        [Flags()]
        public enum enumChildrenUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            folderId = 1,
            filter = 2,
            includeRelationships = 4,
            renditionFilter = 8,
            includePathSegment = 0x10,
            includeAllowableActions = 0x20,
            maxItems = 0x40,
            skipCount = 0x80,
            orderBy = 0x100,
            sourceFolderId = 0x200,
            versioningState = 0x400,
            objectId = 0x800,
            targetFolderId = 0x1000,
            sourceId = 0x2000,
            allVersions = 0x4000,

            // special
            addObjectToFolder = objectId | folderId | allVersions,
            createDocument = folderId | versioningState,
            createDocumentFromSource = sourceId | createDocument,
            createFolder = folderId,
            createPolicy = folderId,
            getChildren = folderId | filter | includeRelationships | renditionFilter | includePathSegment | includeAllowableActions | maxItems | skipCount | orderBy,
            moveObject = objectId | targetFolderId | sourceFolderId
        }

        [Flags()]
        public enum enumContentUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            streamId = 2,
            overwriteFlag = 4,
            changeToken = 8,
            isLastChunk = 0x10,
            /// <summary>
         /// If specified and set to true, appendContentStream is called. Otherwise setContentStream is called.
         /// </summary>
         /// <remarks>3.11.8.2 HTTP PUT</remarks>
            append = 0x20,

            // special
            appendContentStream = objectId | isLastChunk | changeToken,
            deleteContentStream = objectId | changeToken,
            getContentStream = objectId | streamId,
            setContentStream = objectId | overwriteFlag | changeToken | append
        }

        [Flags()]
        public enum enumDescendantsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            folderId = 1,
            filter = 2,
            depth = 4,
            includeAllowableActions = 8,
            includeRelationships = 0x10,
            renditionFilter = 0x20,
            includePathSegment = 0x40,

            // special
            get = folderId | filter | depth | includeAllowableActions | includeRelationships | renditionFilter | includePathSegment
        }

        [Flags()]
        public enum enumFolderTreeUri : int
        {
            none = 0,
            folderId = 1,
            filter = 2,
            depth = 4,
            includeAllowableActions = 8,
            includeRelationships = 0x10,
            includePathSegment = 0x20,
            renditionFilter = 0x40,
            allVersions = 0x80,
            continueOnFailure = 0x100,
            unfileObjects = 0x200,

            // special
            delete = folderId | allVersions | continueOnFailure | unfileObjects,
            get = folderId | filter | depth | includeAllowableActions | includeRelationships | includePathSegment | renditionFilter
        }

        [Flags()]
        public enum enumObjectUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            filter = 2,
            includeRelationships = 4,
            renditionFilter = 8,
            includeAllowableActions = 0x10,
            includePolicyIds = 0x20,
            includeACL = 0x40,
            /// <summary>
         /// Used to differentiate between getObject and getObjectOfLatestVersion. Valid values are are described by the schema element cmisra:enumReturnVersion.
         /// If not specified, return the version specified by the URI.
         /// </summary>
         /// <remarks>3.11.2.1 HTTP GET</remarks>
            returnVersion = 0x80,
            /// <summary>
         /// private working copy
         /// </summary>
         /// <remarks>
         /// pwc is a boolean parameter; used in document-links
         /// </remarks>
            pwc = 0x100,
            /// <summary>
         /// parameter used in deleteObject-service
         /// </summary>
         /// <remarks></remarks>
            allVersions = 0x200,
            /// <summary>
         /// parameter used in checkIn- and getObjectOfLatestVersion-service
         /// </summary>
         /// <remarks></remarks>
            major = 0x400,
            /// <summary>
         /// parameter used in updateProperties-service
         /// </summary>
         /// <remarks></remarks>
            changeToken = 0x800,
            /// <summary>
         /// Used to differentiate between updateProperties or checkIn services. If TRUE, execute checkIn service
         /// </summary>
         /// <remarks>3.11.3.2 HTTP PUT</remarks>
            checkin = 0x1000,
            /// <summary>
         /// parameter used in checkIn-service
         /// </summary>
         /// <remarks></remarks>
            checkinComment = 0x2000,
            /// <summary>
         /// parameter used in getFolderParent-service
         /// </summary>
         /// <remarks></remarks>
            folderId = 0x4000,
            /// <summary>
         /// parameter used in getObjectOfLatestVersion-service
         /// </summary>
         /// <remarks></remarks>
            versionSeriesId = 0x8000,

            // special
            self = objectId,
            deleteObject = objectId | allVersions,
            getObject = objectId | filter | includeRelationships | renditionFilter | includeAllowableActions | includePolicyIds | includeACL,
            specifyVersion = objectId | returnVersion,
            updateProperties = objectId | major | changeToken | checkinComment,
            workingCopy = objectId | pwc,
            /// <summary>
         /// </summary>
         /// <remarks>
         /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
         /// 3.7.1.1.2 Object By Id
         /// </remarks>
            getObjectById = getObject | returnVersion
        }

        [Flags()]
        public enum enumObjectByPathUri : int
        {
            none = 0,
            path = 1,
            filter = 2,
            includeAllowableActions = 4,
            includePolicies = 8,
            includeRelationships = 0x10,
            includeACL = 0x20,
            renditionFilter = 0x40,

            // special
            get = path | filter | includeAllowableActions | includePolicies | includeRelationships | includeACL | renditionFilter
        }

        [Flags()]
        public enum enumObjectParentsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            filter = 2,
            includeRelationships = 4,
            renditionFilter = 8,
            includeAllowableActions = 0x10,
            includeRelativePathSegment = 0x20,

            // special
            get = objectId | filter | includeRelationships | renditionFilter | includeAllowableActions | includeRelativePathSegment
        }

        [Flags()]
        public enum enumPoliciesUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            policyId = 2,
            filter = 4,

            // special
            removePolicy = objectId | policyId,
            getAppliedPolicies = objectId | filter,
            applyPolicy = objectId | policyId
        }

        [Flags()]
        public enum enumQueryUri : int
        {
            none = 0,
            q = 1,
            searchAllVersions = 2,
            includeAllowableActions = 4,
            includeRelationships = 8,
            renditionFilter = 0x10,
            maxItems = 0x20,
            skipCount = 0x40,
            // defined parameter-name in 2.2.6.1 query for the parameter q
            statement = 0x80,

            // special
            query = q | searchAllVersions | includeAllowableActions | includeRelationships | renditionFilter | maxItems | skipCount
        }

        [Flags()]
        public enum enumRelationshipsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            typeId = 2,
            includeSubRelationshipTypes = 4,
            relationshipDirection = 8,
            maxItems = 0x10,
            skipCount = 0x20,
            filter = 0x40,
            includeAllowableActions = 0x80,

            // special
            getObjectRelationships = objectId | typeId | includeSubRelationshipTypes | relationshipDirection | maxItems | skipCount | filter | includeAllowableActions,
            createRelationship = none
        }

        [Flags()]
        public enum enumRepositoriesUri : int
        {
            none = 0,
            repositoryId = 1,
            logout = 2,
            ping = 4,

            // browser binding
            cmisaction = JSON.Enums.enumJSONPredefinedParameter.cmisaction,
            token = JSON.Enums.enumJSONPredefinedParameter.token,
            file = JSON.Enums.enumJSONPredefinedParameter.maxValue << 1
        }

        [Flags()]
        public enum enumRepositoryUri : int
        {
            none = 0,
            logout = enumRepositoriesUri.logout,
            ping = enumRepositoriesUri.ping,

            // browser binding
            callback = JSON.Enums.enumJSONPredefinedParameter.callback,
            cmisaction = JSON.Enums.enumJSONPredefinedParameter.cmisaction,
            cmisselector = JSON.Enums.enumJSONPredefinedParameter.cmisselector,
            succinct = JSON.Enums.enumJSONPredefinedParameter.succinct,
            suppressResponseCodes = JSON.Enums.enumJSONPredefinedParameter.suppressResponseCodes,
            token = JSON.Enums.enumJSONPredefinedParameter.token,
            typeId = JSON.Enums.enumJSONPredefinedParameter.maxValue << 1
        }

        [Flags()]
        public enum enumRootFolderUri : int
        {
            none = 0,

            // predefined
            callback = JSON.Enums.enumJSONPredefinedParameter.callback,
            cmisaction = JSON.Enums.enumJSONPredefinedParameter.cmisaction,
            cmisselector = JSON.Enums.enumJSONPredefinedParameter.cmisselector,
            succinct = JSON.Enums.enumJSONPredefinedParameter.succinct,
            suppressResponseCodes = JSON.Enums.enumJSONPredefinedParameter.suppressResponseCodes,
            token = JSON.Enums.enumJSONPredefinedParameter.token,

            [srs.EnumMember(Value = "id")]
            objectId = JSON.Enums.enumJSONPredefinedParameter.maxValue << 1
        }

        [Flags()]
        public enum enumTypeUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            typeId = 1,

            // special
            delete = typeId,
            get = typeId,
            put = none
        }

        [Flags()]
        public enum enumTypeDescendantsUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            typeId = 1,
            depth = 2,
            includePropertyDefinitions = 4,

            // special
            get = typeId | depth | includePropertyDefinitions
        }

        [Flags()]
        public enum enumTypesUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            typeId = 1,
            includePropertyDefinitions = 2,
            maxItems = 4,
            skipCount = 8,

            // special
            get = typeId | includePropertyDefinitions | maxItems | skipCount
        }

        [Flags()]
        public enum enumUnfiledUri : int
        {
            none = 0,
            [srs.EnumMember(Value = "id")]
            objectId = 1,
            folderId = 2,
            removeFrom = 4,
            versioningState = 8,
            maxItems = 0x10,
            skipCount = 0x20,
            filter = 0x40,
            includeAllowableActions = 0x80,
            includeRelationships = 0x100,
            renditionFilter = 0x200,
            orderBy = 0x400,
            sourceId = 0x800,

            // special
            createUnfiledObject = objectId | versioningState, // createDocument or createPolicy
            getUnfiledObjects = maxItems | skipCount | filter | includeAllowableActions | includeRelationships | renditionFilter | orderBy,
            removeObjectFromFolder = objectId | folderId
        }
        #endregion

        #region Supported TemplateUris
        /// <summary>
      /// BrowserBinding
      /// </summary>
        private const string _absoluteObjectUri = _rootFolderUri + "/{*path}";
        public static string get_AbsoluteObjectUri(enumAbsoluteObjectUri queryStringParameters)
        {
            return GetServiceUri(_absoluteObjectUri, queryStringParameters);
        }

        private const string _aclUri = "/{repositoryId}/acl";
        public static string get_ACLUri(enumACLUri queryStringParameters)
        {
            return GetServiceUri(_aclUri, queryStringParameters);
        }

        private const string _allowableActionsUri = "/{repositoryId}/allowableactions";
        public static string get_AllowableActionsUri(enumAllowableActionsUri queryStringParameters)
        {
            return GetServiceUri(_allowableActionsUri, queryStringParameters);
        }

        private const string _allVersionsUri = "/{repositoryId}/allversions";
        public static string get_AllVersionsUri(enumAllVersionsUri queryStringParameters)
        {
            return GetServiceUri(_allVersionsUri, queryStringParameters);
        }

        private const string _changesUri = "/{repositoryId}/changes";
        public static string get_ChangesUri(enumChangesUri queryStringParameters)
        {
            return GetServiceUri(_changesUri, queryStringParameters);
        }

        private const string _checkedOutUri = "/{repositoryId}/checkedout";
        public static string get_CheckedOutUri(enumCheckedOutUri queryStringParameters)
        {
            return GetServiceUri(_checkedOutUri, queryStringParameters);
        }

        private const string _childrenUri = "/{repositoryId}/children";
        public static string get_ChildrenUri(enumChildrenUri queryStringParameters)
        {
            return GetServiceUri(_childrenUri, queryStringParameters);
        }

        private const string _contentUri = "/{repositoryId}/content";
        public static string get_ContentUri(enumContentUri queryStringParameters)
        {
            return GetServiceUri(_contentUri, queryStringParameters);
        }

        public const string DebugHelpPageUri = "/DebugHelpPage";

        private const string _descendantsUri = "/{repositoryId}/descendants";
        public static string get_DescendantsUri(enumDescendantsUri queryStringParameters)
        {
            return GetServiceUri(_descendantsUri, queryStringParameters);
        }

        private const string _folderTreeUri = "/{repositoryId}/foldertree";
        public static string get_FolderTreeUri(enumFolderTreeUri queryStringParameters)
        {
            return GetServiceUri(_folderTreeUri, queryStringParameters);
        }

        public const string MetaDataUri = "/MetadataExchange";

        private const string _objectByPathUri = "/{repositoryId}/objectbypath";
        public static string get_ObjectByPathUri(enumObjectByPathUri queryStringParameter)
        {
            return GetServiceUri(_objectByPathUri, queryStringParameter);
        }

        private const string _objectParentsUri = "/{repositoryId}/objectparents";
        public static string get_ObjectParentsUri(enumObjectParentsUri queryStringParameter)
        {
            return GetServiceUri(_objectParentsUri, queryStringParameter);
        }

        private const string _objectUri = "/{repositoryId}/object";
        public static string get_ObjectUri(enumObjectUri queryStringParameters = enumObjectUri.self)
        {
            return GetServiceUri(_objectUri, queryStringParameters);
        }

        private const string _policiesUri = "/{repositoryId}/policies";
        public static string get_PoliciesUri(enumPoliciesUri queryStringParameters)
        {
            return GetServiceUri(_policiesUri, queryStringParameters);
        }

        private const string _queryUri = "/{repositoryId}/query";
        public static string get_QueryUri(enumQueryUri queryStringParameters)
        {
            return GetServiceUri(_queryUri, queryStringParameters);
        }

        private const string _relationshipsUri = "/{repositoryId}/relationships";
        public static string get_RelationshipsUri(enumRelationshipsUri queryStringParameters)
        {
            return GetServiceUri(_relationshipsUri, queryStringParameters);
        }

        private const string _repositoriesUri = "/";
        public static string get_RepositoriesUri(enumRepositoriesUri queryStringParameters)
        {
            return GetServiceUri(_repositoriesUri, queryStringParameters);
        }

        private const string _repositoryUri = "/{repositoryId}";
        public static string get_RepositoryUri(enumRepositoryUri queryStringParameters)
        {
            return GetServiceUri(_repositoryUri, queryStringParameters);
        }

        private const string _rootFolderUri = "/{repositoryId}/root";
        /// <summary>
      /// BrowserBinding
      /// </summary>
        public static string get_RootFolderUri(enumRootFolderUri queryStringParameters)
        {
            return GetServiceUri(_rootFolderUri, queryStringParameters);
        }

        private const string _typeUri = "/{repositoryId}/type";
        public static string get_TypeUri(enumTypeUri queryStringParameters)
        {
            return GetServiceUri(_typeUri, queryStringParameters);
        }

        private const string _typeDescendantsUri = "/{repositoryId}/typedescendants";
        public static string get_TypeDescendantsUri(enumTypeDescendantsUri queryStringParameters)
        {
            return GetServiceUri(_typeDescendantsUri, queryStringParameters);
        }

        private const string _typesUri = "/{repositoryId}/types";
        public static string get_TypesUri(enumTypesUri queryStringParameters)
        {
            return GetServiceUri(_typesUri, queryStringParameters);
        }

        private const string _unfiledUri = "/{repositoryId}/unfiled";
        public static string get_UnfiledUri(enumUnfiledUri queryStringParameters)
        {
            return GetServiceUri(_unfiledUri, queryStringParameters);
        }
        #endregion

        #region Repository
        public const string GetRepositories = _repositoriesUri;
        public const string GetRepositoryInfo = _repositoryUri;

        public const string CreateType = _typesUri;
        public const string DeleteType = _typeUri + "?typeId={id}"; // typeId is required, no optional parameters
        public const string GetTypeDefinition = _typeUri + "?typeId={id}"; // typeId is required, no optional parameters
        public const string UpdateType = _typeUri;

        public const string GetTypeChildren = _typesUri;

        public const string GetTypeDescendants = _typeDescendantsUri + "?typeId={id}&depth={depth}&includePropertyDefinitions={includePropertyDefinitions}";
        #endregion

        #region Navigation
        public const string DeleteTree = _folderTreeUri;
        public const string DeleteTreeViaDescendantsFeed = _descendantsUri;
        public const string DeleteTreeViaChildrenFeed = _childrenUri;
        public const string GetCheckedOutDocs = _checkedOutUri;
        public const string GetChildren = _childrenUri;
        public const string GetDescendants = _descendantsUri + "?folderId={id}&filter={filter}&depth={depth}&includeAllowableActions={includeAllowableActions}&includeRelationships={includeRelationships}&renditionFilter={renditionFilter}&includePathSegment={includePathSegment}";
        public const string GetFolderTree = _folderTreeUri;
        public const string GetObjectParents = _objectParentsUri + "?objectId={id}&filter={filter}&includeRelationships={includeRelationships}&renditionFilter={renditionFilter}&includeAllowableActions={includeAllowableActions}&includeRelativePathSegment={includeRelativePathSegment}";
        public const string GetUnfiledObjects = _unfiledUri;
        #endregion

        #region Object
        public const string AppendContentStream = _contentUri;
        public const string BulkUpdateProperties = "/{repositoryId}/updates/";
        public const string CreateOrMoveChildObject = _childrenUri;
        public const string CreateUnfiledObjectOrRemoveObjectFromFolder = _unfiledUri;
        public const string CreateRelationship = _relationshipsUri;
        public const string DeleteContentStream = _contentUri;
        public const string DeleteObject = _objectUri;
        public const string GetAllowableActions = _allowableActionsUri + "?objectId={id}"; // objectId is required, no optional parameters
        public const string GetContentStream = _contentUri;
        public const string GetObject = _objectUri;
        public const string GetObjectByPath = _objectByPathUri + "?path={path}&filter={filter}&includeAllowableActions={includeAllowableActions}&includeRelationships={includeRelationships}&includePolicyIds={includePolicyIds}&includeACL={includeACL}&renditionFilter={renditionFilter}";
        public const string SetContentStream = _contentUri;
        public const string UpdateProperties = _objectUri;

        // Browser Binding
        public const string AbsoluteObject = _absoluteObjectUri;
        public const string RootFolder = _rootFolderUri;
        #endregion

        #region Multi
        // see CreateOrMoveChildObject (addObjectToFolder), CreateUnfiledObjectOrRemoveObjectFromFolder (removeObjectFromFolder)
        #endregion

        #region Discovery
        public const string GetContentChanges = _changesUri + "?filter={filter}&maxItems={maxItems}&includeACL={includeACL}&includePolicyIds={includePolicyIds}&includeProperties={includeProperties}&changeLogToken={changeLogToken}";
        public const string Query = _queryUri;
        #endregion

        #region Versioning
        // see DeleteObject (cancelCheckOut), CheckInOrUpdateProperties (checkIn), GetObject (getObjectOfLatestVersion)
        public const string CheckOut = _checkedOutUri;
        public const string GetAllVersions = _allVersionsUri;
        #endregion

        #region Relationships
        public const string GetObjectRelationships = _relationshipsUri;
        #endregion

        #region Policies
        public const string ApplyPolicy = _policiesUri;
        public const string RemovePolicy = _policiesUri + "?policyId={policyId}&objectId={id}";
        public const string GetAppliedPolicies = _policiesUri + "?objectId={id}";
        #endregion

        #region ACL
        public const string GetAcl = _aclUri + "?objectId={id}";
        public const string ApplyAcl = _aclUri + "?objectId={id}";
        #endregion

    }
}