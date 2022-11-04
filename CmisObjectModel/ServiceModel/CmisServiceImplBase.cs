using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using sn = System.Net;
using ss = System.ServiceModel;
using sss = System.ServiceModel.Syndication;
using ssw = System.ServiceModel.Web;
using cs = CmisObjectModel.ServiceModel;
using Microsoft.VisualBasic.CompilerServices;
using CmisObjectModel.Common;
using CmisObjectModel.Common.Generic;
using CmisObjectModel.Core.Definitions.Types;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel
{
    /// <summary>
    /// Baseclass for classes that implement Contracts.ICmisServicesImpl
    /// </summary>
    /// <remarks></remarks>
    public abstract class CmisServiceImplBase : CmisObjectModel.Contracts.ICmisServicesImpl
    {

        #region Constructors
        protected CmisServiceImplBase(Uri baseUri)
        {
            if (baseUri is null || baseUri.OriginalString.EndsWith(Conversions.ToString('/')))
            {
                _baseUri = baseUri;
            }
            else
            {
                _baseUri = new Uri(baseUri.OriginalString + "/");
            }
        }
        #endregion

        #region Helper-classes
        /// <summary>
        /// Creator of needed cmis-links
        /// </summary>
        /// <remarks></remarks>
        private class LinkFactory
        {

            private string _id;
            private CmisServiceImplBase _owner;
            private string _repositoryId;
            private Uri _selfLinkUri;
            public LinkFactory(CmisServiceImplBase owner, string repositoryId, string id, Uri selfLinkUri)
            {
                _owner = owner;
                _repositoryId = repositoryId;
                _id = id;
            }

            /// <summary>
            /// Appends navigation-links (first/next/previous/last) if the list only returns a part of the complete cmis list.
            /// </summary>
            /// <param name="links"></param>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <remarks></remarks>
            private void AppendNavigationLinks(List<CmisObjectModel.AtomPub.AtomLink> links, long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                // request may be incomplete
                if (hasMoreItems || nMaxItems.HasValue)
                {
                    long skipCount = nSkipCount.HasValue ? nSkipCount.Value : 0L;
                    long maxItems = nMaxItems.HasValue ? nMaxItems.Value : currentEntries;
                    long numCount = nNumCount.HasValue ? nNumCount.Value : hasMoreItems ? long.MaxValue : skipCount + currentEntries;
                    var queryStrings = new List<string>();
                    var regEx = new System.Text.RegularExpressions.Regex(@"\A(maxitems|skipcount)\Z");
                    var currentRequestUri = CurrentRequestUri;
                    string absolutePath = currentRequestUri.AbsolutePath;
                    string uriTemplate;
                    long intValue;

                    // remove maxItems and skipCount ...
                    {
                        var withBlock = System.Web.HttpUtility.ParseQueryString(currentRequestUri.Query);
                        foreach (string key in withBlock.AllKeys)
                        {
                            var match = key is null ? null : regEx.Match(key);
                            if (match is null || !match.Success)
                            {
                                queryStrings.Add(key + "=" + Uri.EscapeDataString(withBlock[key]));
                            }
                        }
                    }
                    // ... and put them to the end
                    queryStrings.Add("maxItems=" + maxItems.ToString() + "&skipCount={skipCount}");
                    if (!absolutePath.EndsWith(Conversions.ToString('/')))
                        absolutePath += "/";
                    uriTemplate = string.Format("{0}{1}{2}{3}?{4}", currentRequestUri.Scheme, Uri.SchemeDelimiter, currentRequestUri.Authority, absolutePath, string.Join("&", queryStrings.ToArray()));

                    // first
                    links.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", "0")), CmisObjectModel.Constants.LinkRelationshipTypes.First, CmisObjectModel.Constants.MediaTypes.Feed));
                    // next (only if there are more objects following the current entries)
                    intValue = skipCount + currentEntries;
                    if (intValue < numCount)
                    {
                        links.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), CmisObjectModel.Constants.LinkRelationshipTypes.Next, CmisObjectModel.Constants.MediaTypes.Feed));
                    }
                    // previous (only if objects in the current feed has been skipped)
                    if (skipCount > 0L)
                    {
                        intValue = Math.Max(0L, skipCount - maxItems);
                        links.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), CmisObjectModel.Constants.LinkRelationshipTypes.Previous, CmisObjectModel.Constants.MediaTypes.Feed));
                    }
                    // last (only if the value of numCount is defined)
                    if (nNumCount.HasValue)
                    {
                        intValue = Math.Max(0L, numCount - maxItems);
                        links.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), CmisObjectModel.Constants.LinkRelationshipTypes.Last, CmisObjectModel.Constants.MediaTypes.Feed));
                    }
                }
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetAllVersions()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateAllVersionsLinks()
            {
                var retVal = CreateLinks();

                // via
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(_owner._baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // first, next, previous, last (defined in 3.10.5 All Versions Feed; how?)

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetCheckedOutDocs()-request
            /// </summary>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateCheckedOutLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();

                AppendNavigationLinks(retVal, currentEntries, nNumCount, hasMoreItems, nSkipCount, nMaxItems);

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetChildren()-request
            /// </summary>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateChildrenLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Up, CmisObjectModel.Constants.MediaTypes.Entry));
                }
                // down
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_DescendantsUri(CmisObjectModel.Constants.ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Tree));
                // foldertree
                if (repositoryInfo.Capabilities.CapabilityGetFolderTree)
                {
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_FolderTreeUri(CmisObjectModel.Constants.ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.FolderTree, CmisObjectModel.Constants.MediaTypes.Feed));
                }
                // first, next, previous, last
                AppendNavigationLinks(retVal, currentEntries, nNumCount, hasMoreItems, nSkipCount, nMaxItems);

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetContentChanges()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateContentChangesLinks()
            {
                return CreateLinks();
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetDescendants()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateDescendantsLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Up, CmisObjectModel.Constants.MediaTypes.Entry));
                }
                // down
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ChildrenUri(CmisObjectModel.Constants.ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Feed));
                // foldertree
                if (repositoryInfo.Capabilities.CapabilityGetFolderTree)
                {
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_FolderTreeUri(CmisObjectModel.Constants.ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.FolderTree, CmisObjectModel.Constants.MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetFolderTree()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateFolderTreeLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Up, CmisObjectModel.Constants.MediaTypes.Entry));
                }
                // down
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ChildrenUri(CmisObjectModel.Constants.ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Feed));
                // down
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_DescendantsUri(CmisObjectModel.Constants.ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Tree));

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from all cmis-requests with feed or tree results
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            private List<CmisObjectModel.AtomPub.AtomLink> CreateLinks()
            {
                var retVal = new List<CmisObjectModel.AtomPub.AtomLink>() { new CmisObjectModel.AtomPub.AtomLink(new Uri(_owner._baseUri, CmisObjectModel.Constants.ServiceURIs.GetRepositoryInfo.ReplaceUri("repositoryId", _repositoryId)), CmisObjectModel.Constants.LinkRelationshipTypes.Service, CmisObjectModel.Constants.MediaTypes.Service) };

                if (_selfLinkUri is not null)
                {
                    // according to guidelines 3.5.1 Feeds
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(_selfLinkUri, CmisObjectModel.Constants.LinkRelationshipTypes.Self, CmisObjectModel.Constants.MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetObjectParents()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateObjectParentLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;

                // via (not defined in 3.10.1 Object Parents Feed; forgotten?)
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // first, next, previous, last (defined in 3.10.1 Object Parents Feed; how?)

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetObjectRelationships()-request
            /// </summary>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateObjectRelationshipsLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();

                // first, next, previous, last
                AppendNavigationLinks(retVal, currentEntries, nNumCount, hasMoreItems, nSkipCount, nMaxItems);

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetAppliedPolicies()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreatePoliciesLinks()
            {
                var retVal = CreateLinks();

                // via
                retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(_owner._baseUri, CmisObjectModel.Constants.ServiceURIs.get_ObjectUri(CmisObjectModel.Constants.ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                // first, next, previous, last (defined in 3.9.3 Policies Collection; how?)

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the Query()-request
            /// </summary>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateQueryLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();

                // first, next, previous, last
                AppendNavigationLinks(retVal, currentEntries, nNumCount, hasMoreItems, nSkipCount, nMaxItems);

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetTypeChildren()-request
            /// </summary>
            /// <param name="currentEntries"></param>
            /// <param name="nNumCount"></param>
            /// <param name="hasMoreItems"></param>
            /// <param name="nSkipCount"></param>
            /// <param name="nMaxItems"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateTypeChildrenLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;

                if (!string.IsNullOrEmpty(_id))
                {
                    // via
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeUri(CmisObjectModel.Constants.ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                    // down
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeDescendantsUri(CmisObjectModel.Constants.ServiceURIs.enumTypeDescendantsUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Tree));
                    // up (only if the currentType is not a baseType)
                    string parentTypeId = _owner.GetParentTypeId(_repositoryId, _id);
                    if (!string.IsNullOrEmpty(parentTypeId))
                    {
                        retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeUri(CmisObjectModel.Constants.ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", parentTypeId)), CmisObjectModel.Constants.LinkRelationshipTypes.Up, CmisObjectModel.Constants.MediaTypes.Entry));
                    }
                }
                else
                {
                    // down
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeDescendantsUri(CmisObjectModel.Constants.ServiceURIs.enumTypeDescendantsUri.none).ReplaceUri("repositoryId", _repositoryId)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Tree));
                }
                // first, next, previous, last
                AppendNavigationLinks(retVal, currentEntries, nNumCount, hasMoreItems, nSkipCount, nMaxItems);

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetTypeDescendants()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateTypeDescendantsLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner._baseUri;

                if (!string.IsNullOrEmpty(_id))
                {
                    // via
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeUri(CmisObjectModel.Constants.ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Via, CmisObjectModel.Constants.MediaTypes.Entry));
                    // up (only if the currentType is not a baseType)
                    string parentTypeId = _owner.GetParentTypeId(_repositoryId, _id);
                    if (!string.IsNullOrEmpty(parentTypeId))
                    {
                        retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypeUri(CmisObjectModel.Constants.ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", parentTypeId)), CmisObjectModel.Constants.LinkRelationshipTypes.Up, CmisObjectModel.Constants.MediaTypes.Entry));
                    }
                    // down
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypesUri(CmisObjectModel.Constants.ServiceURIs.enumTypesUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Feed));
                }
                else
                {
                    // down
                    retVal.Add(new CmisObjectModel.AtomPub.AtomLink(new Uri(baseUri, CmisObjectModel.Constants.ServiceURIs.get_TypesUri(CmisObjectModel.Constants.ServiceURIs.enumTypesUri.none).ReplaceUri("repositoryId", _repositoryId)), CmisObjectModel.Constants.LinkRelationshipTypes.Down, CmisObjectModel.Constants.MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
            /// Creates links that MUST be returned from the GetUnfiledObjects()-request
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public List<CmisObjectModel.AtomPub.AtomLink> CreateUnfiledObjectsLinks()
            {
                return CreateLinks();
            }
        }

        /// <summary>
        /// LinkUriBuilder for uris based on existing URI and relative URI
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <remarks></remarks>
        private class SelfLinkUriBuilder<TEnum> : CmisObjectModel.Common.Generic.LinkUriBuilder<TEnum> where TEnum : struct
        {

            private Uri _baseUri;
            private Func<TEnum, string> _factory;

            public SelfLinkUriBuilder(Uri baseUri, string repositoryId, Func<TEnum, string> factory)
            {
                _baseUri = baseUri;
                _factory = factory;
                this._searchAndReplace = new List<string>() { "repositoryId", repositoryId };
            }

            public override Uri ToUri()
            {
                return new Uri(_baseUri, _factory.Invoke(Conversions.ToGenericParameter<TEnum>((object)base._flags)).ReplaceUri(this._searchAndReplace.ToArray()));
            }
        }
        #endregion

        #region Repository-Section (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> CreateType(string repositoryId, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType newType);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateType(string repositoryId, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType newType) => CreateType(repositoryId, newType);
        protected abstract Exception DeleteType(string repositoryId, string typeId);
        Exception CmisObjectModel.Contracts.ICmisServicesImpl.DeleteType(string repositoryId, string typeId) => DeleteType(repositoryId, typeId);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.cmisRepositoryInfoType[]> GetRepositories();
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.cmisRepositoryInfoType[]> CmisObjectModel.Contracts.ICmisServicesImpl.GetRepositories() => GetRepositories();
        public abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.cmisRepositoryInfoType> GetRepositoryInfo(string repositoryId);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisTypeDefinitionListType> GetTypeChildren(string repositoryId, string typeId, bool includePropertyDefinitions, long? maxItems, long? skipCount);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisTypeDefinitionListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetTypeChildren(string repositoryId, string typeId, bool includePropertyDefinitions, long? maxItems, long? skipCount) => GetTypeChildren(repositoryId, typeId, includePropertyDefinitions, maxItems, skipCount);

        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisTypeContainer> GetTypeDescendants(string repositoryId, string typeId, bool includePropertyDefinitions, long? depth);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisTypeContainer> CmisObjectModel.Contracts.ICmisServicesImpl.GetTypeDescendants(string repositoryId, string typeId, bool includePropertyDefinitions, long? depth) => GetTypeDescendants(repositoryId, typeId, includePropertyDefinitions, depth);
        protected virtual CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> Login(string repositoryId, string authorization)
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return (CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode>)sn.HttpStatusCode.Forbidden;
            }
            else
            {
                try
                {
                    ssw.WebOperationContext.Current.IncomingRequest.Headers.Remove(sn.HttpRequestHeader.Authorization);
                    ssw.WebOperationContext.Current.IncomingRequest.Headers.Add(sn.HttpRequestHeader.Authorization, authorization);
                    return (CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode>)(GetRepositoryInfo(repositoryId) is null ? sn.HttpStatusCode.Forbidden : sn.HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }

        CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> CmisObjectModel.Contracts.ICmisServicesImpl.Login(string repositoryId, string authorization) => Login(repositoryId, authorization);
        [Obsolete("Treat as a mustoverride function.", true)]
        protected virtual CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> Logout(string repositoryId)
        {
            return (CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode>)sn.HttpStatusCode.OK;
        }

        [Obsolete("Treat as a mustoverride function.", true)]
        CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> CmisObjectModel.Contracts.ICmisServicesImpl.Logout(string repositoryId) => Logout(repositoryId);
        [Obsolete("Treat as a mustoverride function.", true)]
        protected virtual CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> Ping(string repositoryId)
        {
            return (CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode>)sn.HttpStatusCode.OK;
        }

        [Obsolete("Treat as a mustoverride function.", true)]
        CmisObjectModel.Common.Generic.Result<sn.HttpStatusCode> CmisObjectModel.Contracts.ICmisServicesImpl.Ping(string repositoryId) => Ping(repositoryId);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> UpdateType(string repositoryId, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType modifiedType);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> CmisObjectModel.Contracts.ICmisServicesImpl.UpdateType(string repositoryId, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType modifiedType) => UpdateType(repositoryId, modifiedType);
        #endregion

        #region Navigation (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> GetCheckedOutDocs(string repositoryId, string folderId, string filter, long? maxItems, long? skipCount, string renditionFilter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetCheckedOutDocs(string repositoryId, string folderId, string filter, long? maxItems, long? skipCount, string renditionFilter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships) => GetCheckedOutDocs(repositoryId, folderId, filter, maxItems, skipCount, renditionFilter, includeAllowableActions, includeRelationships);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderListType> GetChildren(string repositoryId, string folderId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy, bool includePathSegment);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetChildren(string repositoryId, string folderId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy, bool includePathSegment) => GetChildren(repositoryId, folderId, maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy, includePathSegment);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderContainerType> GetDescendants(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includePathSegment);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderContainerType> CmisObjectModel.Contracts.ICmisServicesImpl.GetDescendants(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includePathSegment) => GetDescendants(repositoryId, folderId, filter, depth, includeAllowableActions, includeRelationships, renditionFilter, includePathSegment);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> GetFolderParent(string repositoryId, string folderId, string filter);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.GetFolderParent(string repositoryId, string folderId, string filter) => GetFolderParent(repositoryId, folderId, filter);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderContainerType> GetFolderTree(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool includePathSegment, string renditionFilter);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectInFolderContainerType> CmisObjectModel.Contracts.ICmisServicesImpl.GetFolderTree(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool includePathSegment, string renditionFilter) => GetFolderTree(repositoryId, folderId, filter, depth, includeAllowableActions, includeRelationships, includePathSegment, renditionFilter);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectParentsType[]> GetObjectParents(string repositoryId, string objectId, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeRelativePathSegment);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectParentsType[]> CmisObjectModel.Contracts.ICmisServicesImpl.GetObjectParents(string repositoryId, string objectId, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeRelativePathSegment) => GetObjectParents(repositoryId, objectId, filter, includeAllowableActions, includeRelationships, renditionFilter, includeRelativePathSegment);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> GetUnfiledObjects(string repositoryId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetUnfiledObjects(string repositoryId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy) => GetUnfiledObjects(repositoryId, maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy);
        #endregion

        #region Object (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.setContentStreamResponse> AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool isLastChunk, string changeToken);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.setContentStreamResponse> CmisObjectModel.Contracts.ICmisServicesImpl.AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool isLastChunk, string changeToken) => AppendContentStream(repositoryId, objectId, contentStream, mimeType, fileName, isLastChunk, changeToken);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> BulkUpdateProperties(string repositoryId, CmisObjectModel.Core.cmisBulkUpdateType data);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.BulkUpdateProperties(string repositoryId, CmisObjectModel.Core.cmisBulkUpdateType data) => BulkUpdateProperties(repositoryId, data);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreateDocument(string repositoryId, CmisObjectModel.Core.cmisObjectType newDocument, string folderId, CmisObjectModel.Messaging.cmisContentStreamType content, CmisObjectModel.Core.enumVersioningState? versioningState, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateDocument(string repositoryId, CmisObjectModel.Core.cmisObjectType newDocument, string folderId, CmisObjectModel.Messaging.cmisContentStreamType content, CmisObjectModel.Core.enumVersioningState? versioningState, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreateDocument(repositoryId, newDocument, folderId, content, versioningState, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreateDocumentFromSource(string repositoryId, string sourceId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string folderId, CmisObjectModel.Core.enumVersioningState? versioningState, string[] policies, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateDocumentFromSource(string repositoryId, string sourceId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string folderId, CmisObjectModel.Core.enumVersioningState? versioningState, string[] policies, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreateDocumentFromSource(repositoryId, sourceId, properties, folderId, versioningState, policies, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreateFolder(string repositoryId, CmisObjectModel.Core.cmisObjectType newFolder, string parentFolderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateFolder(string repositoryId, CmisObjectModel.Core.cmisObjectType newFolder, string parentFolderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreateFolder(repositoryId, newFolder, parentFolderId, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreateItem(string repositoryId, CmisObjectModel.Core.cmisObjectType newItem, string folderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateItem(string repositoryId, CmisObjectModel.Core.cmisObjectType newItem, string folderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreateItem(repositoryId, newItem, folderId, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreatePolicy(string repositoryId, CmisObjectModel.Core.cmisObjectType newPolicy, string folderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreatePolicy(string repositoryId, CmisObjectModel.Core.cmisObjectType newPolicy, string folderId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreatePolicy(repositoryId, newPolicy, folderId, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CreateRelationship(string repositoryId, CmisObjectModel.Core.cmisObjectType newRelationship, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CreateRelationship(string repositoryId, CmisObjectModel.Core.cmisObjectType newRelationship, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CreateRelationship(repositoryId, newRelationship, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.deleteContentStreamResponse> DeleteContentStream(string repositoryId, string objectId, string changeToken);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.deleteContentStreamResponse> CmisObjectModel.Contracts.ICmisServicesImpl.DeleteContentStream(string repositoryId, string objectId, string changeToken) => DeleteContentStream(repositoryId, objectId, changeToken);
        protected abstract Exception DeleteObject(string repositoryId, string objectId, bool allVersions);
        Exception CmisObjectModel.Contracts.ICmisServicesImpl.DeleteObject(string repositoryId, string objectId, bool allVersions) => DeleteObject(repositoryId, objectId, allVersions);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.deleteTreeResponse> DeleteTree(string repositoryId, string folderId, bool allVersions, CmisObjectModel.Core.enumUnfileObject? unfileObjects, bool continueOnFailure);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.deleteTreeResponse> CmisObjectModel.Contracts.ICmisServicesImpl.DeleteTree(string repositoryId, string folderId, bool allVersions, CmisObjectModel.Core.enumUnfileObject? unfileObjects, bool continueOnFailure) => DeleteTree(repositoryId, folderId, allVersions, unfileObjects, continueOnFailure);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.cmisAllowableActionsType> GetAllowableActions(string repositoryId, string id);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.cmisAllowableActionsType> CmisObjectModel.Contracts.ICmisServicesImpl.GetAllowableActions(string repositoryId, string id) => GetAllowableActions(repositoryId, id);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisContentStreamType> GetContentStream(string repositoryId, string objectId, string streamId);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.cmisContentStreamType> CmisObjectModel.Contracts.ICmisServicesImpl.GetContentStream(string repositoryId, string objectId, string streamId) => GetContentStream(repositoryId, objectId, streamId);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> GetObject(string repositoryId, string objectId, string filter, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions, CmisObjectModel.RestAtom.enumReturnVersion? returnVersion, bool? privateWorkingCopy);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.GetObject(string repositoryId, string objectId, string filter, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions, CmisObjectModel.RestAtom.enumReturnVersion? returnVersion, bool? privateWorkingCopy) => GetObject(repositoryId, objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, returnVersion, privateWorkingCopy);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> GetObjectByPath(string repositoryId, string path, string filter, bool? includeAllowableActions, bool? includePolicyIds, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool? includeACL, string renditionFilter);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.GetObjectByPath(string repositoryId, string path, string filter, bool? includeAllowableActions, bool? includePolicyIds, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, bool? includeACL, string renditionFilter) => GetObjectByPath(repositoryId, path, filter, includeAllowableActions, includePolicyIds, includeRelationships, includeACL, renditionFilter);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId) => MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.setContentStreamResponse> SetContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool overwriteFlag, string changeToken);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Messaging.Responses.setContentStreamResponse> CmisObjectModel.Contracts.ICmisServicesImpl.SetContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool overwriteFlag, string changeToken) => SetContentStream(repositoryId, objectId, contentStream, mimeType, fileName, overwriteFlag, changeToken);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> UpdateProperties(string repositoryId, string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string changeToken);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.UpdateProperties(string repositoryId, string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string changeToken) => UpdateProperties(repositoryId, objectId, properties, changeToken);
        #endregion

        #region Multi (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions) => AddObjectToFolder(repositoryId, objectId, folderId, allVersions);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> RemoveObjectFromFolder(string repositoryId, string objectId, string folderId);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.RemoveObjectFromFolder(string repositoryId, string objectId, string folderId) => RemoveObjectFromFolder(repositoryId, objectId, folderId);
        #endregion

        #region Discovery (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<cs.getContentChanges> GetContentChanges(string repositoryId, string filter, long? maxItems, bool? includeACL, bool includePolicyIds, bool includeProperties, ref string changeLogToken);
        CmisObjectModel.Common.Generic.Result<cs.getContentChanges> CmisObjectModel.Contracts.ICmisServicesImpl.GetContentChanges(string repositoryId, string filter, long? maxItems, bool? includeACL, bool includePolicyIds, bool includeProperties, ref string changeLogToken) => GetContentChanges(repositoryId, filter, maxItems, includeACL, includePolicyIds, includeProperties, ref changeLogToken);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> Query(string repositoryId, string q, bool searchAllVersions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includeAllowableActions, long? maxItems, long? skipCount);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.Query(string repositoryId, string q, bool searchAllVersions, CmisObjectModel.Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool includeAllowableActions, long? maxItems, long? skipCount) => Query(repositoryId, q, searchAllVersions, includeRelationships, renditionFilter, includeAllowableActions, maxItems, skipCount);
        #endregion

        #region Versioning (3.6 Resources Overview in the cmis documentation file)
        protected abstract Exception CancelCheckOut(string repositoryId, string objectId);
        Exception CmisObjectModel.Contracts.ICmisServicesImpl.CancelCheckOut(string repositoryId, string objectId) => CancelCheckOut(repositoryId, objectId);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CheckIn(string repositoryId, string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string[] policies, CmisObjectModel.Messaging.cmisContentStreamType content, bool major, string checkInComment, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs = null, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs = null);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CheckIn(string repositoryId, string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string[] policies, CmisObjectModel.Messaging.cmisContentStreamType content, bool major, string checkInComment, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs) => CheckIn(repositoryId, objectId, properties, policies, content, major, checkInComment, addACEs, removeACEs);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CheckOut(string repositoryId, string objectId);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.CheckOut(string repositoryId, string objectId) => CheckOut(repositoryId, objectId);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> GetAllVersions(string repositoryId, string objectId, string versionSeriesId, string filter, bool? includeAllowableActions);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetAllVersions(string repositoryId, string objectId, string versionSeriesId, string filter, bool? includeAllowableActions) => GetAllVersions(repositoryId, objectId, versionSeriesId, filter, includeAllowableActions);
        #endregion

        #region Relationships (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> GetObjectRelationships(string repositoryId, string objectId, bool includeSubRelationshipTypes, CmisObjectModel.Core.enumRelationshipDirection? relationshipDirection, string typeId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetObjectRelationships(string repositoryId, string objectId, bool includeSubRelationshipTypes, CmisObjectModel.Core.enumRelationshipDirection? relationshipDirection, string typeId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions) => GetObjectRelationships(repositoryId, objectId, includeSubRelationshipTypes, relationshipDirection, typeId, maxItems, skipCount, filter, includeAllowableActions);
        #endregion

        #region Policy (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> ApplyPolicy(string repositoryId, string objectId, string policyId);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectType> CmisObjectModel.Contracts.ICmisServicesImpl.ApplyPolicy(string repositoryId, string objectId, string policyId) => ApplyPolicy(repositoryId, objectId, policyId);
        protected abstract CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> GetAppliedPolicies(string repositoryId, string objectId, string filter);
        CmisObjectModel.Common.Generic.Result<cs.cmisObjectListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetAppliedPolicies(string repositoryId, string objectId, string filter) => GetAppliedPolicies(repositoryId, objectId, filter);
        protected abstract Exception RemovePolicy(string repositoryId, string objectId, string policyId);
        Exception CmisObjectModel.Contracts.ICmisServicesImpl.RemovePolicy(string repositoryId, string objectId, string policyId) => RemovePolicy(repositoryId, objectId, policyId);
        #endregion

        #region ACL (3.6 Resources Overview in the cmis documentation file)
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Security.cmisAccessControlListType> ApplyACL(string repositoryId, string objectId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs, CmisObjectModel.Core.enumACLPropagation aclPropagation);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Security.cmisAccessControlListType> CmisObjectModel.Contracts.ICmisServicesImpl.ApplyACL(string repositoryId, string objectId, CmisObjectModel.Core.Security.cmisAccessControlListType addACEs, CmisObjectModel.Core.Security.cmisAccessControlListType removeACEs, CmisObjectModel.Core.enumACLPropagation aclPropagation) => ApplyACL(repositoryId, objectId, addACEs, removeACEs, aclPropagation);
        protected abstract CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Security.cmisAccessControlListType> GetACL(string repositoryId, string objectId, bool onlyBasicPermissions);
        CmisObjectModel.Common.Generic.Result<CmisObjectModel.Core.Security.cmisAccessControlListType> CmisObjectModel.Contracts.ICmisServicesImpl.GetACL(string repositoryId, string objectId, bool onlyBasicPermissions) => GetACL(repositoryId, objectId, onlyBasicPermissions);
        #endregion

        /// <summary>
        /// Adds a cookie to the outgoing response
        /// </summary>
        /// <remarks></remarks>
        protected internal static void AddOutgoingCookie(CmisObjectModel.Common.HttpCookie cookie)
        {
            if (cookie is not null)
            {
                CurrentOutgoingCookies.AddOrReplace(cookie);
            }
        }

        protected Uri _baseUri;
        /// <summary>
        /// Returns the baseUri of the service
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Uri BaseUri
        {
            get
            {
                return _baseUri;
            }
        }

        /// <summary>
        /// Creates a WebFaultException to inform the client about the server-fault
        /// </summary>
        /// <param name="serviceException"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected static Exception CreateException(CmisObjectModel.Messaging.enumServiceException serviceException, string message)
        {
            var httpStatusCode = CmisObjectModel.Common.CommonFunctions.ToHttpStatusCode(serviceException);

            return new ssw.WebFaultException<CmisObjectModel.Messaging.cmisFaultType>(new CmisObjectModel.Messaging.cmisFaultType(httpStatusCode, serviceException, message), httpStatusCode);
        }

        /// <summary>
        /// Returns the authenticationInfo of the current web-request
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CmisObjectModel.Common.AuthenticationInfo CurrentAuthenticationInfo
        {
            get
            {
                return CmisObjectModel.Common.AuthenticationInfo.FromCurrentWebRequest();
            }
        }

        /// <summary>
        /// Returns the preferred language of the current request
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static System.Globalization.CultureInfo CurrentCultureInfo
        {
            get
            {
                const string languagePrefix = "Accept-Language: ";
                string language = ssw.WebOperationContext.Current.IncomingRequest.Headers[sn.HttpRequestHeader.AcceptLanguage];

                if (!string.IsNullOrEmpty(language) && language.StartsWith(languagePrefix))
                {
                    try
                    {
                        return System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag(language.Substring(languagePrefix.Length));
                    }
                    catch (Exception ex)
                    {
                    }
                }

                // default
                return global::My.MyProject.Application.Culture;
            }
        }

        /// <summary>
        /// Returns the cookies sent from client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CmisObjectModel.Collections.HttpCookieContainer CurrentIncomingCookies
        {
            get
            {
                if (ssw.WebOperationContext.Current is not null && ssw.WebOperationContext.Current.IncomingRequest is not null)
                {
                    return new CmisObjectModel.Collections.HttpCookieContainer(ssw.WebOperationContext.Current.IncomingRequest.Headers, "Cookie");
                }
                else
                {
                    return new CmisObjectModel.Collections.HttpCookieContainer(null, null);
                }
            }
        }

        /// <summary>
        /// Returns the cookies that will be returned to the client
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CmisObjectModel.Collections.HttpCookieContainer CurrentOutgoingCookies
        {
            get
            {
                if (ssw.WebOperationContext.Current is not null && ssw.WebOperationContext.Current.OutgoingResponse is not null)
                {
                    return new CmisObjectModel.Collections.HttpCookieContainer(ssw.WebOperationContext.Current.OutgoingResponse.Headers, "Set-Cookie");
                }
                else
                {
                    return new CmisObjectModel.Collections.HttpCookieContainer(null, null);
                }
            }
        }

        /// <summary>
        /// Returns the uri of the current web-request
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Uri CurrentRequestUri
        {
            get
            {
                return ss.OperationContext.Current.IncomingMessageHeaders.To;
            }
        }

        /// <summary>
        /// Returns True if the specified object exists in the repository
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="objectId"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract bool get_Exists(string repositoryId, string objectId);

        public abstract CmisObjectModel.Core.cmisRepositoryInfoType get_RepositoryInfo(string repositoryId);

        public abstract Result<cmisTypeDefinitionType> GetTypeDefinition(string repositoryId, string typeId);

        public abstract cmisTypeDefinitionType TypeDefinition(string repositoryId, string typeId);

        //bool CmisObjectModel.Contracts.ICmisServicesImpl.Exists { get => Exists; }

        /// <summary>
        /// Returns the BaseObjectType of cmisObject specified by objectId
        /// </summary>
        protected abstract CmisObjectModel.Core.enumBaseObjectTypeIds GetBaseObjectType(string repositoryId, string objectId);
        CmisObjectModel.Core.enumBaseObjectTypeIds CmisObjectModel.Contracts.ICmisServicesImpl.GetBaseObjectType(string repositoryId, string objectId) => GetBaseObjectType(repositoryId, objectId);

        /// <summary>
        /// Returns the objectId of the object specified by path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected abstract string GetObjectId(string repositoryId, string path);
        string CmisObjectModel.Contracts.ICmisServicesImpl.GetObjectId(string repositoryId, string path) => GetObjectId(repositoryId, path);

        /// <summary>
        /// Returns the parentTypeId of the specified type or Nothing, if the specified type is a baseType
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected abstract string GetParentTypeId(string repositoryId, string typeId);
        string CmisObjectModel.Contracts.ICmisServicesImpl.GetParentTypeId(string repositoryId, string typeId) => GetParentTypeId(repositoryId, typeId);

        /// <summary>
        /// Returns the cookie for the sessionId or Null, if CmisService does not support a sessionIdCookie
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual string GetSessionIdCookieName()
        {
            return null;
        }

        string CmisObjectModel.Contracts.ICmisServicesImpl.GetSessionIdCookieName() => GetSessionIdCookieName();

        /// <summary>
        /// Returns the author for lists of types or objects
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected abstract sss.SyndicationPerson GetSystemAuthor();
        sss.SyndicationPerson CmisObjectModel.Contracts.ICmisServicesImpl.GetSystemAuthor() => GetSystemAuthor();

        /// <summary>
        /// Log exception called before the cmisService throws an exception
        /// </summary>
        /// <param name="ex"></param>
        /// <remarks></remarks>
        private void LogException(Exception ex)
        {
            try
            {
                var st = new StackTrace();
                foreach (StackFrame sf in st.GetFrames())
                {
                    var method = sf.GetMethod();
                    if (method.Name != "LogException")
                    {
                        LogException(ex, method);
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        void CmisObjectModel.Contracts.ICmisServicesImpl.LogException(Exception ex) => LogException(ex);
        protected abstract void LogException(Exception ex, System.Reflection.MethodBase method);

        public abstract bool ValidateUserNamePassword(string userName, string password);

    }
}