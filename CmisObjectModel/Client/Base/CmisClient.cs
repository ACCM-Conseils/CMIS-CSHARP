using System;
using System.Collections.Generic;
using sn = System.Net;
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
using cm = CmisObjectModel.Messaging;
using cmr = CmisObjectModel.Messaging.Responses;
using Microsoft.VisualBasic.CompilerServices;
using System.Threading;
/* TODO ERROR: Skipped IfDirectiveTrivia
#If Not xs_HttpRequestAddRange64 Then
*//* TODO ERROR: Skipped DisabledTextTrivia
#Const HttpRequestAddRangeShortened = True
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*//* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Base
{
    public abstract class CmisClient : Contracts.ICmisClient
    {

        #region Constructors
        protected CmisClient(Uri serviceDocUri, enumVendor vendor, AuthenticationProvider authentication, int? connectTimeout = default, int? readWriteTimeout = default)
        {
            _serviceDocUri = serviceDocUri;
            switch (vendor)
            {
                case enumVendor.Alfresco:
                    {
                        _vendor = new Vendors.Alfresco(this);
                        break;
                    }
                case enumVendor.Agorum:
                    {
                        _vendor = new Vendors.Agorum(this);
                        break;
                    }

                default:
                    {
                        _vendor = new Vendors.Vendor(this);
                        break;
                    }
            }
            _authentication = authentication;
            _connectTimeout = connectTimeout.HasValue && connectTimeout.Value >= -1 ? connectTimeout : default;
            _readWriteTimeout = readWriteTimeout.HasValue && readWriteTimeout.Value >= -1 ? readWriteTimeout : default;
        }
        #endregion

        #region IValueMappingSupport
        protected Data.MapperDictionary _valueMapper = new Data.MapperDictionary();
        /// <summary>
      /// Adds mapping information to the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="mapper"></param>
      /// <remarks></remarks>
        public void AddMapper(string repositoryId, Data.Mapper mapper)
        {
            _valueMapper.AddMapper(repositoryId, mapper);
        }

        /// <summary>
      /// Removes mapping information from the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public void RemoveMapper(string repositoryId)
        {
            _valueMapper.RemoveMapper(repositoryId);
        }
        #endregion

        #region Repository
        public abstract Client.Generic.ResponseType<cmr.createTypeResponse> CreateType(cm.Requests.createType request);
        public abstract Client.Generic.ResponseType<cmr.deleteTypeResponse> DeleteType(cm.Requests.deleteType request);
        public abstract Client.Generic.ResponseType<cmr.getRepositoriesResponse> GetRepositories(cm.Requests.getRepositories request);
        public abstract Client.Generic.ResponseType<cmr.getRepositoryInfoResponse> GetRepositoryInfo(cm.Requests.getRepositoryInfo request, bool ignoreCache = false);
        public abstract Client.Generic.ResponseType<cmr.getTypeChildrenResponse> GetTypeChildren(cm.Requests.getTypeChildren request);
        public abstract Client.Generic.ResponseType<cmr.getTypeDefinitionResponse> GetTypeDefinition(cm.Requests.getTypeDefinition request);
        public abstract Client.Generic.ResponseType<cmr.getTypeDescendantsResponse> GetTypeDescendants(cm.Requests.getTypeDescendants request);
        public abstract Client.Generic.ResponseType<cmr.updateTypeResponse> UpdateType(cm.Requests.updateType request);
        #endregion

        #region Navigation
        public abstract Client.Generic.ResponseType<cmr.getCheckedOutDocsResponse> GetCheckedOutDocs(cm.Requests.getCheckedOutDocs request);
        public abstract Client.Generic.ResponseType<cmr.getChildrenResponse> GetChildren(cm.Requests.getChildren request);
        public abstract Client.Generic.ResponseType<cmr.getDescendantsResponse> GetDescendants(cm.Requests.getDescendants request);
        public abstract Client.Generic.ResponseType<cmr.getFolderParentResponse> GetFolderParent(cm.Requests.getFolderParent request);
        public abstract Client.Generic.ResponseType<cmr.getFolderTreeResponse> GetFolderTree(cm.Requests.getFolderTree request);
        public abstract Client.Generic.ResponseType<cmr.getObjectParentsResponse> GetObjectParents(cm.Requests.getObjectParents request);
        #endregion

        #region Object
        public abstract Client.Generic.ResponseType<cmr.appendContentStreamResponse> AppendContentStream(cm.Requests.appendContentStream request);
        public abstract Client.Generic.ResponseType<cmr.bulkUpdatePropertiesResponse> BulkUpdateProperties(cm.Requests.bulkUpdateProperties request);
        public abstract Client.Generic.ResponseType<cmr.createDocumentResponse> CreateDocument(cm.Requests.createDocument request);
        public abstract Client.Generic.ResponseType<cmr.createDocumentFromSourceResponse> CreateDocumentFromSource(cm.Requests.createDocumentFromSource request);
        public abstract Client.Generic.ResponseType<cmr.createFolderResponse> CreateFolder(cm.Requests.createFolder request);
        public abstract Client.Generic.ResponseType<cmr.createItemResponse> CreateItem(cm.Requests.createItem request);
        public abstract Client.Generic.ResponseType<cmr.createPolicyResponse> CreatePolicy(cm.Requests.createPolicy request);
        public abstract Client.Generic.ResponseType<cmr.createRelationshipResponse> CreateRelationship(cm.Requests.createRelationship request);
        public abstract Client.Generic.ResponseType<cmr.deleteContentStreamResponse> DeleteContentStream(cm.Requests.deleteContentStream request);
        public abstract Client.Generic.ResponseType<cmr.deleteObjectResponse> DeleteObject(cm.Requests.deleteObject request);
        public abstract Client.Generic.ResponseType<cmr.deleteTreeResponse> DeleteTree(cm.Requests.deleteTree request);
        public abstract Client.Generic.ResponseType<cmr.getAllowableActionsResponse> GetAllowableActions(cm.Requests.getAllowableActions request);
        public abstract Client.Generic.ResponseType<cmr.getContentStreamResponse> GetContentStream(cm.Requests.getContentStream request);
        public abstract Client.Generic.ResponseType<string> GetContentStreamLink(string repositoryId, string objectId, string streamId = null);
        public abstract Client.Generic.ResponseType<cmr.getObjectResponse> GetObject(cm.Requests.getObject request);
        public abstract Client.Generic.ResponseType<cmr.getObjectByPathResponse> GetObjectByPath(cm.Requests.getObjectByPath request);
        public abstract Client.Generic.ResponseType<cmr.getPropertiesResponse> GetProperties(cm.Requests.getProperties request);
        public abstract Client.Generic.ResponseType<cmr.getRenditionsResponse> GetRenditions(cm.Requests.getRenditions request);
        public abstract Client.Generic.ResponseType<cmr.moveObjectResponse> MoveObject(cm.Requests.moveObject request);
        public abstract Client.Generic.ResponseType<cmr.setContentStreamResponse> SetContentStream(cm.Requests.setContentStream request);
        public abstract Client.Generic.ResponseType<cmr.updatePropertiesResponse> UpdateProperties(cm.Requests.updateProperties request);
        #endregion

        #region Multi
        public abstract Client.Generic.ResponseType<cmr.addObjectToFolderResponse> AddObjectToFolder(cm.Requests.addObjectToFolder request);
        public abstract Client.Generic.ResponseType<cmr.removeObjectFromFolderResponse> RemoveObjectFromFolder(cm.Requests.removeObjectFromFolder request);
        #endregion

        #region Discovery
        public abstract Client.Generic.ResponseType<cmr.getContentChangesResponse> GetContentChanges(cm.Requests.getContentChanges request);
        public abstract Client.Generic.ResponseType<cmr.queryResponse> Query(cm.Requests.query request);
        #endregion

        #region Versioning
        public abstract Client.Generic.ResponseType<cmr.cancelCheckOutResponse> CancelCheckOut(cm.Requests.cancelCheckOut request);
        public abstract Client.Generic.ResponseType<cmr.checkInResponse> CheckIn(cm.Requests.checkIn request);
        public abstract Client.Generic.ResponseType<cmr.checkOutResponse> CheckOut(cm.Requests.checkOut request);
        public abstract Client.Generic.ResponseType<cmr.getAllVersionsResponse> GetAllVersions(cm.Requests.getAllVersions request);
        public abstract Client.Generic.ResponseType<cmr.getObjectOfLatestVersionResponse> GetObjectOfLatestVersion(cm.Requests.getObjectOfLatestVersion request);
        public abstract Client.Generic.ResponseType<cmr.getPropertiesOfLatestVersionResponse> GetPropertiesOfLatestVersion(cm.Requests.getPropertiesOfLatestVersion request);
        #endregion

        #region Relationship
        public abstract Client.Generic.ResponseType<cmr.getObjectRelationshipsResponse> GetObjectRelationships(cm.Requests.getObjectRelationships request);
        #endregion

        #region Policy
        public abstract Client.Generic.ResponseType<cmr.applyPolicyResponse> ApplyPolicy(cm.Requests.applyPolicy request);
        public abstract Client.Generic.ResponseType<cmr.getAppliedPoliciesResponse> GetAppliedPolicies(cm.Requests.getAppliedPolicies request);
        public abstract Client.Generic.ResponseType<cmr.removePolicyResponse> RemovePolicy(cm.Requests.removePolicy request);
        #endregion

        #region Acl
        public abstract Client.Generic.ResponseType<cmr.applyACLResponse> ApplyAcl(cm.Requests.applyACL request);
        public abstract Client.Generic.ResponseType<cmr.getACLResponse> GetAcl(cm.Requests.getACL request);
        #endregion

        #region Miscellaneous (ICmisClient)
        public abstract void Logout(string repositoryId);
        public abstract void Ping(string repositoryId);

        private Dictionary<string, Tuple<System.Threading.WaitCallback, System.Threading.EventWaitHandle>> _registeredPings = new Dictionary<string, Tuple<System.Threading.WaitCallback, System.Threading.EventWaitHandle>>();
        /// <summary>
      /// Installs an automatic ping to tell the server that the client is still alive
      /// </summary>
      /// <param name="interval">Time-interval in seconds</param>
      /// <remarks></remarks>
        public void RegisterPing(string repositoryId, double interval)
        {
            if (!string.IsNullOrEmpty(repositoryId) && interval > 0d)
            {
                var ewh = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);
                Action<object> callBack = state =>
                    {
                        int milliseconds = (int)Math.Round(interval * 1000.0d);
                        while (!ewh.WaitOne(milliseconds))
                        {
                            try
                            {
                                Ping(repositoryId);
                            }
                            catch (Exception ex)
                            {
                                // stop
                                UnregisterPing(repositoryId);
                            }
                        }
                    };
                // stop the current ping-job
                UnregisterPing(repositoryId);
                lock (_registeredPings)
                {
                    //A remplacer
                    //System.Threading.ThreadPool.QueueUserWorkItem(callBack);
                    //_registeredPings.Add(repositoryId, new Tuple<System.Threading.WaitCallback, System.Threading.EventWaitHandle>(callBack, ewh));
                }
                // the first time we will do it immediately
                Ping(repositoryId);
            }
        }

        /// <summary>
      /// Returns True if the binding supports a succinct representation of properties
      /// </summary>
        public abstract bool SupportsSuccinct { get; }
        /// <summary>
      /// Returns True if the binding supports token parameters
      /// </summary>
        public abstract bool SupportsToken { get; }
        public virtual int? Timeout { get; set; }

        /// <summary>
      /// Removes the automatic ping
      /// </summary>
        public void UnregisterPing(string repositoryId)
        {
            if (!string.IsNullOrEmpty(repositoryId))
            {
                lock (_registeredPings)
                {
                    if (_registeredPings.ContainsKey(repositoryId))
                    {
                        // stop the current ping-job
                        _registeredPings[repositoryId].Item2.Set();
                        _registeredPings.Remove(repositoryId);
                    }
                }
            }
        }

        public virtual string User
        {
            get
            {
                return _authentication is null ? null : _authentication.User;
            }
        }
        #endregion

        #region Requests

        #region Vendor specific and value mapping
        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="properties"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual Vendors.Vendor.State TransformRequest(string repositoryId, Core.Collections.cmisPropertiesType properties)
        {
            // first: value mapping, second: vendor specific
            return _vendor.BeginRequest(repositoryId, properties, _valueMapper.MapProperties(repositoryId, enumMapDirection.outgoing, properties));
        }

        /// <summary>
      /// Executes defined value mappings and processes vendor specific presentation of property values
      /// </summary>
      /// <param name="state"></param>
      /// <remarks></remarks>
        protected void TransformResponse(Vendors.Vendor.State state, Core.Collections.cmisPropertiesType[] propertyCollections)
        {
            // first: vendor specifics
            _vendor.EndRequest(state, propertyCollections);
            // second: value mapping
            _valueMapper.MapProperties(state.RepositoryId, enumMapDirection.incoming, propertyCollections);
        }
        #endregion

        /// <summary>
      /// Creates a HttpWebRequest-instance
      /// </summary>
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Protected Overridable Function CreateHttpWebRequest(uri As Uri, method As String, data As System.IO.MemoryStream, contentType As String,
                                                                  headers As Dictionary(Of String, String), offset As Integer?, length As Integer?) As sn.HttpWebRequest
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        protected virtual sn.HttpWebRequest CreateHttpWebRequest(Uri uri, string method, System.IO.MemoryStream data, string contentType, Dictionary<string, string> headers, long? offset, long? length)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            try
            {
                sn.HttpWebRequest retVal = (sn.HttpWebRequest)sn.WebRequest.Create(uri);

                retVal.Method = method;
                retVal.UserAgent = UserAgent;
                retVal.AllowAutoRedirect = _authentication is null;
                if (_connectTimeout.HasValue)
                    retVal.Timeout = _connectTimeout.Value;
                if (_readWriteTimeout.HasValue)
                    retVal.ReadWriteTimeout = _readWriteTimeout.Value;
                if (!string.IsNullOrEmpty(contentType))
                    retVal.ContentType = contentType;
                if (headers is not null)
                {
                    foreach (KeyValuePair<string, string> de in headers)
                        retVal.Headers.Add(de.Key, de.Value);
                }
                if (_authentication is not null)
                {
                    retVal.PreAuthenticate = true;
                    _authentication.Authenticate(retVal);
                }
                if (offset.HasValue && offset.Value >= 0L)
                {
                    if (length.HasValue && length.Value >= 0L)
                    {
                        retVal.AddRange(offset.Value, offset.Value + length.Value - 1L);
                    }
                    else
                    {
                        retVal.AddRange(offset.Value);
                    }
                }
                if (AppSettings.Compression)
                    retVal.AutomaticDecompression = sn.DecompressionMethods.GZip | sn.DecompressionMethods.Deflate;
                if (data is not null)
                {
                    System.IO.Stream requestStream;

                    retVal.SendChunked = true;
                    requestStream = retVal.GetRequestStream();
                    data.Position = 0L;
                    data.CopyTo(requestStream);
                    requestStream.Close();
                }
                if (Timeout.HasValue)
                    retVal.Timeout = Timeout.Value;

                return retVal;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to access '" + uri.AbsoluteUri + "'", ex);
            }
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Protected Overridable Function GetResponse(uri As Uri, method As String, contentWriter As Action(Of IO.Stream), contentType As String,
                                                         headers As Dictionary(Of String, String), offset As Integer?, length As Integer?) As sn.HttpWebResponse
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        protected virtual sn.HttpWebResponse GetResponse(Uri uri, string method, Action<System.IO.Stream> contentWriter, string contentType, Dictionary<string, string> headers, long? offset, long? length)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */         // up to the maximum of 50 redirections; see https://msdn.microsoft.com/de-de/library/system.net.httpwebrequest.maximumautomaticredirections(v=vs.100).aspx
            int redirectionCounter = 50;
            sn.HttpWebResponse retVal;
            System.IO.MemoryStream content;

            if (contentWriter is null)
            {
                content = null;
            }
            else
            {
                content = new System.IO.MemoryStream();
                contentWriter.Invoke(content);
            }
            try
            {
                // If a server uses cookies and supports case insensitive uris, the server will answer a request of GetRepositoryInfo() or GetRepositories(), even if the
                // client doesn't use the correct uri the server listens on. Using the AtomPub-binding the uris to the functions GetRepositoryInfo() or GetRepositories()
                // can only be derived from the serviceDocUri the client is configured with. The uris of all other request will be constructed by using uri-templates
                // the server has to provide. If the spelling differs in case sensitivity a problem might occur as can be seen in the following scenario.
                // Configuration of client: serviceDocUri = https://host/CmisServiceUri/
                // Configuration of server: serviceDocUri = https://host/CMISServiceUri/
                // Log on via GetRepositoryInfo to repositoryId repId: https://host/CmisServiceUri/?repositoryId=repId
                // If the server uses cookies, a cookie could be returned with name="sessionId" value=guid. The client will register the cookie to the path
                // /CmisServiceUri/ because that's the absolutePath of the requested uri.
                // If a subsequent call to the function GetObject() is made the client has to use the uri-template "getObjectById". If this uri-template starts with
                // https://host/CMISServiceUri (see server configuration) it will not send the received cookie "sessionId" back to server because of the difference
                // in case sensitivity and as a result the server will reject the request.
                // The supported mechanism of caseInsensitiveCookies adds cookies to the request which exists from prior requests but possess a divergent spelling.
                var caseInsensitiveCookies = _authentication.get_CaseInsensitiveCookies(uri);

                if (caseInsensitiveCookies.Count > 0)
                    _authentication.Cookies.Add(caseInsensitiveCookies);
                do
                {
                    var request = CreateHttpWebRequest(uri, method, content, contentType, headers, offset, length);

                    redirectionCounter -= 1;
                    retVal = (sn.HttpWebResponse)request.GetResponse();
                    // all statuscodes between 300 and 399 handled as redirection
                    if ((int)retVal.StatusCode < 300 || (int)retVal.StatusCode >= 400 || redirectionCounter == 0)
                    {
                        _authentication.CaseInsensitiveCookies = retVal.Cookies;
                        return retVal;
                    }
                    uri = new Uri(retVal.Headers[sn.HttpResponseHeader.Location]);
                }
                while (true);
            }
            finally
            {
                if (content is not null)
                    content.Dispose();
            }
        }

        #endregion

        protected AuthenticationProvider _authentication;
        public AuthenticationInfo Authentication
        {
            get
            {
                return _authentication;
            }
        }

        public abstract enumClientType ClientType { get; }

        protected int? _connectTimeout;
        /// <summary>
      /// Timeout HttpWebRequest.Timeout. If not set default is used.
      /// </summary>
      /// <returns></returns>
        public int? ConnectTimeout
        {
            get
            {
                return _connectTimeout;
            }
        }

        protected int? _readWriteTimeout;
        /// <summary>
      /// Timeout read or write operations (HttpWebRequest.ReadWriteTimeout). If not set default is used.
      /// </summary>
      /// <returns></returns>
        public int? ReadWriteTimeout
        {
            get
            {
                return _readWriteTimeout;
            }
        }

        protected Uri _serviceDocUri;
        /// <summary>
      /// The base address of the cmis-service the client is connected with
      /// </summary>
      /// <returns></returns>
        public Uri ServiceDocUri
        {
            get
            {
                return _serviceDocUri;
            }
        }

        protected abstract string UserAgent { get; }

        protected Vendors.Vendor _vendor;
        public Vendors.Vendor Vendor
        {
            get
            {
                return _vendor;
            }
        }
    }

    namespace Generic
    {
        public abstract class CmisClient<TRespositoryInfo> : CmisClient
        {

            private const string csRepositoryCachePattern = "urn:f1a34b95d1164b6da869fca38e6a19a3:{0}";

            #region Constructors
            protected CmisClient(Uri serviceDocUri, enumVendor vendor, AuthenticationProvider authentication, int? connectTimeout = default, int? readWriteTimeout = default) : base(serviceDocUri, vendor, authentication, connectTimeout, readWriteTimeout)
            {
            }
            #endregion

            #region Cache
            protected object _syncObject = new object();
            /// <summary>
         /// Access to cached repositoryInfo
         /// </summary>
         /// <param name="repositoryId"></param>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            protected TRespositoryInfo get_RepositoryInfo(string repositoryId)
            {
                string key = string.Format(csRepositoryCachePattern, ServiceURIs.GetServiceUri(_serviceDocUri.AbsoluteUri, ServiceURIs.enumRepositoriesUri.repositoryId).ReplaceUri("repositoryId", repositoryId));
                object retVal;

                lock (_syncObject)
                    retVal = System.Runtime.Caching.MemoryCache.Default[key];
                if (retVal is TRespositoryInfo)
                {
                    // renew lease time
                    System.Threading.ThreadPool.QueueUserWorkItem(state => { lock (_syncObject) { System.Runtime.Caching.MemoryCache.Default.Remove(key); System.Runtime.Caching.MemoryCache.Default.Add(key, retVal, DateTimeOffset.Now.AddSeconds(AppSettings.CacheLeaseTime)); } });
                    return Conversions.ToGenericParameter<TRespositoryInfo>(retVal);
                }
                else
                {
                    return default;
                }
            }

            protected void set_RepositoryInfo(string repositoryId, TRespositoryInfo value)
            {
                string key = string.Format(csRepositoryCachePattern, ServiceURIs.GetServiceUri(_serviceDocUri.AbsoluteUri, ServiceURIs.enumRepositoriesUri.repositoryId).ReplaceUri("repositoryId", repositoryId));
                lock (_syncObject)
                {
                    System.Runtime.Caching.MemoryCache.Default.Remove(key);
                    if (value is not null)
                    {
                        System.Runtime.Caching.MemoryCache.Default.Add(key, value, DateTimeOffset.Now.AddSeconds(AppSettings.CacheLeaseTime));
                    }
                    else
                    {
                        System.Runtime.Caching.MemoryCache.Default.Remove(key);
                    }
                }
            }
            #endregion

        }
    }
}