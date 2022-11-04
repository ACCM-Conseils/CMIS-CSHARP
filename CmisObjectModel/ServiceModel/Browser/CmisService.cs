using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using ccg = CmisObjectModel.Common.Generic;
using CmisObjectModel.Constants;
using cc = CmisObjectModel.Core;
using ccc = CmisObjectModel.Core.Collections;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cm = CmisObjectModel.Messaging;
using cr = CmisObjectModel.RestAtom;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel.Browser
{
    /// <summary>
   /// Implements the functionality of the cmis-webservice version 1.1
   /// </summary>
   /// <remarks>
   /// The browser binding supports authentication with tokens for browser clients in the following way:
   /// if the hosting system (that is the system which implements the ICmisServiceImpl) supports sessionIdCookie
   /// this instance checks login processes. Three login methods are supported
   ///   a) via DispatchWebGetService() with query parameter repositoryId
   ///   b) via DispatchWebPostService() with query parameter cmisaction=login
   ///   c) via DispatchWebGetRepository() with query parameter cmisselector=repositoryInfo
   /// In these three cases a check is made if the client wants the repository to enable authentication with tokens.
   /// The client demands this sending a non empty token query parameter. If the client enables this authentication
   /// type cross-site request forgery can be prevented.
   /// </remarks>
    public class CmisService : Base.CmisService, Contracts.IBrowserBinding
    {

        #region Constructors
        static CmisService()
        {
            if (JSONTypeDefinitions.RepresentationTypes.ContainsKey(JSONTypeDefinitions.queryResultListUri))
            {
                _queryResultListType = JSONTypeDefinitions.RepresentationTypes[JSONTypeDefinitions.queryResultListUri].RepresentationType ?? typeof(cm.cmisObjectListType);
            }
            else
            {
                _queryResultListType = typeof(cm.cmisObjectListType);
            }
        }
        #endregion

        #region Helper classes
        private enum enumTokenTransmission : int
        {
            asControl,
            asQueryParameter
        }

        /// <summary>
      /// Contains a token sent from the client either as a form control or as a query parameter
      /// </summary>
      /// <remarks></remarks>
        private class TokenType
        {

            private TokenType(string token, enumTokenTransmission transmission)
            {
                Token = token;
                Transmission = transmission;
            }

            public static implicit operator TokenType(JSON.MultipartFormDataContent value)
            {
                string token = value is null ? null : value.ToString("token");

                return string.IsNullOrEmpty(token) ? null : new TokenType(token, enumTokenTransmission.asControl);
            }

            public static implicit operator TokenType(string value)
            {
                return string.IsNullOrEmpty(value) ? null : new TokenType(value, enumTokenTransmission.asQueryParameter);
            }

            public static implicit operator string(TokenType value)
            {
                return value is null ? null : value.Token;
            }

            /// <summary>
         /// Token; a non null value is guaranteed
         /// </summary>
         /// <remarks></remarks>
            public readonly string Token;
            public readonly enumTokenTransmission Transmission;

        }
        #endregion

        #region 5.3.1. Service URL
        /// <summary>
      /// Service-requests:
      /// GetRepositories():                      defined in cmis
      /// GetLoginPage(), GetEmbeddedFrame():     repository specific for browser binding
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public System.IO.Stream DispatchWebGetService()
        {
            try
            {
                CheckTokenAuthentication(null);
                switch ((CommonFunctions.GetRequestParameter("file") ?? string.Empty).ToLowerInvariant() ?? "")
                {
                    case "loginpage.htm":
                        {
                            // GetLoginPage(); repository specific; see 5.2.9.2.2 Login and Tokens
                            return GetFile(JSON.Enums.enumJSONFile.loginPageHtm);
                        }
                    case "embeddedframe.htm":
                        {
                            // GetEmbeddedFrame(); repository specific
                            return GetFile(JSON.Enums.enumJSONFile.embeddedFrameHtm);
                        }
                    case "cmis.js":
                        {
                            return GetFile(JSON.Enums.enumJSONFile.cmisJS);
                        }

                    default:
                        {
                            switch ((CommonFunctions.GetRequestParameter("cmisaction") ?? string.Empty).ToLowerInvariant() ?? "")
                            {
                                case "login":
                                    {
                                        // GetLoginPage(); repository specific; see 5.2.9.2.2 Login and Tokens
                                        return GetFile(JSON.Enums.enumJSONFile.loginPageHtm);
                                    }

                                default:
                                    {
                                        return GetRepositories();
                                    }
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                return SerializeException(ex);
            }
        }

        /// <summary>
      /// Service-post-requests
      /// Login(), Logout():                    repository specific for browser binding
      /// </summary>
      /// <param name="stream"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public System.IO.Stream DispatchWebPostService(System.IO.Stream stream)
        {
            var multipart = JSON.MultipartFormDataContent.FromStream(stream, ssw.WebOperationContext.Current.IncomingRequest.ContentType);
            string cmisaction = multipart.ToString("cmisaction") ?? CommonFunctions.GetRequestParameter("cmisaction");
            string repositoryId = multipart.ToString("repositoryId") ?? CommonFunctions.GetRequestParameter("repositoryId");
            var serviceImpl = CmisServiceImpl;
            ccg.Result<System.Net.HttpStatusCode> result;

            switch (cmisaction ?? "")
            {
                case "login":
                    {
                        // try to login
                        string authorization = multipart.ToString("authorization") ?? CommonFunctions.GetRequestParameter("authorization");
                        string hostingApplicationUri = multipart.ToString("hostingApplicationUri") ?? CommonFunctions.GetRequestParameter("hostingApplicationUri");
                        string user = multipart.ToString("user") ?? CommonFunctions.GetRequestParameter("user");

                        result = serviceImpl.Login(repositoryId, authorization);
                        if (result.Failure is not null || result.Success != System.Net.HttpStatusCode.OK)
                        {
                            // failure => present the login page once more
                            var regEx = new System.Text.RegularExpressions.Regex(@"window\.history\.go\(\-2\)", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            return PrepareResult(regEx.Replace(My.Resources.Resources.RedirectPreviousPage, "window.history.back()"), MediaTypes.Html);
                        }
                        else
                        {
                            // sending a token signals enabling authentication with tokens
                            EnableTokenAuthentication();
                            // success => browse to window before login page
                            return PrepareResult(My.Resources.Resources.RedirectPreviousPage, MediaTypes.Html);
                        }
                    }
                case "logout":
                    {
                        result = serviceImpl.Logout(repositoryId);
                        ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = result.Failure is null ? result.Success : System.Net.HttpStatusCode.InternalServerError;
                        break;
                    }
                case "ping":
                    {
                        result = serviceImpl.Ping(repositoryId);
                        ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = result.Failure is null ? result.Success : System.Net.HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            // default response
            return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
        }
        #endregion

        #region 5.3.2 Repository URL
        public System.IO.Stream DispatchWebGetRepository(string repositoryId)
        {
            try
            {
                var serviceImpl = CmisServiceImpl;
                string cmisselector = CommonFunctions.GetRequestParameter("cmisselector") ?? string.Empty;

                if (string.IsNullOrEmpty(cmisselector))
                    cmisselector = "repositoryInfo";
                if (string.Compare(cmisselector, "lastResult", true) == 0)
                {
                    // GetLastResult() is the only request without a CheckTokenAuthentication()-call
                    return GetLastResult(serviceImpl, repositoryId);
                }
                else
                {
                    CheckTokenAuthentication(null);
                    switch (cmisselector.ToLowerInvariant() ?? "")
                    {
                        case "checkedout":
                            {
                                return GetCheckedOutDocs(serviceImpl, repositoryId, null, CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("succinct")));
                            }
                        case "contentchanges":
                            {
                                return GetContentChanges(serviceImpl, repositoryId);
                            }
                        case "query":
                            {
                                return Query(serviceImpl, repositoryId);
                            }
                        case "repositoryinfo":
                            {
                                return GetRepositoryInfo(repositoryId);
                            }
                        case "typechildren":
                            {
                                return GetTypeChildren(serviceImpl, repositoryId);
                            }
                        case "typedefinition":
                            {
                                return GetTypeDefinition(serviceImpl, repositoryId);
                            }
                        case "typedescendants":
                            {
                                return GetTypeDescendants(serviceImpl, repositoryId);
                            }

                        default:
                            {
                                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'cmisselector' is not valid.", false), serviceImpl);
                            }
                    }
                }
            }
            catch (ssw.WebFaultException exWeb)
            {
                if (exWeb.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return GetFile(JSON.Enums.enumJSONFile.loginRefPageHtm);
                }
                else
                {
                    return SerializeException(exWeb);
                }
            }
            catch (Exception ex)
            {
                return SerializeException(ex);
            }
        }

        public System.IO.Stream DispatchWebPostRepository(string repositoryId, System.IO.Stream stream)
        {
            TokenType token = null;

            try
            {
                var multipart = JSON.MultipartFormDataContent.FromStream(stream, ssw.WebOperationContext.Current.IncomingRequest.ContentType);
                var serviceImpl = CmisServiceImpl;
                string cmisaction = multipart.ToString("cmisaction") ?? string.Empty;

                token = CheckTokenAuthentication(multipart);
                switch (cmisaction.ToLowerInvariant() ?? "")
                {
                    case "bulkupdate":
                        {
                            return BulkUpdateProperties(serviceImpl, repositoryId, token, multipart);
                        }
                    case "createdocument":
                        {
                            return CreateDocument(serviceImpl, repositoryId, null, token, multipart);
                        }
                    case "createdocumentfromsource":
                        {
                            return CreateDocumentFromSource(serviceImpl, repositoryId, null, token, multipart);
                        }
                    case "createitem":
                        {
                            return CreateItem(serviceImpl, repositoryId, null, token, multipart);
                        }
                    case "createpolicy":
                        {
                            return CreatePolicy(serviceImpl, repositoryId, null, token, multipart);
                        }
                    case "createrelationship":
                        {
                            return CreateRelationship(serviceImpl, repositoryId, token, multipart);
                        }
                    case "createtype":
                        {
                            return CreateType(serviceImpl, repositoryId, token, multipart);
                        }
                    case "deletetype":
                        {
                            return DeleteType(serviceImpl, repositoryId, token, multipart);
                        }
                    case "query":
                        {
                            return Query(serviceImpl, repositoryId, token, multipart);
                        }
                    case "updatetype":
                        {
                            return UpdateType(serviceImpl, repositoryId, token, multipart);
                        }

                    default:
                        {
                            throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'cmisaction' is not valid.", false), serviceImpl);
                        }
                }
            }
            catch (Exception ex)
            {
                return SerializeException(ex, repositoryId, token);
            }
        }
        #endregion

        #region 5.3.3 Root Folder URL (ObjectById)
        public System.IO.Stream DispatchWebGetRootFolder(string repositoryId, string objectId)
        {
            try
            {
                var serviceImpl = CmisServiceImpl;
                string cmisselector = CommonFunctions.GetRequestParameter("cmisselector");
                var succinct = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("succinct"));

                CheckTokenAuthentication(null);
                // select default selector, if selector is not specified (chapter 5.4  Services)
                if (string.IsNullOrEmpty(cmisselector))
                {
                    switch (serviceImpl.GetBaseObjectType(repositoryId, objectId))
                    {
                        case cc.enumBaseObjectTypeIds.cmisDocument:
                            {
                                cmisselector = "content";
                                break;
                            }
                        case cc.enumBaseObjectTypeIds.cmisFolder:
                            {
                                cmisselector = "children";
                                break;
                            }

                        default:
                            {
                                cmisselector = "object";
                                break;
                            }
                    }
                }
                switch (cmisselector.ToLowerInvariant() ?? "")
                {
                    case "acl":
                        {
                            return GetACL(serviceImpl, repositoryId, objectId);
                        }
                    case "allowableactions":
                        {
                            return GetAllowableActions(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "checkedout":
                        {
                            return GetCheckedOutDocs(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), succinct);
                        }
                    case "children":
                        {
                            return GetChildren(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "content":
                        {
                            return GetContentStream(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "descendants":
                        {
                            return GetDescendants(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "foldertree":
                        {
                            return GetFolderTree(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "object":
                        {
                            return GetObject(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "parent":
                        {
                            return GetFolderParent(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "parents":
                        {
                            return GetObjectParents(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "policies":
                        {
                            return GetAppliedPolicies(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "properties":
                        {
                            return GetProperties(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "relationships":
                        {
                            return GetObjectRelationships(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "renditions":
                        {
                            return GetRenditions(serviceImpl, repositoryId, objectId, succinct);
                        }
                    case "versions":
                        {
                            return GetAllVersions(serviceImpl, repositoryId, objectId, succinct);
                        }

                    default:
                        {
                            throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'cmisaction' is not valid.", false), serviceImpl);
                        }
                }
            }
            catch (Exception ex)
            {
                return SerializeException(ex);
            }
        }

        public System.IO.Stream DispatchWebPostRootFolder(string repositoryId, string objectId, System.IO.Stream stream)
        {
            TokenType token = null;

            try
            {
                var multipart = JSON.MultipartFormDataContent.FromStream(stream, ssw.WebOperationContext.Current.IncomingRequest.ContentType);
                var serviceImpl = CmisServiceImpl;
                string cmisaction = multipart.ToString("cmisaction") ?? string.Empty;
                var succinct = CommonFunctions.ParseBoolean(multipart.ToString("succinct"));

                token = CheckTokenAuthentication(multipart);
                if (objectId is null)
                    objectId = CommonFunctions.GetRequestParameter("id");
                switch (cmisaction.ToLowerInvariant() ?? "")
                {
                    case "addobjecttofolder":
                        {
                            return AddObjectToFolder(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "appendcontent":
                        {
                            return AppendContentStream(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "applyacl":
                        {
                            return ApplyACL(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "applypolicy":
                        {
                            return ApplyPolicy(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "cancelcheckout":
                        {
                            return CancelCheckOut(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "checkin":
                        {
                            return CheckIn(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "checkout":
                        {
                            return CheckOut(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "createdocument":
                        {
                            return CreateDocument(CmisServiceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "createdocumentfromsource":
                        {
                            return CreateDocumentFromSource(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "createfolder":
                        {
                            return CreateFolder(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "createitem":
                        {
                            return CreateItem(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "createpolicy":
                        {
                            return CreatePolicy(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "delete":
                        {
                            return DeleteObject(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "deletecontent":
                        {
                            return DeleteContentStream(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "deletetree":
                        {
                            return DeleteTree(serviceImpl, repositoryId, GetFolderId(serviceImpl, repositoryId, objectId), token, multipart);
                        }
                    case "move":
                        {
                            return MoveObject(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "removeobjectfromfolder":
                        {
                            return RemoveObjectFromFolder(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "removepolicy":
                        {
                            return RemovePolicy(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "setcontent":
                        {
                            return SetContentStream(serviceImpl, repositoryId, objectId, token, multipart);
                        }
                    case "update":
                        {
                            return UpdateProperties(serviceImpl, repositoryId, objectId, token, multipart);
                        }

                    default:
                        {
                            throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'cmisaction' is not valid.", false), serviceImpl);
                        }
                }
            }
            catch (Exception ex)
            {
                return SerializeException(ex, repositoryId, token);
            }
        }
        #endregion

        #region 5.3.4 Object URL (ObjectByPath)
        public System.IO.Stream DispatchWebGetObjects(string repositoryId, string path)
        {
            return DispatchWebGetRootFolder(repositoryId, CmisServiceImpl.GetObjectId(repositoryId, "/" + path) ?? string.Empty);
        }

        public System.IO.Stream DispatchWebPostObjects(string repositoryId, string path, System.IO.Stream stream)
        {
            return DispatchWebPostRootFolder(repositoryId, CmisServiceImpl.GetObjectId(repositoryId, path) ?? string.Empty, stream);
        }
        #endregion

        #region Repository
        /// <summary>
      /// Creates a new type
      /// </summary>
        private System.IO.MemoryStream CreateType(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            var newType = data.GetTypeDefinition();

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (newType is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("type"), serviceImpl);

            result = serviceImpl.CreateType(repositoryId, newType);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Deletes a type definition
      /// </summary>
        private System.IO.MemoryStream DeleteType(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            Exception failure;
            string typeId = data.ToString("typeId");

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(typeId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("typeId"), serviceImpl);

            failure = serviceImpl.DeleteType(repositoryId, typeId);
            if (failure is null)
            {
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = typeId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return null;
                }
            }

            // failure
            throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Gets the transaction instance of the POST request identified by token
      /// </summary>
        private System.IO.MemoryStream GetLastResult(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            string token = CommonFunctions.GetRequestParameter("token") ?? string.Empty;
            var transaction = get_Transaction(token);

            if (transaction is null)
            {
                // create fault transaction (see chapter 5.4.4.4  Access to Form Response Content)
                transaction = new JSON.Transaction() { Code = 0L, Exception = "invalidArgument", Message = "The parameter 'token' is not valid.", ObjectId = null };
            }
            else
            {
                // remove cookie
                set_Transaction(repositoryId, token, null);
            }

            return SerializeXmlSerializable(transaction);
        }

        /// <summary>
      /// Returns all available repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private new System.IO.MemoryStream GetRepositories()
        {
            try
            {
                return GetRepositories(SerializeRepositories);
            }
            finally
            {
                // sending a token signals enabling authentication with tokens
                EnableTokenAuthentication();
            }
        }

        /// <summary>
      /// Returns the specified repository
      /// </summary>
        private new System.IO.MemoryStream GetRepositoryInfo(string repositoryId)
        {
            try
            {
                return GetRepositoryInfo(repositoryId, SerializeRepositories);
            }
            finally
            {
                // sending a token signals enabling authentication with tokens
                EnableTokenAuthentication();
            }
        }

        /// <summary>
      /// Returns all child types of the specified type, if defined, otherwise the basetypes of the repository.
      /// </summary>
        private System.IO.MemoryStream GetTypeChildren(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            ccg.Result<cm.cmisTypeDefinitionListType> result;
            // get the optional parameters from the queryString
            string typeId = CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.typeId) ?? CommonFunctions.GetRequestParameter("id");
            var includePropertyDefinitions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.includePropertyDefinitions));
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.skipCount));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            result = serviceImpl.GetTypeChildren(repositoryId, typeId, includePropertyDefinitions.HasValue && includePropertyDefinitions.Value, maxItems, skipCount);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(result.Success);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the type-definition of the specified type
      /// </summary>
        private System.IO.MemoryStream GetTypeDefinition(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            string typeId = CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.typeId) ?? CommonFunctions.GetRequestParameter("id");

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(typeId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("typeId"), serviceImpl);
            result = serviceImpl.get_TypeDefinition(repositoryId, typeId);

            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(result.Success);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the descendant object-types under the specified type.
      /// </summary>
        private System.IO.MemoryStream GetTypeDescendants(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            ccg.Result<cm.cmisTypeContainer> result;
            string typeId = CommonFunctions.GetRequestParameter(ServiceURIs.enumTypeDescendantsUri.typeId) ?? CommonFunctions.GetRequestParameter("id");
            var depth = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypeDescendantsUri.depth));
            var includePropertyDefinitions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypeDescendantsUri.includePropertyDefinitions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (depth.HasValue && (depth.Value == 0L || depth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'depth' MUST NOT be 0 or less than -1", false), serviceImpl);

            result = serviceImpl.GetTypeDescendants(repositoryId, typeId, includePropertyDefinitions.HasValue && includePropertyDefinitions.Value, depth);

            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var typeContainer = result.Success;

                if (typeContainer.Type is null)
                {
                    // no typeId defined
                    return SerializeArray(typeContainer.Children);
                }
                else
                {
                    // typeId defined
                    return SerializeArray(new cm.cmisTypeContainer[] { typeContainer });
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Updates a type definition
      /// </summary>
        private System.IO.MemoryStream UpdateType(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            var type = data.GetTypeDefinition();

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (type is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("type"), serviceImpl);

            result = serviceImpl.UpdateType(repositoryId, type);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.OK);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Returns a list of check out object the user has access to.
      /// </summary>
        private System.IO.MemoryStream GetCheckedOutDocs(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, bool? succinct)
        {
            ccg.Result<cmisObjectListType> result;
            // get the optional parameters from the queryString
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.skipCount));
            string orderBy = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.orderBy);
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.renditionFilter);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            result = serviceImpl.GetCheckedOutDocs(repositoryId, folderId, filter, maxItems, skipCount, renditionFilter, includeAllowableActions, includeRelationships);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(new cm.Responses.getContentChangesResponse() { Objects = result.Success }, succinct: succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns all children of the specified CMIS object.
      /// </summary>
        private System.IO.MemoryStream GetChildren(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, bool? succinct)
        {
            ccg.Result<cmisObjectInFolderListType> result;
            // get optional parameters from the queryString
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.skipCount));
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.renditionFilter);
            string orderBy = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.orderBy);
            var includePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includePathSegment));

            if (string.IsNullOrEmpty(folderId))
                folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            result = serviceImpl.GetChildren(repositoryId, folderId, maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy, includePathSegment.HasValue && includePathSegment.Value);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable<cm.cmisObjectInFolderListType>(result.Success, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the descendant objects contained in the specified folder or any of its child-folders.
      /// </summary>
        private System.IO.MemoryStream GetDescendants(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, bool? succinct)
        {
            ccg.Result<cmisObjectInFolderContainerType> result;
            string filter = CommonFunctions.GetRequestParameter("filter");
            var depth = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("depth"));
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeAllowableActions"));
            var includePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includePathSegment"));
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter("includeRelationships"));
            string renditionFilter = CommonFunctions.GetRequestParameter("renditionFilter");

            if (string.IsNullOrEmpty(folderId))
                folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (depth.HasValue && (depth.Value == 0L || depth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'depth' MUST NOT be 0 or less than -1", false), serviceImpl);

            result = serviceImpl.GetDescendants(repositoryId, folderId, filter, depth, includeAllowableActions, includeRelationships, renditionFilter, includePathSegment.HasValue && includePathSegment.Value);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeArray((result.Success ?? new cmisObjectInFolderContainerType()).Children, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the parent folder-object of the specified folder
      /// </summary>
        private System.IO.MemoryStream GetFolderParent(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, bool? succinct)
        {
            ccg.Result<cmisObjectType> result;
            string filter = CommonFunctions.GetRequestParameter("filter");

            if (string.IsNullOrEmpty(folderId))
                folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            result = serviceImpl.GetFolderParent(repositoryId, folderId, filter);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable<cc.cmisObjectType>(result.Success, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the descendant folders contained in the specified folder
      /// </summary>
        private System.IO.MemoryStream GetFolderTree(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, bool? succinct)
        {
            ccg.Result<cmisObjectInFolderContainerType> result;
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.filter);
            var depth = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.depth));
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includeRelationships));
            var includePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includePathSegment));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.renditionFilter);

            if (string.IsNullOrEmpty(folderId))
                folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (depth.HasValue && (depth.Value == 0L || depth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("depth"), serviceImpl);

            result = serviceImpl.GetFolderTree(repositoryId, folderId, filter, depth, includeAllowableActions, includeRelationships, includePathSegment.HasValue && includePathSegment.Value, renditionFilter);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeArray<cm.cmisObjectInFolderContainerType>((result.Success ?? new cmisObjectInFolderContainerType()).Children, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the parent folders for the specified object
      /// </summary>
        private System.IO.MemoryStream GetObjectParents(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectParentsType[]> result;
            string filter = CommonFunctions.GetRequestParameter("filter");
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeAllowableActions"));
            var includeRelativePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeRelativePathSegment"));
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter("includeRelationships"));
            string renditionFilter = CommonFunctions.GetRequestParameter("renditionFilter");

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = CmisServiceImpl.GetObjectParents(repositoryId, objectId, filter, includeAllowableActions, includeRelationships, renditionFilter, includeRelativePathSegment);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var parents = result.Success is null ? null : (from parent in result.Success
                                                               select parent).ToArray();
                return SerializeArray(parents, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the specified document object.
      /// </summary>
        private System.IO.MemoryStream AppendContentStream(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cm.Responses.setContentStreamResponse> result;
            var isLastChunk = CommonFunctions.ParseBoolean(data.ToString("isLastChunk"));
            string changeToken = data.ToString("changeToken");
            var httpContent = data.get_Content("content");
            var content = httpContent is null ? null : httpContent.Value;
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (content is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("content"), serviceImpl);

            string argdisposition = "";
            result = serviceImpl.AppendContentStream(repositoryId, objectId, new System.IO.MemoryStream(content) { Position = 0L }, httpContent.Headers[RFC2231Helper.ContentTypeHeaderName], RFC2231Helper.DecodeContentDisposition(httpContent.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition), isLastChunk.HasValue && isLastChunk.Value, changeToken);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = result.Success.ObjectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return GetObject(serviceImpl, repositoryId, result.Success.ObjectId, succinct);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
        private System.IO.MemoryStream BulkUpdateProperties(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectListType> result;
            var changeTokens = data.GetAutoIndexedValues(JSON.Enums.enumValueType.changeToken);
            var objectIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.objectId);
            int length = changeTokens is null ? 0 : changeTokens.Length;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (changeTokens is null || objectIds is null || length == 0 || objectIds.Length != length)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectIdAndChangeToken"), serviceImpl);
            }

            var bulkUpdate = new cc.cmisBulkUpdateType()
            {
                AddSecondaryTypeIds = data.GetSecondaryTypeIds(JSON.Enums.enumCollectionAction.add),
                Properties = data.GetProperties(typeId => serviceImpl.get_TypeDefinition(repositoryId, typeId).Success),
                RemoveSecondaryTypeIds = data.GetSecondaryTypeIds(JSON.Enums.enumCollectionAction.remove)
            };
            var objectIdAndChangeTokens = new List<cc.cmisObjectIdAndChangeTokenType>(length);

            for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                objectIdAndChangeTokens.Add(new cc.cmisObjectIdAndChangeTokenType() { ChangeToken = changeTokens[index], Id = objectIds[index] });
            bulkUpdate.ObjectIdAndChangeTokens = objectIdAndChangeTokens.ToArray();

            result = serviceImpl.BulkUpdateProperties(repositoryId, bulkUpdate);

            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                // GetFormResponse() cannot be used because bulkUpdateProperties returns an array of cmisObjectIdAndChangeTokenType, not a single cmisObject
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = null };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    var list = result.Success ?? new cmisObjectListType();
                    var objects = list.Objects ?? (new cmisObjectType[] { });

                    return SerializeArray((from obj in objects
                                           select new cc.cmisObjectIdAndChangeTokenType()
                                           {
                                               ChangeToken = obj.ChangeToken,
                                               Id = obj.ObjectId,
                                               NewId = obj.BulkUpdateProperties.NewId
                                           }).ToArray());
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a new document in the specified folder or as unfiled document
      /// </summary>
        private System.IO.MemoryStream CreateDocument(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            bool hasProperties = false;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;
            var httpContent = data.get_Content("content");
            string argdisposition = "";
            var content = httpContent is null ? null : new cm.cmisContentStreamType(new System.IO.MemoryStream(httpContent.Value ?? (new byte[] { })) { Position = 0L }, RFC2231Helper.DecodeContentDisposition(httpContent.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition), httpContent.Headers[RFC2231Helper.ContentTypeHeaderName]);
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId =>
               {
                   // at least the cmis:objectTypeId MUST be set
                   hasProperties = true;
                   return serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
               });
            var versioningState = CommonFunctions.ParseEnum<cc.enumVersioningState>(data.ToString("versioningState"));
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (!hasProperties)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.CreateDocument(repositoryId, new cc.cmisObjectType() { Properties = properties, PolicyIds = new ccc.cmisListOfIdsType(policyIds) }, folderId, content, versioningState, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
      /// </summary>
      /// <remarks>In chapter 5.4.2.7 Action "createDocumentFromSource" the listed relevant CMIS controls contains "Content",
      /// but there is no equivalent in chapter 2.2.4.2  createDocumentFromSource. Therefore content is ignored.</remarks>
        private System.IO.MemoryStream CreateDocumentFromSource(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId => serviceImpl.get_TypeDefinition(repositoryId, typeId).Success);
            string sourceId = data.ToString("sourceId");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));
            var versioningState = CommonFunctions.ParseEnum<cc.enumVersioningState>(data.ToString("versioningState"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(sourceId) || !CmisServiceImpl.get_Exists(repositoryId, sourceId))
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("sourceId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }

            result = serviceImpl.CreateDocumentFromSource(repositoryId, sourceId, properties, folderId, versioningState, policyIds, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a folder object of the specified type in the specified location
      /// </summary>
        private System.IO.MemoryStream CreateFolder(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string parentFolderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            bool hasProperties = false;
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId =>
               {
                   // at least the cmis:objectTypeId MUST be set
                   hasProperties = true;
                   return serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
               });
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(parentFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (!hasProperties)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.CreateFolder(repositoryId, new cc.cmisObjectType() { Properties = properties, PolicyIds = new ccc.cmisListOfIdsType(policyIds) }, parentFolderId, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates an item object of the specified type in the specified location
      /// </summary>
        private System.IO.MemoryStream CreateItem(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            bool hasProperties = false;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId =>
               {
                   // at least the cmis:objectTypeId MUST be set
                   hasProperties = true;
                   return serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
               });
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (!hasProperties)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.CreateItem(repositoryId, new cc.cmisObjectType() { Properties = properties, PolicyIds = new ccc.cmisListOfIdsType(policyIds) }, folderId, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a policy object of the specified type in the specified location
      /// </summary>
        private System.IO.MemoryStream CreatePolicy(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            bool hasProperties = false;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId =>
               {
                   // at least the cmis:objectTypeId MUST be set
                   hasProperties = true;
                   return serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
               });
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (!hasProperties)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.CreatePolicy(repositoryId, new cc.cmisObjectType() { Properties = properties, PolicyIds = new ccc.cmisListOfIdsType(policyIds) }, folderId, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a relationship object of the specified type
      /// </summary>
        private System.IO.MemoryStream CreateRelationship(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            bool hasProperties = false;
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var properties = data.GetProperties(typeId =>
               {
                   // at least the cmis:objectTypeId MUST be set
                   hasProperties = true;
                   return serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
               });
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (!hasProperties)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.CreateRelationship(repositoryId, new cc.cmisObjectType() { Properties = properties, PolicyIds = new ccc.cmisListOfIdsType(policyIds) }, data.GetACEs(JSON.Enums.enumCollectionAction.add), data.GetACEs(JSON.Enums.enumCollectionAction.remove));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Deletes the content stream of a cmis document
      /// </summary>
        private System.IO.MemoryStream DeleteContentStream(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cm.Responses.deleteContentStreamResponse> result;
            string changeToken = data.ToString("changeToken");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.DeleteContentStream(repositoryId, objectId, changeToken);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = result.Success.ObjectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return GetObject(serviceImpl, repositoryId, result.Success.ObjectId, succinct);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Removes the submitted document
      /// </summary>
        private System.IO.MemoryStream DeleteObject(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            Exception failure;
            var allVersions = CommonFunctions.ParseBoolean(data.ToString("allVersions"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            failure = serviceImpl.DeleteObject(repositoryId, objectId, !allVersions.HasValue || allVersions.Value);
            if (failure is null)
            {
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = objectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return null;
                }
            }

            // failure
            throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects.
      /// </summary>
        private System.IO.MemoryStream DeleteTree(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cm.Responses.deleteTreeResponse> result;
            var allVersion = CommonFunctions.ParseBoolean(data.ToString("allVersions"));
            var unfileObjects = CommonFunctions.ParseEnum<cc.enumUnfileObject>(data.ToString("unfileObjects"));
            var continueOnFailure = CommonFunctions.ParseBoolean(data.ToString("continueOnFailure"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            result = serviceImpl.DeleteTree(repositoryId, folderId, !allVersion.HasValue || allVersion.Value, unfileObjects, continueOnFailure.HasValue && continueOnFailure.Value);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var objectIds = result.Success is null || result.Success.FailedToDelete is null ? null : result.Success.FailedToDelete.ObjectIds;

                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                if (objectIds is null || objectIds.Length == 0)
                {
                    return null;
                }
                else
                {
                    return SerializeXmlSerializable(new ccc.cmisListOfIdsType(objectIds));
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the allowable actions for the specified document.
      /// </summary>
        private System.IO.MemoryStream GetAllowableActions(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cc.cmisAllowableActionsType> result;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetAllowableActions(repositoryId, objectId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(result.Success, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the content stream of the specified object.
      /// </summary>
        private System.IO.MemoryStream GetContentStream(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cm.cmisContentStreamType> result;
            string streamId = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.streamId);
            string download = CommonFunctions.GetRequestParameter("download");

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetContentStream(repositoryId, objectId, streamId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var contentStream = result.Success;

                if (contentStream is null)
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return null;
                }
                else
                {
                    string fileName = string.IsNullOrEmpty(contentStream.Filename) ? "NotSet" : contentStream.Filename;
                    var binaryStream = contentStream.BinaryStream;

                    download = string.Compare("attachment", download, true) == 0 ? "attachment" : "inline";
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = contentStream.StatusCode;
                    ssw.WebOperationContext.Current.OutgoingResponse.ContentType = contentStream.MimeType;
                    ssw.WebOperationContext.Current.OutgoingResponse.Headers.Add(RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(fileName, download));
                    if (binaryStream is System.IO.MemoryStream)
                    {
                        binaryStream.Position = 0L;
                        return (System.IO.MemoryStream)binaryStream;
                    }
                    else
                    {
                        var retVal = new System.IO.MemoryStream();

                        contentStream.BinaryStream.CopyTo(retVal);
                        retVal.Position = 0L;
                        return retVal;
                    }
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the cmisobject with the specified id.
      /// </summary>
      /// <remarks>Method supports privateWorkingCopy-parameter, not defined in chapter 5.4.3.13 Selector "object".
      /// Implements the services GetObject(), GetObjectByPath() and GetObjectOfLatest().</remarks>
        private System.IO.MemoryStream GetObject(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectType> result;
            // optional parameters from the queryString
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.filter);
            var includeRelationships = CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeRelationships));
            var includePolicyIds = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includePolicyIds));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.renditionFilter);
            var includeACL = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeACL));
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeAllowableActions));
            var returnVersion = CommonFunctions.ParseEnum<cr.enumReturnVersion>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.returnVersion));
            var major = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.major));
            var privateWorkingCopy = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.pwc));

            // getObjectOfLatestVersion: parameter versionSeriesId is used instead of objectId and parameter major instead of returnVersion
            if (string.IsNullOrEmpty(objectId))
            {
                objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.versionSeriesId);
                if (!string.IsNullOrEmpty(objectId))
                {
                    returnVersion = major.HasValue && major.Value ? cr.enumReturnVersion.latestmajor : cr.enumReturnVersion.latest;
                }
            }
            else if (!returnVersion.HasValue && major.HasValue)
            {
                returnVersion = major.Value ? cr.enumReturnVersion.latestmajor : cr.enumReturnVersion.latest;
            }
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetObject(repositoryId, objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, returnVersion, privateWorkingCopy);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable<cc.cmisObjectType>(result.Success, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        /// <summary>
      /// Returns the object at the specified path
      /// </summary>
      /// <remarks>Using the browser binding every service-call that targets an object can be made using the objectId queryString-parameter
      /// or the path of that object. That means not only the GetObject() method has a pendant using the path of the object, but all services
      /// addressing an object can be called using the objectId-parameter or the path of the object. So a special GetObject()/GetObjectByPath()-
      /// couple is obsolete in the browser binding.</remarks>
        [Obsolete("Use GetObject instead.", true)]
        private System.IO.MemoryStream GetObjectByPath(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            return GetObject(serviceImpl, repositoryId, objectId, succinct);
        }

        /// <summary>
      /// Get the properties of an object.
      /// </summary>
      /// <remarks>Implements the services GetProperties() and GetPropertiesOfLatestVersion().</remarks>
        private System.IO.MemoryStream GetProperties(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectType> result;
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.filter);
            var returnVersion = CommonFunctions.ParseEnum<cr.enumReturnVersion>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.returnVersion));
            var major = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.major));

            // getPropertiesOfLatestVersion: parameter versionSeriesId is used instead of objectId and parameter major instead of returnVersion
            if (string.IsNullOrEmpty(objectId))
            {
                objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.versionSeriesId);
                if (!string.IsNullOrEmpty(objectId))
                {
                    returnVersion = major.HasValue && major.Value ? cr.enumReturnVersion.latestmajor : cr.enumReturnVersion.latest;
                }
            }
            else if (!returnVersion.HasValue && major.HasValue)
            {
                returnVersion = major.Value ? cr.enumReturnVersion.latestmajor : cr.enumReturnVersion.latest;
            }
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetObject(repositoryId, objectId, filter, cc.enumIncludeRelationships.none, false, "cmis:none", false, false, returnVersion, default(bool?));
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var properties = result.Success is null ? null : result.Success.Properties;
                return SerializeXmlSerializable(properties, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Get the properties of an object.
      /// </summary>
        private System.IO.MemoryStream GetRenditions(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectType> result;
            // optional parameters from the queryString
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.renditionFilter);
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("maxItems"));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("skipCount"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetObject(repositoryId, objectId, CmisPredefinedPropertyNames.ObjectId, cc.enumIncludeRelationships.none, false, renditionFilter, false, false, default, default);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var renditions = result.Success is null ? null : result.Success.Renditions;

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
                        renditions = new cc.cmisRenditionType[] { };
                    }
                    else
                    {
                        long newLength = Math.Min(length - nSkipCount, nMaxItems);
                        cc.cmisRenditionType[] cutout = (cc.cmisRenditionType[])Array.CreateInstance(typeof(cc.cmisRenditionType), newLength);
                        Array.Copy(renditions, nSkipCount, cutout, 0L, newLength);
                        renditions = cutout;
                    }
                }
                return SerializeArray(renditions, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
        private System.IO.MemoryStream MoveObject(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            string targetFolderId = data.ToString("targetFolderId");
            string sourceFolderId = data.ToString("sourceFolderId");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(targetFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("targetFolderId"), serviceImpl);
            if (string.IsNullOrEmpty(sourceFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("sourceFolderId"), serviceImpl);

            result = serviceImpl.MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Sets the content stream of the specified object.
      /// </summary>
        private System.IO.MemoryStream SetContentStream(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cm.Responses.setContentStreamResponse> result;
            var overwriteFlag = CommonFunctions.ParseBoolean(data.ToString("overwriteFlag"));
            string changeToken = data.ToString("changeToken");
            var httpContent = data.get_Content("content");
            var content = httpContent is null ? null : httpContent.Value;
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (content is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("content"), serviceImpl);

            string argdisposition = "";
            result = serviceImpl.SetContentStream(repositoryId, objectId, new System.IO.MemoryStream(content) { Position = 0L }, httpContent.Headers[RFC2231Helper.ContentTypeHeaderName], RFC2231Helper.DecodeContentDisposition(httpContent.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition), !overwriteFlag.HasValue || overwriteFlag.Value, changeToken);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = result.Success.ObjectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return GetObject(serviceImpl, repositoryId, result.Success.ObjectId, succinct);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Updates the submitted cmis-object
      /// </summary>
        private System.IO.MemoryStream UpdateProperties(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            string changeToken = data.ToString("changeToken");
            ccdt.cmisTypeDefinitionType[] typeDefinitions;
            ccc.cmisPropertiesType properties;
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            typeDefinitions = GetTypeDefinitions(serviceImpl, repositoryId, objectId);
            properties = data.GetProperties(typeId => serviceImpl.get_TypeDefinition(repositoryId, typeId).Success, typeDefinitions);
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (properties is null || properties.Count == 0)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            result = serviceImpl.UpdateProperties(repositoryId, objectId, properties, changeToken);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, result.Success is null || result.Success.ObjectId != objectId ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.OK, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder.
      /// </summary>
        private System.IO.MemoryStream AddObjectToFolder(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            string folderId = data.ToString("folderId");
            var allVersions = CommonFunctions.ParseBoolean(data.ToString("allVersions") ?? "true");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            result = serviceImpl.AddObjectToFolder(repositoryId, objectId, folderId, !allVersions.HasValue || allVersions.Value);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder.
      /// </summary>
        private System.IO.MemoryStream RemoveObjectFromFolder(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            string folderId = data.ToString("folderId");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.RemoveObjectFromFolder(repositoryId, objectId, folderId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Discovery
        /// <summary>
      /// Returns a list of content changes
      /// </summary>
      /// <remarks>Filter parameter is not specified in the cmis documentation for browser binding, but included in the corresponding
      /// definition in the atompub binding. Therefore the browser binding supports the filter parameter as well.</remarks>
        private System.IO.MemoryStream GetContentChanges(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            ccg.Result<getContentChanges> result;
            string changeLogToken = CommonFunctions.GetRequestParameter("changeLogToken");
            string filter = CommonFunctions.GetRequestParameter("filter");
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("maxItems"));
            var includeACL = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeACL"));
            var includePolicyIds = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includePolicyIds"));
            var includeProperties = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeProperties"));
            var succinct = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            result = serviceImpl.GetContentChanges(repositoryId, filter, maxItems, includeACL, includePolicyIds.HasValue && includePolicyIds.Value, includeProperties.HasValue && includeProperties.Value, ref changeLogToken);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable<cm.Responses.getContentChangesResponse>(result.Success, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the data described by the specified CMIS query. (GET Request)
      /// </summary>
        private System.IO.MemoryStream Query(Contracts.ICmisServicesImpl serviceImpl, string repositoryId)
        {
            return this.Query(serviceImpl, repositoryId, null, CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("succinct")), CommonFunctions.GetRequestParameter("q") ?? CommonFunctions.GetRequestParameter("statement"), CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("searchAllVersions")), CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(CommonFunctions.GetRequestParameter("includeRelationships")), CommonFunctions.GetRequestParameter("renditionFilter"), CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("includeAllowableActions")), CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("maxItems")), CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter("skipCount")));
        }

        /// <summary>
      /// Returns the data described by the specified CMIS query. (POST Request)
      /// </summary>
        private System.IO.MemoryStream Query(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, JSON.MultipartFormDataContent data)
        {
            return this.Query(serviceImpl, repositoryId, token, CommonFunctions.ParseBoolean(data.ToString("succinct")), data.ToString("statement") ?? data.ToString("q"), CommonFunctions.ParseBoolean(data.ToString("searchAllVersions")), CommonFunctions.ParseEnum<cc.enumIncludeRelationships>(data.ToString("includeRelationships")), data.ToString("renditionFilter"), CommonFunctions.ParseBoolean(data.ToString("includeAllowableActions")), CommonFunctions.ParseInteger(data.ToString("maxItems")), CommonFunctions.ParseInteger(data.ToString("skipCount")));
        }
        private System.IO.MemoryStream Query(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, TokenType token, bool? succinct, string q, bool? searchAllVersions, cc.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeAllowableActions, long? maxItems, long? skipCount)
        {
            ccg.Result<cmisObjectListType> result;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(q))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("statement"), serviceImpl);
            result = serviceImpl.Query(repositoryId, q, searchAllVersions.HasValue && searchAllVersions.Value, includeRelationships, renditionFilter, includeAllowableActions.HasValue && includeAllowableActions.Value, maxItems, skipCount);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                // GetFormResponse() cannot be used because bulkUpdateProperties returns an array of cmisObjectIdAndChangeTokenType, not a single cmisObject
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = null };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return SerializeXmlSerializable<cm.cmisObjectListType>(result.Success, succinct);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents in the version series to be checked out again.
      /// If the private working copy has been created by createDocument, cancelCheckOut MUST delete the created document.
      /// </summary>
        private System.IO.MemoryStream CancelCheckOut(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            Exception failure;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            failure = serviceImpl.CancelCheckOut(repositoryId, objectId);
            if (failure is null)
            {
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return null;
            }
            else if (!IsWebException(failure))
            {
                failure = cm.cmisFaultType.CreateUnknownException(failure);
            }

            // failure
            throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Checks-in the Private Working Copy document.
      /// </summary>
        private System.IO.MemoryStream CheckIn(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            ccc.cmisPropertiesType properties;
            ccdt.cmisTypeDefinitionType[] typeDefinitions;
            var major = CommonFunctions.ParseBoolean(data.ToString("major"));
            var httpContent = data.get_Content("content");
            string argdisposition = "";
            var content = httpContent is null ? null : new cm.cmisContentStreamType(new System.IO.MemoryStream(httpContent.Value ?? (new byte[] { })) { Position = 0L }, RFC2231Helper.DecodeContentDisposition(httpContent.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition), httpContent.Headers[RFC2231Helper.ContentTypeHeaderName]);
            string checkInComment = data.ToString("checkInComment");
            var policyIds = data.GetAutoIndexedValues(JSON.Enums.enumValueType.policy);
            var addACEs = data.GetACEs(JSON.Enums.enumCollectionAction.add);
            var removeACEs = data.GetACEs(JSON.Enums.enumCollectionAction.remove);
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            typeDefinitions = GetTypeDefinitions(serviceImpl, repositoryId, objectId);
            properties = data.GetProperties(typeId => serviceImpl.get_TypeDefinition(repositoryId, typeId).Success, typeDefinitions);
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.CheckIn(repositoryId, objectId, properties, policyIds, content, !major.HasValue || major.Value, checkInComment, addACEs, removeACEs);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Checks out the specified CMIS object.
      /// </summary>
        private System.IO.MemoryStream CheckOut(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.CheckOut(repositoryId, objectId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.Created, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns all Documents in the specified version series.
      /// </summary>
      /// <param name="serviceImpl"></param>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="succinct"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private System.IO.MemoryStream GetAllVersions(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectListType> result;
            string versionSeriesId = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.versionSeriesId);
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.includeAllowableActions));

            if (string.IsNullOrEmpty(objectId))
                objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.versionSeriesId);
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId) && string.IsNullOrEmpty(versionSeriesId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetAllVersions(repositoryId, objectId, versionSeriesId, filter, includeAllowableActions);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var objects = result.Success is null ? null : result.Success.Objects;
                return SerializeArray(objects, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
        [Obsolete("Use GetObject instead.", true)]
        private System.IO.MemoryStream GetObjectOfLatestVersion(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            return GetObject(serviceImpl, repositoryId, objectId, succinct);
        }

        /// <summary>
      /// Get a subset of the properties for the latest document object in the version series.
      /// </summary>
        [Obsolete("Use GetProperties instead.", true)]
        private System.IO.MemoryStream GetPropertiesOfLatestVersion(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            return GetProperties(serviceImpl, repositoryId, objectId, succinct);
        }
        #endregion

        #region Relationships
        /// <summary>
      /// Returns the relationships for the specified object.
      /// </summary>
        private System.IO.MemoryStream GetObjectRelationships(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectListType> result;
            var includeSubRelationshipTypes = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.includeSubRelationshipTypes));
            var relationshipDirection = CommonFunctions.ParseEnum<cc.enumRelationshipDirection>(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.relationshipDirection));
            string typeId = CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.typeId);
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.skipCount));
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.includeAllowableActions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetObjectRelationships(repositoryId, objectId, includeSubRelationshipTypes.HasValue && includeSubRelationshipTypes.Value, relationshipDirection, typeId, maxItems, skipCount, filter, includeAllowableActions);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(new cm.Responses.getContentChangesResponse() { Objects = result.Success }, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Policy
        /// <summary>
      /// Applies a policy to the specified object.
      /// </summary>
        private System.IO.MemoryStream ApplyPolicy(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cmisObjectType> result;
            string policyId = data.ToString("policyId");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(policyId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("policyId"), serviceImpl);

            result = serviceImpl.ApplyPolicy(repositoryId, objectId, policyId);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return GetFormResponse(result.Success, repositoryId, token, System.Net.HttpStatusCode.OK, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns a list of policies applied to the specified object.
      /// </summary>
        private System.IO.MemoryStream GetAppliedPolicies(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, bool? succinct)
        {
            ccg.Result<cmisObjectListType> result;
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.filter);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetAppliedPolicies(repositoryId, objectId, filter);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                var objects = result.Success is null ? null : result.Success.Objects;
                return SerializeArray(objects, succinct);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Removes a policy from the specified object.
      /// </summary>
        private System.IO.MemoryStream RemovePolicy(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            Exception failure;
            string policyId = data.ToString("policyId");
            var succinct = CommonFunctions.ParseBoolean(data.ToString("succinct"));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(policyId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("policyId"), serviceImpl);

            failure = serviceImpl.RemovePolicy(repositoryId, objectId, policyId);
            if (failure is null)
            {
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = objectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return GetObject(serviceImpl, repositoryId, objectId, succinct);
                }
            }
            else
            {
                // failure
                throw LogException(failure, serviceImpl);
            }
        }
        #endregion

        #region ACL
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of document or folder object.
      /// </summary>
        private System.IO.MemoryStream ApplyACL(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId, TokenType token, JSON.MultipartFormDataContent data)
        {
            ccg.Result<cc.Security.cmisAccessControlListType> result;
            var aclPropagation = data.GetACLPropagation();
            var addACEs = data.GetACEs(JSON.Enums.enumCollectionAction.add);
            var removeACEs = data.GetACEs(JSON.Enums.enumCollectionAction.remove);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.ApplyACL(repositoryId, objectId, addACEs, removeACEs, aclPropagation.HasValue ? aclPropagation.Value : cc.enumACLPropagation.repositorydetermined);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                if (IsHtmlPageRequired(token))
                {
                    var transaction = new JSON.Transaction() { Code = (long)System.Net.HttpStatusCode.OK, Exception = null, Message = null, ObjectId = objectId };

                    set_Transaction(repositoryId, token, transaction);
                    return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                }
                else
                {
                    return SerializeXmlSerializable(result.Success);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Get the ACL currently applied to the specified document or folder object.
      /// </summary>
        private System.IO.MemoryStream GetACL(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId)
        {
            ccg.Result<cc.Security.cmisAccessControlListType> result;
            var onlyBasicPermissions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumACLUri.onlyBasicPermissions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            result = serviceImpl.GetACL(repositoryId, objectId, !onlyBasicPermissions.HasValue || onlyBasicPermissions.Value);
            if (result is null)
            {
                result = cm.cmisFaultType.CreateUnknownException();
            }
            else if (result.Failure is null)
            {
                return SerializeXmlSerializable(result.Success);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region 5.2.9.2  Authentication with Tokens for Browser Clients
        /// <summary>
      /// Throws an exception if authentication with token is enabled, but no valid token is sent
      /// </summary>
      /// <remarks></remarks>
        private TokenType CheckTokenAuthentication(JSON.MultipartFormDataContent multipart)
        {
            string sessionIdCookieName = CmisServiceImpl.GetSessionIdCookieName();
            var retVal = (TokenType)multipart ?? (TokenType)CommonFunctions.GetRequestParameter("token");

            if (!string.IsNullOrEmpty(sessionIdCookieName))
            {
                string sessionIdCookie = CmisServiceImplBase.CurrentIncomingCookies.get_Value(sessionIdCookieName) ?? string.Empty;

                if (_tokenRequiringSessionIdCookies.Contains(sessionIdCookie.GetHashCode()) && (retVal is null || !retVal.Token.StartsWith(sessionIdCookie)))
                {
                    throw LogException(cm.cmisFaultType.CreatePermissionDeniedException(), CmisServiceImpl);
                }
            }

            return retVal;
        }

        /// <summary>
      /// Within a sessionIdCookie supporting system this method checks if the hosting system added a sessionIdCookie
      /// which signals a login process. If so the method stores the sessionId (from the outgoing cookie), if a token
      /// query parameter is set by the client. Each following request from the client MUST support a valid token,
      /// otherwise the request will be denied. This can prevent a form of cross-site request forgery.
      /// </summary>
      /// <remarks></remarks>
        private void EnableTokenAuthentication()
        {
            string sessionIdCookieName = CmisServiceImpl.GetSessionIdCookieName();
            string token = CommonFunctions.GetRequestParameter("token");

            if (!(string.IsNullOrEmpty(sessionIdCookieName) || string.IsNullOrEmpty(token)))
            {
                // the system supports sessionIdCookies and a token is set by the client to enable authentication with tokens
                // (see chapter 5.2.9.2 Authentication with Tokens for Browser Clients)
                string sessionIdCookie = CmisServiceImplBase.CurrentOutgoingCookies.get_Value(sessionIdCookieName);

                if (!string.IsNullOrEmpty(sessionIdCookie))
                    _tokenRequiringSessionIdCookies.Add(sessionIdCookie.GetHashCode());
            }
        }

        private static readonly HashSet<int> _tokenRequiringSessionIdCookies = new HashSet<int>();
        #endregion

        private static Type _genericWebFaultExceptionTypeDefinition = typeof(ssw.WebFaultException<string>).GetGenericTypeDefinition();

        /// <summary>
        /// Browser-binding extensions to allow cross-domain requests
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private System.IO.MemoryStream GetFile(JSON.Enums.enumJSONFile file)
        {
            switch (file)
            {
                case JSON.Enums.enumJSONFile.cmisJS:
                    {
                        var regEx = new System.Text.RegularExpressions.Regex("((?<FrameUri>" + JSONConstants.jsFrameUri + ")" + "|(?<LoginKeyCookie>" + JSONConstants.jsLoginKeyCookie + ")" + "|(?<RepositoryDomain>" + JSONConstants.jsRepositoryDomain + ")" + "|(?<ServiceUri>" + JSONConstants.jsServiceUri + ")" + "|(?<SessionIdCookieName>" + JSONConstants.jsSessionIdCookie + ")" + ")", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                        var baseUri = BaseUri;
                        string relativeUri = ServiceURIs.get_RepositoriesUri(ServiceURIs.enumRepositoriesUri.file).ReplaceUri("file", "embeddedFrame.htm");
                        var frameUri = baseUri.Combine(relativeUri);
                        string loginKeyCookie = CommonFunctions.GetRequestParameter("loginKeyCookie");
                        var serviceUri = ServiceURIs.GetRepositories == "/" ? baseUri.AbsoluteUri.EndsWith("/") ? baseUri : new Uri(baseUri.AbsoluteUri + "/") : new Uri(baseUri, ServiceURIs.GetRepositories);
                        var replacements = new Dictionary<string, string>() { { "FrameUri", frameUri.AbsoluteUri }, { "LoginKeyCookie", loginKeyCookie }, { "RepositoryDomain", serviceUri.Authority }, { "ServiceUri", serviceUri.AbsoluteUri }, { "SessionIdCookieName", CmisServiceImpl.GetSessionIdCookieName() } };
                        System.Text.RegularExpressions.MatchEvaluator evaluator = currentMatch =>
                        {
                            foreach (KeyValuePair<string, string> de in replacements)
                            {
                                System.Text.RegularExpressions.Group gr = currentMatch.Groups[de.Key];
                                if (gr != null && gr.Success)
                                    return de.Value;
                            }
                            return currentMatch.Value;
                        };
                        return PrepareResult(regEx.Replace(My.Resources.Resources.cmis, evaluator), MediaTypes.JavaScript);
                    }
                case JSON.Enums.enumJSONFile.embeddedFrameHtm:
                    {
                        // iFrame within cmis.js
                        string relativeUri = ServiceURIs.get_RepositoriesUri(ServiceURIs.enumRepositoriesUri.cmisaction).ReplaceUri("cmisaction", "login");
                        var uri = BaseUri.Combine(relativeUri);
                        return PrepareResult(My.Resources.Resources.EmbeddedFrame.Replace(JSONConstants.jsLoginUri, uri.AbsoluteUri), MediaTypes.Html);
                    }
                case JSON.Enums.enumJSONFile.loginPageHtm:
                    {
                        System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex("((?<Host>" + JSONConstants.jsHostingApplicationUri + ")" + @"|(?<input>\<input\s+((?<name>[^\=\/\s]*)\s*\=\s*""(?<value>[^""]*)""\s*)+\s*\/\>))", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                        System.Text.RegularExpressions.MatchEvaluator evaluator = currentMatch =>
                        {
                            System.Text.RegularExpressions.Group grHost = currentMatch.Groups["Host"];

                            if (grHost != null && grHost.Success)
                                return CommonFunctions.GetRequestParameter("hostingApplicationUri");
                            else
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                System.Text.RegularExpressions.Group grName = currentMatch.Groups["name"];
                                System.Text.RegularExpressions.Group grValue = currentMatch.Groups["value"];
                                for (int index = 0; index <= grName.Captures.Count - 1; index++)
                                    dic.Add(grName.Captures[index].Value, grValue.Captures[index].Value);
                                if (dic.ContainsKey("name"))
                                {
                                    foreach (string inputName in new string[] { "repositoryId", "user" })
                                    {
                                        if (dic["name"] == inputName)
                                        {
                                            dic.Remove("value");
                                            dic.Add("value", CommonFunctions.GetRequestParameter(inputName));
                                            return "<input " + string.Join(" ", (from de in dic
                                                                                 select de.Key + "=\"" + de.Value + "\"").ToArray()) + " />";
                                        }
                                    }
                                }

                                return currentMatch.Value;
                            }
                        };
                        return PrepareResult(regEx.Replace(My.Resources.Resources.LoginPage, evaluator), Constants.MediaTypes.Html);
                    }
                case JSON.Enums.enumJSONFile.loginRefPageHtm:
                    {
                        // give link to the login page
                        string relativeUri = ServiceURIs.get_RepositoriesUri(ServiceURIs.enumRepositoriesUri.cmisaction).ReplaceUri("cmisaction", "login");
                        var uri = BaseUri.Combine(relativeUri);
                        return PrepareResult(My.Resources.Resources.LoginRefPage.Replace("LoginPage.htm", uri.AbsoluteUri), MediaTypes.Html);
                    }

                default:
                    {
                        return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
                    }
            }
        }

        /// <summary>
        /// Returns folderId using following preferences:
        ///   1. return folderId if it is not null or empty
        ///   2. return folderId-querystring parameter if exist and not null or empty
        ///   3. return id-querystring parameter if exist and not null or empty
        ///   4. return rootFolderId of the repository
        /// </summary>
        /// <param name="serviceImpl"></param>
        /// <param name="repositoryId"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetFolderId(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string folderId)
        {
            if (string.IsNullOrEmpty(folderId))
                folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            if (string.IsNullOrEmpty(folderId))
            {
                // rootfolder
                var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;
                if (repositoryInfo is not null)
                    folderId = repositoryInfo.RootFolderId;
            }

            return folderId;
        }

        /// <summary>
      /// Returns the response of a form request (POST)
      /// </summary>
      /// <remarks>see chapter 5.4.4.4 Access to Form Response Content of cmis documentation</remarks>
        private System.IO.MemoryStream GetFormResponse(cc.cmisObjectType cmisObject, string repositoryId, TokenType token, System.Net.HttpStatusCode statusCode, bool? succinct)
        {
            // all operations that return the HTTP status code 201 SHOULD also return a HTTP Location header (see 5.4 Services)
            if (statusCode == System.Net.HttpStatusCode.Created)
            {
                var locationUri = BaseUri.Combine(ServiceURIs.get_RootFolderUri(ServiceURIs.enumRootFolderUri.objectId).ReplaceUri("repositoryId", repositoryId, "id", cmisObject.ObjectId));
                ssw.WebOperationContext.Current.OutgoingResponse.Location = locationUri.AbsoluteUri;
            }
            if (IsHtmlPageRequired(token))
            {
                var transaction = new JSON.Transaction() { Code = (long)statusCode, Exception = null, Message = null, ObjectId = cmisObject.ObjectId };

                set_Transaction(repositoryId, token, transaction);
                return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
            }
            else
            {
                return SerializeXmlSerializable(cmisObject, succinct, statusCode);
            }
        }

        /// <summary>
      /// Returns the response of a form request (POST)
      /// </summary>
      /// <remarks>see chapter 5.4.4.4 Access to Form Response Content of cmis documentation</remarks>
        private System.IO.MemoryStream GetFormResponse(ccdt.cmisTypeDefinitionType typeDefinition, string repositoryId, TokenType token, System.Net.HttpStatusCode statusCode)
        {
            // all operations that return the HTTP status code 201 SHOULD also return a HTTP Location header (see 5.4 Services)
            if (statusCode == System.Net.HttpStatusCode.Created)
            {
                var locationUri = BaseUri.Combine(ServiceURIs.get_RepositoryUri(ServiceURIs.enumRepositoryUri.typeId).ReplaceUri("repositoryId", repositoryId, "typeId", typeDefinition.Id));
                ssw.WebOperationContext.Current.OutgoingResponse.Location = locationUri.AbsoluteUri;
            }
            if (IsHtmlPageRequired(token))
            {
                var transaction = new JSON.Transaction() { Code = (long)statusCode, Exception = null, Message = null, ObjectId = typeDefinition.Id };

                set_Transaction(repositoryId, token, transaction);
                return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
            }
            else
            {
                return SerializeXmlSerializable(typeDefinition, default, statusCode);
            }
        }

        /// <summary>
      /// Returns the typeDefinition declared for specified objectId (objectTypeId and secondaryTypeIds)
      /// </summary>
        private ccdt.cmisTypeDefinitionType[] GetTypeDefinitions(Contracts.ICmisServicesImpl serviceImpl, string repositoryId, string objectId)
        {
            var typeDefinitions = new List<ccdt.cmisTypeDefinitionType>();

            // get current typeDefinitions (objectTypeId and secondaryTypeIds)
            var result = serviceImpl.GetObject(repositoryId, objectId, CmisPredefinedPropertyNames.ObjectTypeId + "," + CmisPredefinedPropertyNames.SecondaryObjectTypeIds, cc.enumIncludeRelationships.none, false, "cmis:none", false, false, default, default);
            if (result is not null && result.Failure is null && result.Success is not null)
            {
                // collect typeIds
                var properties = result.Success.GetProperties();

                foreach (string propertyName in new string[] { CmisPredefinedPropertyNames.ObjectTypeId, CmisPredefinedPropertyNames.SecondaryObjectTypeIds })
                {
                    if (properties.ContainsKey(propertyName))
                    {
                        cc.Properties.Generic.cmisProperty<string> stringProperty = properties[propertyName] as cc.Properties.Generic.cmisProperty<string>;
                        var typeIds = stringProperty is null ? null : stringProperty.Values;

                        if (typeIds is not null)
                        {
                            foreach (string typeId in typeIds)
                            {
                                var typeDefinition = serviceImpl.get_TypeDefinition(repositoryId, typeId).Success;
                                if (typeDefinition is not null)
                                    typeDefinitions.Add(typeDefinition);
                            }
                        }
                    }
                }
            }

            return typeDefinitions.ToArray();
        }

        /// <summary>
      /// Returns True if a token control is used in a non ajax request 
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      /// <remarks>Minor deviation to chapter 5.4.4.4.1 Client Implementation Hints of the cmis documentation, which explicitly defines:
      /// whenever the token control is used, the repository must respond with a HTML page.
      /// This implementation allows the usage of a token parameter within the querystring in POST methods (cmis documentation specifies
      /// only a token control in a POST method). As a result clients can enable token authentication AND receive JSON objects from a
      /// POST method.</remarks>
        private bool IsHtmlPageRequired(TokenType token)
        {
            return token is not null && token.Transmission == enumTokenTransmission.asControl && ssw.WebOperationContext.Current.IncomingRequest.Headers["X-Requested-With"] != "XMLHttpRequest";
        }

        /// <summary>
      /// Returns content as an utf-8 encoded stream
      /// </summary>
      /// <param name="content"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private System.IO.MemoryStream PrepareResult(string content, string contentType)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var retVal = new System.IO.MemoryStream(buffer, 0, buffer.Length);

            retVal.Position = 0L;
            ssw.WebOperationContext.Current.OutgoingResponse.ContentType = contentType;

            return retVal;
        }

        /// <summary>
      /// Contains the type used to represent a queryResultList
      /// </summary>
      /// <remarks></remarks>
        private static Type _queryResultListType;

        /// <summary>
      /// Serializes an unspecific XmlSerializable-object-array
      /// </summary>
        private System.IO.MemoryStream SerializeArray<TXmlSerializable>(TXmlSerializable[] objects, bool? succinct = default, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK) where TXmlSerializable : Serialization.XmlSerializable
        {
            var retVal = new System.IO.MemoryStream();
            // 5.2.11  Succinct Representation of Properties
            var serializer = new JSON.Serialization.JavaScriptSerializer(succinct.HasValue && succinct.Value);

            // serialization for query (see Constants.JSONTypeDefinitions.RepresentationTypes("http://docs.oasis-open.org/ns/cmis/browser/201103/queryResultList"))
            if (_queryResultListType.IsAssignableFrom(objects.GetType()))
            {
                // respect the slightly differences of serializations for cmisObjects in queryResultLists
                serializer.AttributesOverrides.set_TypeConverter(typeof(cc.cmisObjectType), new JSON.Core.QueryResultConverter());
            }

            // see chapter 5.2.8  Callback in cmis documentation
            string callback = CommonFunctions.GetRequestParameter("callback");
            string jsonString = objects is null ? "[]" : serializer.SerializeArray(objects);

            if (string.IsNullOrEmpty(callback))
            {
                Write(retVal, jsonString);
            }
            else
            {
                Write(retVal, callback, "(", jsonString, ")");
            }

            ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            ssw.WebOperationContext.Current.OutgoingResponse.ContentType = string.IsNullOrEmpty(callback) ? MediaTypes.Json : MediaTypes.JavaScript;
            retVal.Position = 0L;

            return retVal;
        }

        /// <summary>
      /// Serializes an exception
      /// </summary>
      /// <param name="token">Set this parameter only in POST-requests</param>
      /// <returns></returns>
      /// <remarks>see chapter 5.2.10  Error Handling and Return Codes in cmis documentation</remarks>
        private System.IO.MemoryStream SerializeException(Exception ex, string repositoryId = null, TokenType token = null)
        {
            var exceptionType = ex.GetType();
            cm.cmisFaultType cmisFault;

            if (exceptionType.IsGenericType && ReferenceEquals(exceptionType.GetGenericTypeDefinition(), _genericWebFaultExceptionTypeDefinition))
            {
                var grh = GenericRuntimeHelper.GetInstance(exceptionType.GetGenericArguments()[0]);
                cmisFault = grh.CreateCmisFaultType(ex);
            }
            else if (typeof(ssw.WebFaultException).IsAssignableFrom(exceptionType))
            {
                cmisFault = cm.cmisFaultType.CreateInstance((ssw.WebFaultException)ex);
            }
            else
            {
                cmisFault = cm.cmisFaultType.CreateInstance(ex);
            }

            if (IsHtmlPageRequired(token))
            {
                // POST-request with token
                var transaction = new JSON.Transaction() { Code = cmisFault.Code, Exception = cmisFault.Type.GetName(), Message = cmisFault.Message, ObjectId = null };
                var serializer = new JSON.Serialization.JavaScriptSerializer();

                set_Transaction(repositoryId, token, transaction);
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
            }
            else
            {
                // GET-request or POST-request without token
                var suppressResponseCodes = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter("suppressResponseCodes"));

                return SerializeXmlSerializable(cmisFault, default, suppressResponseCodes.HasValue && suppressResponseCodes.Value ? System.Net.HttpStatusCode.OK : (System.Net.HttpStatusCode)cmisFault.Code);
            }
        }

        /// <summary>
      /// Serializes an array of cmisRepositoryInfoType-objects as utf-8 encoded JSON-stream
      /// </summary>
      /// <param name="repositories"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private System.IO.MemoryStream SerializeRepositories(cc.cmisRepositoryInfoType[] repositories)
        {
            if (repositories is null)
            {
                // statuscode already set!
                return PrepareResult(My.Resources.Resources.EmptyPage, MediaTypes.Html);
            }
            else
            {
                var retVal = new System.IO.MemoryStream();
                var baseUri = BaseUri;
                var serializer = new JSON.Serialization.JavaScriptSerializer();
                // see chapter 5.2.8  Callback in cmis documentation
                string callback = CommonFunctions.GetRequestParameter("callback");

                // add URLs (see chapter 5.3.1  Service URL in the cmis documentation)
                foreach (cc.cmisRepositoryInfoType repository in repositories)
                {
                    if (repository is not null)
                    {
                        var uri = baseUri.Combine(ServiceURIs.get_RepositoryUri(ServiceURIs.enumRepositoryUri.none).ReplaceUri("repositoryId", repository.RepositoryId));
                        repository.RepositoryUrl = uri.AbsoluteUri;
                        uri = baseUri.Combine(ServiceURIs.RootFolder.ReplaceUri("repositoryId", repository.RepositoryId));
                        repository.RootFolderUrl = uri.AbsoluteUri;
                    }
                }
                string jsonString = serializer.SerializeMap(repositories, cc.cmisRepositoryInfoType.DefaultKeyProperty);
                if (string.IsNullOrEmpty(callback))
                {
                    Write(retVal, jsonString);
                }
                else
                {
                    Write(retVal, callback, "(", jsonString, ")");
                }

                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                ssw.WebOperationContext.Current.OutgoingResponse.ContentType = string.IsNullOrEmpty(callback) ? MediaTypes.Json : MediaTypes.JavaScript;
                retVal.Position = 0L;

                return retVal;
            }
        }

        /// <summary>
      /// Serializes an unspecific XmlSerializable-object
      /// </summary>
        private System.IO.MemoryStream SerializeXmlSerializable<TXmlSerializable>(TXmlSerializable obj, bool? succinct = default, System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK) where TXmlSerializable : Serialization.XmlSerializable
        {
            var retVal = new System.IO.MemoryStream();
            var serializer = new JSON.Serialization.JavaScriptSerializer();

            // 5.2.11  Succinct Representation of Properties
            if (succinct.HasValue && succinct.Value)
            {
                serializer.AttributesOverrides.set_ElementAttribute(typeof(cc.cmisObjectType), "properties", new JSON.Serialization.JSONAttributeOverrides.JSONElementAttribute("succinctProperties", new JSON.Collections.SuccinctPropertiesConverter()));
            }
            // serialization for query (see Constants.JSONTypeDefinitions.RepresentationTypes("http://docs.oasis-open.org/ns/cmis/browser/201103/queryResultList"))
            if (_queryResultListType.IsAssignableFrom(obj.GetType()))
            {
                // respect the slightly differences of serializations for cmisObjects in queryResultLists
                serializer.AttributesOverrides.set_TypeConverter(typeof(cc.cmisObjectType), new JSON.Core.QueryResultConverter());
            }

            string jsonString = obj is null ? "{}" : serializer.Serialize(obj);

            // see chapter 5.2.8  Callback in cmis documentation
            if (!QueryParameterExists("callback"))
            {
                Write(retVal, jsonString);
                ssw.WebOperationContext.Current.OutgoingResponse.ContentType = MediaTypes.Json;
            }
            else
            {
                string callback = CommonFunctions.GetRequestParameter("callback");

                if (string.IsNullOrEmpty(callback))
                {
                    throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("If queryparameter 'callback' is defined it MUST NOT be null.", false), CmisServiceImpl);
                }
                else
                {
                    Write(retVal, callback, "(", jsonString, ")");
                    ssw.WebOperationContext.Current.OutgoingResponse.ContentType = MediaTypes.JavaScript;
                }
            }

            ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
            retVal.Position = 0L;

            return retVal;
        }

        /// <summary>
      /// Gets or sets lastResultCookie for specified token
      /// </summary>
        private JSON.Transaction get_Transaction(TokenType token)
        {
            string key = "lastResultCookie" + (token is null ? string.Empty : token.Token).GetHashCode();
            var cookie = CmisServiceImplBase.CurrentIncomingCookies.get_Cookie(key);

            if (cookie is null)
            {
                return null;
            }
            else
            {
                var serializer = new JSON.Serialization.JavaScriptSerializer();
                try
                {
                    return serializer.Deserialize<JSON.Transaction>(System.Net.WebUtility.HtmlDecode(cookie.Value));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        private void set_Transaction(string repositoryId, TokenType token, JSON.Transaction value)
        {
            var currentDate = DateTimeOffset.Now;
            int maxAge = value is null ? 0 : 3600;
            var expires = DateTime.UtcNow.AddHours(maxAge == 0 ? -1.0d : 1.0d);
            string cookieValue = value is null ? "_" : new JSON.Serialization.JavaScriptSerializer().Serialize(value);
            var cookie = new HttpCookie("lastResultCookie" + (token is null ? string.Empty : token.Token).GetHashCode(), cookieValue) { Expires = expires, MaxAge = maxAge, Path = BaseUri.Combine(ServiceURIs.get_RepositoryUri(ServiceURIs.enumRepositoryUri.none).ReplaceUri("repositoryId", repositoryId)).AbsolutePath };
            CmisServiceImplBase.AddOutgoingCookie(cookie);
        }

        /// <summary>
      /// Writes value as utf-8 encoded buffer into ms
      /// </summary>
        private void Write(System.IO.MemoryStream destination, params string[] values)
        {
            foreach (string value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes(value);
                    destination.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}