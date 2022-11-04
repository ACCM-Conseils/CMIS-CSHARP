using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sn = System.Net;
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
using CmisObjectModel.Common;
using CmisObjectModel.Constants;
using ccdp = CmisObjectModel.Core.Definitions.Properties;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cm = CmisObjectModel.Messaging;
using cmr = CmisObjectModel.Messaging.Responses;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped IfDirectiveTrivia
#If Not xs_HttpRequestAddRange64 Then
*//* TODO ERROR: Skipped DisabledTextTrivia
#Const HttpRequestAddRangeShortened = True
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Browser
{
    /// <summary>
   /// Implements the functionality of a cmis-client version 1.1
   /// </summary>
   /// <remarks></remarks>
    public class CmisClient : Base.Generic.CmisClient<Core.cmisRepositoryInfoType>
    {

        #region Constructors
        public CmisClient(Uri serviceDocUri, enumVendor vendor, AuthenticationProvider authentication, int? connectTimeout = default, int? readWriteTimeout = default) : base(serviceDocUri, vendor, authentication, connectTimeout, readWriteTimeout)
        {
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Extends the Vendors.Vendor.State class with succinct properties support
      /// </summary>
      /// <remarks></remarks>
        private class State : Vendors.Vendor.State
        {

            public State(string repositoryId, bool succinct) : base(repositoryId)
            {
                _succinct = succinct;
            }

            /// <summary>
         /// Returns a collection of property definitions that are defined in tds
         /// </summary>
            private Dictionary<string, ccdp.cmisPropertyDefinitionType> GetPropertyDefinitions(ccdt.cmisTypeDefinitionType[] tds)
            {
                var retVal = new Dictionary<string, ccdp.cmisPropertyDefinitionType>();

                foreach (ccdt.cmisTypeDefinitionType td in tds)
                {
                    if (td.PropertyDefinitions is not null)
                    {
                        foreach (ccdp.cmisPropertyDefinitionType pd in td.PropertyDefinitions)
                        {
                            if (pd is not null)
                            {
                                string id = pd.Id ?? string.Empty;
                                if (!retVal.ContainsKey(id))
                                    retVal.Add(id, pd);
                            }
                        }
                    }
                }

                return retVal;
            }

            /// <summary>
         /// Returns all typedefinitions defined by cmis:objectTypeId and cmis:secondaryObjectTypeIds
         /// </summary>
         /// <param name="properties"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            private ccdt.cmisTypeDefinitionType[] GetTypeDefinitions(Dictionary<string, Core.Properties.cmisProperty> properties)
            {
                var typeDefinitions = new List<ccdt.cmisTypeDefinitionType>();

                foreach (string propertyName in new string[] { CmisPredefinedPropertyNames.ObjectTypeId, CmisPredefinedPropertyNames.SecondaryObjectTypeIds })
                {
                    if (properties.ContainsKey(propertyName))
                    {
                        Core.Properties.Generic.cmisProperty<string> stringProperty = properties[propertyName] as Core.Properties.Generic.cmisProperty<string>;
                        var typeIds = stringProperty is null ? null : stringProperty.Values;

                        if (typeIds is not null)
                        {
                            foreach (string typeId in typeIds)
                            {
                                var td = get_TypeDefinition(RepositoryId, typeId);
                                if (td is not null)
                                    typeDefinitions.Add(td);
                            }
                        }
                    }
                }

                return typeDefinitions.ToArray();
            }

            private bool _succinct;

            /// <summary>
         /// Converts DateTime properties and object properties when succinct parameter is in use
         /// </summary>
            public Core.Collections.cmisPropertiesType[] TransformResponse(Core.Collections.cmisPropertiesType[] result)
            {
                // convert datetime properties
                if (_succinct && result is not null && !string.IsNullOrEmpty(RepositoryId))
                {
                    foreach (Core.Collections.cmisPropertiesType propertyCollection in result)
                    {
                        var propertyMap = propertyCollection is null ? null : propertyCollection.GetProperties();
                        int length = propertyMap is null ? 0 : propertyMap.Count;

                        if (length > 0)
                        {
                            var tds = GetTypeDefinitions(propertyMap);
                            var pds = GetPropertyDefinitions(tds);
                            bool containsChanges = false;
                            Core.Properties.cmisProperty[] properties = (Core.Properties.cmisProperty[])Array.CreateInstance(typeof(Core.Properties.cmisProperty), length);

                            Array.Copy(propertyCollection.Properties, properties, length);
                            for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                            {
                                var cmisProperty = properties[index];

                                if (cmisProperty is not null && pds.ContainsKey(cmisProperty.PropertyDefinitionId))
                                {
                                    var pd = pds[cmisProperty.PropertyDefinitionId];

                                    try
                                    {
                                        if (pd.PropertyType == Core.enumPropertyType.datetime)
                                        {
                                            properties[index] = cmisProperty.Values is null ? pd.CreateProperty() : pd.CreateProperty((from value in cmisProperty.Values
                                                                                                                                       select (object)(DateTimeOffset)Conversions.ToLong(value).FromJSONTime()).ToArray());
                                        }
                                        else if (cmisProperty.Values is null)
                                        {
                                            properties[index] = pd.CreateProperty();
                                        }
                                        else
                                        {
                                            properties[index] = pd.CreateProperty(cmisProperty.Values);
                                        }
                                        containsChanges = true;
                                    }
                                    catch
                                    {
                                    }
                                }
                            }

                            // at least one property has been converted
                            if (containsChanges)
                                propertyCollection.Properties = properties;
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
      /// Specific UriBuilder for browser binding
      /// </summary>
      /// <remarks></remarks>
        private class UriBuilder
        {

            #region Constructors
            public UriBuilder(string baseUri, cm.Requests.RequestBase request, params object[] searchAndReplace) : this(baseUri, request.BrowserBinding, searchAndReplace)
            {
            }
            public UriBuilder(Uri baseUri, cm.Requests.RequestBase request, params object[] searchAndReplace) : this(baseUri.OriginalString, request.BrowserBinding, searchAndReplace)
            {
            }
            private UriBuilder(string baseUri, cm.Requests.RequestBase.BrowserBindingExtensions browserBindingExtensions, object[] searchAndReplace)
            {
                _baseUri = baseUri;

                // check queryString-parameters defined in browserBindingExtensions
                if (browserBindingExtensions is not null)
                {
                    if (!string.IsNullOrEmpty(browserBindingExtensions.CmisSelector) && !_replacements.ContainsKey("cmisselector"))
                        _replacements.Add("cmisselector", browserBindingExtensions.CmisSelector);
                    if (browserBindingExtensions.Succinct && !_replacements.ContainsKey("succinct"))
                        _replacements.Add("succinct", "true");
                    if (!string.IsNullOrEmpty(browserBindingExtensions.Token) && !_replacements.ContainsKey("token"))
                        _replacements.Add("token", browserBindingExtensions.Token);
                }
                // process searchAndReplace values
                if (searchAndReplace is not null)
                    AddRange(searchAndReplace);
            }
            #endregion

            #region Add- and AddRange-methods
            public void Add(string parameterName, bool value)
            {
                Add(parameterName, value ? "true" : "false");
            }
            public void Add(string parameterName, bool? value)
            {
                if (value.HasValue)
                    Add(parameterName, value.Value);
            }
            public void Add<TValue>(string parameterName, TValue value) where TValue : struct
            {
                Add(parameterName, ((Enum)(object)value).GetName());
            }
            public void Add<TValue>(string parameterName, TValue? value) where TValue : struct
            {
                if (value.HasValue)
                    Add(parameterName, value.Value);
            }
            public void Add(string parameterName, int value)
            {
                Add(parameterName, value.ToString());
            }
            public void Add(string parameterName, int? value)
            {
                if (value.HasValue)
                    Add(parameterName, value.Value);
            }
            public void Add(string parameterName, long value)
            {
                Add(parameterName, value.ToString());
            }
            public void Add(string parameterName, long? value)
            {
                if (value.HasValue)
                    Add(parameterName, value.Value);
            }
            public void Add(string parameterName, string value, bool required = false)
            {
                if ((required || !string.IsNullOrEmpty(value)) && !_replacements.ContainsKey(parameterName))
                {
                    _replacements.Add(parameterName, value);
                }
            }

            public void AddRange(params object[] searchAndReplace)
            {
                int length = searchAndReplace is null ? 0 : searchAndReplace.Length;

                if (length > 0)
                {
                    var queue = new Queue<object>(searchAndReplace);

                    while (queue.Count > 1)
                    {
                        string placeHolderName;
                        object replacement;
                        Type replacementType;
                        bool required;
                        var current = queue.Dequeue();

                        // current must be the placeHolderName, expected type is string
                        if (current is string)
                            placeHolderName = Conversions.ToString(current);
                        else
                            continue;
                        replacement = queue.Dequeue();
                        replacementType = replacement is null ? typeof(string) : replacement.GetType();
                        required = queue.Count > 0 && replacement is string && queue.Peek() is bool ? Conversions.ToBoolean(queue.Dequeue()) : false;
                        if (ReferenceEquals(replacementType, typeof(string)))
                        {
                            Add(placeHolderName, Conversions.ToString(replacement), required);
                        }
                        else if (replacementType.IsNullableType())
                        {
                            // get a helper for genericArgumentType (for example: if replacementType is GetType(Boolean?) the expression
                            // replacementType.GetGenericArguments()(0) gets GetType(Boolean))
                            var helper = GenericRuntimeHelper.GetInstance(replacementType.GetGenericArguments()[0]);
                            if (helper.HasValue(replacement))
                                Add(placeHolderName, helper.Convert(helper.GetValue(replacement)));
                        }
                        else
                        {
                            var helper = GenericRuntimeHelper.GetInstance(replacementType);
                            Add(placeHolderName, helper.Convert(replacement));
                        }
                    }
                }
            }
            #endregion

            private string _baseUri;
            private Dictionary<string, string> _replacements = new Dictionary<string, string>();

            public Uri ToUri()
            {
                int indexOf = _baseUri.IndexOf("?") + 1;
                string baseUri;
                string separator;

                if (indexOf > 0)
                {
                    // append predefined queryString-parameters
                    {
                        var withBlock = System.Web.HttpUtility.ParseQueryString(_baseUri.Substring(indexOf));
                        baseUri = _baseUri.Substring(0, indexOf) + string.Join("&", from key in withBlock.AllKeys
                                                                                    where !_replacements.ContainsKey(key)
                                                                                    let value = withBlock[key]
                                                                                    select (key + "=" + Uri.EscapeDataString(value)));
                        separator = baseUri.EndsWith("?") ? null : "&";
                    }
                }
                else
                {
                    baseUri = _baseUri;
                    separator = "?";
                }

                // append parameters
                if (_replacements.Count > 0)
                {
                    baseUri = baseUri + separator + string.Join("&", from de in _replacements
                                                                     select (de.Key + "={" + de.Key + "}"));
                    baseUri = baseUri.ReplaceUri(_replacements.Keys.ToArray());
                }

                return new Uri(baseUri);
            }

            public static implicit operator Uri(UriBuilder value)
            {
                return value is null ? null : value.ToUri();
            }

        }
        #endregion

        #region Repository
        /// <summary>
      /// Creates a new type definition that is a subtype of an existing specified parent type
      /// </summary>
        public override Generic.ResponseType<cmr.createTypeResponse> CreateType(cm.Requests.createType request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "createType");

                    content.Add(request.Type);
                    var result = Post(new UriBuilder(withBlock.Response.RepositoryUrl, request), content, null, Deserialize<ccdt.cmisTypeDefinitionType>, false);
                    if (result.Exception is null)
                    {
                        var type = result.Response;

                        // cache typeDefinition
                        if (type is not null)
                        {
                            string typeId = type.Id;
                            string repositoryId = request.RepositoryId;

                            if (!(string.IsNullOrEmpty(repositoryId) || string.IsNullOrEmpty(typeId)))
                            {
                                set_TypeDefinition(repositoryId, typeId, type);
                            }
                        }

                        return new cmr.createTypeResponse() { Type = type };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Deletes a type definition
      /// </summary>
        public override Generic.ResponseType<cmr.deleteTypeResponse> DeleteType(cm.Requests.deleteType request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "deletetype");

                    content.Add("typeId", request.TypeId);
                    var result = Post(new UriBuilder(withBlock.Response.RepositoryUrl, request), content, null);
                    if (result.Exception is null)
                    {
                        return new cmr.deleteTypeResponse();
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.getRepositoriesResponse> GetRepositories(cm.Requests.getRepositories request)
        {
            var result = Get(new UriBuilder(_serviceDocUri, request), DeserializeRepositories, false);

            if (result.Exception is null)
            {
                var entries = new List<cm.cmisRepositoryEntryType>();

                if (result.Response is not null)
                {
                    foreach (Core.cmisRepositoryInfoType repository in result.Response)
                        entries.Add(new cm.cmisRepositoryEntryType() { Repository = repository });
                }
                return new cmr.getRepositoriesResponse() { Repositories = entries.ToArray() };
            }
            else
            {
                return result.Exception;
            }
        }

        /// <summary>
      /// Returns the workspace of specified repository
      /// </summary>
        public override Generic.ResponseType<cmr.getRepositoryInfoResponse> GetRepositoryInfo(cm.Requests.getRepositoryInfo request, bool ignoreCache = false)
        {
            string repositoryId = request.RepositoryId;
            // try to get the info using the cache
            var repositoryInfo = ignoreCache ? null : get_RepositoryInfo(repositoryId);

            request.BrowserBinding.CmisSelector = "repositoryInfo";
            // workspace of specified repository could not be found in the cache
            if (repositoryInfo is null)
            {
                // perhaps the server supports an optional repositoryId-parameter, otherwise the request will get all repositories
                var result = Get(new UriBuilder(_serviceDocUri, request, "repositoryId", repositoryId), DeserializeRepositories, false);

                for (int index = 0; index <= 1; index++)
                {
                    if (result.Exception is not null)
                    {
                        return result.Exception;
                    }
                    else if (result.Response is not null)
                    {
                        // search for the requested repositoryId
                        repositoryInfo = FindRepository(result.Response, repositoryId);
                        if (repositoryInfo is null)
                        {
                            break;
                        }
                        else if (index == 0)
                        {
                            // repeat the request with the RepositoryUrl of the repository to get all informations about the repository
                            // (minimum of retreived informations at this time: RepositoryId, RepositoryName, RepositoryUrl and RootFolderUrl)
                            result = Get(new UriBuilder(repositoryInfo.RepositoryUrl, request), DeserializeRepositories, false);
                            continue;
                        }
                        else
                        {
                            set_RepositoryInfo(repositoryId, repositoryInfo);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (repositoryInfo is null)
            {
                var cmisFault = new cm.cmisFaultType(sn.HttpStatusCode.NotFound, cm.enumServiceException.objectNotFound, "Workspace not found.");
                return cmisFault.ToFaultException();
            }
            else
            {
                return new cmr.getRepositoryInfoResponse() { RepositoryInfo = repositoryInfo };
            }
        }
        /// <summary>
      /// internal usage
      /// </summary>
        private Generic.ResponseType<Core.cmisRepositoryInfoType> GetRepositoryInfo(string repositoryId, string token)
        {
            var request = new cm.Requests.getRepositoryInfo() { RepositoryId = repositoryId };

            request.BrowserBinding.Token = token;
            var response = GetRepositoryInfo(request);

            if (response.Exception is null)
            {
                return response.Response.RepositoryInfo;
            }
            else
            {
                return response.Exception;
            }
        }

        /// <summary>
      /// Returns the list of object-types defined for the repository that are children of the specified type
      /// </summary>
        public override Generic.ResponseType<cmr.getTypeChildrenResponse> GetTypeChildren(cm.Requests.getTypeChildren request)
        {
            request.BrowserBinding.CmisSelector = "typeChildren";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RepositoryUrl, request, "typeId", request.TypeId, true, "includePropertyDefinitions", request.IncludePropertyDefinitions, "maxItems", request.MaxItems, "skipCount", request.SkipCount), Deserialize<cm.cmisTypeDefinitionListType>, false);
                    if (result.Exception is null)
                    {
                        return new cmr.getTypeChildrenResponse() { Types = SetTypeDefinitions(result.Response, request.RepositoryId) };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Gets the definition of the specified object-type
      /// </summary>
        public override Generic.ResponseType<cmr.getTypeDefinitionResponse> GetTypeDefinition(cm.Requests.getTypeDefinition request)
        {
            request.BrowserBinding.CmisSelector = "typeDefinition";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    string typeId = request.TypeId;
                    var state = _vendor.BeginRequest(request.RepositoryId, ref typeId);
                    var result = Get(new UriBuilder(withBlock.Response.RepositoryUrl, request, "typeId", typeId, true), Deserialize<ccdt.cmisTypeDefinitionType>, false);
                    if (result.Exception is null)
                    {
                        var type = result.Response;

                        // cache typeDefinition
                        if (type is not null)
                        {
                            string repositoryId = request.RepositoryId;

                            if (!(string.IsNullOrEmpty(repositoryId) || string.IsNullOrEmpty(typeId)))
                            {
                                set_TypeDefinition(repositoryId, typeId, (ccdt.cmisTypeDefinitionType)type.Copy());
                            }
                        }
                        _vendor.EndRequest(state, type);
                        return new cmr.getTypeDefinitionResponse() { Type = type };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Returns the set of the descendant object-types defined for the Repository under the specified type
      /// </summary>
        public override Generic.ResponseType<cmr.getTypeDescendantsResponse> GetTypeDescendants(cm.Requests.getTypeDescendants request)
        {
            request.BrowserBinding.CmisSelector = "typeDescendants";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    string typeId = request.TypeId;
                    var uriBuilder = new UriBuilder(withBlock.Response.RepositoryUrl, request, "typeId", typeId, "includePropertyDefinitions", request.IncludePropertyDefinitions);

                    // if typeId is not defined then all types have to be returned (see 2.2.2.4.1 Inputs)
                    if (!string.IsNullOrEmpty(typeId))
                        uriBuilder.Add("depth", request.Depth);
                    var result = Get(uriBuilder.ToUri(), DeserializeArray<cm.cmisTypeContainer>, false);
                    if (result.Exception is null)
                    {
                        return new cmr.getTypeDescendantsResponse() { Types = SetTypeDefinitions(result.Response, request.RepositoryId) };
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.updateTypeResponse> UpdateType(cm.Requests.updateType request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "updatetype");

                    content.Add(request.Type);
                    var result = Post(new UriBuilder(withBlock.Response.RepositoryUrl, request), content, null, Deserialize<ccdt.cmisTypeDefinitionType>, false);
                    if (result.Exception is null)
                    {
                        var type = result.Response;

                        // cache typeDefinition
                        if (type is not null)
                        {
                            string typeId = type.Id;
                            string repositoryId = request.RepositoryId;

                            if (!(string.IsNullOrEmpty(repositoryId) || string.IsNullOrEmpty(typeId)))
                            {
                                set_TypeDefinition(repositoryId, typeId, type);
                            }
                        }

                        return new cmr.updateTypeResponse() { Type = type };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Gets the list of documents that are checked out that the user has access to
      /// </summary>
        public override Generic.ResponseType<cmr.getCheckedOutDocsResponse> GetCheckedOutDocs(cm.Requests.getCheckedOutDocs request)
        {
            request.BrowserBinding.CmisSelector = "checkedOut";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var uriBuilder = new UriBuilder(string.IsNullOrEmpty(request.FolderId) ? withBlock.Response.RepositoryUrl : withBlock.Response.RootFolderUrl, request);

                    if (!string.IsNullOrEmpty(request.FolderId))
                        uriBuilder.Add("objectId", request.FolderId);
                    uriBuilder.AddRange("maxItems", request.MaxItems, "skipCount", request.SkipCount, "orderBy", request.OrderBy, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter, "includeAllowableActions", request.IncludeAllowableActions);
                    var result = Get(uriBuilder.ToUri(), Deserialize<cmr.getContentChangesResponse>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getCheckedOutDocsResponse() { Objects = result.Response.Objects };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getChildrenResponse> GetChildren(cm.Requests.getChildren request)
        {
            request.BrowserBinding.CmisSelector = "children";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true, "maxItems", request.MaxItems, "skipCount", request.SkipCount, "orderBy", request.OrderBy, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter, "includeAllowableActions", request.IncludeAllowableActions, "includePathSegment", request.IncludePathSegment), Deserialize<cm.cmisObjectInFolderListType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getChildrenResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getDescendantsResponse> GetDescendants(cm.Requests.getDescendants request)
        {
            request.BrowserBinding.CmisSelector = "descendants";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true, "depth", request.Depth, "filter", request.Filter, "includeAllowableActions", request.IncludeAllowableActions, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter, "includePathSegment", request.IncludePathSegment), DeserializeArray<cm.cmisObjectInFolderContainerType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(new cm.cmisObjectInFolderContainerType() { Children = result.Response }, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getDescendantsResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getFolderParentResponse> GetFolderParent(cm.Requests.getFolderParent request)
        {
            request.BrowserBinding.CmisSelector = "parent";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true, "filter", request.Filter), Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getFolderParentResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getFolderTreeResponse> GetFolderTree(cm.Requests.getFolderTree request)
        {
            request.BrowserBinding.CmisSelector = "folderTree";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true, "depth", request.Depth, "filter", request.Filter, "includeAllowableActions", request.IncludeAllowableActions, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter, "includePathSegment", request.IncludePathSegment), DeserializeArray<cm.cmisObjectInFolderContainerType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(new cm.cmisObjectInFolderContainerType() { Children = result.Response }, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getFolderTreeResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getObjectParentsResponse> GetObjectParents(cm.Requests.getObjectParents request)
        {
            request.BrowserBinding.CmisSelector = "parents";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter, "includeAllowableActions", request.IncludeAllowableActions, "includeRelativePathSegment", request.IncludeRelativePathSegment), DeserializeArray<cm.cmisObjectParentsType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        var parents = from parent in result.Response ?? (new cm.cmisObjectParentsType[] { })
                                      let parentObject = parent is null ? null : parent.Object
                                      where parentObject is not null
                                      select parentObject;
                        TransformResponse(parents, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getObjectParentsResponse() { Parents = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the specified document object
      /// </summary>
        public override Generic.ResponseType<cmr.appendContentStreamResponse> AppendContentStream(cm.Requests.appendContentStream request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "appendContent", request);

                    AddToContent(content, null, null, null, null, request.ContentStream);
                    if (request.IsLastChunk.HasValue)
                        content.Add("isLastChunk", CommonFunctions.Convert(request.IsLastChunk.Value));
                    if (!string.IsNullOrEmpty(request.ChangeToken))
                        content.Add("changeToken", request.ChangeToken);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.appendContentStreamResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.bulkUpdatePropertiesResponse> BulkUpdateProperties(cm.Requests.bulkUpdateProperties request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "bulkUpdate", request);
                    var state = TransformRequest(request.RepositoryId, request.BulkUpdateData.Properties);

                    try
                    {
                        {
                            var withBlock1 = request.BulkUpdateData;
                            // Properties
                            AddToContent(content, withBlock1.Properties, null, null, null);
                            // changeTokens, objectIds
                            if (withBlock1.ObjectIdAndChangeTokens is not null)
                            {
                                foreach (Core.cmisObjectIdAndChangeTokenType objectIdAndChangeToken in withBlock1.ObjectIdAndChangeTokens)
                                {
                                    content.Add(objectIdAndChangeToken.ChangeToken, JSON.Enums.enumValueType.changeToken);
                                    content.Add(objectIdAndChangeToken.Id, JSON.Enums.enumValueType.objectId);
                                }
                            }
                            // addSecondaryIds and removeSecondaryIds
                            foreach (KeyValuePair<JSON.Enums.enumCollectionAction, string[]> de in new Dictionary<JSON.Enums.enumCollectionAction, string[]>() { { JSON.Enums.enumCollectionAction.add, withBlock1.AddSecondaryTypeIds }, { JSON.Enums.enumCollectionAction.remove, withBlock1.RemoveSecondaryTypeIds } })
                            {
                                if (de.Value is not null)
                                {
                                    foreach (string secondaryTypeId in de.Value)
                                        content.Add(secondaryTypeId, de.Key);
                                }
                            }
                        }
                        var result = Post(new UriBuilder(withBlock.Response.RepositoryUrl, request), content, null, DeserializeArray<Core.cmisObjectIdAndChangeTokenType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            return new cmr.bulkUpdatePropertiesResponse() { ObjectIdAndChangeTokens = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
        public override Generic.ResponseType<cmr.createDocumentResponse> CreateDocument(cm.Requests.createDocument request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "createDocument", request);
                    var uriBuilder = new UriBuilder(string.IsNullOrEmpty(request.FolderId) ? withBlock.Response.RepositoryUrl : withBlock.Response.RootFolderUrl, request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        if (!string.IsNullOrEmpty(request.FolderId))
                            uriBuilder.Add("objectId", request.FolderId);
                        // append cmis:contentStreamFileName, cmis:contentStreamLength and cmis:contentStreamMimeType if necessary
                        AddToContent(content, request.ContentStream is null ? request.Properties : request.ContentStream.ExtendProperties(request.Properties), request.Policies, request.AddACEs, request.RemoveACEs, request.ContentStream, request.VersioningState);
                        var result = Post(uriBuilder.ToUri(), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createDocumentResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
      /// <remarks>In chapter 5.4.2.7 Action "createDocumentFromSource" the listed relevant CMIS controls contains "Content",
      /// but there is no equivalent in chapter 2.2.4.2  createDocumentFromSource. Therefore content is ignored.</remarks>
        public override Generic.ResponseType<cmr.createDocumentFromSourceResponse> CreateDocumentFromSource(cm.Requests.createDocumentFromSource request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "createDocumentFromSource", request);
                    var uriBuilder = new UriBuilder(string.IsNullOrEmpty(request.FolderId) ? withBlock.Response.RepositoryUrl : withBlock.Response.RootFolderUrl, request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        if (!string.IsNullOrEmpty(request.FolderId))
                            uriBuilder.Add("objectId", request.FolderId);
                        // append cmis:contentStreamFileName, cmis:contentStreamLength and cmis:contentStreamMimeType if necessary
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs, null, request.VersioningState);
                        content.Add("sourceId", request.SourceId);
                        var result = Post(uriBuilder.ToUri(), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createDocumentFromSourceResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
        public override Generic.ResponseType<cmr.createFolderResponse> CreateFolder(cm.Requests.createFolder request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "createFolder", request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs);
                        var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createFolderResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
      /// Creates a item object of the specified type (given by the cmis:objectTypeId property) in (optionally) the specified location
      /// </summary>
        public override Generic.ResponseType<cmr.createItemResponse> CreateItem(cm.Requests.createItem request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "createItem", request);
                    var uriBuilder = new UriBuilder(string.IsNullOrEmpty(request.FolderId) ? withBlock.Response.RepositoryUrl : withBlock.Response.RootFolderUrl, request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        if (!string.IsNullOrEmpty(request.FolderId))
                            uriBuilder.Add("objectId", request.FolderId);
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs);
                        var result = Post(uriBuilder.ToUri(), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createItemResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
      /// Creates a policy object of the specified type (given by the cmis:objectTypeId property) in the (optionally) specified location
      /// </summary>
        public override Generic.ResponseType<cmr.createPolicyResponse> CreatePolicy(cm.Requests.createPolicy request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "createPolicy", request);
                    var uriBuilder = new UriBuilder(string.IsNullOrEmpty(request.FolderId) ? withBlock.Response.RepositoryUrl : withBlock.Response.RootFolderUrl, request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        if (!string.IsNullOrEmpty(request.FolderId))
                            uriBuilder.Add("objectId", request.FolderId);
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs);
                        var result = Post(uriBuilder.ToUri(), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createPolicyResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
        public override Generic.ResponseType<cmr.createRelationshipResponse> CreateRelationship(cm.Requests.createRelationship request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "createRelationship", request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs);
                        var result = Post(new UriBuilder(withBlock.Response.RepositoryUrl, request), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.createRelationshipResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
      /// Deletes the content stream of a cmis document
      /// </summary>
        public override Generic.ResponseType<cmr.deleteContentStreamResponse> DeleteContentStream(cm.Requests.deleteContentStream request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "deleteContent", request);

                    content.Add("changeToken", request.ChangeToken);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.deleteContentStreamResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.deleteObjectResponse> DeleteObject(cm.Requests.deleteObject request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, request.RepositoryId, EventBus.enumBuiltInEvents.DeleteObject, request.ObjectId);
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "delete");

                    if (request.AllVersions.HasValue)
                        content.Add("allVersions", CommonFunctions.Convert(request.AllVersions.Value));
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null);
                    if (result.Exception is null)
                    {
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true } });
                        return new cmr.deleteObjectResponse();
                    }
                    else
                    {
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, result.Exception } });
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.deleteTreeResponse> DeleteTree(cm.Requests.deleteTree request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "deleteTree");

                    if (request.AllVersions.HasValue)
                        content.Add("allVersions", CommonFunctions.Convert(request.AllVersions.Value));
                    if (request.UnfileObjects.HasValue)
                        content.Add("unfileObjects", request.UnfileObjects.Value.GetName());
                    if (request.ContinueOnFailure.HasValue)
                        content.Add("continueOnFailure", CommonFunctions.Convert(request.ContinueOnFailure.Value));
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.FolderId, true), content, null, Deserialize<Core.Collections.cmisListOfIdsType>, request.BrowserBinding.Succinct);
                    if (result is null || result.Exception is null)
                    {
                        return new cmr.deleteTreeResponse() { FailedToDelete = result is null || result.Response is null || result.Response.Ids is null ? null : new cm.failedToDelete(result.Response.Ids) };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getAllowableActionsResponse> GetAllowableActions(cm.Requests.getAllowableActions request)
        {
            request.BrowserBinding.CmisSelector = "allowableActions";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), Deserialize<Core.cmisAllowableActionsType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        return new cmr.getAllowableActionsResponse() { AllowableActions = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Gets the content stream for the specified document object, or gets a rendition stream for a specified rendition of a document or folder object.
      /// </summary>
        public override Generic.ResponseType<cmr.getContentStreamResponse> GetContentStream(cm.Requests.getContentStream request)
        {
            request.BrowserBinding.CmisSelector = "content";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
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

                                   Dim result = Me.Get(New UriBuilder(.Response.RootFolderUrl, request,
                                                                      "objectId", request.ObjectId, True,
                                                                      "streamId", request.StreamId),
                                                       offset, length)
                    *//* TODO ERROR: Skipped ElseDirectiveTrivia
                    #Else
                    */
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "streamId", request.StreamId), request.Offset, request.Length);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    if (result.Exception is null)
                    {
                        // Maybe the filename is sent via Content-Disposition
                        var headers = result.WebResponse is null ? null : result.WebResponse.Headers;
                        string disposition = null;
                        string fileName = headers is null ? null : RFC2231Helper.DecodeContentDisposition(headers[RFC2231Helper.ContentDispositionHeaderName], ref disposition);

                        return new cmr.getContentStreamResponse() { ContentStream = new cm.cmisContentStreamType(result.Stream, fileName, result.ContentType) };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Returns the uri to get the content of a cmisDocument
      /// </summary>
        public override Generic.ResponseType<string> GetContentStreamLink(string repositoryId, string objectId, string streamId = null)
        {
            var request = new cm.Requests.getContentStream();

            request.BrowserBinding.CmisSelector = "content";
            request.ObjectId = objectId;
            request.RepositoryId = repositoryId;
            if (streamId is not null)
                request.StreamId = streamId;
            {
                var withBlock = GetRepositoryInfo(repositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var uriBuilder = new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", objectId, true, "streamId", streamId);
                    return uriBuilder.ToUri().AbsoluteUri;
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
        public override Generic.ResponseType<cmr.getObjectResponse> GetObject(cm.Requests.getObject request)
        {
            request.BrowserBinding.CmisSelector = "object";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "includePolicyIds", request.IncludePolicyIds, "renditionFilter", request.RenditionFilter, "includeACL", request.IncludeACL, "includeAllowableActions", request.IncludeAllowableActions), Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getObjectResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.getObjectByPathResponse> GetObjectByPath(cm.Requests.getObjectByPath request)
        {
            request.BrowserBinding.CmisSelector = "object";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(CreateObjectUri(withBlock.Response.RootFolderUrl, request.Path), request, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "includePolicyIds", request.IncludePolicyIds, "renditionFilter", request.RenditionFilter, "includeACL", request.IncludeACL, "includeAllowableActions", request.IncludeAllowableActions), Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getObjectByPathResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.getPropertiesResponse> GetProperties(cm.Requests.getProperties request)
        {
            request.BrowserBinding.CmisSelector = "properties";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter), Deserialize<Core.Collections.cmisPropertiesType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getPropertiesResponse() { Properties = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.getRenditionsResponse> GetRenditions(cm.Requests.getRenditions request)
        {
            request.BrowserBinding.CmisSelector = "renditions";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "renditionFilter", request.RenditionFilter, "maxItems", request.MaxItems, "skipCount", request.SkipCount), DeserializeArray<Core.cmisRenditionType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        return new cmr.getRenditionsResponse() { Renditions = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.moveObjectResponse> MoveObject(cm.Requests.moveObject request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "move", request);

                    content.Add("sourceFolderId", request.SourceFolderId);
                    content.Add("targetFolderId", request.TargetFolderId);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.moveObjectResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.setContentStreamResponse> SetContentStream(cm.Requests.setContentStream request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "setContent", request);

                    content.Add("changeToken", request.ChangeToken);
                    if (request.OverwriteFlag.HasValue)
                        content.Add("overwriteFlag", CommonFunctions.Convert(request.OverwriteFlag.Value));
                    AddToContent(content, null, null, null, null, request.ContentStream);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.setContentStreamResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.updatePropertiesResponse> UpdateProperties(cm.Requests.updateProperties request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "update", request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        content.Add("changeToken", request.ChangeToken);
                        AddToContent(content, request.Properties, null, null, null);
                        var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            return new cmr.updatePropertiesResponse() { Object = result.Response };
                        }
                        else
                        {
                            return result.Exception;
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
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder
      /// </summary>
        public override Generic.ResponseType<cmr.addObjectToFolderResponse> AddObjectToFolder(cm.Requests.addObjectToFolder request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "addObjectToFolder", request);

                    content.Add("folderId", request.FolderId);
                    if (request.AllVersions.HasValue)
                        content.Add("allVersions", CommonFunctions.Convert(request.AllVersions.Value));
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.addObjectToFolderResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.removeObjectFromFolderResponse> RemoveObjectFromFolder(cm.Requests.removeObjectFromFolder request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "removeObjectFromFolder", request);

                    content.Add("folderId", request.FolderId);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.removeObjectFromFolderResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Disc
        /// <summary>
      /// Gets a list of content changes. This service is intended to be used by search crawlers or other applications that need to
      /// efficiently understand what has changed in the repository
      /// </summary>
        public override Generic.ResponseType<cmr.getContentChangesResponse> GetContentChanges(cm.Requests.getContentChanges request)
        {
            request.BrowserBinding.CmisSelector = "contentChanges";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RepositoryUrl, request, "filter", request.Filter, "changeLogToken", request.ChangeLogToken, "includeProperties", request.IncludeProperties, "includePolicyIds", request.IncludePolicyIds, "includeACL", request.IncludeACL, "maxItems", request.MaxItems), Deserialize<cmr.getContentChangesResponse>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return result;
                    }
                    else
                    {
                        return result.Exception;
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
      /// <remarks>Another way to implement this method is to use a POST-request</remarks>
        public override Generic.ResponseType<cmr.queryResponse> Query(cm.Requests.query request)
        {
            request.BrowserBinding.CmisSelector = "query";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RepositoryUrl, request, "q", request.Statement, true, "searchAllVersions", request.SearchAllVersions, "maxItems", request.MaxItems, "skipCount", request.SkipCount, "includeAllowableActions", request.IncludeAllowableActions, "includeRelationships", request.IncludeRelationships, "renditionFilter", request.RenditionFilter), Deserialize<cm.cmisObjectListType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.queryResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
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
        public override Generic.ResponseType<cmr.cancelCheckOutResponse> CancelCheckOut(cm.Requests.cancelCheckOut request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, request.RepositoryId, EventBus.enumBuiltInEvents.CancelCheckout, request.ObjectId);
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "cancelCheckOut", request);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null);
                    if (result.Exception is null)
                    {
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true } });
                        return new cmr.cancelCheckOutResponse();
                    }
                    else
                    {
                        e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, result.Exception } });
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Checks-in the Private Working Copy document.
      /// </summary>
        public override Generic.ResponseType<cmr.checkInResponse> CheckIn(cm.Requests.checkIn request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var e = EventBus.EventArgs.DispatchBeginEvent(this, null, ServiceDocUri.AbsoluteUri, request.RepositoryId, EventBus.enumBuiltInEvents.CheckIn, request.ObjectId);
                    var content = new JSON.MultipartFormDataContent(MediaTypes.MultipartFormData, "checkIn", request);
                    var state = TransformRequest(request.RepositoryId, request.Properties);

                    try
                    {
                        AddToContent(content, request.Properties, request.Policies, request.AddACEs, request.RemoveACEs, request.ContentStream);
                        if (request.Major.HasValue)
                            content.Add("major", CommonFunctions.Convert(request.Major.Value));
                        content.Add("checkinComment", request.CheckinComment);
                        var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                        if (result.Exception is null)
                        {
                            TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                            e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, true }, { EventBus.EventArgs.PredefinedPropertyNames.NewObjectId, result.Response.ObjectId.Value } });
                            return new cmr.checkInResponse() { Object = result.Response };
                        }
                        else
                        {
                            e.DispatchEndEvent(new Dictionary<string, object>() { { EventBus.EventArgs.PredefinedPropertyNames.Succeeded, false }, { EventBus.EventArgs.PredefinedPropertyNames.Failure, result.Exception } });
                            return result.Exception;
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
      /// Checks out the specified CMIS object.
      /// </summary>
        public override Generic.ResponseType<cmr.checkOutResponse> CheckOut(cm.Requests.checkOut request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "checkOut", request);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.checkOutResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getAllVersionsResponse> GetAllVersions(cm.Requests.getAllVersions request)
        {
            request.BrowserBinding.CmisSelector = "versions";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter, "includeAllowableActions", request.IncludeAllowableActions), DeserializeArray<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getAllVersionsResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
        public override Generic.ResponseType<cmr.getObjectOfLatestVersionResponse> GetObjectOfLatestVersion(cm.Requests.getObjectOfLatestVersion request)
        {
            request.BrowserBinding.CmisSelector = "object";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    RestAtom.enumReturnVersion? returnVersion = !request.Major.HasValue ? default : request.Major.Value ? RestAtom.enumReturnVersion.latestmajor : RestAtom.enumReturnVersion.latest;
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter, "includeRelationships", request.IncludeRelationships, "includePolicyIds", request.IncludePolicyIds, "renditionFilter", request.RenditionFilter, "includeACL", request.IncludeACL, "includeAllowableActions", request.IncludeAllowableActions, "returnVersion", returnVersion), Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getObjectOfLatestVersionResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
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
        public override Generic.ResponseType<cmr.getPropertiesOfLatestVersionResponse> GetPropertiesOfLatestVersion(cm.Requests.getPropertiesOfLatestVersion request)
        {
            request.BrowserBinding.CmisSelector = "properties";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    RestAtom.enumReturnVersion? returnVersion = !request.Major.HasValue ? default : request.Major.Value ? RestAtom.enumReturnVersion.latestmajor : RestAtom.enumReturnVersion.latest;
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter, "returnVersion", returnVersion), Deserialize<Core.Collections.cmisPropertiesType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getPropertiesOfLatestVersionResponse() { Properties = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Rel
        /// <summary>
      /// Gets all or a subset of relationships associated with an independent object
      /// </summary>
        public override Generic.ResponseType<cmr.getObjectRelationshipsResponse> GetObjectRelationships(cm.Requests.getObjectRelationships request)
        {
            request.BrowserBinding.CmisSelector = "relationships";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "includeSubRelationshipTypes", request.IncludeSubRelationshipTypes, "relationshipDirection", request.RelationshipDirection, "typeId", request.TypeId, "maxItems", request.MaxItems, "skipCount", request.SkipCount, "filter", request.Filter, "includeAllowableActions", request.IncludeAllowableActions), Deserialize<cmr.getContentChangesResponse>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getObjectRelationshipsResponse() { Objects = result.Response.Objects };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Policy
        /// <summary>
      /// Applies a specified policy to an object
      /// </summary>
      /// <param name="request"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public override Generic.ResponseType<cmr.applyPolicyResponse> ApplyPolicy(cm.Requests.applyPolicy request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "applyPolicy", request);

                    content.Add("policyId", request.PolicyId);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.applyPolicyResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getAppliedPoliciesResponse> GetAppliedPolicies(cm.Requests.getAppliedPolicies request)
        {
            request.BrowserBinding.CmisSelector = "policies";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "filter", request.Filter), DeserializeArray<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.getAppliedPoliciesResponse() { Objects = result.Response };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.removePolicyResponse> RemovePolicy(cm.Requests.removePolicy request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "removePolicy", request);

                    content.Add("policyId", request.PolicyId);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.cmisObjectType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        TransformResponse(result.Response, new State(request.RepositoryId, request.BrowserBinding.Succinct));
                        return new cmr.removePolicyResponse() { Object = result.Response };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region ACL
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of an object
      /// </summary>
        public override Generic.ResponseType<cmr.applyACLResponse> ApplyAcl(cm.Requests.applyACL request)
        {
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "applyACL", request);

                    AddToContent(content, null, null, request.AddACEs, request.RemoveACEs);
                    if (request.ACLPropagation.HasValue)
                        content.Add(request.ACLPropagation.Value);
                    var result = Post(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true), content, null, Deserialize<Core.Security.cmisAccessControlListType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        return new cmr.applyACLResponse()
                        {
                            ACL = new cm.cmisACLType()
                            {
                                ACL = result.Response,
                                Exact = result.Response is null ? default : result.Response.IsExact
                            }
                        };
                    }
                    else
                    {
                        return result.Exception;
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
        public override Generic.ResponseType<cmr.getACLResponse> GetAcl(cm.Requests.getACL request)
        {
            request.BrowserBinding.CmisSelector = "acl";
            {
                var withBlock = GetRepositoryInfo(request.RepositoryId, request.BrowserBinding.Token);
                if (withBlock.Exception is null)
                {
                    var result = Get(new UriBuilder(withBlock.Response.RootFolderUrl, request, "objectId", request.ObjectId, true, "onlyBasicPermissions", request.OnlyBasicPermissions), Deserialize<Core.Security.cmisAccessControlListType>, request.BrowserBinding.Succinct);
                    if (result.Exception is null)
                    {
                        return new cmr.getACLResponse()
                        {
                            ACL = new cm.cmisACLType()
                            {
                                ACL = result.Response,
                                Exact = result.Response is null ? default : result.Response.IsExact
                            }
                        };
                    }
                    else
                    {
                        return result.Exception;
                    }
                }
                else
                {
                    return withBlock.Exception;
                }
            }
        }
        #endregion

        #region Miscellaneous (ICmisClient)
        public override enumClientType ClientType
        {
            get
            {
                return enumClientType.BrowserBinding;
            }
        }

        /// <summary>
      /// Logs out from repository
      /// </summary>
        public override void Logout(string repositoryId)
        {
            var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "logout");
            var uri = new Uri(ServiceURIs.GetServiceUri(_serviceDocUri.OriginalString, ServiceURIs.enumRepositoriesUri.repositoryId | ServiceURIs.enumRepositoriesUri.logout).ReplaceUri("repositoryId", repositoryId, "logout", "true"));
            Post(uri, content, null);
            set_RepositoryInfo(repositoryId, null);
        }

        /// <summary>
      /// Tells the server, that this client is still alive
      /// </summary>
      /// <remarks></remarks>
        public override void Ping(string repositoryId)
        {
            var content = new JSON.MultipartFormDataContent(MediaTypes.UrlEncodedUTF8, "ping");
            var uri = new Uri(ServiceURIs.GetServiceUri(_serviceDocUri.OriginalString, ServiceURIs.enumRepositoriesUri.repositoryId | ServiceURIs.enumRepositoriesUri.ping).ReplaceUri("repositoryId", repositoryId, "ping", "true"));
            Post(uri, content, null);
        }

        public override bool SupportsSuccinct
        {
            get
            {
                return true;
            }
        }
        public override bool SupportsToken
        {
            get
            {
                return true;
            }
        }

        /// <summary>
      /// UserAgent-name of current instance
      /// </summary>
        protected override string UserAgent
        {
            get
            {
                return "Brügmann Software CmisObjectModel.Client.BrowserBinding.CmisClient";
            }
        }
        #endregion

        #region Requests

        #region Vendor specific and value mapping
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values on all cmisObjects in objects
      /// </summary>
        private new void TransformResponse(IEnumerable<Core.cmisObjectType> objects, State state)
        {
            if (objects is not null)
            {
                string repositoryId = state.RepositoryId;
                var propertyCollections = (from cmisObject in objects
                                           let propertyCollection = cmisObject is null ? null : cmisObject.Properties
                                           where propertyCollection is not null
                                           select propertyCollection).ToArray();

                TransformResponse(state, state.TransformResponse(propertyCollections));
            }
        }
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values on a single cmisObject
      /// </summary>
        private new void TransformResponse(Core.cmisObjectType cmisObject, State state)
        {
            if (cmisObject is not null && cmisObject.Properties is not null)
            {
                TransformResponse(state, state.TransformResponse(new Core.Collections.cmisPropertiesType[] { cmisObject.Properties }));
            }
        }
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values on cmisPropertiesType
      /// </summary>
        private new void TransformResponse(Core.Collections.cmisPropertiesType properties, State state)
        {
            if (properties is not null)
            {
                TransformResponse(state, state.TransformResponse(new Core.Collections.cmisPropertiesType[] { properties }));
            }
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
            try
            {
                return new Response(GetResponse(uri, "GET", null, null, null, offset, length));
            }
            catch (sn.WebException ex)
            {
                return new Response(ex);
            }
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Private Function [Get](Of TResponse)(uri As Uri, responseFactory As Func(Of String, String, Boolean, TResponse), succinct As Boolean,
                                                   Optional offset As Integer? = Nothing, Optional length As Integer? = Nothing) As Generic.Response(Of TResponse)
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        private Generic.ResponseType<TResponse> Get<TResponse>(Uri uri, Func<string, string, bool, TResponse> responseFactory, bool succinct, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            try
            {
                return new Generic.ResponseType<TResponse>(GetResponse(uri, "GET", null, null, null, offset, length), (input, contentType) => responseFactory(input, contentType, succinct));
            }
            catch (sn.WebException ex)
            {
                return new Generic.ResponseType<TResponse>(ex);
            }
        }
        #endregion

        #region Post-Requests
        private Response Post(Uri uri, JSON.MultipartFormDataContent content, Dictionary<string, string> headers)
        {
            var contentWriter = content is null ? null : new Action<System.IO.Stream>((requestStream) => content.WriteTo(requestStream));
            try
            {
                return new Response(GetResponse(uri, "POST", contentWriter, content.ContentType, headers, default, default));
            }
            catch (sn.WebException ex)
            {
                return new Response(ex);
            }
        }

        private Generic.ResponseType<TResponse> Post<TResponse>(Uri uri, JSON.MultipartFormDataContent content, Dictionary<string, string> headers, Func<string, string, bool, TResponse> responseFactory, bool succinct)
        {
            var contentWriter = content is null ? null : new Action<System.IO.Stream>((requestStream) => content.WriteTo(requestStream));
            try
            {
                return new Generic.ResponseType<TResponse>(GetResponse(uri, "POST", contentWriter, content.ContentType, headers, default, default), (input, contentType) => responseFactory(input, contentType, succinct));
            }
            catch (sn.WebException ex)
            {
                return new Generic.ResponseType<TResponse>(ex);
            }
        }
        #endregion

        #endregion

        #region Deserialization of Responses
        /// <summary>
      /// Deserializes a simple XmlSerializable
      /// </summary>
        private TResult Deserialize<TResult>(string input, string contentType, bool succinct) where TResult : Serialization.XmlSerializable
        {
            if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith(MediaTypes.Json, StringComparison.InvariantCultureIgnoreCase))
            {
                var serializer = new JSON.Serialization.JavaScriptSerializer(succinct);
                return serializer.Deserialize<TResult>(input);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Deserializes an array of XmlSerializable-instances
      /// </summary>
        private TResult[] DeserializeArray<TResult>(string input, string contentType, bool succinct) where TResult : Serialization.XmlSerializable
        {
            if (!string.IsNullOrEmpty(contentType) && contentType.StartsWith(MediaTypes.Json, StringComparison.InvariantCultureIgnoreCase))
            {
                var serializer = new JSON.Serialization.JavaScriptSerializer(succinct);
                return serializer.DeserializeArray<TResult>(input);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Deserializes cmisRepositoryInfoType-instances (transmitted as a map)
      /// </summary>
        private Core.cmisRepositoryInfoType[] DeserializeRepositories(string input, string contentType, bool succinct)
        {
            try
            {
                var serializer = new JSON.Serialization.JavaScriptSerializer(succinct);
                return serializer.DeserializeMap(input, Core.cmisRepositoryInfoType.DefaultKeyProperty);
            }
            catch (Exception ex)
            {
                throw new ssw.WebFaultException<Exception>(ex, sn.HttpStatusCode.ExpectationFailed);
            }
        }
        #endregion

        /// <summary>
      /// Copies object properties to content
      /// </summary>
        private void AddToContent(JSON.MultipartFormDataContent content, Core.Collections.cmisPropertiesType properties, string[] policies, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs, cm.cmisContentStreamType contentStream = null, Core.enumVersioningState? versioningState = default)
        {
            // properties
            if (properties is not null && properties.Properties is not null)
            {
                foreach (Core.Properties.cmisProperty cmisProperty in properties.Properties)
                    content.Add(cmisProperty);
                if (properties.Extensions is not null)
                {
                    content.Add(new Core.cmisObjectType.PropertiesExtensions(properties));
                }
            }
            // policies
            if (policies is not null)
            {
                foreach (string policyId in policies)
                    content.Add(policyId, JSON.Enums.enumValueType.policy);
            }
            // addACEs and removeACEs
            foreach (KeyValuePair<JSON.Enums.enumCollectionAction, Core.Security.cmisAccessControlListType> de in new Dictionary<JSON.Enums.enumCollectionAction, Core.Security.cmisAccessControlListType>() { { JSON.Enums.enumCollectionAction.add, addACEs }, { JSON.Enums.enumCollectionAction.remove, removeACEs } })
            {
                if (de.Value is not null && de.Value.Permissions is not null)
                {
                    foreach (Core.Security.cmisAccessControlEntryType ace in de.Value.Permissions)
                        content.Add(ace, de.Key);
                }
            }
            // contentStream (documents only)
            if (contentStream is not null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    var stream = contentStream.BinaryStream;

                    if (stream is not null)
                    {
                        stream.CopyTo(ms);
                        ms.Position = 0L;
                        var httpContent = new JSON.HttpContent(ms.ToArray());
                        if (!string.IsNullOrEmpty(contentStream.Filename))
                        {
                            httpContent.Headers.Add(RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename, "form-data; name=\"content\""));
                        }
                        if (!string.IsNullOrEmpty(contentStream.MimeType))
                            httpContent.Headers.Add(RFC2231Helper.ContentTypeHeaderName, contentStream.MimeType);
                        httpContent.Headers.Add(RFC2231Helper.ContentTransferEncoding, "binary");
                        content.Add(httpContent);
                    }
                }
            }
            // versioningState (documents only)
            if (versioningState.HasValue)
                content.Add("versioningState", versioningState.Value.GetName());
        }

        /// <summary>
      /// Returns the uri of an Object (ObjectByPath)
      /// </summary>
        private string CreateObjectUri(string rootFolderUri, string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return rootFolderUri;
            }
            else
            {
                if (!string.IsNullOrEmpty(rootFolderUri))
                    rootFolderUri = rootFolderUri.TrimEnd('/');
                relativePath = relativePath.TrimStart('/');
                return rootFolderUri + "/" + relativePath;
            }
        }

        /// <summary>
      /// Returns the repository that matches the repositoryId-parameter or null, if no repository matches the filter
      /// </summary>
        private Core.cmisRepositoryInfoType FindRepository(Core.cmisRepositoryInfoType[] repositories, string repositoryId)
        {
            if (repositories is not null)
            {
                foreach (Core.cmisRepositoryInfoType repository in repositories)
                {
                    if (repository is not null && (repository.RepositoryId ?? "") == (repositoryId ?? ""))
                        return repository;
                }
            }

            // not found
            return null;
        }

        /// <summary>
      /// Caches types
      /// </summary>
        private static cm.cmisTypeDefinitionListType SetTypeDefinitions(cm.cmisTypeDefinitionListType result, string repositoryId)
        {
            if (result is not null && result.Types is not null)
            {
                foreach (ccdt.cmisTypeDefinitionType type in result.Types)
                {
                    if (type is not null)
                    {
                        string id = type.Id;
                        if (!string.IsNullOrEmpty(id))
                        {
                            set_TypeDefinition(repositoryId, id, type);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
      /// Caches types
      /// </summary>
        private static cm.cmisTypeContainer[] SetTypeDefinitions(cm.cmisTypeContainer[] result, string repositoryId)
        {
            if (result is not null)
            {
                var containerCollections = new Stack<cm.cmisTypeContainer>(from container in result
                                                                           where container is not null
                                                                           select container);
                while (containerCollections.Count > 0)
                {
                    var container = containerCollections.Pop();
                    var type = container.Type;

                    if (type is not null)
                    {
                        string id = type.Id;
                        if (!string.IsNullOrEmpty(id))
                        {
                            set_TypeDefinition(repositoryId, id, type);
                        }
                    }
                    if (container.Children is not null)
                    {
                        foreach (cm.cmisTypeContainer child in container.Children)
                        {
                            if (child is not null)
                                containerCollections.Push(child);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
      /// Cached TypeDefinitions to determine DateTime-properties
      /// </summary>
      /// <remarks></remarks>
        private static Collections.Generic.DictionaryTree<string, ccdt.cmisTypeDefinitionType> _typeDefinitions = new Collections.Generic.DictionaryTree<string, ccdt.cmisTypeDefinitionType>();
        private static ccdt.cmisTypeDefinitionType get_TypeDefinition(string repositoryId, string typeId)
        {
            return _typeDefinitions.get_Item(repositoryId, typeId);
        }

        private static void set_TypeDefinition(string repositoryId, string typeId, ccdt.cmisTypeDefinitionType value)
        {
            _typeDefinitions.set_Item(new string[] { repositoryId, typeId }, value : value );
        }
    }
}