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
using CmisObjectModel.Constants;
using cm = CmisObjectModel.Messaging;
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
   /// Simplifies requests to cmis document
   /// </summary>
   /// <remarks></remarks>
    public class CmisDocument : CmisObject
    {

        #region Constructors
        public CmisDocument(Core.cmisObjectType cmisObject, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(cmisObject, client, repositoryInfo)
        {

            _onPWCRemoved = OnPWCRemoved;
        }
        #endregion

        #region Predefined properties
        public virtual ccg.Nullable<string> CheckinComment
        {
            get
            {
                return _cmisObject.CheckinComment;
            }
            set
            {
                _cmisObject.CheckinComment = value;
            }
        }

        public virtual ccg.Nullable<string> ContentStreamFileName
        {
            get
            {
                return _cmisObject.ContentStreamFileName;
            }
            set
            {
                _cmisObject.ContentStreamFileName = value;
            }
        }

        public virtual ccg.Nullable<string> ContentStreamId
        {
            get
            {
                return _cmisObject.ContentStreamId;
            }
            set
            {
                _cmisObject.ContentStreamId = value;
            }
        }

        public virtual long? ContentStreamLength
        {
            get
            {
                return _cmisObject.ContentStreamLength;
            }
            set
            {
                _cmisObject.ContentStreamLength = value;
            }
        }

        public virtual ccg.Nullable<string> ContentStreamMimeType
        {
            get
            {
                return _cmisObject.ContentStreamMimeType;
            }
            set
            {
                _cmisObject.ContentStreamMimeType = value;
            }
        }

        public virtual bool? IsImmutable
        {
            get
            {
                return _cmisObject.IsImmutable;
            }
            set
            {
                _cmisObject.IsImmutable = value;
            }
        }

        public virtual bool? IsLatestMajorVersion
        {
            get
            {
                return _cmisObject.IsLatestMajorVersion;
            }
            set
            {
                _cmisObject.IsLatestMajorVersion = value;
            }
        }

        public virtual bool? IsLatestVersion
        {
            get
            {
                return _cmisObject.IsLatestVersion;
            }
            set
            {
                _cmisObject.IsLatestVersion = value;
            }
        }

        public virtual bool? IsMajorVersion
        {
            get
            {
                return _cmisObject.IsMajorVersion;
            }
            set
            {
                _cmisObject.IsMajorVersion = value;
            }
        }

        public virtual bool? IsPrivateWorkingCopy
        {
            get
            {
                if (_repositoryInfo is not null && _repositoryInfo.CmisVersionSupported == "1.0")
                {
                    // the version 1.0 doesn't support cmis:isPrivateWorkingCopy-property
                    {
                        ref var withBlock = ref _cmisObject;
                        if (withBlock.ObjectId.HasValue && withBlock.VersionSeriesCheckedOutId.HasValue)
                        {
                            return (_cmisObject.ObjectId.Value ?? "") == (_cmisObject.VersionSeriesCheckedOutId.Value ?? "");
                        }
                        else
                        {
                            return default;
                        }
                    }
                }
                else
                {
                    return _cmisObject.IsPrivateWorkingCopy;
                }
            }
        }

        public virtual bool? IsVersionSeriesCheckedOut
        {
            get
            {
                return _cmisObject.IsVersionSeriesCheckedOut;
            }
            set
            {
                _cmisObject.IsVersionSeriesCheckedOut = value;
            }
        }

        public virtual ccg.Nullable<string> VersionLabel
        {
            get
            {
                return _cmisObject.VersionLabel;
            }
            set
            {
                _cmisObject.VersionLabel = value;
            }
        }

        public virtual ccg.Nullable<string> VersionSeriesCheckedOutBy
        {
            get
            {
                return _cmisObject.VersionSeriesCheckedOutBy;
            }
            set
            {
                _cmisObject.VersionSeriesCheckedOutBy = value;
            }
        }

        public virtual ccg.Nullable<string> VersionSeriesCheckedOutId
        {
            get
            {
                return _cmisObject.VersionSeriesCheckedOutId;
            }
            set
            {
                _cmisObject.VersionSeriesCheckedOutId = value;
            }
        }

        public virtual ccg.Nullable<string> VersionSeriesId
        {
            get
            {
                return _cmisObject.VersionSeriesId;
            }
            set
            {
                _cmisObject.VersionSeriesId = value;
            }
        }
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the current document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool AppendContentStream(cm.cmisContentStreamType contentStream, bool isLastChunk = false)
        {
            {
                var withBlock = _client.AppendContentStream(new cmr.appendContentStream()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    ContentStream = contentStream,
                    IsLastChunk = isLastChunk,
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
      /// Deletes the content stream for the specified document object
      /// </summary>
        public cm.Responses.deleteContentStreamResponse DeleteContentStream(string changeToken = null)
        {
            {
                var withBlock = _client.DeleteContentStream(new cmr.deleteContentStream()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    ChangeToken = changeToken
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Deletes the current object
      /// </summary>
        public override bool DeleteObject(bool allVersions = true)
        {
            try
            {
                _onPWCRemovedPaused = true;
                return base.DeleteObject(allVersions);
            }
            finally
            {
                _onPWCRemovedPaused = false;
            }
        }

        /// <summary>
      /// Gets the content stream for the current document object, or gets a rendition stream for a specified rendition of the current document
      /// </summary>
      /// <param name="streamId"></param>
      /// <param name="offset"></param>
      /// <param name="length"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        /* TODO ERROR: Skipped IfDirectiveTrivia
        #If HttpRequestAddRangeShortened Then
        *//* TODO ERROR: Skipped DisabledTextTrivia
              Public Shadows Function GetContentStream(Optional streamId As String = Nothing,
                                                       Optional offset As Integer? = Nothing,
                                                       Optional length As Integer? = Nothing) As Messaging.cmisContentStreamType
        *//* TODO ERROR: Skipped ElseDirectiveTrivia
        #Else
        */
        public new cm.cmisContentStreamType GetContentStream(string streamId = null, long? offset = default, long? length = default)
        {
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            return base.GetContentStream(streamId, offset, length);
        }

        /// <summary>
      /// Sets the content stream for the current document object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool SetContentStream(cm.cmisContentStreamType contentStream, bool overwriteFlag = true)
        {
            {
                var withBlock = _client.SetContentStream(new cmr.setContentStream()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    ContentStream = contentStream,
                    OverwriteFlag = overwriteFlag,
                    ChangeToken = _cmisObject.ChangeToken
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    if (withBlock.Response is not null)
                    {
                        string objectId = withBlock.Response.ObjectId;
                        string changeToken = withBlock.Response.ChangeToken;

                        if (!string.IsNullOrEmpty(objectId))
                            _cmisObject.ObjectId = objectId;
                        if (!string.IsNullOrEmpty(changeToken))
                            _cmisObject.ChangeToken = changeToken;
                    }

                    // get last modification info from server
                    string filter = string.Join(",", CmisPredefinedPropertyNames.LastModificationDate, CmisPredefinedPropertyNames.LastModifiedBy);
                    var cmisObject = GetObject(_cmisObject.ObjectId, filter);
                    if (cmisObject is not null)
                    {
                        _cmisObject.LastModificationDate = cmisObject.LastModificationDate;
                        _cmisObject.LastModifiedBy = cmisObject.LastModifiedBy;
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

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents
      /// in the version series to be checked out again. If the private working copy has been created by createDocument, cancelCheckOut MUST
      /// delete the created document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool CancelCheckOut(bool? pwcRequired = default)
        {
            var objectOfLatestVersion = GetObjectOfLatestVersion(filter: string.Join(",", CmisPredefinedPropertyNames.Description, CmisPredefinedPropertyNames.ObjectId));
            string cancelCheckOutFallbackId = CancelCheckOutFallbackId;

            try
            {
                _onPWCRemovedPaused = true;
                {
                    var withBlock = _client.CancelCheckOut(new cmr.cancelCheckOut()
                    {
                        RepositoryId = _repositoryInfo.RepositoryId,
                        ObjectId = _cmisObject.ObjectId,
                        PWCLinkRequired = pwcRequired.HasValue ? pwcRequired.Value : !(IsPrivateWorkingCopy.HasValue && IsPrivateWorkingCopy.Value)
                    });
                    _lastException = withBlock.Exception;
                    if (_lastException is null)
                    {
                        bool isAdded = string.IsNullOrEmpty(cancelCheckOutFallbackId);
                        var cmisObject = isAdded ? null : GetObject(cancelCheckOutFallbackId);
                        if (cmisObject is CmisDocument)
                        {
                            _cmisObject = cmisObject.Object;
                            CancelCheckOutFallbackId = null;
                            return true;
                        }
                    }
                    return false;
                }
            }
            finally
            {
                _onPWCRemovedPaused = false;
                // remove complete versionseries if the CancelCheckOut belongs to a document created with versioningState checkedout
                if (objectOfLatestVersion is not null && (objectOfLatestVersion.Description.Value ?? string.Empty).StartsWith(VersioningStateCheckedOutPrefix))
                {
                    objectOfLatestVersion.DeleteObject(true);
                }
            }
        }

        /// <summary>
      /// Checks-in the Private Working Copy document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool CheckIn(bool major = true, Core.Collections.cmisPropertiesType properties = null, cm.cmisContentStreamType contentStream = null, string checkinComment = null, Core.Collections.cmisListOfIdsType policies = null, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null, bool? pwcRequired = default)
        {
            try
            {
                _onPWCRemovedPaused = true;
                {
                    var withBlock = _client.CheckIn(new cmr.checkIn()
                    {
                        RepositoryId = _repositoryInfo.RepositoryId,
                        ObjectId = _cmisObject.ObjectId,
                        Major = major,
                        Properties = properties,
                        ContentStream = contentStream,
                        CheckinComment = checkinComment,
                        Policies = policies,
                        AddACEs = addACEs,
                        RemoveACEs = removeACEs,
                        PWCLinkRequired = pwcRequired.HasValue ? pwcRequired.Value : !(IsPrivateWorkingCopy.HasValue && IsPrivateWorkingCopy.Value)
                    });
                    _lastException = withBlock.Exception;
                    if (_lastException is null)
                    {
                        var cmisObject = GetObject(withBlock.Response.ObjectId);
                        if (cmisObject is CmisDocument)
                        {
                            _cmisObject = cmisObject.Object;
                            CancelCheckOutFallbackId = null;
                            return true;
                        }
                    }
                    return false;
                }
            }
            finally
            {
                _onPWCRemovedPaused = false;
            }
        }

        /// <summary>
      /// Create a private working copy (PWC) of the document
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool CheckOut()
        {
            bool retVal = false;

            try
            {
                if (IsPrivateWorkingCopy.HasValue && IsPrivateWorkingCopy.Value)
                {
                    if (string.IsNullOrEmpty(CancelCheckOutFallbackId))
                    {
                        CmisObject cmisObject = GetObjectOfLatestVersion();
                        if (cmisObject is CmisDocument)
                            CancelCheckOutFallbackId = cmisObject.ObjectId;
                    }
                    // already checked out
                    retVal = true;
                }
                else
                {
                    {
                        var withBlock = _client.CheckOut(new cmr.checkOut() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = _cmisObject.ObjectId });
                        _lastException = withBlock.Exception;
                        if (_lastException is null)
                        {
                            var cmisObject = GetObject(withBlock.Response.ObjectId);
                            if (cmisObject is CmisDocument)
                            {
                                CancelCheckOutFallbackId = _cmisObject.ObjectId;
                                _cmisObject = cmisObject.Object;
                                retVal = true;
                            }
                        }
                    }
                }
            }
            finally
            {
                // this is a pwc; we have to install listeners, because another CmisDocument-instance or a low-level client request within this
                // application might execute a checkIn or cancelCheckout
                if (retVal)
                    AddPWCRemovedListeners();
            }

            return retVal;
        }

        /// <summary>
      /// Returns the list of all document objects in the specified version series, sorted by cmis:creationDate descending
      /// </summary>
      /// <returns></returns>
      /// <remarks>If a Private Working Copy exists for the version series and the caller has permissions to access it,
      /// then it MUST be returned as the first object in the result list</remarks>
        public CmisDocument[] GetAllVersions(string filter = null, bool? includeAllowableActions = default)
        {
            {
                var withBlock = _client.GetAllVersions(new cmr.getAllVersions()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = _cmisObject.ObjectId,
                    Filter = filter,
                    IncludeAllowableActions = includeAllowableActions
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return (from @object in withBlock.Response.Objects
                            let cmisObject = CreateCmisObject(@object)
                            where cmisObject is CmisDocument
                            select ((CmisDocument)cmisObject)).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument GetObjectOfLatestVersion(bool major = false, string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, enumCheckedOutState acceptPWC = enumCheckedOutState.notCheckedOut)
        {
            var versionSeriesId = _cmisObject.VersionSeriesId;
            // returns the private working copy if exists and is accepted, otherwise returns ObjectOfLatestVersion
            return GetObjectOfLatestVersion((string)(versionSeriesId.HasValue ? versionSeriesId : _cmisObject.ObjectId), major, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, acceptPWC);
        }

        /// <summary>
      /// Get the private working copy if the current document is checked out by current user (acceptPWC=enumCheckedOutState.checkedOutByMe)
      /// or by any user (acceptPWC=enumCheckedOutState.checkedOut)
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new CmisDocument GetPrivateWorkingCopy(string filter = null, Core.enumIncludeRelationships? includeRelationships = default, bool? includePolicyIds = default, string renditionFilter = null, bool? includeACL = default, bool? includeAllowableActions = default, enumCheckedOutState acceptPWC = enumCheckedOutState.checkedOutByMe)
        {
            return GetPrivateWorkingCopy(_cmisObject.ObjectId, filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions, acceptPWC);
        }

        /// <summary>
      /// Get a subset of the properties for the latest document object in the version series
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.Collections.cmisPropertiesType GetPropertiesOfLatestVersion(bool major = false, string filter = null, enumCheckedOutState acceptPWC = enumCheckedOutState.notCheckedOut)
        {
            // check for private working copy
            if (acceptPWC != enumCheckedOutState.notCheckedOut)
            {
                var pwc = GetPrivateWorkingCopy(_cmisObject.ObjectId, filter, acceptPWC: acceptPWC);
                if (pwc is not null)
                    return pwc.Object.Properties;
            }

            // default: PropertiesOfLatestVersion
            var versionSeriesId = _cmisObject.VersionSeriesId;
            {
                var withBlock = _client.GetPropertiesOfLatestVersion(new cmr.getPropertiesOfLatestVersion()
                {
                    RepositoryId = _repositoryInfo.RepositoryId,
                    ObjectId = (string)(versionSeriesId.HasValue ? versionSeriesId : _cmisObject.ObjectId),
                    Major = major,
                    Filter = filter
                });
                _lastException = withBlock.Exception;
                if (_lastException is null)
                {
                    return withBlock.Response.Properties;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region AllowableActions
        public bool CanCheckIn
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
                        var withBlock = _cmisObject.AllowableActions.CanCheckIn;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanCheckIn = value;
            }
        }

        public bool CanCheckOut
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
                        var withBlock = _cmisObject.AllowableActions.CanCheckOut;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanCheckOut = value;
            }
        }

        public bool CanCancelCheckOut
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
                        var withBlock = _cmisObject.AllowableActions.CanCancelCheckOut;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanCancelCheckOut = value;
            }
        }

        public bool CanGetContentStream
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
                        var withBlock = _cmisObject.AllowableActions.CanGetContentStream;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanGetContentStream = value;
            }
        }

        public bool CanSetContentStream
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
                        var withBlock = _cmisObject.AllowableActions.CanSetContentStream;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanSetContentStream = value;
            }
        }

        public bool CanGetAllVersions
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
                        var withBlock = _cmisObject.AllowableActions.CanGetAllVersions;
                        return withBlock.HasValue && withBlock.Value;
                    }
                }
            }
            set
            {
                if (_cmisObject.AllowableActions is null)
                    _cmisObject.AllowableActions.CanGetAllVersions = value;
            }
        }
        #endregion

        /// <summary>
      /// Bind to PWCRemovedListeners
      /// </summary>
        internal void AddPWCRemovedListeners()
        {
            _onPWCRemoved.AddPWCRemovedListeners(ref _onPWCRemovedListeners, _client.ServiceDocUri.AbsoluteUri, _repositoryInfo.RepositoryId, _cmisObject.ObjectId);
        }

        internal string CancelCheckOutFallbackId;

        private object[] Combine(Dictionary<string, Core.Properties.cmisProperty>[] propertiesCollections, string propertyName, string[] addIds, string[] removeIds)
        {
            var properties = (from propertyCollection in propertiesCollections
                              where propertyCollection.ContainsKey(propertyName)
                              let cmisProperty = propertyCollection[propertyName]
                              select cmisProperty).ToArray();
            return Combine(properties, addIds, removeIds);
        }
        private object[] Combine(Core.Properties.cmisProperty[] properties, string[] addIds, string[] removeIds)
        {
            var verify = new HashSet<object>();
            var result = new List<object>();

            // mark ids for remove operation
            if (removeIds is not null)
            {
                for (int index = 0, loopTo = removeIds.Length - 1; index <= loopTo; index++)
                    verify.Add(removeIds[index]);
            }
            // combine the values of given properties
            if (properties is not null)
            {
                for (int propertyIndex = 0, loopTo1 = properties.Length - 1; propertyIndex <= loopTo1; propertyIndex++)
                {
                    var cmisProperty = properties[propertyIndex];
                    var values = cmisProperty is null ? null : cmisProperty.Values;

                    if (values is not null)
                    {
                        for (int index = 0, loopTo2 = values.Length - 1; index <= loopTo2; index++)
                        {
                            var value = values[index];

                            if (value is not null && verify.Add(value))
                                result.Add(value);
                        }
                    }
                }
            }
            // add new identifiers
            if (addIds is not null)
            {
                for (int index = 0, loopTo3 = addIds.Length - 1; index <= loopTo3; index++)
                {
                    string addId = addIds[index];
                    if (!string.IsNullOrEmpty(addId) && verify.Add(addId))
                        result.Add(addId);
                }
            }

            return result.ToArray();
        }

        /// <summary>
      /// Returns a tristate logic: notCheckOut, checkedOut and checkedOutByMe
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new enumCheckedOutState GetCheckedOut()
        {
            return GetCheckedOut(_cmisObject, _client);
        }
        /// <summary>
      /// Returns a tristate logic: notCheckOut, checkedOut and checkedOutByMe
      /// </summary>
      /// <param name="cmisObject"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public new enumCheckedOutState GetCheckedOut(Core.cmisObjectType cmisObject)
        {
            return GetCheckedOut(cmisObject, _client);
        }

        private EventBus.WeakListenerCallback _onPWCRemoved;
        /// <summary>
      /// Reverts the checkedout-state after the pwc has been cancelled, checked in or deleted
      /// </summary>
        protected EventBus.enumEventBusListenerResult OnPWCRemoved(EventBus.EventArgs e)
        {
            if (!_onPWCRemovedPaused)
            {
                if (string.Equals(e.EventName, EventBus.BuiltInEventNames.EndCheckIn))
                {
                    var cmisObject = GetObject(e.NewObjectId);
                    if (cmisObject is CmisDocument)
                    {
                        _cmisObject = cmisObject.Object;
                    }
                }
                else if (string.IsNullOrEmpty(CancelCheckOutFallbackId))
                {
                    CmisObject cmisObject = GetObjectOfLatestVersion(acceptPWC: enumCheckedOutState.checkedOutByMe);
                    if (cmisObject is CmisDocument)
                    {
                        _cmisObject = cmisObject.Object;
                    }
                }
                else
                {
                    var cmisObject = GetObject(CancelCheckOutFallbackId);
                    if (cmisObject is CmisDocument)
                    {
                        _cmisObject = cmisObject.Object;
                    }
                }
            }

            CancelCheckOutFallbackId = null;

            // remove the listeners, because the document stored in this object is not checkedout anymore
            _onPWCRemoved.RemovePWCRemovedListeners(ref _onPWCRemovedListeners);

            return EventBus.enumEventBusListenerResult.success;
        }
        private EventBus.WeakListener[] _onPWCRemovedListeners;
        private bool _onPWCRemovedPaused = false;

        public DateTimeOffset? RM_DestructionRetention
        {
            get
            {
                return get_RM_RetentionDate(CmisPredefinedPropertyNames.RM_DestructionDate);
            }
            set
            {
                set_RM_RetentionDate(Core.Definitions.Types.cmisTypeRM_DestructionRetentionDefinitionType.TargetTypeName, CmisPredefinedPropertyNames.RM_DestructionDate, factory => factory.RM_DestructionDate(true, true), value);
            }
        }

        public DateTimeOffset? RM_ExpirationDate
        {
            get
            {
                return get_RM_RetentionDate(CmisPredefinedPropertyNames.RM_ExpirationDate);
            }
            set
            {
                set_RM_RetentionDate(Core.Definitions.Types.cmisTypeRM_ClientMgtRetentionDefinitionType.TargetTypeName, CmisPredefinedPropertyNames.RM_ExpirationDate, factory => factory.RM_ExpirationDate(true), value);
            }
        }

        /// <summary>
      /// Returns the hold-identifiers the current document is protected by
      /// </summary>
        public string[] RM_HoldIds
        {
            get
            {
                var properties = _cmisObject.GetProperties(CmisPredefinedPropertyNames.RM_HoldIds);

                if (properties.Count > 0)
                {
                    var cmisProperty = properties[CmisPredefinedPropertyNames.RM_HoldIds];
                    var values = cmisProperty.Values;

                    if (values is not null)
                    {
                        return (from value in values
                                let holdId = value is null ? null : value.ToString()
                                where !string.IsNullOrEmpty(holdId)
                                select holdId).ToArray();
                    }
                }

                return null;
            }
        }


        public enumRetentionState RM_Preserved
        {
            get
            {
                string filter = string.Join(",", CmisPredefinedPropertyNames.BaseTypeId, CmisPredefinedPropertyNames.ObjectId, CmisPredefinedPropertyNames.ObjectTypeId, CmisPredefinedPropertyNames.RM_ExpirationDate, CmisPredefinedPropertyNames.RM_HoldIds, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                var documentOfLatestVersion = GetObjectOfLatestVersion(filter: filter, acceptPWC: enumCheckedOutState.checkedOutByMe);
                var properties = documentOfLatestVersion._cmisObject.GetProperties(CmisPredefinedPropertyNames.RM_DestructionDate, CmisPredefinedPropertyNames.RM_ExpirationDate, CmisPredefinedPropertyNames.RM_HoldIds, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                var retVal = enumRetentionState.none;

                // ExpirationDate
                if (properties.ContainsKey(CmisPredefinedPropertyNames.RM_ExpirationDate))
                {
                    Core.Properties.cmisPropertyDateTime dateTimeProperty = properties[CmisPredefinedPropertyNames.RM_ExpirationDate] as Core.Properties.cmisPropertyDateTime;
                    if (dateTimeProperty is not null && dateTimeProperty.Value.DateTime > DateTime.Now)
                    {
                        retVal = retVal | enumRetentionState.preservedByExpirationDate;
                    }
                }
                // HoldIds
                if (properties.ContainsKey(CmisPredefinedPropertyNames.RM_HoldIds) && properties[CmisPredefinedPropertyNames.RM_HoldIds].Value is not null)
                {
                    retVal = retVal | enumRetentionState.preservedByClientHoldIds;
                }
                // preserved by repository
                if (properties.ContainsKey(CmisPredefinedPropertyNames.SecondaryObjectTypeIds))
                {
                    var values = properties[CmisPredefinedPropertyNames.SecondaryObjectTypeIds].Values;

                    if (values is not null)
                    {
                        var secondaryObjectTypeIds = new HashSet<object>();

                        foreach (object value in values)
                            secondaryObjectTypeIds.Add(value);
                        if (secondaryObjectTypeIds.Add(Core.Definitions.Types.cmisTypeRM_RepMgtRetentionDefinitionType.TargetTypeName))
                        {
                            // second chance: check for derived types
                            do
                            {
                                try
                                {
                                    var cmisTypes = Generic.ItemContainer<CmisType>.GetAllItems(GetTypeDescendants(Core.Definitions.Types.cmisTypeRM_RepMgtRetentionDefinitionType.TargetTypeName, -1, false));
                                    bool exitTry = false;
                                    for (int index = 0, loopTo = cmisTypes.Count - 1; index <= loopTo; index++)
                                    {
                                        // retention managed by repository detected
                                        if (!secondaryObjectTypeIds.Add(cmisTypes[index].Type.Id))
                                        {
                                            retVal = retVal | enumRetentionState.preservedByRepository;
                                            exitTry = true;
                                            break;
                                        }
                                    }

                                    if (exitTry)
                                    {
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                            while (false);
                        }
                        else
                        {
                            retVal = retVal | enumRetentionState.preservedByRepository;
                        }
                    }
                }

                return retVal;
            }
        }

        private DateTimeOffset? get_RM_RetentionDate(string propertyDefinitionId)
        {
            if ((RetentionCapability & enumRetentionCapability.clientMgt) == enumRetentionCapability.clientMgt)
            {
                var properties = _cmisObject.GetProperties(propertyDefinitionId);
                if (properties.Count > 0)
                {
                    Core.Properties.cmisPropertyDateTime dateTimeProperty = properties[propertyDefinitionId] as Core.Properties.cmisPropertyDateTime;
                    if (dateTimeProperty is not null)
                        return dateTimeProperty.Value;
                }
            }

            return default;
        }
        private void set_RM_RetentionDate(string secondaryObjectTypeId, string propertyDefinitionId, Func<PredefinedPropertyDefinitionFactory, Core.Definitions.Properties.cmisPropertyDefinitionType> factory, DateTimeOffset? value)
        {
            if ((RetentionCapability & enumRetentionCapability.clientMgt) == enumRetentionCapability.clientMgt)
            {
                if (value.HasValue)
                {
                    var retentionDate = value.Value;
                    string filter = string.Join(",", propertyDefinitionId, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                    var documentOfLatestVersion = GetObjectOfLatestVersion(filter: filter, acceptPWC: enumCheckedOutState.checkedOutByMe);
                    var properties = _cmisObject.GetProperties(propertyDefinitionId, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                    var propertiesOfLatestVersion = (documentOfLatestVersion ?? this)._cmisObject.GetProperties(propertyDefinitionId, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                    var propertiesCollections = new Dictionary<string, Core.Properties.cmisProperty>[] { properties, propertiesOfLatestVersion };
                    var secondaryObjectTypeIdsValues = Combine(propertiesCollections, CmisPredefinedPropertyNames.SecondaryObjectTypeIds, new string[] { secondaryObjectTypeId }, null);
                    // update retention-date and secondaryObjectTypeIds
                    {
                        var withBlock = new PredefinedPropertyDefinitionFactory(null);
                        if (properties.ContainsKey(propertyDefinitionId))
                        {
                            properties[propertyDefinitionId].Value = retentionDate;
                        }
                        else if (_cmisObject.Properties is null)
                        {
                            _cmisObject.Properties = new Core.Collections.cmisPropertiesType(factory(withBlock.Self()).CreateProperty(retentionDate));
                        }
                        else
                        {
                            _cmisObject.Properties.Append(factory(withBlock.Self()).CreateProperty(retentionDate));
                        }
                        if (properties.ContainsKey(CmisPredefinedPropertyNames.SecondaryObjectTypeIds))
                        {
                            properties[CmisPredefinedPropertyNames.SecondaryObjectTypeIds].Values = secondaryObjectTypeIdsValues;
                        }
                        else
                        {
                            _cmisObject.Properties.Append(withBlock.SecondaryObjectTypeIds().CreateProperty(secondaryObjectTypeIdsValues));
                        }
                    }
                }
                else if (_cmisObject.Properties is not null)
                {
                    _cmisObject.Properties.RemoveProperty(propertyDefinitionId);
                }
            }
        }

        public DateTimeOffset? RM_StartOfRetention
        {
            get
            {
                return get_RM_RetentionDate(CmisPredefinedPropertyNames.RM_StartOfRetention);
            }
            set
            {
                set_RM_RetentionDate(Core.Definitions.Types.cmisTypeRM_ClientMgtRetentionDefinitionType.TargetTypeName, CmisPredefinedPropertyNames.RM_StartOfRetention, factory => factory.RM_StartOfRetention(true, true), value);
            }
        }

        private T[] ToArray<T>(params T[] result)
        {
            return result;
        }

        /// <summary>
      /// Modifies the current cmis:rm_holdIds property in the following manner:
      /// 1. step: merges the the rm_holdIds of this instance with the values currently found in the repository
      /// 2. step: adds the identifiers given in addHoldIds
      /// 3. step: removes the identifiers given in removeHoldIds
      /// </summary>
      /// <param name="addHoldIds"></param>
      /// <param name="removeHoldIds"></param>
        public void UpdateRM_HoldIds(string[] addHoldIds, string[] removeHoldIds)
        {
            if (HoldCapability)
            {
                string filter = string.Join(",", CmisPredefinedPropertyNames.RM_HoldIds, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                var documentOfLatestVersion = GetObjectOfLatestVersion(filter: filter, acceptPWC: enumCheckedOutState.checkedOutByMe);
                var properties = _cmisObject.GetProperties(CmisPredefinedPropertyNames.RM_HoldIds, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                var propertiesOfLatestVersion = (documentOfLatestVersion ?? this)._cmisObject.GetProperties(CmisPredefinedPropertyNames.RM_HoldIds, CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
                var propertiesCollections = ToArray(properties, propertiesOfLatestVersion);
                var holdIdsValues = Combine(propertiesCollections, CmisPredefinedPropertyNames.RM_HoldIds, addHoldIds, removeHoldIds);
                var secondaryObjectTypeIdsValues = Combine(propertiesCollections, CmisPredefinedPropertyNames.SecondaryObjectTypeIds, ToArray(Core.Definitions.Types.cmisTypeRM_HoldDefinitionType.TargetTypeName), null);
                // update holdIds and secondaryObjectTypeIds
                {
                    var withBlock = new PredefinedPropertyDefinitionFactory(null);
                    if (properties.ContainsKey(CmisPredefinedPropertyNames.RM_HoldIds))
                    {
                        properties[CmisPredefinedPropertyNames.RM_HoldIds].Values = holdIdsValues;
                    }
                    else if (_cmisObject.Properties is null)
                    {
                        _cmisObject.Properties = new Core.Collections.cmisPropertiesType(withBlock.RM_HoldIds().CreateProperty(holdIdsValues));
                    }
                    else
                    {
                        _cmisObject.Properties.Append(withBlock.RM_HoldIds().CreateProperty(holdIdsValues));
                    }
                    if (properties.ContainsKey(CmisPredefinedPropertyNames.SecondaryObjectTypeIds))
                    {
                        properties[CmisPredefinedPropertyNames.SecondaryObjectTypeIds].Values = secondaryObjectTypeIdsValues;
                    }
                    else
                    {
                        _cmisObject.Properties.Append(withBlock.SecondaryObjectTypeIds().CreateProperty(secondaryObjectTypeIdsValues));
                    }
                }
            }
        }

    }
}