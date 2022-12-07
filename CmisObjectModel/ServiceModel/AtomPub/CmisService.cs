using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sss = System.ServiceModel.Syndication;
using ssw = System.ServiceModel.Web;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using ca = CmisObjectModel.AtomPub;
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
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cm = CmisObjectModel.Messaging;
using css = CmisObjectModel.Serialization.SerializationHelper;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel.AtomPub
{
    /// <summary>
   /// Implements the functionality of the cmis-webservice version 1.1
   /// </summary>
   /// <remarks></remarks>
    public class CmisService : Base.CmisService, Contracts.IAtomPubBinding
    {

        #region Helper-classes
        /// <summary>
      /// Describes the creation-guidance of AtomPub-objects
      /// </summary>
      /// <remarks></remarks>
        private class AtomPubObjectGeneratingGuidance
        {

            public AtomPubObjectGeneratingGuidance(string repositoryId, Contracts.ICmisServicesImpl serviceImpl)
            {
                // initialization
                BaseUri = serviceImpl.BaseUri;
                Repository = serviceImpl.GetRepositoryInfo(repositoryId).Success;
                RepositoryId = repositoryId;
                ServiceImpl = serviceImpl;
            }
            public AtomPubObjectGeneratingGuidance(string repositoryId, Contracts.ICmisServicesImpl serviceImpl, IEnumerable objects, bool hasMoreItems, long? numItems, List<ca.AtomLink> links, string urnSuffix, string methodName, ccg.Nullable<string> changeLogToken = default, long? depth = default, ccg.Nullable<string> filter = default, ccg.Nullable<string> folderId = default, bool? includeACL = default, bool? includeAllowableActions = default, bool? includePathSegment = default, bool? includePolicyIds = default, bool? includeProperties = default, Core.enumIncludeRelationships? includeRelationships = default, bool? includeRelativePathSegment = default, bool? includeSubRelationshipTypes = default, long? maxItems = default, ccg.Nullable<string> objectId = default, ccg.Nullable<string> orderBy = default, ccg.Nullable<string> q = default, Core.enumRelationshipDirection? relationshipDirection = default, ccg.Nullable<string> renditionFilter = default, bool? searchAllVersions = default, long? skipCount = default, ccg.Nullable<string> typeId = default, ccg.Nullable<string> versionSeriesId = default) : this(repositoryId, serviceImpl)
            {
                // initialization
                _currentChangeLogToken = changeLogToken;
                _currentDepth = depth;
                _currentFilter = filter;
                _currentFolderId = folderId;
                _currentHasMoreItems = hasMoreItems;
                _currentIncludeACL = includeACL;
                _currentIncludeAllowableActions = includeAllowableActions;
                _currentIncludePathSegment = includePathSegment;
                _currentIncludePolicyIds = includePolicyIds;
                _currentIncludeProperties = includeProperties;
                _currentIncludeRelationships = includeRelationships;
                _currentIncludeRelativePathSegment = includeRelativePathSegment;
                _currentIncludeSubRelationshipTypes = includeSubRelationshipTypes;
                _currentLinks = links;
                _currentMaxItems = maxItems;
                _currentMethodName = methodName;
                _currentNumItems = numItems;
                _currentObjectId = objectId;
                _currentObjects = objects;
                _currentOrderBy = orderBy;
                _currentQuery = q;
                _currentRelationshipDirection = relationshipDirection;
                _currentRenditionFilter = renditionFilter;
                _currentSearchAllVersions = searchAllVersions;
                _currentSkipCount = skipCount;
                _currentTypeId = typeId;
                _currentUrnSuffix = urnSuffix;
                _currentVersionSeriesId = versionSeriesId;
            }

            #region Transaction
            /// <summary>
         /// Starts a new series of property-modification
         /// </summary>
         /// <remarks></remarks>
            public void BeginTransaction()
            {
                _transactions.Push(_currentTransaction);
                _currentTransaction = new List<Action>();
            }

            /// <summary>
         /// Rollback since BeginTransaction()-call
         /// </summary>
         /// <remarks></remarks>
            public void EndTransaction()
            {
                // rollback all modification since the last BeginTransaction()-call
                foreach (Action rollbackAction in _currentTransaction)
                    rollbackAction.Invoke();
                if (_transactions.Count == 0)
                {
                    _currentTransaction.Clear();
                }
                else
                {
                    _currentTransaction = _transactions.Pop();
                }
            }

            private List<Action> _currentTransaction = new List<Action>();
            private Stack<List<Action>> _transactions = new Stack<List<Action>>();
            #endregion

            #region transactional properties
            private ccg.Nullable<string> _currentChangeLogToken;
            private Stack<ccg.Nullable<string>> _changeLogTokenStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> ChangeLogToken
            {
                get
                {
                    return _currentChangeLogToken;
                }
                set
                {
                    _changeLogTokenStack.Push(_currentChangeLogToken);
                    _currentChangeLogToken = value;
                    _currentTransaction.Add(() => _currentChangeLogToken = _changeLogTokenStack.Pop());
                }
            }
            private long? _currentDepth;
            private Stack<long?> _depthStack = new Stack<long?>();
            public long? Depth
            {
                get
                {
                    return _currentDepth;
                }
                set
                {
                    _depthStack.Push(_currentDepth);
                    _currentDepth = value;
                    _currentTransaction.Add(() => _currentDepth = _depthStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentFilter;
            private Stack<ccg.Nullable<string>> _filterStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> Filter
            {
                get
                {
                    return _currentFilter;
                }
                set
                {
                    _filterStack.Push(_currentFilter);
                    _currentFilter = value;
                    _currentTransaction.Add(() => _currentFilter = _filterStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentFolderId;
            private Stack<ccg.Nullable<string>> _folderIdStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> FolderId
            {
                get
                {
                    return _currentFolderId;
                }
                set
                {
                    _folderIdStack.Push(_currentFolderId);
                    _currentFolderId = value;
                    _currentTransaction.Add(() => _currentFolderId = _folderIdStack.Pop());
                }
            }
            private bool _currentHasMoreItems;
            private Stack<bool> _hasMoreItemsStack = new Stack<bool>();
            public bool HasMoreItems
            {
                get
                {
                    return _currentHasMoreItems;
                }
                set
                {
                    _hasMoreItemsStack.Push(_currentHasMoreItems);
                    _currentHasMoreItems = value;
                    _currentTransaction.Add(() => _currentHasMoreItems = _hasMoreItemsStack.Pop());
                }
            }
            private bool? _currentIncludeACL;
            private Stack<bool?> _includeACLStack = new Stack<bool?>();
            public bool? IncludeACL
            {
                get
                {
                    return _currentIncludeACL;
                }
                set
                {
                    _includeACLStack.Push(_currentIncludeACL);
                    _currentIncludeACL = value;
                    _currentTransaction.Add(() => _currentIncludeACL = _includeACLStack.Pop());
                }
            }
            private bool? _currentIncludeAllowableActions;
            private Stack<bool?> _includeAllowableActionsStack = new Stack<bool?>();
            public bool? IncludeAllowableActions
            {
                get
                {
                    return _currentIncludeAllowableActions;
                }
                set
                {
                    _includeAllowableActionsStack.Push(_currentIncludeAllowableActions);
                    _currentIncludeAllowableActions = value;
                    _currentTransaction.Add(() => _currentIncludeAllowableActions = _includeAllowableActionsStack.Pop());
                }
            }
            private bool? _currentIncludePathSegment;
            private Stack<bool?> _includePathSegmentStack = new Stack<bool?>();
            public bool? IncludePathSegment
            {
                get
                {
                    return _currentIncludePathSegment;
                }
                set
                {
                    _includePathSegmentStack.Push(_currentIncludePathSegment);
                    _currentIncludePathSegment = value;
                    _currentTransaction.Add(() => _currentIncludePathSegment = _includePathSegmentStack.Pop());
                }
            }
            private bool? _currentIncludePolicyIds;
            private Stack<bool?> _includePolicyIdsStack = new Stack<bool?>();
            public bool? IncludePolicyIds
            {
                get
                {
                    return _currentIncludePolicyIds;
                }
                set
                {
                    _includePolicyIdsStack.Push(_currentIncludePolicyIds);
                    _currentIncludePolicyIds = value;
                    _currentTransaction.Add(() => _currentIncludePolicyIds = _includePolicyIdsStack.Pop());
                }
            }
            private bool? _currentIncludeProperties;
            private Stack<bool?> _includePropertiesStack = new Stack<bool?>();
            public bool? IncludeProperties
            {
                get
                {
                    return _currentIncludeProperties;
                }
                set
                {
                    _includePropertiesStack.Push(_currentIncludeProperties);
                    _currentIncludeProperties = value;
                    _currentTransaction.Add(() => _currentIncludeProperties = _includePropertiesStack.Pop());
                }
            }
            private Core.enumIncludeRelationships? _currentIncludeRelationships;
            private Stack<Core.enumIncludeRelationships?> _includeRelationshipsStack = new Stack<Core.enumIncludeRelationships?>();
            public Core.enumIncludeRelationships? IncludeRelationships
            {
                get
                {
                    return _currentIncludeRelationships;
                }
                set
                {
                    _includeRelationshipsStack.Push(_currentIncludeRelationships);
                    _currentIncludeRelationships = value;
                    _currentTransaction.Add(() => _currentIncludeRelationships = _includeRelationshipsStack.Pop());
                }
            }
            private bool? _currentIncludeRelativePathSegment;
            private Stack<bool?> _includeRelativePathSegmentStack = new Stack<bool?>();
            public bool? IncludeRelativePathSegment
            {
                get
                {
                    return _currentIncludeRelativePathSegment;
                }
                set
                {
                    _includeRelativePathSegmentStack.Push(_currentIncludeRelativePathSegment);
                    _currentIncludeRelativePathSegment = value;
                    _currentTransaction.Add(() => _currentIncludeRelativePathSegment = _includeRelativePathSegmentStack.Pop());
                }
            }
            private bool? _currentIncludeSubRelationshipTypes;
            private Stack<bool?> _includeSubRelationshipTypesStack = new Stack<bool?>();
            public bool? IncludeSubRelationshipTypes
            {
                get
                {
                    return _currentIncludeSubRelationshipTypes;
                }
                set
                {
                    _includeSubRelationshipTypesStack.Push(_currentIncludeSubRelationshipTypes);
                    _currentIncludeSubRelationshipTypes = value;
                    _currentTransaction.Add(() => _currentIncludeSubRelationshipTypes = _includeSubRelationshipTypesStack.Pop());
                }
            }
            private List<ca.AtomLink> _currentLinks;
            private Stack<List<ca.AtomLink>> _linksStack = new Stack<List<ca.AtomLink>>();
            public List<ca.AtomLink> Links
            {
                get
                {
                    return _currentLinks;
                }
                set
                {
                    _linksStack.Push(_currentLinks);
                    _currentLinks = value;
                    _currentTransaction.Add(() => _currentLinks = _linksStack.Pop());
                }
            }
            private long? _currentMaxItems;
            private Stack<long?> _maxItemsStack = new Stack<long?>();
            public long? MaxItems
            {
                get
                {
                    return _currentMaxItems;
                }
                set
                {
                    _maxItemsStack.Push(_currentMaxItems);
                    _currentMaxItems = value;
                    _currentTransaction.Add(() => _currentMaxItems = _maxItemsStack.Pop());
                }
            }
            private string _currentMethodName;
            private Stack<string> _methodNameStack = new Stack<string>();
            public string MethodName
            {
                get
                {
                    return _currentMethodName;
                }
                set
                {
                    _methodNameStack.Push(_currentMethodName);
                    _currentMethodName = value;
                    _currentTransaction.Add(() => _currentMethodName = _methodNameStack.Pop());
                }
            }
            private long? _currentNumItems;
            private Stack<long?> _numItemsStack = new Stack<long?>();
            public long? NumItems
            {
                get
                {
                    return _currentNumItems;
                }
                set
                {
                    _numItemsStack.Push(_currentNumItems);
                    _currentNumItems = value;
                    _currentTransaction.Add(() => _currentNumItems = _numItemsStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentObjectId;
            private Stack<ccg.Nullable<string>> _objectIdStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> ObjectId
            {
                get
                {
                    return _currentObjectId;
                }
                set
                {
                    _objectIdStack.Push(_currentObjectId);
                    _currentObjectId = value;
                    _currentTransaction.Add(() => _currentObjectId = _objectIdStack.Pop());
                }
            }
            private IEnumerable _currentObjects;
            private Stack<IEnumerable> _objectsStack = new Stack<IEnumerable>();
            public IEnumerable Objects
            {
                get
                {
                    return _currentObjects;
                }
                set
                {
                    _objectsStack.Push(_currentObjects);
                    _currentObjects = value;
                    _currentTransaction.Add(() => _currentObjects = _objectsStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentOrderBy;
            private Stack<ccg.Nullable<string>> _orderByStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> OrderBy
            {
                get
                {
                    return _currentOrderBy;
                }
                set
                {
                    _orderByStack.Push(_currentOrderBy);
                    _currentOrderBy = value;
                    _currentTransaction.Add(() => _currentOrderBy = _orderByStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentQuery;
            private Stack<ccg.Nullable<string>> _queryStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> Query
            {
                get
                {
                    return _currentQuery;
                }
                set
                {
                    _queryStack.Push(_currentQuery);
                    _currentQuery = value;
                    _currentTransaction.Add(() => _currentQuery = _queryStack.Pop());
                }
            }
            private Core.enumRelationshipDirection? _currentRelationshipDirection;
            private Stack<Core.enumRelationshipDirection?> _relationshipDirectionStack = new Stack<Core.enumRelationshipDirection?>();
            public Core.enumRelationshipDirection? RelationshipDirection
            {
                get
                {
                    return _currentRelationshipDirection;
                }
                set
                {
                    _relationshipDirectionStack.Push(_currentRelationshipDirection);
                    _currentRelationshipDirection = value;
                    _currentTransaction.Add(() => _currentRelationshipDirection = _relationshipDirectionStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentRenditionFilter;
            private Stack<ccg.Nullable<string>> _renditionFilterStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> RenditionFilter
            {
                get
                {
                    return _currentRenditionFilter;
                }
                set
                {
                    _renditionFilterStack.Push(_currentRenditionFilter);
                    _currentRenditionFilter = value;
                    _currentTransaction.Add(() => _currentRenditionFilter = _renditionFilterStack.Pop());
                }
            }
            private bool? _currentSearchAllVersions;
            private Stack<bool?> _searchAllVersionsStack = new Stack<bool?>();
            public bool? SearchAllVersions
            {
                get
                {
                    return _currentSearchAllVersions;
                }
                set
                {
                    _searchAllVersionsStack.Push(_currentSearchAllVersions);
                    _currentSearchAllVersions = value;
                    _currentTransaction.Add(() => _currentSearchAllVersions = _searchAllVersionsStack.Pop());
                }
            }
            private long? _currentSkipCount;
            private Stack<long?> _skipCountStack = new Stack<long?>();
            public long? SkipCount
            {
                get
                {
                    return _currentSkipCount;
                }
                set
                {
                    _skipCountStack.Push(_currentSkipCount);
                    _currentSkipCount = value;
                    _currentTransaction.Add(() => _currentSkipCount = _skipCountStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentTypeId;
            private Stack<ccg.Nullable<string>> _typeIdStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> TypeId
            {
                get
                {
                    return _currentTypeId;
                }
                set
                {
                    _typeIdStack.Push(_currentTypeId);
                    _currentTypeId = value;
                    _currentTransaction.Add(() => _currentTypeId = _typeIdStack.Pop());
                }
            }
            private string _currentUrnSuffix;
            private Stack<string> _UrnSuffixStack = new Stack<string>();
            public string UrnSuffix
            {
                get
                {
                    return _currentUrnSuffix;
                }
                set
                {
                    _UrnSuffixStack.Push(_currentUrnSuffix);
                    _currentUrnSuffix = value;
                    _currentTransaction.Add(() => _currentUrnSuffix = _UrnSuffixStack.Pop());
                }
            }
            private ccg.Nullable<string> _currentVersionSeriesId;
            private Stack<ccg.Nullable<string>> _versionSeriesIdStack = new Stack<ccg.Nullable<string>>();
            public ccg.Nullable<string> VersionSeriesId
            {
                get
                {
                    return _currentVersionSeriesId;
                }
                set
                {
                    _versionSeriesIdStack.Push(_currentVersionSeriesId);
                    _currentVersionSeriesId = value;
                    _currentTransaction.Add(() => _currentVersionSeriesId = _versionSeriesIdStack.Pop());
                }
            }
            #endregion

            public readonly Uri BaseUri;
            public readonly Core.cmisRepositoryInfoType Repository;
            public readonly string RepositoryId;
            public readonly Contracts.ICmisServicesImpl ServiceImpl;
        }
        /// <summary>
      /// Creator of needed cmis-links
      /// </summary>
      /// <remarks></remarks>
        private class LinkFactory
        {

            private string _id;
            private Contracts.ICmisServicesImpl _owner;
            private string _repositoryId;
            private Uri _selfLinkUri;
            public LinkFactory(Contracts.ICmisServicesImpl owner, string repositoryId, string id, Uri selfLinkUri)
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
            private void AppendNavigationLinks(List<ca.AtomLink> links, long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                // request may be incomplete
                if (hasMoreItems || nMaxItems.HasValue)
                {
                    long skipCount = nSkipCount.HasValue ? nSkipCount.Value : 0L;
                    long maxItems = nMaxItems.HasValue ? nMaxItems.Value : currentEntries;
                    long numCount = nNumCount.HasValue ? nNumCount.Value : hasMoreItems ? long.MaxValue : skipCount + currentEntries;
                    var queryStrings = new List<string>();
                    var regEx = new System.Text.RegularExpressions.Regex(@"\A(maxitems|skipcount)\Z");
                    var currentRequestUri = CmisServiceImplBase.CurrentRequestUri;
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
                    links.Add(new ca.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", "0")), LinkRelationshipTypes.First, MediaTypes.Feed));
                    // next (only if there are more objects following the current entries)
                    intValue = skipCount + currentEntries;
                    if (intValue < numCount)
                    {
                        links.Add(new ca.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), LinkRelationshipTypes.Next, MediaTypes.Feed));
                    }
                    // previous (only if objects in the current feed has been skipped)
                    if (skipCount > 0L)
                    {
                        intValue = Math.Max(0L, skipCount - maxItems);
                        links.Add(new ca.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), LinkRelationshipTypes.Previous, MediaTypes.Feed));
                    }
                    // last (only if the value of numCount is defined)
                    if (nNumCount.HasValue)
                    {
                        intValue = Math.Max(0L, numCount - maxItems);
                        links.Add(new ca.AtomLink(new Uri(uriTemplate.ReplaceUri("skipCount", intValue.ToString())), LinkRelationshipTypes.Last, MediaTypes.Feed));
                    }
                }
            }

            /// <summary>
         /// Creates links that MUST be returned from the GetAllVersions()-request
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public List<ca.AtomLink> CreateAllVersionsLinks()
            {
                var retVal = CreateLinks();

                // via
                retVal.Add(new ca.AtomLink(new Uri(_owner.BaseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
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
            public List<ca.AtomLink> CreateCheckedOutLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
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
            public List<ca.AtomLink> CreateChildrenLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), LinkRelationshipTypes.Up, MediaTypes.Entry));
                }
                // down
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_DescendantsUri(ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Tree));
                // foldertree
                if (repositoryInfo.Capabilities.CapabilityGetFolderTree)
                {
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_FolderTreeUri(ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), LinkRelationshipTypes.FolderTree, MediaTypes.Feed));
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
            public List<ca.AtomLink> CreateContentChangesLinks()
            {
                return CreateLinks();
            }

            /// <summary>
         /// Creates links that MUST be returned from the GetDescendants()-request
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public List<ca.AtomLink> CreateDescendantsLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), LinkRelationshipTypes.Up, MediaTypes.Entry));
                }
                // down
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ChildrenUri(ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Feed));
                // foldertree
                if (repositoryInfo.Capabilities.CapabilityGetFolderTree)
                {
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_FolderTreeUri(ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), LinkRelationshipTypes.FolderTree, MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
         /// Creates links that MUST be returned from the GetFolderTree()-request
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public List<ca.AtomLink> CreateFolderTreeLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;
                var repositoryInfo = _owner.GetRepositoryInfo(_repositoryId).Success;

                // via
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
                // up
                if ((_id ?? "") != (repositoryInfo.RootFolderId ?? ""))
                {
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.folderId).ReplaceUri("repositoryId", _repositoryId, "folderId", _id)), LinkRelationshipTypes.Up, MediaTypes.Entry));
                }
                // down
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ChildrenUri(ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Feed));
                // down
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_DescendantsUri(ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Tree));

                return retVal;
            }

            /// <summary>
         /// Creates links that MUST be returned from all cmis-requests with feed or tree results
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            private List<ca.AtomLink> CreateLinks()
            {
                var retVal = new List<ca.AtomLink>() { new ca.AtomLink(new Uri(_owner.BaseUri, ServiceURIs.GetRepositoryInfo.ReplaceUri("repositoryId", _repositoryId)), LinkRelationshipTypes.Service, MediaTypes.Service) };

                if (_selfLinkUri is not null)
                {
                    // according to guidelines 3.5.1 Feeds
                    retVal.Add(new ca.AtomLink(_selfLinkUri, LinkRelationshipTypes.Self, MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
         /// Creates links that MUST be returned from the GetObjectParents()-request
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public List<ca.AtomLink> CreateObjectParentLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;

                // via (not defined in 3.10.1 Object Parents Feed; forgotten?)
                retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
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
            public List<ca.AtomLink> CreateObjectRelationshipsLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
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
            public List<ca.AtomLink> CreatePoliciesLinks()
            {
                var retVal = CreateLinks();

                // via
                retVal.Add(new ca.AtomLink(new Uri(_owner.BaseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
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
            public List<ca.AtomLink> CreateQueryLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
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
            public List<ca.AtomLink> CreateTypeChildrenLinks(long currentEntries, long? nNumCount, bool hasMoreItems, long? nSkipCount, long? nMaxItems)
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;

                if (!string.IsNullOrEmpty(_id))
                {
                    // via
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
                    // down
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeDescendantsUri(ServiceURIs.enumTypeDescendantsUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Tree));
                    // up (only if the currentType is not a baseType)
                    string parentTypeId = _owner.GetParentTypeId(_repositoryId, _id);
                    if (!string.IsNullOrEmpty(parentTypeId))
                    {
                        retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", parentTypeId)), LinkRelationshipTypes.Up, MediaTypes.Entry));
                    }
                }
                else
                {
                    // down
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeDescendantsUri(ServiceURIs.enumTypeDescendantsUri.none).ReplaceUri("repositoryId", _repositoryId)), LinkRelationshipTypes.Down, MediaTypes.Tree));
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
            public List<ca.AtomLink> CreateTypeDescendantsLinks()
            {
                var retVal = CreateLinks();
                var baseUri = _owner.BaseUri;

                if (!string.IsNullOrEmpty(_id))
                {
                    // via
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Via, MediaTypes.Entry));
                    // up (only if the currentType is not a baseType)
                    string parentTypeId = _owner.GetParentTypeId(_repositoryId, _id);
                    if (!string.IsNullOrEmpty(parentTypeId))
                    {
                        retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", parentTypeId)), LinkRelationshipTypes.Up, MediaTypes.Entry));
                    }
                    // down
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypesUri(ServiceURIs.enumTypesUri.typeId).ReplaceUri("repositoryId", _repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Feed));
                }
                else
                {
                    // down
                    retVal.Add(new ca.AtomLink(new Uri(baseUri, ServiceURIs.get_TypesUri(ServiceURIs.enumTypesUri.none).ReplaceUri("repositoryId", _repositoryId)), LinkRelationshipTypes.Down, MediaTypes.Feed));
                }

                return retVal;
            }

            /// <summary>
         /// Creates links that MUST be returned from the GetUnfiledObjects()-request
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public List<ca.AtomLink> CreateUnfiledObjectsLinks()
            {
                return CreateLinks();
            }
        }

        /// <summary>
      /// LinkUriBuilder for uris based on existing URI and relative URI
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <remarks></remarks>
        private class SelfLinkUriBuilder<TEnum> : ccg.LinkUriBuilder<TEnum> where TEnum : struct
        {

            private Uri _baseUri;
            private Func<TEnum, string> _factory;

            public SelfLinkUriBuilder(Uri baseUri, string repositoryId, Func<TEnum, string> factory)
            {
                _baseUri = baseUri;
                _factory = factory;
                _searchAndReplace = new List<string>() { "repositoryId", repositoryId };
            }

            public override Uri ToUri()
            {
                return new Uri(_baseUri, _factory.Invoke(Conversions.ToGenericParameter<TEnum>(_flags)).ReplaceUri(_searchAndReplace.ToArray()));
            }
        }
        #endregion

        #region Repository
        /// <summary>
      /// Creates a new type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument CreateType(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (data is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("type"), serviceImpl);

            using (var ms = new System.IO.MemoryStream())
            {
                data.CopyTo(ms);
                try
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    result = serviceImpl.CreateType(repositoryId, (ccdt.cmisTypeDefinitionType)ToAtomEntry(ms, true));
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        var type = result.Success;

                        if (type is null)
                        {
                            return null;
                        }
                        else
                        {
                            var entry = new ca.AtomEntry(type, type.GetLinks(serviceImpl.BaseUri, repositoryId), serviceImpl.GetSystemAuthor());

                            context.ContentType = MediaTypes.Entry;
                            context.StatusCode = System.Net.HttpStatusCode.Created;

                            AddLocation(context, repositoryId, entry.TypeId, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId));

                            return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }
                finally
                {
                    ms.Close();
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="typeId"></param>
      /// <remarks></remarks>
        public void DeleteType(string repositoryId, string typeId)
        {
            Exception failure;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(typeId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("typeId"), serviceImpl);

            try
            {
                failure = serviceImpl.DeleteType(repositoryId, typeId);
                if (failure is null)
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                }
                else if (!IsWebException(failure))
                {
                    failure = cm.cmisFaultType.CreateUnknownException(failure);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    failure = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            if (failure is not null)
                throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Returns the CMIS service-documents for all available repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new sx.XmlDocument GetRepositories()
        {
            string repositoryId = CommonFunctions.GetRequestParameter("repositoryId");

            if (string.IsNullOrEmpty(repositoryId))
            {
                return GetRepositories(SerializeRepositories);
            }
            else
            {
                // redirect request to address of GetRepositoryInfo()
                string location = CmisServiceImpl.BaseUri.AbsoluteUri;

                location += location.EndsWith("/") ? repositoryId : "/" + repositoryId;
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
                ssw.WebOperationContext.Current.OutgoingResponse.Location = location;

                return null;
            }
        }

        /// <summary>
      /// Returns the CMIS service-document for the specified repository
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public new sx.XmlDocument GetRepositoryInfo(string repositoryId)
        {
            return GetRepositoryInfo(repositoryId, SerializeRepositories);
        }

        /// <summary>
      /// Returns all child types of the specified type, if defined, otherwise the basetypes of the repository.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks>
      /// Optional parameters:
      /// typeId, includePropertyDefinitions, maxItems, skipCount
      /// </remarks>
        public sx.XmlDocument GetTypeChildren(string repositoryId)
        {
            ccg.Result<cm.cmisTypeDefinitionListType> result;
            var serviceImpl = CmisServiceImpl;
            // get the optional parameters from the queryString
            string typeId = CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.typeId) ?? CommonFunctions.GetRequestParameter("id");
            var includePropertyDefinitions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.includePropertyDefinitions));
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumTypesUri.skipCount));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            try
            {
                result = serviceImpl.GetTypeChildren(repositoryId, typeId, includePropertyDefinitions.HasValue && includePropertyDefinitions.Value, maxItems, skipCount);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    ca.AtomFeed feed;
                    var baseUri = serviceImpl.BaseUri;
                    var typeList = result.Success ?? new cm.cmisTypeDefinitionListType();
                    var types = typeList.Types;
                    var entries = types is null ? new List<ca.AtomEntry>() : (from type in types
                                                                              where type is not null
                                                                              select new ca.AtomEntry(type, type.GetLinks(baseUri, repositoryId), serviceImpl.GetSystemAuthor())).ToList();
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumTypesUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_TypesUri(queryString));
                        withBlock.Add(ServiceURIs.enumTypesUri.typeId, typeId);
                        withBlock.Add(ServiceURIs.enumTypesUri.includePropertyDefinitions, includePropertyDefinitions);
                        withBlock.Add(ServiceURIs.enumTypesUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumTypesUri.skipCount, skipCount);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, typeId, withBlock.ToUri());
                            var links = withBlock1.CreateTypeChildrenLinks(entries.Count, typeList.NumItems, typeList.HasMoreItems, skipCount, maxItems);
                            feed = new ca.AtomFeed("urn:feeds:typeChildren:" + typeId, "Result of GetTypeChildren('" + repositoryId + "', typeId:='" + typeId + "', includePropertyDefinitions:=" + (includePropertyDefinitions.HasValue ? Conversions.ToString(includePropertyDefinitions.Value) : "Null") + ", maxItems:=" + (maxItems.HasValue ? maxItems.Value.ToString() : "Null") + ", skipCount" + (skipCount.HasValue ? skipCount.Value.ToString() : "Null") + ")", DateTimeOffset.Now, entries, typeList.HasMoreItems, typeList.NumItems, links, serviceImpl.GetSystemAuthor());
                        }
                    }

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the type-definition of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetTypeDefinition(string repositoryId, string typeId)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(typeId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("typeId"), serviceImpl);

            try
            {
                result = serviceImpl.TypeDefinition(repositoryId, typeId);

                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var type = result.Success;

                    if (type is null)
                    {
                        return null;
                    }
                    else
                    {
                        var entry = new ca.AtomEntry(type, type.GetLinks(serviceImpl.BaseUri, repositoryId), serviceImpl.GetSystemAuthor());
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        context.ContentType = MediaTypes.Entry;
                        context.StatusCode = System.Net.HttpStatusCode.OK;

                        return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the descendant object-types under the specified type.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="id">TypeId; optional
      /// If specified, then the repository MUST return all of descendant types of the speciﬁed type
      /// If not specified, then the Repository MUST return all types and MUST ignore the value of the depth parameter</param>
      /// <param name="includePropertyDefinitions">If TRUE, then the repository MUST return the property deﬁnitions for each object-type.
      /// If FALSE (default), the repository MUST return only the attributes for each object-type</param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetTypeDescendants(string repositoryId, string id, string includePropertyDefinitions, string depth)
        {
            ccg.Result<cm.cmisTypeContainer> result;
            var serviceImpl = CmisServiceImpl;
            var nDepth = CommonFunctions.ParseInteger(depth);
            var nIncludePropertyDefinitions = CommonFunctions.ParseBoolean(includePropertyDefinitions);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (nDepth.HasValue && (nDepth.Value == 0L || nDepth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'depth' MUST NOT be 0 or less than -1", false), serviceImpl);

            try
            {
                result = serviceImpl.GetTypeDescendants(repositoryId, id, nIncludePropertyDefinitions.HasValue && nIncludePropertyDefinitions.Value, nDepth);

                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var typeContainer = result.Success ?? new cm.cmisTypeContainer();
                    // result.Type ist Nothing, wenn die gesamte Type-Hierarchie des Repositories abgefragt wurde
                    var feed = CreateAtomFeed(repositoryId, id, nIncludePropertyDefinitions.HasValue && nIncludePropertyDefinitions.Value, nDepth, typeContainer.Type is null ? typeContainer.Children : (new cm.cmisTypeContainer[] { typeContainer }), serviceImpl.BaseUri);
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="data">A type definition object with the property definitions that are to change.
      /// Repositories MUST ignore all fields in the type definition except for the type id and the list of properties.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument UpdateType(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<ccdt.cmisTypeDefinitionType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (data is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("type"), serviceImpl);

            using (var ms = new System.IO.MemoryStream())
            {
                data.CopyTo(ms);

                try
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    result = serviceImpl.UpdateType(repositoryId, (ccdt.cmisTypeDefinitionType)ToAtomEntry(ms, false));
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        var type = result.Success;

                        if (type is null)
                        {
                            return null;
                        }
                        else
                        {
                            var entry = new ca.AtomEntry(type, type.GetLinks(serviceImpl.BaseUri, repositoryId), serviceImpl.GetSystemAuthor());

                            context.ContentType = MediaTypes.Entry;
                            context.StatusCode = System.Net.HttpStatusCode.OK;
                            AddLocation(context, repositoryId, entry.TypeId, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId));

                            return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }
                finally
                {
                    ms.Close();
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Navigation
        /// <summary>
      /// Returns a list of check out object the user has access to.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// 
      /// Optional parameters:
      /// folderId, maxItems, skipCount, orderBy, filter, includeAllowableActions, includeRelationships, renditionFilter
      /// </remarks>
        public sx.XmlDocument GetCheckedOutDocs(string repositoryId)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;
            // get the optional parameters from the queryString
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.skipCount));
            string orderBy = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.orderBy);
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.renditionFilter);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            try
            {
                result = serviceImpl.GetCheckedOutDocs(repositoryId, folderId, filter, maxItems, skipCount, renditionFilter, includeAllowableActions, includeRelationships);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objectList = result.Success ?? new cmisObjectListType();
                    ca.AtomFeed feed;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumCheckedOutUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_CheckedOutUri(queryString));
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.folderId, folderId);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.skipCount, skipCount);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumCheckedOutUri.includeRelationships, includeRelationships);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, folderId, withBlock.ToUri());
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                            *//* TODO ERROR: Skipped DisabledTextTrivia
                                                 Dim links = .CreateCheckedOutLinks(If(objectList.Objects Is Nothing, 0, objectList.Objects.Length), objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems)
                            *//* TODO ERROR: Skipped ElseDirectiveTrivia
                            #Else
                            */
                            var links = withBlock1.CreateCheckedOutLinks(objectList.Objects is null ? 0L : objectList.Objects.LongLength, objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "checkedOutDocs:" + folderId, "GetCheckedOutDocs", filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), folderId: string.IsNullOrEmpty(folderId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(folderId), includeAllowableActions: includeAllowableActions, includeRelationships: includeRelationships, maxItems: maxItems, orderBy: string.IsNullOrEmpty(orderBy) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(orderBy), renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter), skipCount: skipCount);
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns all children of the specified CMIS object.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns>
      /// Required parameters:
      /// folderId
      /// Optional parameters:
      /// maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy, includePathSegment
      /// </returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetChildren(string repositoryId)
        {
            ccg.Result<cmisObjectInFolderListType> result;
            var serviceImpl = CmisServiceImpl;
            // get the required ...
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // ... and optional parameters from the queryString
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.skipCount));
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.renditionFilter);
            string orderBy = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.orderBy);
            var includePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.includePathSegment));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            try
            {
                result = serviceImpl.GetChildren(repositoryId, folderId, maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy, includePathSegment.HasValue && includePathSegment.Value);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;
                    var objectList = result.Success ?? new cmisObjectInFolderListType();

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumChildrenUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_ChildrenUri(queryString));
                        withBlock.Add(ServiceURIs.enumChildrenUri.folderId, folderId);
                        withBlock.Add(ServiceURIs.enumChildrenUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumChildrenUri.skipCount, skipCount);
                        withBlock.Add(ServiceURIs.enumChildrenUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumChildrenUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumChildrenUri.includeRelationships, includeRelationships);
                        withBlock.Add(ServiceURIs.enumChildrenUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumChildrenUri.orderBy, orderBy);
                        withBlock.Add(ServiceURIs.enumChildrenUri.includePathSegment, includePathSegment);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, folderId, withBlock.ToUri());
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                            *//* TODO ERROR: Skipped DisabledTextTrivia
                                                 links = .CreateChildrenLinks(If(objects.Objects Is Nothing, 0, objects.Objects.Length), objects.NumItems, objects.HasMoreItems, skipCount, maxItems)
                            *//* TODO ERROR: Skipped ElseDirectiveTrivia
                            #Else
                            */
                            links = withBlock1.CreateChildrenLinks(objectList.Objects is null ? 0L : objectList.Objects.LongLength, objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "children:" + folderId, "GetChildren", filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), folderId: string.IsNullOrEmpty(folderId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(folderId), includeAllowableActions: includeAllowableActions, includeRelationships: includeRelationships, maxItems: maxItems, orderBy: string.IsNullOrEmpty(orderBy) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(orderBy), renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter), skipCount: skipCount);
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

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
        public sx.XmlDocument GetDescendants(string repositoryId, string folderId, string filter, string depth, string includeAllowableActions, string includeRelationships, string renditionFilter, string includePathSegment)
        {
            ccg.Result<cmisObjectInFolderContainerType> result;
            var serviceImpl = CmisServiceImpl;
            var nDepth = CommonFunctions.ParseInteger(depth);
            var nIncludeAllowableActions = CommonFunctions.ParseBoolean(includeAllowableActions);
            var nIncludePathSegment = CommonFunctions.ParseBoolean(includePathSegment);
            var nIncludeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(includeRelationships);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (nDepth.HasValue && (nDepth.Value == 0L || nDepth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("The parameter 'depth' MUST NOT be 0 or less than -1", false), serviceImpl);

            try
            {
                result = serviceImpl.GetDescendants(repositoryId, folderId, filter, nDepth, nIncludeAllowableActions, nIncludeRelationships, renditionFilter, nIncludePathSegment.HasValue && nIncludePathSegment.Value);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var container = result.Success ?? new cmisObjectInFolderContainerType();
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumDescendantsUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_DescendantsUri(queryString));
                        withBlock.Add(ServiceURIs.enumDescendantsUri.folderId, folderId);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.depth, nDepth);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includeAllowableActions, nIncludeAllowableActions);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includeRelationships, nIncludeRelationships);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includePathSegment, nIncludePathSegment);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, folderId, withBlock.ToUri());
                            links = withBlock1.CreateDescendantsLinks();

                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, container.Children, false, default(long?), links, "descendants:" + folderId, "GetDescendants", depth: nDepth, filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), folderId: string.IsNullOrEmpty(folderId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(folderId), includeAllowableActions: nIncludeAllowableActions, includePathSegment: nIncludePathSegment, includeRelationships: nIncludeRelationships, renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter));
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    if (feed is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.ContentType = MediaTypes.Feed;
                        context.StatusCode = System.Net.HttpStatusCode.OK;

                        return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the parent folder-object of the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument GetFolderParent(string repositoryId, string folderId, string filter)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            try
            {
                result = serviceImpl.GetFolderParent(repositoryId, folderId, filter);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, false);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.GetFolderParent(string repositoryId, string folderId, string filter) => GetFolderParent(repositoryId, folderId, filter);

        /// <summary>
      /// Returns the descendant folders contained in the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks>
      /// Required parameters:
      /// folderId
      /// Optional parameters:
      /// filter, depth, includeAllowableActions, includeRelationships, includePathSegment, renditionFilter
      /// </remarks>
        public sx.XmlDocument GetFolderTree(string repositoryId)
        {
            ccg.Result<cmisObjectInFolderContainerType> result;
            var serviceImpl = CmisServiceImpl;
            // get the required ...
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            // ... and optional parameters from the queryString
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.filter);
            var depth = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.depth));
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includeRelationships));
            var includePathSegment = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.includePathSegment));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.renditionFilter);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (depth.HasValue && (depth.Value == 0L || depth.Value < -1))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("depth"), serviceImpl);

            try
            {
                result = serviceImpl.GetFolderTree(repositoryId, folderId, filter, depth, includeAllowableActions, includeRelationships, includePathSegment.HasValue && includePathSegment.Value, renditionFilter);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var container = result.Success ?? new cmisObjectInFolderContainerType();
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumFolderTreeUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_FolderTreeUri(queryString));
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.folderId, folderId);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.depth, depth);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.includeRelationships, includeRelationships);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.includePathSegment, includePathSegment);
                        withBlock.Add(ServiceURIs.enumFolderTreeUri.renditionFilter, renditionFilter);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, folderId, withBlock.ToUri());
                            links = withBlock1.CreateFolderTreeLinks();

                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, container.Children, false, default(long?), links, "tree:" + folderId, "GetFolderTree", depth: depth, filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), folderId: string.IsNullOrEmpty(folderId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(folderId), includeAllowableActions: includeAllowableActions, includePathSegment: includePathSegment, includeRelationships: includeRelationships, renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter));
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    if (feed is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.ContentType = MediaTypes.Feed;
                        context.StatusCode = System.Net.HttpStatusCode.OK;

                        return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

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
        public sx.XmlDocument GetObjectParents(string repositoryId, string objectId, string filter, string includeAllowableActions, string includeRelationships, string renditionFilter, string includeRelativePathSegment)
        {
            ccg.Result<cmisObjectParentsType[]> result;
            var serviceImpl = CmisServiceImpl;
            var bIncludeAllowableActions = CommonFunctions.ParseBoolean(includeAllowableActions);
            var bIncludeRelativePathSegment = CommonFunctions.ParseBoolean(includeRelativePathSegment);
            var nIncludeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(includeRelationships);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = CmisServiceImpl.GetObjectParents(repositoryId, objectId, filter, bIncludeAllowableActions, nIncludeRelationships, renditionFilter, bIncludeRelativePathSegment);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;
                    var parents = result.Success ?? (new cmisObjectParentsType[] { });
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                    *//* TODO ERROR: Skipped DisabledTextTrivia
                                   Dim numItems As xs_Integer=parents.Length
                    *//* TODO ERROR: Skipped ElseDirectiveTrivia
                    #Else
                    */
                    long numItems = parents.LongLength;
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumObjectParentsUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_ObjectParentsUri(queryString));
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.objectId, objectId);
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.includeRelationships, includeRelationships);
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumObjectParentsUri.includeRelativePathSegment, includeRelativePathSegment);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, objectId, withBlock.ToUri());
                            links = withBlock1.CreateObjectParentLinks();
                        }
                        var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, parents, false, numItems, links, "parents:" + objectId, "GetObjectParents", filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), includeAllowableActions: bIncludeAllowableActions, includeRelativePathSegment: bIncludeRelativePathSegment, includeRelationships: nIncludeRelationships, objectId: objectId, renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter));
                        feed = CreateAtomFeed(generatingGuidance);
                    }
                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns a list of all unfiled documents in the repository.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetUnfiledObjects(string repositoryId)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;
            // optional parameters from the queryString
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.skipCount));
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.includeAllowableActions));
            var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.renditionFilter);
            string orderBy = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.orderBy);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            try
            {
                result = serviceImpl.GetUnfiledObjects(repositoryId, maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objectList = result.Success ?? new cmisObjectListType();
                    ca.AtomFeed feed;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumUnfiledUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_UnfiledUri(queryString));
                        withBlock.Add(ServiceURIs.enumUnfiledUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.skipCount, skipCount);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.includeRelationships, includeRelationships);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumUnfiledUri.orderBy, orderBy);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, null, withBlock.ToUri());
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                            *//* TODO ERROR: Skipped DisabledTextTrivia
                                                 Dim links = .CreateCheckedOutLinks(If(objectList.Objects Is Nothing, 0, objectList.Objects.Length), objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems)
                            *//* TODO ERROR: Skipped ElseDirectiveTrivia
                            #Else
                            */
                            var links = withBlock1.CreateCheckedOutLinks(objectList.Objects is null ? 0L : objectList.Objects.LongLength, objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "unfiledObjects", "GetUnfiledObjects", filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), includeAllowableActions: includeAllowableActions, includeRelationships: includeRelationships, maxItems: maxItems, orderBy: string.IsNullOrEmpty(orderBy) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(orderBy), renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(renditionFilter), skipCount: skipCount);
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
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
        private sx.XmlDocument AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, bool isLastChunk, string changeToken)
        {
            ccg.Result<cm.Responses.setContentStreamResponse> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (contentStream is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("contentStream"), serviceImpl);

            try
            {
                string mimeType = ssw.WebOperationContext.Current.IncomingRequest.ContentType;
                string argdisposition = "";
                string fileName = RFC2231Helper.DecodeContentDisposition(ssw.WebOperationContext.Current.IncomingRequest.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition);

                result = serviceImpl.AppendContentStream(repositoryId, objectId, contentStream, mimeType, fileName, isLastChunk, changeToken);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, objectId, serviceImpl, result.Success);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, bool isLastChunk, string changeToken) => AppendContentStream(repositoryId, objectId, contentStream, isLastChunk, changeToken);

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument BulkUpdateProperties(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;
            ca.AtomEntry entry;
            Core.cmisBulkUpdateType bulkUpdate;

            if (data is null)
            {
                entry = null;
                bulkUpdate = null;
            }
            else
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        entry = ToAtomEntry(ms, true);
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
                bulkUpdate = entry is null ? null : entry.BulkUpdate;
            }

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (bulkUpdate is null || bulkUpdate.ObjectIdAndChangeTokens is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectIdAndChangeToken"), serviceImpl);

            try
            {
                result = serviceImpl.BulkUpdateProperties(repositoryId, bulkUpdate);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objects = result.Success ?? new cmisObjectListType();
                    var genratingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, CmisServiceImpl, objects.Objects, objects.HasMoreItems, objects.NumItems, null, "bulkUpdates", "BulkUpdateProperties");
                    var feed = CreateAtomFeed(genratingGuidance);
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.Created;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Creates a new document in the specified folder or as unfiled document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">If specified, the identifier for the folder that MUST be the parent folder for the newly-created document object.
      /// This parameter MUST be specified if the repository does NOT support the optional "unfiling" capability.</param>
      /// <param name="versioningState"></param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if specified, or being applied if no folderId is specified.</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if specified, or being ignored if no folderId is specified.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreateDocument(string repositoryId, string folderId, Core.enumVersioningState? versioningState, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            cm.cmisContentStreamType content;
            var serviceImpl = CmisServiceImpl;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (data is null || data.Object is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            if (data.Content is null)
            {
                content = null;
            }
            else
            {
                string mimeType = data.Content.Mediatype;
                string argdisposition = "";
                string fileName = RFC2231Helper.DecodeContentDisposition(ssw.WebOperationContext.Current.IncomingRequest.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition);
                content = new cm.cmisContentStreamType(data.Content.ToStream(), fileName, mimeType, true);
            }

            try
            {
                result = serviceImpl.CreateDocument(repositoryId, data.Object, folderId, content, versioningState, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CreateDocument(string repositoryId, string folderId, Core.enumVersioningState? versioningState, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CreateDocument(repositoryId, folderId, versioningState, data, addACEs, removeACEs);

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) specified location
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
        private sx.XmlDocument CreateDocumentFromSource(string repositoryId, string sourceId, Core.Collections.cmisPropertiesType properties, string folderId, Core.enumVersioningState? versioningState, string[] policies, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;

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

            try
            {
                result = serviceImpl.CreateDocumentFromSource(repositoryId, sourceId, properties, folderId, versioningState, policies, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CreateDocumentFromSource(string repositoryId, string sourceId, Core.Collections.cmisPropertiesType properties, string folderId, Core.enumVersioningState? versioningState, string[] policies, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CreateDocumentFromSource(repositoryId, sourceId, properties, folderId, versioningState, policies, addACEs, removeACEs);

        /// <summary>
      /// Creates a folder object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="parentFolderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created folder object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created folder object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreateFolder(string repositoryId, string parentFolderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(parentFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            if (data is null || data.Object is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            try
            {
                result = serviceImpl.CreateFolder(repositoryId, data.Object, parentFolderId, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CreateFolder(string repositoryId, string parentFolderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CreateFolder(repositoryId, parentFolderId, data, addACEs, removeACEs);

        /// <summary>
      /// Creates an item object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreateItem(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (data is null || data.Object is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            try
            {
                result = serviceImpl.CreateItem(repositoryId, data.Object, folderId, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CreateItem(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CreateItem(repositoryId, folderId, data, addACEs, removeACEs);

        /// <summary>
      /// Creates a policy object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreatePolicy(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;
            var repositoryInfo = serviceImpl.GetRepositoryInfo(repositoryId).Success;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId) || repositoryInfo is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            }
            if (string.IsNullOrEmpty(folderId) && !repositoryInfo.Capabilities.CapabilityUnfiling)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);
            }
            if (data is null || data.Object is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            try
            {
                result = serviceImpl.CreatePolicy(repositoryId, data.Object, folderId, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CreatePolicy(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CreatePolicy(repositoryId, folderId, data, addACEs, removeACEs);

        /// <summary>
      /// Creates a relationship object of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument CreateRelationship(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<cmisObjectType> result = null;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            if (data is not null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        // try to interpret data as a request-instance
                        var requestBase = ToRequest(ms, repositoryId);
                        ca.AtomEntry entry;

                        if (requestBase is null)
                        {
                            entry = ToAtomEntry(ms, true);
                            if (entry is null || entry.Object is null)
                            {
                                result = cm.cmisFaultType.CreateInvalidArgumentException("properties");
                            }
                            else
                            {
                                result = serviceImpl.CreateRelationship(repositoryId, entry.Object, null, null);
                            }
                        }
                        else if (requestBase is cm.Requests.createRelationship)
                        {
                            cm.Requests.createRelationship request = (cm.Requests.createRelationship)requestBase;

                            entry = (ca.AtomEntry)request;
                            if (entry is null || entry.Object is null)
                            {
                                result = cm.cmisFaultType.CreateInvalidArgumentException("properties");
                            }
                            else
                            {
                                result = serviceImpl.CreateRelationship(repositoryId, entry.Object, request.AddACEs, request.RemoveACEs);
                            }
                        }
                        if (result is null)
                        {
                            result = cm.cmisFaultType.CreateUnknownException();
                        }
                        else if (result.Failure is null)
                        {
                            return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsWebException(ex))
                        {
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If EnableExceptionLogging = "True" Then
                            */
                            serviceImpl.LogException(ex);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            throw;
                        }
                        else
                        {
                            result = cm.cmisFaultType.CreateUnknownException(ex);
                        }
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        public sx.XmlDocument DeleteContentStream(string repositoryId)
        {
            ccg.Result<cm.Responses.deleteContentStreamResponse> result;
            var serviceImpl = CmisServiceImpl;
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            string changeToken = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.changeToken);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                var context = ssw.WebOperationContext.Current.OutgoingResponse;

                result = serviceImpl.DeleteContentStream(repositoryId, objectId, changeToken);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else
                {
                    var response = result.Success;

                    if (response is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.StatusCode = System.Net.HttpStatusCode.NoContent;
                        context.ContentType = MediaTypes.Xml;

                        return css.ToXmlDocument(response);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Removes the submitted document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public void DeleteObject(string repositoryId)
        {
            var serviceImpl = CmisServiceImpl;
            Exception failure;
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            var allVersions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.allVersions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                failure = serviceImpl.DeleteObject(repositoryId, objectId, !allVersions.HasValue || allVersions.Value);
                if (failure is null)
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                }
                else if (!IsWebException(failure))
                {
                    failure = cm.cmisFaultType.CreateUnknownException(failure);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    failure = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            if (failure is not null)
                throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public sx.XmlDocument DeleteTree(string repositoryId)
        {
            ccg.Result<cm.Responses.deleteTreeResponse> result;
            var serviceImpl = CmisServiceImpl;
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            var allVersion = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.allVersions));
            var unfileObjects = CommonFunctions.ParseEnum<Core.enumUnfileObject>(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.unfileObjects));
            var continueOnFailure = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumFolderTreeUri.continueOnFailure));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            try
            {
                result = serviceImpl.DeleteTree(repositoryId, folderId, !allVersion.HasValue || allVersion.Value, unfileObjects, continueOnFailure.HasValue && continueOnFailure.Value);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var response = result.Success;

                    if (response is null)
                    {
                        return null;
                    }
                    else
                    {
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        context.ContentType = MediaTypes.Xml;
                        context.StatusCode = response.StatusCode;

                        return css.ToXmlDocument(new Core.Collections.cmisListOfIdsType(response.FailedToDelete.ObjectIds));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        private sx.XmlDocument DeleteTreeViaDescendantsFeed(string repositoryId)
        {
            return DeleteTree(repositoryId);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.DeleteTreeViaDescendantsFeed(string repositoryId) => DeleteTreeViaDescendantsFeed(repositoryId);
        private sx.XmlDocument DeleteTreeViaChildrenFeed(string repositoryId)
        {
            return DeleteTree(repositoryId);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.DeleteTreeViaChildrenFeed(string repositoryId) => DeleteTreeViaChildrenFeed(repositoryId);

        /// <summary>
      /// Returns the allowable actions for the specified document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetAllowableActions(string repositoryId, string id)
        {
            ccg.Result<Core.cmisAllowableActionsType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(id))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = serviceImpl.GetAllowableActions(repositoryId, id);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    if (result.Success is null)
                    {
                        return null;
                    }
                    else
                    {
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        context.ContentType = MediaTypes.AllowableActions;
                        context.StatusCode = System.Net.HttpStatusCode.OK;

                        return css.ToXmlDocument(result.Success);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the content stream of the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// This returns the content stream. 
      /// It is RECOMMENDED that HTTP Range requests are supported on this resource.  
      /// It is RECOMMENDED that HTTP compression is also supported. 
      /// Please see RFC2616 for more information on HTTP Range requests.
      /// 
      /// Required parameters:
      /// objectId
      /// Optional parameters:
      /// streamId
      /// </remarks>
        public System.IO.Stream GetContentStream(string repositoryId)
        {
            ccg.Result<cm.cmisContentStreamType> result;
            var serviceImpl = CmisServiceImpl;
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            string streamId = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.streamId);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = serviceImpl.GetContentStream(repositoryId, objectId, streamId);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var contentStream = result.Success;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    if (contentStream is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.ContentType = contentStream.MimeType;
                        context.StatusCode = contentStream.StatusCode;
                        if (!string.IsNullOrEmpty(contentStream.Filename))
                        {
                            context.Headers.Add(RFC2231Helper.ContentDispositionHeaderName, RFC2231Helper.EncodeContentDisposition(contentStream.Filename));
                        }

                        return contentStream.BinaryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the cmisobject with the specified id.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// requested parameters:
      /// objectId
      /// optional parameters:
      /// filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions
      /// returnVersion(getObjectOfLatestVersion), major(getObjectOfLatestVersion), versionSeriesId(getObjectOfLatestVersion)
      /// </remarks>
        public sx.XmlDocument GetObject(string repositoryId)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;
            // required for the getFolderParent-service
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.folderId);
            // optional parameter
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.filter);

            if (string.IsNullOrEmpty(folderId))
            {
                // get the required ...
                string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
                // ... and optional parameters from the queryString
                var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeRelationships));
                var includePolicyIds = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includePolicyIds));
                string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.renditionFilter);
                var includeACL = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeACL));
                var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeAllowableActions));
                var returnVersion = CommonFunctions.ParseEnum<RestAtom.enumReturnVersion>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.returnVersion));
                var privateWorkingCopy = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.pwc));
                var major = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.major));
                string versionSeriesId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.versionSeriesId);

                // getObjectOfLatestVersion: parameter versionSeriesId is used instead of objectId and parameter major instead of returnVersion
                if (!string.IsNullOrEmpty(versionSeriesId))
                {
                    if (string.IsNullOrEmpty(objectId))
                        objectId = versionSeriesId;
                    returnVersion = major.HasValue && major.Value ? RestAtom.enumReturnVersion.latestmajor : RestAtom.enumReturnVersion.latest;
                }
                // invalid arguments
                if (string.IsNullOrEmpty(repositoryId))
                    throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
                if (string.IsNullOrEmpty(objectId))
                    throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

                try
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    result = serviceImpl.GetObject(repositoryId, objectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, returnVersion, privateWorkingCopy);
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, false);
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }
            }
            else
            {
                return GetFolderParent(repositoryId, folderId, filter);
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the object at the specified path
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="path"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includeACL"></param>
      /// <param name="renditionFilter"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetObjectByPath(string repositoryId, string path, string filter, string includeAllowableActions, string includePolicyIds, string includeRelationships, string includeACL, string renditionFilter)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(path))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("path"), serviceImpl);

            var nIncludeAllowableActions = CommonFunctions.ParseBoolean(includeAllowableActions);
            var nIncludePolicyIds = CommonFunctions.ParseBoolean(includePolicyIds);
            var nIncludeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(includeRelationships);
            var nIncludeACL = CommonFunctions.ParseBoolean(includeACL);

            try
            {
                result = serviceImpl.GetObjectByPath(repositoryId, path, filter, nIncludeAllowableActions, nIncludePolicyIds, nIncludeRelationships, nIncludeACL, renditionFilter);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, false);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="targetFolderId"></param>
      /// <param name="sourceFolderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(targetFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("targetFolderId"), serviceImpl);
            if (string.IsNullOrEmpty(sourceFolderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("sourceFolderId"), serviceImpl);

            try
            {
                result = serviceImpl.MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId) => MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);

        /// <summary>
      /// Sets the content stream of the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument SetContentStream(string repositoryId, System.IO.Stream data)
        {
            var serviceImpl = CmisServiceImpl;
            // get the required ...
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            // ... and optional parameters from the queryString
            var overwriteFlag = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.overwriteFlag));
            string changeToken = CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.changeToken);
            var append = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.append));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (data is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("contentStream"), serviceImpl);

            if (append.HasValue && append.Value)
            {
                var isLastChunk = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumContentUri.isLastChunk));
                return AppendContentStream(repositoryId, objectId, data, isLastChunk.HasValue && isLastChunk.Value, changeToken);
            }
            else
            {
                ccg.Result<cm.Responses.setContentStreamResponse> result;
                string mimeType = ssw.WebOperationContext.Current.IncomingRequest.ContentType;
                string argdisposition = "";
                string fileName = RFC2231Helper.DecodeContentDisposition(ssw.WebOperationContext.Current.IncomingRequest.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition);

                try
                {
                    result = serviceImpl.SetContentStream(repositoryId, objectId, data, mimeType, fileName, !overwriteFlag.HasValue || overwriteFlag.Value, changeToken);
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        if (result.Success is not null)
                            objectId = result.Success.ObjectId ?? objectId;
                        return CreateXmlDocument(repositoryId, objectId, serviceImpl, result.Success);
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }

                // failure
                throw LogException(result.Failure, serviceImpl);
            }
        }

        /// <summary>
      /// Updates the submitted cmis-object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="changeToken"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument UpdateProperties(string repositoryId, string objectId, ca.AtomEntry data, string changeToken)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (data.Object is null || data.Object.Properties is null || data.Object.Properties.Properties is null)
            {
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("properties"), serviceImpl);
            }

            try
            {
                result = serviceImpl.UpdateProperties(repositoryId, objectId, data.Object.Properties, changeToken);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.UpdateProperties(string repositoryId, string objectId, ca.AtomEntry data, string changeToken) => UpdateProperties(repositoryId, objectId, data, changeToken);
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
        private sx.XmlDocument AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(folderId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("folderId"), serviceImpl);

            try
            {
                result = serviceImpl.AddObjectToFolder(repositoryId, objectId, folderId, allVersions);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions) => AddObjectToFolder(repositoryId, objectId, folderId, allVersions);

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId">The folder from which the object is to be removed.
      /// If no value is specified, then the repository MUST remove the object from all folders in which it is currently filed.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument RemoveObjectFromFolder(string repositoryId, string objectId, string folderId)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = serviceImpl.RemoveObjectFromFolder(repositoryId, objectId, folderId);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.RemoveObjectFromFolder(string repositoryId, string objectId, string folderId) => RemoveObjectFromFolder(repositoryId, objectId, folderId);
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
      /// <param name="changeLogToken">If this parameter is specified, start the changes from the specified token. The changeLogToken is embedded in the paging link relations for normal iteration through the change list. </param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetContentChanges(string repositoryId, string filter, string maxItems, string includeACL, string includePolicyIds, string includeProperties, string changeLogToken)
        {
            ccg.Result<getContentChanges> result;
            var serviceImpl = CmisServiceImpl;
            var nMaxItems = CommonFunctions.ParseInteger(maxItems);
            var nIncludeACL = CommonFunctions.ParseBoolean(includeACL);
            var nIncludePolicyIds = CommonFunctions.ParseBoolean(includePolicyIds);
            var nIncludeProperties = CommonFunctions.ParseBoolean(includeProperties);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            try
            {
                result = serviceImpl.GetContentChanges(repositoryId, filter, nMaxItems, nIncludeACL, nIncludePolicyIds.HasValue && nIncludePolicyIds.Value, nIncludeProperties.HasValue && nIncludeProperties.Value, ref changeLogToken);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objectList = result.Success ?? new getContentChanges();
                    ca.AtomFeed feed;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumChangesUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_ChangesUri(queryString));
                        withBlock.Add(ServiceURIs.enumChangesUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumChangesUri.maxItems, nMaxItems);
                        withBlock.Add(ServiceURIs.enumChangesUri.includeACL, nIncludeACL);
                        withBlock.Add(ServiceURIs.enumChangesUri.includePolicyIds, nIncludePolicyIds);
                        withBlock.Add(ServiceURIs.enumChangesUri.includeProperties, nIncludeProperties);
                        withBlock.Add(ServiceURIs.enumChangesUri.changeLogToken, changeLogToken);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, null, withBlock.ToUri());
                            var links = withBlock1.CreateContentChangesLinks();
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "changes", "GetContentChanges", changeLogToken: string.IsNullOrEmpty(changeLogToken) ? default : new ccg.Nullable<string>(changeLogToken), filter: string.IsNullOrEmpty(filter) ? default : new ccg.Nullable<string>(filter), includeACL: nIncludeACL, includePolicyIds: nIncludePolicyIds, includeProperties: nIncludeProperties, maxItems: nMaxItems);
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns the data described by the specified CMIS query. (GET Request)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument Query(string repositoryId)
        {
            // get the required ...
            string q = (CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.q) ?? CommonFunctions.GetRequestParameter("query")) ?? CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.statement);
            // ... and optional parameters from the queryString
            var searchAllVersion = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.searchAllVersions));
            var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.includeRelationships));
            string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.renditionFilter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.includeAllowableActions));
            var maxItems = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.maxItems));
            var skipCount = CommonFunctions.ParseInteger(CommonFunctions.GetRequestParameter(ServiceURIs.enumQueryUri.skipCount));

            return this.Query(System.Net.HttpStatusCode.OK, repositoryId, q, searchAllVersion, includeRelationships, renditionFilter, includeAllowableActions, maxItems, skipCount);
        }
        /// <summary>
      /// Returns the data described by the specified CMIS query. (POST Request)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument Query(string repositoryId, System.IO.Stream data)
        {
            var serviceImpl = CmisServiceImpl;

            if (data is null)
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("statement"), serviceImpl);

            using (var ms = new System.IO.MemoryStream())
            {
                data.CopyTo(ms);

                try
                {
                    cm.Requests.query request = ToRequest(ms, null) as cm.Requests.query;
                    if (request is null || string.IsNullOrEmpty(request.RepositoryId))
                    {
                        {
                            var withBlock = ConvertData(ms, reader =>
                            {
                                var retVal = new Core.cmisQueryType();
                                retVal.ReadXml(reader);
                                return retVal;
                            });
                            return Query(System.Net.HttpStatusCode.Created, repositoryId, withBlock.Statement, withBlock.SearchAllVersions, withBlock.IncludeRelationships, withBlock.RenditionFilter, withBlock.IncludeAllowableActions, withBlock.MaxItems, withBlock.SkipCount);
                        }
                    }
                    else
                    {
                        return Query(System.Net.HttpStatusCode.Created, repositoryId, request.Statement, request.SearchAllVersions, request.IncludeRelationships, request.RenditionFilter, request.IncludeAllowableActions, request.MaxItems, request.SkipCount);
                    }
                }
                finally
                {
                    ms.Close();
                }
            }
        }
        private sx.XmlDocument Query(System.Net.HttpStatusCode success, string repositoryId, string q, bool? searchAllVersions, Core.enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeAllowableActions, long? maxItems, long? skipCount)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(q))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("statement"), serviceImpl);

            try
            {
                result = serviceImpl.Query(repositoryId, q, searchAllVersions.HasValue && searchAllVersions.Value, includeRelationships, renditionFilter, includeAllowableActions.HasValue && includeAllowableActions.Value, maxItems, skipCount);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objectList = result.Success ?? new cmisObjectListType();
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumQueryUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_QueryUri(queryString));
                        withBlock.Add(ServiceURIs.enumQueryUri.q, q);
                        withBlock.Add(ServiceURIs.enumQueryUri.searchAllVersions, searchAllVersions);
                        withBlock.Add(ServiceURIs.enumQueryUri.includeRelationships, includeRelationships);
                        withBlock.Add(ServiceURIs.enumQueryUri.renditionFilter, renditionFilter);
                        withBlock.Add(ServiceURIs.enumQueryUri.includeAllowableActions, includeAllowableActions);
                        withBlock.Add(ServiceURIs.enumQueryUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumQueryUri.skipCount, skipCount);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, null, withBlock.ToUri());
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                            *//* TODO ERROR: Skipped DisabledTextTrivia
                                                 links = .CreateQueryLinks(If(objectList.Objects Is Nothing, 0, objectList.Objects.Length), objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems)
                            *//* TODO ERROR: Skipped ElseDirectiveTrivia
                            #Else
                            */
                            links = withBlock1.CreateQueryLinks(objectList.Objects is null ? 0L : objectList.Objects.LongLength, objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "query", "Query", includeAllowableActions: includeAllowableActions, includeRelationships: includeRelationships, maxItems: maxItems, q: string.IsNullOrEmpty(q) ? default : new ccg.Nullable<string>(q), renditionFilter: string.IsNullOrEmpty(renditionFilter) ? default : new ccg.Nullable<string>(renditionFilter), searchAllVersions: searchAllVersions, skipCount: skipCount);
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    if (feed is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.StatusCode = success;
                        context.ContentType = MediaTypes.Feed;
                        // Header: Location, Content-Location
                        var uriBuilder = new UriBuilder(BaseUri.Combine(ServiceURIs.get_QueryUri(ServiceURIs.enumQueryUri.q).ReplaceUri("repositoryId", repositoryId, "q", q)));
                        var queryStrings = new List<string>();

                        if (!string.IsNullOrEmpty(uriBuilder.Query))
                            queryStrings.Add(uriBuilder.Query.TrimStart('?'));
                        if (searchAllVersions.HasValue)
                            queryStrings.Add("searchAllVersions=" + searchAllVersions.Value.ToString().ToLowerInvariant());
                        if (includeRelationships.HasValue)
                            queryStrings.Add("includeRelationships=" + Uri.EscapeDataString(includeRelationships.Value.GetName()));
                        if (!string.IsNullOrEmpty(renditionFilter))
                            queryStrings.Add("renditionFilter=" + Uri.EscapeDataString(renditionFilter));
                        if (includeAllowableActions.HasValue)
                            queryStrings.Add("includeAllowableActions=" + includeAllowableActions.Value.ToString().ToLowerInvariant());
                        if (maxItems.HasValue)
                            queryStrings.Add("maxItems=" + maxItems.Value.ToString());
                        if (skipCount.HasValue)
                            queryStrings.Add("skipCount=" + skipCount.Value.ToString());
                        if (queryStrings.Count > 0)
                            uriBuilder.Query = string.Join("&", queryStrings.ToArray());
                        context.Location = uriBuilder.Uri.AbsoluteUri;
                        context.Headers.Add("Content-Location", context.Location);

                        return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
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
      /// <param name="repositoryId"></param>
      /// <remarks>Handled by DeleteObject()</remarks>
        public void CancelCheckOut(string repositoryId)
        {
            var serviceImpl = CmisServiceImpl;
            Exception failure;
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.objectId) ?? CommonFunctions.GetRequestParameter("id");

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                failure = serviceImpl.CancelCheckOut(repositoryId, objectId);
                if (failure is null)
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                }
                else if (!IsWebException(failure))
                {
                    failure = cm.cmisFaultType.CreateUnknownException(failure);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    failure = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            if (failure is not null)
                throw LogException(failure, serviceImpl);
        }

        /// <summary>
      /// Checks-in the Private Working Copy document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
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
        private sx.XmlDocument CheckIn(string repositoryId, string objectId, ca.AtomEntry data, bool? major, string checkInComment, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            cm.cmisContentStreamType content;
            var cmisObject = data.Object;

            if (data.Content is null)
            {
                content = null;
            }
            else
            {
                string mimeType = data.Content.Mediatype;
                string argdisposition = "";
                string fileName = RFC2231Helper.DecodeContentDisposition(ssw.WebOperationContext.Current.IncomingRequest.Headers[RFC2231Helper.ContentDispositionHeaderName], ref argdisposition);
                content = new cm.cmisContentStreamType(data.Content.ToStream(), fileName, mimeType, true);
            }

            return CheckIn(repositoryId, objectId, cmisObject is null ? null : cmisObject.Properties, cmisObject is null || cmisObject.PolicyIds is null ? null : cmisObject.PolicyIds.Ids, content, major, checkInComment, addACEs, removeACEs);
        }

        sx.XmlDocument Contracts.IAtomPubBinding.CheckIn(string repositoryId, string objectId, ca.AtomEntry data, bool? major, string checkInComment, Core.Security.cmisAccessControlListType addACEs, Core.Security.cmisAccessControlListType removeACEs) => CheckIn(repositoryId, objectId, data, major, checkInComment, addACEs, removeACEs);

        private sx.XmlDocument CheckIn(string repositoryId, string objectId, Core.Collections.cmisPropertiesType properties, string[] policies, cm.cmisContentStreamType content, bool? major, string checkInComment, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = serviceImpl.CheckIn(repositoryId, objectId, properties, policies, content, !major.HasValue || major.Value, checkInComment, addACEs, removeACEs);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, false);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Checks out the specified CMIS object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument CheckOut(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            using (var ms = new System.IO.MemoryStream())
            {
                if (data is not null)
                    data.CopyTo(ms);

                try
                {
                    var document = data is null ? null : ToAtomEntry(ms, true);
                    string objectId = document is null ? null : document.ObjectId;

                    if (string.IsNullOrEmpty(objectId))
                        objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumCheckedOutUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
                    // invalid arguments
                    if (string.IsNullOrEmpty(repositoryId))
                        throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
                    if (string.IsNullOrEmpty(objectId))
                        throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

                    try
                    {
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        result = serviceImpl.CheckOut(repositoryId, objectId);

                        if (result is null)
                        {
                            result = cm.cmisFaultType.CreateUnknownException();
                        }
                        else if (result.Failure is null)
                        {
                            var entry = CreateAtomEntry(new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl), result.Success);

                            if (entry is null)
                            {
                                return null;
                            }
                            else
                            {
                                context.ContentType = MediaTypes.Entry;
                                context.StatusCode = System.Net.HttpStatusCode.Created;
                                {
                                    var withBlock = BaseUri.Combine(ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId).ReplaceUri("repositoryId", repositoryId, "id", entry.ObjectId));
                                    context.Headers.Add("Content-Location", withBlock.AbsoluteUri);
                                }

                                return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsWebException(ex))
                        {
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If EnableExceptionLogging = "True" Then
                            */
                            serviceImpl.LogException(ex);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            throw;
                        }
                        else
                        {
                            result = cm.cmisFaultType.CreateUnknownException(ex);
                        }
                    }
                }
                finally
                {
                    ms.Close();
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns all Documents in the specified version series.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>In the cmis documentation the GetAllVersions()-method is described with a required versionSeriesId-parameter.
      /// Unfortunality this parameter is not defined in the messaging xsd-file, but there is used the objectId.
      /// So this method only checks if at least one of the parameters is set</remarks>
        public sx.XmlDocument GetAllVersions(string repositoryId)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;
            // get the required ...
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            string versionSeriesId = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.versionSeriesId);
            // ... and optional parameters from the queryString
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.filter);
            var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumAllVersionsUri.includeAllowableActions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId) && string.IsNullOrEmpty(versionSeriesId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId/versionSeriesId"), serviceImpl);

            try
            {
                result = serviceImpl.GetAllVersions(repositoryId, objectId, versionSeriesId, filter, includeAllowableActions);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;
                    var objectList = result.Success ?? new cmisObjectListType();
                    ca.AtomFeed feed;
                    List<ca.AtomLink> links;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumAllVersionsUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_AllVersionsUri(queryString));
                        withBlock.Add(ServiceURIs.enumAllVersionsUri.objectId, objectId);
                        withBlock.Add(ServiceURIs.enumAllVersionsUri.versionSeriesId, versionSeriesId);
                        withBlock.Add(ServiceURIs.enumAllVersionsUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumAllVersionsUri.includeAllowableActions, includeAllowableActions);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, versionSeriesId, withBlock.ToUri());
                            links = withBlock1.CreateAllVersionsLinks();
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "allVersions", "GetAllVersions", filter: string.IsNullOrEmpty(filter) ? default : new ccg.Nullable<string>(filter), includeAllowableActions: includeAllowableActions, objectId: string.IsNullOrEmpty(objectId) ? default : new ccg.Nullable<string>(objectId), versionSeriesId: string.IsNullOrEmpty(versionSeriesId) ? default : new ccg.Nullable<string>(versionSeriesId));
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="versionSeriesId"></param>
      /// <param name="major"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks>Handled by GetObject()
      /// In the cmis documentation the GetObjectOfLatestVersion()-method is described with a required versionSeriesId-parameter.
      /// Unfortunality this parameter is not defined in the messaging xsd-file, but there is used the objectId.
      /// So this method only checks if at least one of the parameters is set and prefers the objectId</remarks>
        public sx.XmlDocument GetObjectOfLatestVersion(string repositoryId, string objectId, string versionSeriesId, bool? major, string filter, Core.enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;
            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId) && string.IsNullOrEmpty(versionSeriesId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId/versionSeriesId"), serviceImpl);

            try
            {
                var context = ssw.WebOperationContext.Current.OutgoingResponse;

                result = serviceImpl.GetObject(repositoryId, objectId.NVL(versionSeriesId), filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, major.HasValue && major.Value ? RestAtom.enumReturnVersion.latestmajor : RestAtom.enumReturnVersion.latest, false);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var entry = CreateAtomEntry(new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl), result.Success);

                    if (entry is null)
                    {
                        return null;
                    }
                    else
                    {
                        context.ContentType = MediaTypes.Entry;
                        context.StatusCode = System.Net.HttpStatusCode.OK;

                        return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Relationships
        /// <summary>
      /// Returns the relationships for the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetObjectRelationships(string repositoryId)
        {
            ccg.Result<cmisObjectListType> result;
            var serviceImpl = CmisServiceImpl;
            // get the required ...
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            // ... and optional parameters from the queryString
            var includeSubRelationshipTypes = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.includeSubRelationshipTypes));
            var relationshipDirection = CommonFunctions.ParseEnum<Core.enumRelationshipDirection>(CommonFunctions.GetRequestParameter(ServiceURIs.enumRelationshipsUri.relationshipDirection));
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

            try
            {
                result = serviceImpl.GetObjectRelationships(repositoryId, objectId, includeSubRelationshipTypes.HasValue && includeSubRelationshipTypes.Value, relationshipDirection, typeId, maxItems, skipCount, filter, includeAllowableActions);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var objectList = result.Success ?? new cmisObjectListType();
                    ca.AtomFeed feed;
                    var context = ssw.WebOperationContext.Current.OutgoingResponse;

                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumRelationshipsUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_RelationshipsUri(queryString));
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.objectId, objectId);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.includeSubRelationshipTypes, includeSubRelationshipTypes);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.relationshipDirection, relationshipDirection);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.typeId, typeId);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.maxItems, maxItems);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.skipCount, skipCount);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.filter, filter);
                        withBlock.Add(ServiceURIs.enumRelationshipsUri.includeAllowableActions, includeAllowableActions);

                        {
                            var withBlock1 = new LinkFactory(serviceImpl, repositoryId, objectId, withBlock.ToUri());
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                            *//* TODO ERROR: Skipped DisabledTextTrivia
                                                 Dim links = .CreateCheckedOutLinks(If(objectList.Objects Is Nothing, 0, objectList.Objects.Length), objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems)
                            *//* TODO ERROR: Skipped ElseDirectiveTrivia
                            #Else
                            */
                            var links = withBlock1.CreateCheckedOutLinks(objectList.Objects is null ? 0L : objectList.Objects.LongLength, objectList.NumItems, objectList.HasMoreItems, skipCount, maxItems);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "relationships:" + objectId, "GetObjectRelationships", filter: string.IsNullOrEmpty(filter) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(filter), includeAllowableActions: includeAllowableActions, includeSubRelationshipTypes: includeSubRelationshipTypes, maxItems: maxItems, objectId: string.IsNullOrEmpty(objectId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(objectId), relationshipDirection: relationshipDirection, skipCount: skipCount, typeId: string.IsNullOrEmpty(typeId) ? default(ccg.Nullable<string>) : new ccg.Nullable<string>(typeId));
                            feed = CreateAtomFeed(generatingGuidance);
                        }
                    }

                    context.ContentType = MediaTypes.Feed;
                    context.StatusCode = System.Net.HttpStatusCode.OK;

                    return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Policy
        /// <summary>
      /// Applies a policy to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// if data is null the objectId- and policyId-parameter MUST be defined in the querystring,
      /// otherwise an instance of Messaging.Request.applyPolicy or an instance of ca.AtomEntry representing the policy is assumed
      /// </remarks>
        public sx.XmlDocument ApplyPolicy(string repositoryId, System.IO.Stream data)
        {
            ccg.Result<cmisObjectType> result;
            var serviceImpl = CmisServiceImpl;

            using (var ms = new System.IO.MemoryStream())
            {
                if (data is not null)
                    data.CopyTo(ms);

                try
                {
                    var request = data is null ? null : ToRequest(ms, repositoryId) as cm.Requests.applyPolicy;
                    var entry = data is null || request is not null ? null : ToAtomEntry(ms, true);
                    var cmisObject = entry is null ? null : entry.Object;
                    string baseTypeId = cmisObject is null ? default : cmisObject.BaseTypeId;
                    bool objectIsPolicy = (baseTypeId ?? "") == ccdt.cmisTypePolicyDefinitionType.TargetTypeName;
                    // queryString
                    string objectId = request is null ? !(string.IsNullOrEmpty(baseTypeId) || objectIsPolicy) ? entry.ObjectId : null : request.ObjectId;
                    string policyId = request is null ? objectIsPolicy ? entry.ObjectId : null : request.PolicyId;

                    if (string.IsNullOrEmpty(objectId))
                        objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
                    if (string.IsNullOrEmpty(policyId))
                        policyId = CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.policyId);
                    // invalid arguments
                    if (string.IsNullOrEmpty(repositoryId))
                        throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
                    if (string.IsNullOrEmpty(objectId))
                        throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
                    if (string.IsNullOrEmpty(policyId))
                        throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("policyId"), serviceImpl);

                    try
                    {
                        result = serviceImpl.ApplyPolicy(repositoryId, objectId, policyId);
                        if (result is null)
                        {
                            result = cm.cmisFaultType.CreateUnknownException();
                        }
                        else if (result.Failure is null)
                        {
                            return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.Created, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsWebException(ex))
                        {
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If EnableExceptionLogging = "True" Then
                            */
                            serviceImpl.LogException(ex);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            throw;
                        }
                        else
                        {
                            result = cm.cmisFaultType.CreateUnknownException(ex);
                        }
                    }
                }
                finally
                {
                    ms.Close();
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Returns a list of policies applied to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks>This is the only collection where the URI’s of the objects in the collection MUST be specific to that collection.
      /// A DELETE on the policy object in the collection is a removal of the policy from the object NOT a deletion of the policy object itself</remarks>
        public sx.XmlDocument GetAppliedPolicies(string repositoryId, string id)
        {
            var serviceImpl = CmisServiceImpl;
            string objectId = string.IsNullOrEmpty(id) ? CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.objectId) : id;
            // queryString
            string filter = CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.filter);
            string policyId = CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.policyId);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            if (!string.IsNullOrEmpty(policyId))
            {
                // GetAppliedPolicies() returns modified self-links for policy-objects to enable the calling
                // client to remove a policy from an object (using the modified link) but not to remove the
                // policy object itself. Therefore it is possible that the client uses the modified link to
                // get the policy-object
                ccg.Result<cmisObjectType> result;
                var includeACL = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeACL));
                var includeAllowableActions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includeAllowableActions));
                var includePolicyIds = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.includePolicyIds));
                var includeRelationships = CommonFunctions.ParseEnum<Core.enumIncludeRelationships>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.filter));
                var privateWorkingCopy = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.pwc));
                string renditionFilter = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.renditionFilter);
                var returnVersion = CommonFunctions.ParseEnum<RestAtom.enumReturnVersion>(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.returnVersion));

                try
                {
                    result = serviceImpl.GetObject(repositoryId, policyId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, returnVersion, privateWorkingCopy);
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        return CreateXmlDocument(repositoryId, serviceImpl, result.Success, System.Net.HttpStatusCode.OK, false);
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }

                // failure
                throw LogException(result.Failure, serviceImpl);
            }
            else
            {
                // normal request
                ccg.Result<cmisObjectListType> result;

                // invalid arguments
                if (string.IsNullOrEmpty(objectId))
                    throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

                try
                {
                    result = serviceImpl.GetAppliedPolicies(repositoryId, objectId, filter);
                    if (result is null)
                    {
                        result = cm.cmisFaultType.CreateUnknownException();
                    }
                    else if (result.Failure is null)
                    {
                        var objectList = result.Success ?? new cmisObjectListType();
                        ca.AtomFeed feed;
                        List<ca.AtomLink> links;
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        {
                            var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumPoliciesUri>(serviceImpl.BaseUri, repositoryId, queryString => ServiceURIs.get_PoliciesUri(queryString));
                            withBlock.Add(ServiceURIs.enumPoliciesUri.objectId, objectId);
                            withBlock.Add(ServiceURIs.enumPoliciesUri.filter, filter);
                            {
                                var withBlock1 = new LinkFactory(serviceImpl, repositoryId, objectId, withBlock.ToUri());
                                links = withBlock1.CreatePoliciesLinks();

                                var generatingGuidance = new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl, objectList, objectList.HasMoreItems, objectList.NumItems, links, "policies:" + objectId, "GetAppliedPolicies", filter: string.IsNullOrEmpty(filter) ? default : new ccg.Nullable<string>(filter), objectId: string.IsNullOrEmpty(objectId) ? default : new ccg.Nullable<string>(objectId));
                                feed = CreateAtomFeed(generatingGuidance);
                            }
                        }

                        if (feed is null)
                        {
                            return null;
                        }
                        else
                        {
                            // modify self-link to avoid prevent clients from deleting the the policy itself from repository
                            foreach (ca.AtomEntry policy in feed.Entries)
                            {
                                var link = policy.get_Link(LinkRelationshipTypes.Self);
                                if (link is not null)
                                {
                                    link.Uri = new Uri(serviceImpl.BaseUri, ServiceURIs.get_PoliciesUri(ServiceURIs.enumPoliciesUri.objectId | ServiceURIs.enumPoliciesUri.policyId).ReplaceUri("repositoryId", repositoryId, "id", objectId, "policyId", ((cmisObjectType)policy.Object).ServiceModel.ObjectId));
                                }
                            }
                            context.ContentType = MediaTypes.Feed;
                            context.StatusCode = System.Net.HttpStatusCode.OK;

                            return css.ToXmlDocument(new sss.Atom10FeedFormatter(feed));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }

                // failure
                throw LogException(result.Failure, serviceImpl);
            }
        }

        /// <summary>
      /// Removes a policy from the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="policyId"></param>
      /// <remarks></remarks>
        public void RemovePolicy(string repositoryId, string id, string policyId)
        {
            Exception failure;
            var serviceImpl = CmisServiceImpl;
            string objectId = string.IsNullOrEmpty(id) ? CommonFunctions.GetRequestParameter(ServiceURIs.enumPoliciesUri.objectId) : id;
            var context = ssw.WebOperationContext.Current.OutgoingResponse;

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);
            if (string.IsNullOrEmpty(policyId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("policyId"), serviceImpl);

            try
            {
                failure = serviceImpl.RemovePolicy(repositoryId, objectId, policyId);
                if (failure is null)
                {
                    context.StatusCode = System.Net.HttpStatusCode.NoContent;
                }
                else if (!IsWebException(failure))
                {
                    failure = cm.cmisFaultType.CreateUnknownException(failure);
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    failure = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            if (failure is not null)
                throw LogException(failure, serviceImpl);
        }
        #endregion

        #region ACL
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument ApplyACL(string repositoryId, string id, System.IO.Stream data)
        {
            ccg.Result<Core.Security.cmisAccessControlListType> result = null;
            var serviceImpl = CmisServiceImpl;
            string objectId = string.IsNullOrEmpty(id) ? CommonFunctions.GetRequestParameter(ServiceURIs.enumACLUri.objectId) : id;
            Core.Security.cmisAccessControlListType addACEs = null;
            Core.Security.cmisAccessControlListType removeACEs = null;
            // queryString
            var aclPropagation = CommonFunctions.ParseEnum<Core.enumACLPropagation>(CommonFunctions.GetRequestParameter(ServiceURIs.enumACLUri.aclPropagation));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            if (data is not null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        cm.Requests.applyACL request = ToRequest(ms, repositoryId) as cm.Requests.applyACL;

                        if (request is null)
                        {
                            var newACEs = ConvertData(ms, reader =>
                                   {
                                       var retVal = new Core.Security.cmisAccessControlListType();
                                       retVal.ReadXml(reader);
                                       return retVal;
                                   });
                            var currentACEs = serviceImpl.GetACL(repositoryId, objectId, false);

                            if (currentACEs is null)
                            {
                                addACEs = newACEs;
                            }
                            else if (currentACEs.Failure is null)
                            {
                                if (currentACEs.Success is null)
                                {
                                    addACEs = newACEs;
                                }
                                else
                                {
                                    {
                                        var withBlock = currentACEs.Success.Split(newACEs);
                                        addACEs = withBlock.AddACEs;
                                        removeACEs = withBlock.RemoveACEs;
                                    }
                                }
                            }
                            else
                            {
                                result = currentACEs.Failure;
                            }
                        }
                        else
                        {
                            addACEs = request.AddACEs;
                            removeACEs = request.RemoveACEs;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsWebException(ex))
                        {
                            /* TODO ERROR: Skipped IfDirectiveTrivia
                            #If EnableExceptionLogging = "True" Then
                            */
                            serviceImpl.LogException(ex);
                            /* TODO ERROR: Skipped EndIfDirectiveTrivia
                            #End If
                            */
                            throw;
                        }
                        else
                        {
                            result = cm.cmisFaultType.CreateUnknownException(ex);
                        }
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
            }

            try
            {
                // don't ignore possible exception from the GetACL()-call
                result = result ?? serviceImpl.ApplyACL(repositoryId, objectId, addACEs, removeACEs, aclPropagation.HasValue ? aclPropagation.Value : Core.enumACLPropagation.repositorydetermined);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var acl = result.Success;

                    if (acl is null)
                    {
                        return null;
                    }
                    else
                    {
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        context.StatusCode = System.Net.HttpStatusCode.OK;
                        context.ContentType = MediaTypes.Acl;

                        return css.ToXmlDocument(acl);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }

        /// <summary>
      /// Get the ACL currently applied to the specified document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument GetACL(string repositoryId, string id)
        {
            ccg.Result<Core.Security.cmisAccessControlListType> result;
            var serviceImpl = CmisServiceImpl;
            string objectId = string.IsNullOrEmpty(id) ? CommonFunctions.GetRequestParameter(ServiceURIs.enumACLUri.objectId) : id;
            // queryString
            var onlyBasicPermissions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumACLUri.onlyBasicPermissions));

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);
            if (string.IsNullOrEmpty(objectId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("objectId"), serviceImpl);

            try
            {
                result = serviceImpl.GetACL(repositoryId, objectId, !onlyBasicPermissions.HasValue || onlyBasicPermissions.Value);
                if (result is null)
                {
                    result = cm.cmisFaultType.CreateUnknownException();
                }
                else if (result.Failure is null)
                {
                    var acl = result.Success;

                    if (acl is null)
                    {
                        return null;
                    }
                    else
                    {
                        var context = ssw.WebOperationContext.Current.OutgoingResponse;

                        context.StatusCode = System.Net.HttpStatusCode.OK;
                        context.ContentType = MediaTypes.Acl;

                        return css.ToXmlDocument(acl);
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsWebException(ex))
                {
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If EnableExceptionLogging = "True" Then
                    */
                    serviceImpl.LogException(ex);
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    throw;
                }
                else
                {
                    result = cm.cmisFaultType.CreateUnknownException(ex);
                }
            }

            // failure
            throw LogException(result.Failure, serviceImpl);
        }
        #endregion

        #region Miscellaneous
        /// <summary>
      /// Handles every POST on object resource.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public sx.XmlDocument CheckInOrUpdateProperties(string repositoryId, System.IO.Stream data)
        {
            var serviceImpl = CmisServiceImpl;
            sx.XmlDocument retVal = null;
            // queryString
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            string changeToken = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.changeToken);
            var major = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.major));
            var checkIn = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.checkin));
            string checkInComment = CommonFunctions.GetRequestParameter(ServiceURIs.enumObjectUri.checkinComment);

            if (data is null)
            {
                // parameters of checkIn can be defined completely through the queryString
                retVal = CheckIn(repositoryId, objectId, null, null, null, major, checkInComment);
            }
            else
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        // try to interpret data as a request-instance
                        var requestBase = ToRequest(ms, repositoryId);

                        if (requestBase is null)
                        {
                            var entry = ToAtomEntry(ms, false);

                            if (checkIn.HasValue && checkIn.Value)
                            {
                                retVal = CheckIn(repositoryId, objectId, entry, major, checkInComment);
                            }
                            else
                            {
                                retVal = UpdateProperties(repositoryId, objectId, entry, changeToken);
                            }
                        }
                        else if (requestBase is cm.Requests.checkIn)
                        {
                            cm.Requests.checkIn request = (cm.Requests.checkIn)requestBase;

                            retVal = CheckIn(repositoryId, objectId, request.Properties, request.Policies, request.ContentStream, request.Major, request.CheckinComment, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.updateProperties)
                        {
                            cm.Requests.updateProperties request = (cm.Requests.updateProperties)requestBase;
                            retVal = UpdateProperties(repositoryId, objectId, (ca.AtomEntry)request, changeToken);
                        }
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
            }

            if (retVal is null)
                throw LogException(cm.cmisFaultType.CreateUnknownException(), serviceImpl);

            return retVal;
        }

        /// <summary>
      /// Handles every POST on the folder children collection. As defined in 3.9.2.2 HTTP POST the function has to return in the AtomPub-Binding
      /// an ca.AtomEntry-Object (MediaType: application/atom+xml;type=entry)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// Handled cmis services:
      /// createDocument, createDocumentFromSource, createFolder, createPolicy, moveObject
      /// The function supports data as a serialized ca.AtomEntry-instance or as a serialized request-Object
      /// out of the Namespace CmisObjectModel.Messaging.Requests
      /// </remarks>
        public sx.XmlDocument CreateOrMoveChildObject(string repositoryId, System.IO.Stream data)
        {
            var serviceImpl = CmisServiceImpl;
            sx.XmlDocument retVal = null;
            var context = ssw.WebOperationContext.Current.OutgoingResponse;
            // queryString
            var allVersions = CommonFunctions.ParseBoolean(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.allVersions) ?? "true");
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.folderId) ?? CommonFunctions.GetRequestParameter("id");
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.objectId);
            string sourceFolderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.sourceFolderId);
            string sourceId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.sourceId);
            string targetFolderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.targetFolderId);
            var versioningState = CommonFunctions.ParseEnum<Core.enumVersioningState>(CommonFunctions.GetRequestParameter(ServiceURIs.enumChildrenUri.versioningState));

            if (data is null)
            {
                // parameters of addObjectToFolder-, createDocumentFromSource- and moveObject-service can be defined completely through the queryString
                if (!string.IsNullOrEmpty(sourceId))
                {
                    retVal = this.CreateDocumentFromSource(repositoryId, sourceId, null, folderId, versioningState, null, null, null);
                }
                else if (CmisServiceImpl.get_Exists(repositoryId, objectId))
                {
                    // the specified object already exists in the repository: detect if the object should be moved or multi-filed
                    if (!string.IsNullOrEmpty(sourceFolderId) && !string.IsNullOrEmpty(targetFolderId))
                    {
                        // moveObject
                        retVal = MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);
                    }
                    else
                    {
                        // addObjectToFolder
                        retVal = AddObjectToFolder(repositoryId, objectId, folderId, !allVersions.HasValue || allVersions.Value);
                    }
                }
            }
            else
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        // try to interpret data as a request-instance
                        var requestBase = ToRequest(ms, repositoryId);

                        if (requestBase is null)
                        {
                            var entry = ToAtomEntry(ms, true);

                            // higher priority from objectIdProperty if set
                            if (entry is not null)
                                objectId = entry.ObjectId.NVL(objectId);
                            if (!string.IsNullOrEmpty(sourceId))
                            {
                                retVal = this.CreateDocumentFromSource(repositoryId, sourceId, entry.Object is null ? null : entry.Object.Properties, folderId, versioningState, entry.Object is null || entry.Object.PolicyIds is null ? null : entry.Object.PolicyIds.Ids, null, null);
                            }
                            else if (CmisServiceImpl.get_Exists(repositoryId, objectId))
                            {
                                // the specified object already exists in the repository: detect if the object should be moved or multi-filed
                                if (!string.IsNullOrEmpty(sourceFolderId) && !string.IsNullOrEmpty(targetFolderId))
                                {
                                    // moveObject
                                    retVal = MoveObject(repositoryId, objectId, targetFolderId, sourceFolderId);
                                }
                                else
                                {
                                    // addObjectToFolder
                                    retVal = AddObjectToFolder(repositoryId, objectId, folderId, !allVersions.HasValue || allVersions.Value);
                                }
                            }
                            else
                            {
                                var typeDefinition = CmisServiceImpl.TypeDefinition(repositoryId, entry is null ? null : entry.TypeId);

                                if (typeDefinition is ccdt.cmisTypeDocumentDefinitionType)
                                {
                                    retVal = this.CreateDocument(repositoryId, folderId, versioningState, entry);
                                }
                                else if (typeDefinition is ccdt.cmisTypeFolderDefinitionType)
                                {
                                    retVal = CreateFolder(repositoryId, folderId, entry);
                                }
                                else if (typeDefinition is ccdt.cmisTypeItemDefinitionType)
                                {
                                    retVal = CreateItem(repositoryId, folderId, entry);
                                }
                                else if (typeDefinition is ccdt.cmisTypePolicyDefinitionType)
                                {
                                    retVal = CreatePolicy(repositoryId, folderId, entry);
                                }
                            }
                        }
                        else if (requestBase is cm.Requests.addObjectToFolder)
                        {
                            cm.Requests.addObjectToFolder request = (cm.Requests.addObjectToFolder)requestBase;
                            retVal = AddObjectToFolder(repositoryId, request.ObjectId, request.FolderId, !request.AllVersions.HasValue || request.AllVersions.Value);
                        }
                        else if (requestBase is cm.Requests.createDocument)
                        {
                            cm.Requests.createDocument request = (cm.Requests.createDocument)requestBase;
                            retVal = CreateDocument(repositoryId, request.FolderId, request.VersioningState, (ca.AtomEntry)request, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.createDocumentFromSource)
                        {
                            cm.Requests.createDocumentFromSource request = (cm.Requests.createDocumentFromSource)requestBase;
                            retVal = CreateDocumentFromSource(repositoryId, request.SourceId, request.Properties, request.FolderId, request.VersioningState, request.Policies, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.createFolder)
                        {
                            cm.Requests.createFolder request = (cm.Requests.createFolder)requestBase;
                            retVal = CreateFolder(repositoryId, request.FolderId, (ca.AtomEntry)request, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.createItem)
                        {
                            cm.Requests.createItem request = (cm.Requests.createItem)requestBase;
                            retVal = CreateItem(repositoryId, request.FolderId, (ca.AtomEntry)request, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.createPolicy)
                        {
                            cm.Requests.createPolicy request = (cm.Requests.createPolicy)requestBase;
                            retVal = CreatePolicy(repositoryId, request.FolderId, (ca.AtomEntry)request, request.AddACEs, request.RemoveACEs);
                        }
                        else if (requestBase is cm.Requests.moveObject)
                        {
                            cm.Requests.moveObject request = (cm.Requests.moveObject)requestBase;
                            retVal = MoveObject(repositoryId, request.ObjectId, request.TargetFolderId, request.SourceFolderId);
                        }
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
            }

            if (retVal is null)
                throw LogException(cm.cmisFaultType.CreateUnknownException(), serviceImpl);

            return retVal;
        }

        /// <summary>
      /// Returns the new object created in the unfiled-resource.
      /// Handles every POST on the unfiled collection. As defined in 3.9.2.2 HTTP POST the function has to return in the AtomPub-Binding
      /// an ca.AtomEntry-Object (MediaType: application/atom+xml;type=entry)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// Handled cmis services:
      /// createDocument, createDocumentFromSource, createPolicy, removeObjectFromFolder
      /// The function supports data as a serialized ca.AtomEntry-instance or as a serialized request-Object
      /// out of the Namespace CmisObjectModel.Messaging.Requests
      /// Parameter folderId should not be set, if a non existing object should be created.
      /// </remarks>
        public sx.XmlDocument CreateUnfiledObjectOrRemoveObjectFromFolder(string repositoryId, System.IO.Stream data)
        {
            var serviceImpl = CmisServiceImpl;
            // queryString
            string objectId = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.objectId) ?? CommonFunctions.GetRequestParameter("id");
            string folderId = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.folderId) ?? CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.removeFrom);
            string sourceId = CommonFunctions.GetRequestParameter(ServiceURIs.enumUnfiledUri.sourceId);

            // invalid arguments
            if (string.IsNullOrEmpty(repositoryId))
                throw LogException(cm.cmisFaultType.CreateInvalidArgumentException("repositoryId"), serviceImpl);

            if (data is not null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    data.CopyTo(ms);

                    try
                    {
                        // try to interpret data as a request-instance
                        var requestBase = ToRequest(ms, repositoryId);

                        if (requestBase is null)
                        {
                            var entry = ToAtomEntry(ms, true);

                            // higher priority from objectIdProperty if set
                            if (entry is not null)
                                objectId = entry.ObjectId.NVL(objectId);
                        }
                        else if (requestBase is cm.Requests.removeObjectFromFolder)
                        {
                            cm.Requests.removeObjectFromFolder request = (cm.Requests.removeObjectFromFolder)requestBase;
                            objectId = request.ObjectId.NVL(objectId);
                            folderId = request.FolderId.NVL(folderId);
                        }
                    }
                    finally
                    {
                        ms.Close();
                    }
                }
            }

            if (!string.IsNullOrEmpty(folderId))
            {
                return RemoveObjectFromFolder(repositoryId, objectId, folderId);
            }
            else
            {
                return CreateOrMoveChildObject(repositoryId, data);
            }
        }
        #endregion

        /// <summary>
      /// Adds the location of the object to the response
      /// </summary>
      /// <param name="context"></param>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="uriTemplate">If not set, the uriTemplate of an object is assumed</param>
      /// <remarks></remarks>
        private void AddLocation(ssw.OutgoingWebResponseContext context, string repositoryId, string id, string uriTemplate = null)
        {
            if (string.IsNullOrEmpty(uriTemplate))
                uriTemplate = ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.objectId);

            {
                var withBlock = BaseUri.Combine(uriTemplate.ReplaceUri("repositoryId", repositoryId, "id", id));
                context.Location = withBlock.AbsoluteUri;
            }
        }

        /// <summary>
      /// Transforms data to a XmlReader-instance and returns via createInstance.Invoke() the specified result-type
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="data"></param>
      /// <param name="createInstance"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private TResult ConvertData<TResult>(System.IO.MemoryStream data, Func<sx.XmlReader, TResult> createInstance, sxs.XmlAttributeOverrides attrOverrides = null)
        {
            try
            {
                data.Position = 0L;

                var reader = sx.XmlReader.Create(data);

                if (attrOverrides is null)
                {
                    return createInstance(reader);
                }
                else
                {
                    using (var attributeOverrides = new Serialization.XmlAttributeOverrides(reader, attrOverrides))
                    {
                        return createInstance(reader);
                    }
                }
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
      /// Creates a AtomEntry-instance for the given serviceModelObject
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private ca.AtomEntry CreateAtomEntry(AtomPubObjectGeneratingGuidance generatingGuidance, Contracts.IServiceModelObject serviceModelObject)
        {
            Contracts.IServiceModelObjectEnumerable asEnumerable = serviceModelObject as Contracts.IServiceModelObjectEnumerable;
            var cmisObject = serviceModelObject is null ? null : serviceModelObject.Object;

            if (cmisObject is null)
            {
                return null;
            }
            else
            {
                var serviceModel = serviceModelObject.ServiceModel;
                string objectId = serviceModel.ObjectId;
                var children = CreateAtomFeed(generatingGuidance, serviceModel.VersionSeriesId, asEnumerable);
                bool includeRelativePathSegment = generatingGuidance.IncludeRelativePathSegment.HasValue && generatingGuidance.IncludeRelativePathSegment.Value;
                bool includePathSegment = generatingGuidance.IncludePathSegment.HasValue && generatingGuidance.IncludePathSegment.Value;
                List<ca.AtomLink> links;

                switch (serviceModel.BaseObjectType)
                {
                    case Core.enumBaseObjectTypeIds.cmisDocument:
                        {
                            var allowableActions = cmisObject.AllowableActions;
                            bool canGetAllVersions = allowableActions is not null && allowableActions.CanGetAllVersions.HasValue && allowableActions.CanGetAllVersions.Value;
                            links = cmisObject.GetDocumentLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository, canGetAllVersions, serviceModel.IsLatestVersion, serviceModel.VersionSeriesId, serviceModel.VersionSeriesCheckedOutId);
                            break;
                        }
                    case Core.enumBaseObjectTypeIds.cmisFolder:
                        {
                            links = cmisObject.GetFolderLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository, serviceModel.ParentId);
                            break;
                        }
                    case Core.enumBaseObjectTypeIds.cmisItem:
                        {
                            links = cmisObject.GetItemLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository);
                            break;
                        }
                    case Core.enumBaseObjectTypeIds.cmisPolicy:
                        {
                            links = cmisObject.GetPolicyLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository);
                            break;
                        }
                    case Core.enumBaseObjectTypeIds.cmisRelationship:
                        {
                            links = cmisObject.GetRelationshipLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository, serviceModel.SourceId, serviceModel.TargetId);
                            break;
                        }
                    case Core.enumBaseObjectTypeIds.cmisSecondary:
                        {
                            links = cmisObject.GetSecondaryLinks(generatingGuidance.ServiceImpl.BaseUri, generatingGuidance.Repository);
                            break;
                        }

                    default:
                        {
                            links = null;
                            break;
                        }
                }
                return new ca.AtomEntry("urn:objects:" + objectId, objectId, serviceModel.Summary, serviceModel.PublishDate, serviceModel.LastUpdatedTime, cmisObject, serviceModel.ContentLink, children, links, includeRelativePathSegment ? serviceModelObject.RelativePathSegment : null, includePathSegment ? serviceModelObject.PathSegment : null, serviceModel.Authors);
            }
        }

        /// <summary>
      /// Creates a list of AtomEntries based on objects
      /// </summary>
      /// <returns></returns>
      /// <remarks>Supported objects-enumeration: the items MUST implement the Contracts.IServiceModelObject-interface</remarks>
        private List<ca.AtomEntry> CreateAtomEntryList(AtomPubObjectGeneratingGuidance generatingGuidance)
        {
            if (generatingGuidance.Objects is null)
            {
                return new List<ca.AtomEntry>();
            }
            else
            {
                Type t = generatingGuidance.Objects.GetType();

                if (t == typeof(cmisObjectInFolderListType))
                {
                    return (from item in (cmisObjectInFolderListType)generatingGuidance.Objects
                            let entry = CreateAtomEntry(generatingGuidance, item as Contracts.IServiceModelObject)
                            where entry is not null
                            select entry).ToList();
                }
                else if (t == typeof(cmisObjectListType))
                {
                    return (from item in (cmisObjectListType)generatingGuidance.Objects
                            let entry = CreateAtomEntry(generatingGuidance, item as Contracts.IServiceModelObject)
                            where entry is not null
                            select entry).ToList();
                }
                else
                {
                    return (from item in (cmisObjectInFolderContainerType[])generatingGuidance.Objects
                            let entry = CreateAtomEntry(generatingGuidance, item as Contracts.IServiceModelObject)
                            where entry is not null
                            select entry).ToList();
                }
            }
        }

        /// <summary>
      /// Creates an AtomFeed-object respecting the generatingGuidance
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private ca.AtomFeed CreateAtomFeed(AtomPubObjectGeneratingGuidance generatingGuidance)
        {
            var entries = CreateAtomEntryList(generatingGuidance);

            return new ca.AtomFeed("urn:feeds:" + generatingGuidance.UrnSuffix, "Result of " + generatingGuidance.MethodName + "('" + generatingGuidance.RepositoryId + "'" + (generatingGuidance.ChangeLogToken.HasValue ? ", changeLogToken:='" + generatingGuidance.ChangeLogToken.Value + "'" : null) + (generatingGuidance.Filter.HasValue ? ", filter:='" + generatingGuidance.Filter.Value + "'" : null) + (generatingGuidance.FolderId.HasValue ? ", folderId:='" + generatingGuidance.FolderId.Value + "'" : null) + (generatingGuidance.IncludeACL.HasValue ? ", includeACL:=" + Conversions.ToString(generatingGuidance.IncludeACL.Value) : null) + (generatingGuidance.IncludeAllowableActions.HasValue ? ", includeAllowableActions:=" + Conversions.ToString(generatingGuidance.IncludeAllowableActions.Value) : null) + (generatingGuidance.IncludePathSegment.HasValue ? ", includePathSegment:=" + Conversions.ToString(generatingGuidance.IncludePathSegment.Value) : null) + (generatingGuidance.IncludePolicyIds.HasValue ? ", includePolicyIds:=" + Conversions.ToString(generatingGuidance.IncludePolicyIds.Value) : null) + (generatingGuidance.IncludeProperties.HasValue ? ", includeProperties:=" + Conversions.ToString(generatingGuidance.IncludeProperties.Value) : null) + (generatingGuidance.IncludeRelationships.HasValue ? ", includeRelationships:=" + generatingGuidance.IncludeRelationships.Value.GetName() : null) + (generatingGuidance.IncludeRelativePathSegment.HasValue ? ", includeRelativePathSegment:=" + Conversions.ToString(generatingGuidance.IncludeRelativePathSegment.Value) : null) + (generatingGuidance.IncludeSubRelationshipTypes.HasValue ? ", includeSubRelationshipTypes:=" + Conversions.ToString(generatingGuidance.IncludeSubRelationshipTypes.Value) : null) + (generatingGuidance.MaxItems.HasValue ? ", maxItems:=" + generatingGuidance.MaxItems.Value.ToString() : null) + (generatingGuidance.ObjectId.HasValue ? ", objectId:='" + generatingGuidance.ObjectId.Value + "'" : null) + (generatingGuidance.OrderBy.HasValue ? ", orderBy:='" + generatingGuidance.OrderBy.Value + "'" : null) + (generatingGuidance.Query.HasValue ? ", query:='" + generatingGuidance.Query.Value + "'" : null) + (generatingGuidance.RelationshipDirection.HasValue ? ", relationshipDirection:=" + generatingGuidance.RelationshipDirection.Value.GetName() : null) + (generatingGuidance.RenditionFilter.HasValue ? ", renditionFilter:='" + generatingGuidance.RenditionFilter.Value + "'" : null) + (generatingGuidance.SearchAllVersions.HasValue ? ", searchAllVersions:=" + Conversions.ToString(generatingGuidance.SearchAllVersions.Value) : null) + (generatingGuidance.SkipCount.HasValue ? ", skipCount:=" + generatingGuidance.SkipCount.Value.ToString() : null) + (generatingGuidance.TypeId.HasValue ? ", typeId:='" + generatingGuidance.TypeId.Value + "'" : null) + (generatingGuidance.VersionSeriesId.HasValue ? ", versionSeriesId:='" + generatingGuidance.VersionSeriesId.Value + "'" : null) + ")", DateTimeOffset.UtcNow, entries, generatingGuidance.HasMoreItems, generatingGuidance.NumItems, generatingGuidance.Links, generatingGuidance.ServiceImpl.GetSystemAuthor());
        }

        /// <summary>
      /// Creates an AtomFeed-object to encapsulate the children of a folder-object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private ca.AtomFeed CreateAtomFeed(AtomPubObjectGeneratingGuidance generatingGuidance, string folderId, Contracts.IServiceModelObjectEnumerable children)
        {
            if (children is null || !children.ContainsObjects)
            {
                return null;
            }
            else
            {
                try
                {
                    // modify several properties
                    generatingGuidance.BeginTransaction();
                    if (generatingGuidance.Depth.HasValue)
                    {
                        long depth = generatingGuidance.Depth.Value;
                        if (depth > 1L)
                            generatingGuidance.Depth = depth - 1L;
                    }
                    generatingGuidance.FolderId = folderId;
                    generatingGuidance.HasMoreItems = children.HasMoreItems;
                    generatingGuidance.NumItems = children.NumItems;
                    generatingGuidance.Objects = children;
                    generatingGuidance.UrnSuffix = "descendants:" + folderId;
                    {
                        var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumDescendantsUri>(generatingGuidance.BaseUri, generatingGuidance.RepositoryId, queryString => ServiceURIs.get_DescendantsUri(queryString));
                        withBlock.Add(ServiceURIs.enumDescendantsUri.folderId, folderId);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.filter, generatingGuidance.Filter.Value);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.depth, generatingGuidance.Depth);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includeAllowableActions, generatingGuidance.IncludeAllowableActions);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includeRelationships, generatingGuidance.IncludeRelationships);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.renditionFilter, generatingGuidance.RenditionFilter.Value);
                        withBlock.Add(ServiceURIs.enumDescendantsUri.includePathSegment, generatingGuidance.IncludePathSegment);

                        {
                            var withBlock1 = new LinkFactory(generatingGuidance.ServiceImpl, generatingGuidance.RepositoryId, generatingGuidance.FolderId, withBlock.ToUri());
                            generatingGuidance.Links = withBlock1.CreateDescendantsLinks();
                        }
                    }
                    return CreateAtomFeed(generatingGuidance);
                }
                finally
                {
                    // restore descriptor
                    generatingGuidance.EndTransaction();
                }
            }
        }

        /// <summary>
      /// Creates an AtomFeed-object based on containers
      /// </summary>
      /// <param name="containers"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private ca.AtomFeed CreateAtomFeed(string repositoryId, string typeId, bool includePropertyDefinitions, long? depth, cm.cmisTypeContainer[] containers, Uri baseUri)
        {
            if (containers is null || containers.Length == 0)
            {
                return new ca.AtomFeed();
            }
            else
            {
                var serviceImpl = CmisServiceImpl;
                List<ca.AtomLink> links;

                // 2.2.2.4 getTypeDescendants:
                // If typeId is not specified, then the Repository MUST return all types and MUST ignore the value of the depth parameter
                if (string.IsNullOrEmpty(typeId))
                    depth = -1;
                {
                    var withBlock = new SelfLinkUriBuilder<ServiceURIs.enumTypeDescendantsUri>(baseUri, repositoryId, queryString => ServiceURIs.get_TypeDescendantsUri(queryString));
                    withBlock.Add(ServiceURIs.enumTypeDescendantsUri.typeId, typeId);
                    withBlock.Add(ServiceURIs.enumTypeDescendantsUri.includePropertyDefinitions, includePropertyDefinitions);
                    withBlock.Add(ServiceURIs.enumTypeDescendantsUri.depth, depth);

                    {
                        var withBlock1 = new LinkFactory(serviceImpl, repositoryId, typeId, withBlock.ToUri());
                        links = withBlock1.CreateTypeDescendantsLinks();
                    }
                }

                var entries = (from container in containers
                               where container.Type is not null
                               let children = container.Children is null || container.Children.Length == 0 ? null : CreateAtomFeed(repositoryId, container.Type.Id, includePropertyDefinitions, depth.HasValue && depth.Value > 1L ? depth.Value - 1L : depth, container.Children, baseUri)
                               select new ca.AtomEntry(container.Type, children, container.Type.GetLinks(baseUri, repositoryId))).ToList();
                return new ca.AtomFeed("urn:feeds:typeDescendants:" + typeId, "Result of GetTypeDescendants('" + repositoryId + "', typeId:='" + typeId + "', depth:=" + (depth.HasValue ? depth.Value.ToString() : "Null") + ", includePropertyDefinitions:=" + includePropertyDefinitions + ")", DateTimeOffset.UtcNow, entries, false, entries.Count, links, serviceImpl.GetSystemAuthor());
            }
        }

        /// <summary>
      /// erstellt zum serviceModelObject passend ein AtomEntry-Objekt mit Location-Angaben im Context der
      /// Web-Anfrage
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreateXmlDocument(string repositoryId, Contracts.ICmisServicesImpl serviceImpl, cmisObjectType cmisObject, System.Net.HttpStatusCode status, bool addLocation)
        {
            if (cmisObject is null)
            {
                return null;
            }
            else
            {
                var context = ssw.WebOperationContext.Current.OutgoingResponse;
                var entry = CreateAtomEntry(new AtomPubObjectGeneratingGuidance(repositoryId, serviceImpl), cmisObject);

                if (entry is null)
                {
                    return null;
                }
                else
                {
                    context.ContentType = MediaTypes.Entry;
                    context.StatusCode = status;
                    if (addLocation)
                        AddLocation(context, repositoryId, cmisObject.ServiceModel.ObjectId);

                    return css.ToXmlDocument(new sss.Atom10ItemFormatter(entry));
                }
            }
        }

        /// <summary>
      /// erstellt zum contentStreamResponse passend ein AtomEntry-Objekt mit Location-Angaben im Context der
      /// Web-Anfrage
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private sx.XmlDocument CreateXmlDocument(string repositoryId, string objectId, Contracts.ICmisServicesImpl serviceImpl, cm.Responses.setContentStreamResponse contentStreamResponse)
        {
            var context = ssw.WebOperationContext.Current.OutgoingResponse;

            if (contentStreamResponse is null)
            {
                contentStreamResponse = new cm.Responses.setContentStreamResponse(objectId, null, cm.enumSetContentStreamResult.NotSet);
            }
            context.StatusCode = contentStreamResponse.StatusCode;
            context.ContentType = MediaTypes.Xml;

            {
                var withBlock = BaseUri.Combine(ServiceURIs.get_ContentUri(ServiceURIs.enumContentUri.objectId).ReplaceUri("repositoryId", repositoryId, "id", objectId));
                context.Headers.Add("Content-Location", withBlock.AbsoluteUri);
            }
            AddLocation(context, repositoryId, objectId);
            return css.ToXmlDocument(contentStreamResponse);
        }

        /// <summary>
        /// Serializes repositories with AtomPub10ServiceDocumentFormatter
        /// </summary>
        private sx.XmlDocument SerializeRepositories(Core.cmisRepositoryInfoType[] repositories)
        {
            if (repositories == null)
                // statuscode already set!
                return null/* TODO Change to default(_) if this is not a reference type */;
            else
            {
                Uri baseUri = CmisServiceImpl.BaseUri;
                ca.AtomWorkspace[] workspaces = (from repositoryInfo in repositories
                                                 where repositoryInfo != null
                                                 select new ca.AtomWorkspace(repositoryInfo.RepositoryName, repositoryInfo, repositoryInfo.GetCollectionInfos(baseUri), repositoryInfo.GetLinks(baseUri, Constants.Namespaces.atom, "link"), repositoryInfo.GetUriTemplates(baseUri))).ToArray();

                var withBlock = new ca.AtomServiceDocument(workspaces);
                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                ssw.WebOperationContext.Current.OutgoingResponse.ContentType = Constants.MediaTypes.Service;
                
                return css.ToXmlDocument((sss.AtomPub10ServiceDocumentFormatter)withBlock.GetFormatter());
            }
        }

        /// <summary>
        /// Creates an AtomEntry-instance from data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private ca.AtomEntry ToAtomEntry(System.IO.MemoryStream data, bool ensurePOSTRuleOfPrecedence)
        {
            var retVal = ConvertData(data, ca.AtomEntry.CreateInstance);
            if (retVal is not null && ensurePOSTRuleOfPrecedence)
                retVal.EnsurePOSTRuleOfPrecedence();
            return retVal;
        }

        /// <summary>
      /// Creates an AtomFeed-instance from data
      /// </summary>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private ca.AtomFeed ToAtomFeed(System.IO.MemoryStream data)
        {
            return ConvertData(data, ca.AtomFeed.CreateInstance);
        }

        /// <summary>
      /// Creates a RequestBase-instance from data
      /// </summary>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private cm.Requests.RequestBase ToRequest(System.IO.MemoryStream data, string repositoryId)
        {
            cm.Requests.RequestBase retVal;
            var attributeOverrides = new sxs.XmlAttributeOverrides();
            var attrs = new sxs.XmlAttributes() { XmlRoot = new sxs.XmlRootAttribute() { Namespace = "" } }; // ignore Namespace

            attributeOverrides.Add(typeof(cm.Requests.RequestBase), attrs);
            retVal = ConvertData(data, cm.Requests.RequestBase.CreateInstance, attributeOverrides);
            if (retVal is not null)
                retVal.ReadQueryString(repositoryId);
            return retVal;
        }

    }
}