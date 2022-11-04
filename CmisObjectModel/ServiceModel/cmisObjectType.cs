using System;
using System.Data;
using System.Linq;
using sss = System.ServiceModel.Syndication;
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
using CmisObjectModel.Core;

namespace CmisObjectModel.ServiceModel
{
    public class cmisObjectType : Core.cmisObjectType, Contracts.IServiceModelObject
    {

        [Obsolete("Constructor defined for serialization usage only", true)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public cmisObjectType() : base()
        {
            _serviceModel = new ServiceModelExtension(this);
        }
        public cmisObjectType(ServiceModelExtension serviceModel, params Core.Properties.cmisProperty[] properties) : base(properties)
        {
            _serviceModel = serviceModel ?? new ServiceModelExtension(this);
        }

        #region Helper classes
        /// <summary>
      /// Summary of object properties in a bulkUpdateProperties-call
      /// </summary>
      /// <remarks></remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public class BulkUpdatePropertiesExtension
        {

            internal BulkUpdatePropertiesExtension(cmisObjectType owner)
            {
                _owner = owner;
            }

            /// <summary>
         /// NewId if the bulkUpdateProperties-call modified the objectId
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public string NewId
            {
                get
                {
                    var newIdExtension = _owner.FindExtension<Extensions.Common.NewIdExtension>();
                    return newIdExtension is null ? null : newIdExtension.NewId;
                }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var newIdExtension = _owner.FindExtension<Extensions.Common.NewIdExtension>();

                        if (newIdExtension is null)
                        {
                            Extensions.Extension[] currentExtensions;
                            int length;

                            newIdExtension = new Extensions.Common.NewIdExtension() { NewId = value };
                            if (_owner._properties is null)
                                _owner._properties = new Core.Collections.cmisPropertiesType();
                            currentExtensions = _owner._properties.Extensions;
                            length = currentExtensions is null ? 0 : currentExtensions.Length;
                            if (length == 0)
                            {
                                _owner._properties.Extensions = new Extensions.Extension[] { newIdExtension };
                            }
                            else
                            {
                                Extensions.Extension[] newExtensions = (Extensions.Extension[])Array.CreateInstance(typeof(Extensions.Extension), length + 1);

                                Array.Copy(currentExtensions, newExtensions, length);
                                newExtensions[length] = newIdExtension;
                                _owner._properties.Extensions = newExtensions;
                            }
                        }
                        else
                        {
                            newIdExtension.NewId = value;
                        }
                    }
                    else if (_owner._properties is not null && _owner._properties.Extensions is not null)
                    {
                        // delete NewIdExtension if existing
                        var extensions = (from extension in _owner._properties.Extensions
                                          where !(extension is Extensions.Common.NewIdExtension)
                                          select extension).ToArray();
                        _owner._properties.Extensions = extensions.Length == 0 ? null : extensions;
                    }
                }
            }

            private cmisObjectType _owner;

        }

        /// <summary>
      /// Summary of the objects properties to allow the system to create AtomPub-Links
      /// </summary>
      /// <remarks></remarks>
        public class ServiceModelExtension
        {

            protected internal ServiceModelExtension(cmisObjectType owner)
            {
                _owner = owner ?? _emptyOwner;
            }
            public ServiceModelExtension(string objectId, enumBaseObjectTypeIds baseObjectType, string summary, string parentId, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, bool isLatestVersion, string versionSeriesId, string versionSeriesCheckedOutId, ca.AtomLink contentLink, params sss.SyndicationPerson[] authors)
            {
                _authors = authors;
                _lastUpdatedTime = lastUpdatedTime;
                _publishDate = publishDate;
                _baseObjectType = baseObjectType;
                ContentLink = contentLink;
                _isLatestVersion = isLatestVersion;
                _objectId = objectId;
                _parentId = parentId;
                _summary = summary;
                _versionSeriesId = versionSeriesId;
                _versionSeriesCheckedOutId = versionSeriesCheckedOutId;
                // ensure a valid instance
                _owner = _emptyOwner;
            }
            public ServiceModelExtension(string objectId, enumBaseObjectTypeIds baseObjectType, string summary, string parentId, DateTime publishDate, DateTime lastUpdatedTime, bool isLatestVersion, string versionSeriesId, string versionSeriesCheckedOutId, ca.AtomLink contentLink, params sss.SyndicationPerson[] authors) : this(objectId, baseObjectType, summary, parentId, ToDateTimeOffset(publishDate), ToDateTimeOffset(lastUpdatedTime), isLatestVersion, versionSeriesId, versionSeriesCheckedOutId, contentLink, authors)
            {
            }
            /// <summary>
         /// Relationship-Overload
         /// </summary>
         /// <remarks></remarks>
            public ServiceModelExtension(string objectId, string sourceId, string targetId, DateTimeOffset publishDate, DateTimeOffset lastUpdatedTime, bool isLatestVersion, string versionSeriesId, string versionSeriesCheckedOutId, params sss.SyndicationPerson[] authors) : this(objectId, enumBaseObjectTypeIds.cmisRelationship, null, null, publishDate, lastUpdatedTime, isLatestVersion, versionSeriesId, versionSeriesCheckedOutId, null, authors)
            {
                SourceId = sourceId;
                TargetId = targetId;
            }
            public ServiceModelExtension(string objectId, string sourceId, string targetId, DateTime publishDate, DateTime lastUpdatedTime, bool isLatestVersion, string versionSeriesId, string versionSeriesCheckedOutId, params sss.SyndicationPerson[] authors) : this(objectId, sourceId, targetId, ToDateTimeOffset(publishDate), ToDateTimeOffset(lastUpdatedTime), isLatestVersion, versionSeriesId, versionSeriesCheckedOutId, authors)
            {
            }

            /// <summary>
         /// Support for systems which provide only one author per cmisObject
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public sss.SyndicationPerson Author
            {
                get
                {
                    return _authors is null || _authors.Length == 0 ? null : _authors[0];
                }
                set
                {
                    if (value is null)
                    {
                        _authors = null;
                    }
                    else
                    {
                        _authors = new sss.SyndicationPerson[] { value };
                    }
                }
            }
            private sss.SyndicationPerson[] _authors;
            public sss.SyndicationPerson[] Authors
            {
                get
                {
                    return _authors;
                }
                set
                {
                    _authors = value;
                }
            }

            private enumBaseObjectTypeIds? _baseObjectType;
            public enumBaseObjectTypeIds BaseObjectType
            {
                get
                {
                    if (_baseObjectType.HasValue)
                    {
                        return _baseObjectType.Value;
                    }
                    else
                    {
                        string baseTypeId = _owner.BaseTypeId;
                        var retVal = enumBaseObjectTypeIds.cmisDocument;

                        if (!string.IsNullOrEmpty(baseTypeId))
                        {
                            baseTypeId.TryParse(ref retVal, true);
                        }

                        return retVal;
                    }
                }
                set
                {
                    _baseObjectType = value;
                }
            }

            /// <summary>
         /// Reset BaseObjectType
         /// </summary>
            public void ClearBaseObjectType()
            {
                _baseObjectType = default;
            }
            /// <summary>
         /// Reset IsLatestVersion
         /// </summary>
            public void ClearIsLatestVersion()
            {
                _isLatestVersion = default;
            }
            /// <summary>
         /// Reset LastUpdatedTime
         /// </summary>
            public void ClearLastUpdatedTime()
            {
                _lastUpdatedTime = default;
            }
            /// <summary>
         /// Reset ObjectId
         /// </summary>
            public void ClearObjectId()
            {
                _objectId = default;
            }
            /// <summary>
         /// Reset ParentId
         /// </summary>
            public void ClearParentId()
            {
                _parentId = default;
            }
            /// <summary>
         /// Reset PublishDate
         /// </summary>
            public void ClearPublishDate()
            {
                _publishDate = default;
            }
            /// <summary>
         /// Reset SourceId
         /// </summary>
            public void ClearSourceId()
            {
                _sourceId = default;
            }
            /// <summary>
         /// Reset Summary
         /// </summary>
            public void ClearSummary()
            {
                _summary = default;
            }
            /// <summary>
         /// Reset TargetId
         /// </summary>
            public void ClearTargetId()
            {
                _targetId = default;
            }
            /// <summary>
         /// Reset VersionSeriesCheckedOutId
         /// </summary>
            public void ClearVersionSeriesCheckedOutId()
            {
                _versionSeriesCheckedOutId = default;
            }
            /// <summary>
         /// Reset VersionSeriesId
         /// </summary>
            public void ClearVersionSeriesId()
            {
                _versionSeriesId = default;
            }

            public ca.AtomLink ContentLink { get; set; }
            private static cmisObjectType _emptyOwner = new cmisObjectType(null);

            private bool? _isLatestVersion;
            public bool IsLatestVersion
            {
                get
                {
                    return _isLatestVersion.HasValue ? _isLatestVersion.Value : _owner.IsLatestVersion.HasValue ? _owner.IsLatestVersion.Value : false;
                }
                set
                {
                    _isLatestVersion = value;
                }
            }

            private DateTimeOffset? _lastUpdatedTime;
            public DateTimeOffset LastUpdatedTime
            {
                get
                {
                    var retVal = _lastUpdatedTime.HasValue ? _lastUpdatedTime.Value : _owner.LastModificationDate.HasValue ? _owner.LastModificationDate.Value : DateTimeOffset.UtcNow;
                    return retVal.CompareTo(DateTimeOffset.MinValue) == 0 ? DateTimeOffset.UtcNow : retVal;
                }
                set
                {
                    _lastUpdatedTime = value;
                }
            }

            private Common.Generic.Nullable<string> _objectId;
            public string ObjectId
            {
                get
                {
                    return _objectId.HasValue ? _objectId.Value : _owner.ObjectId.Value;
                }
                set
                {
                    _objectId = value;
                }
            }

            private cmisObjectType _owner;

            private Common.Generic.Nullable<string> _parentId;
            public string ParentId
            {
                get
                {
                    return _parentId.HasValue ? _parentId.Value : _owner.ParentId.Value;
                }
                set
                {
                    _parentId = value;
                }
            }

            private DateTimeOffset? _publishDate;
            public DateTimeOffset PublishDate
            {
                get
                {
                    var retVal = _publishDate.HasValue ? _publishDate.Value : _owner.CreationDate.HasValue ? _owner.CreationDate.Value : DateTimeOffset.UtcNow;
                    return retVal.CompareTo(DateTimeOffset.MinValue) == 0 ? DateTimeOffset.UtcNow : retVal;
                }
                set
                {
                    _publishDate = value;
                }
            }

            private Common.Generic.Nullable<string> _sourceId;
            public string SourceId
            {
                get
                {
                    return _sourceId.HasValue ? _sourceId.Value : _owner.SourceId.Value;
                }
                set
                {
                    _sourceId = value;
                }
            }

            private Common.Generic.Nullable<string> _summary;
            public string Summary
            {
                get
                {
                    return _summary.HasValue ? _summary.Value : _owner.Description.Value;
                }
                set
                {
                    _summary = value;
                }
            }

            private Common.Generic.Nullable<string> _targetId;
            public string TargetId
            {
                get
                {
                    return _targetId.HasValue ? _targetId.Value : _owner.TargetId.Value;
                }
                set
                {
                    _targetId = value;
                }
            }

            private Common.Generic.Nullable<string> _versionSeriesCheckedOutId;
            public string VersionSeriesCheckedOutId
            {
                get
                {
                    return _versionSeriesCheckedOutId.HasValue ? _versionSeriesCheckedOutId.Value : _owner.VersionSeriesCheckedOutId.Value;
                }
                set
                {
                    _versionSeriesCheckedOutId = value;
                }
            }

            private Common.Generic.Nullable<string> _versionSeriesId;
            public string VersionSeriesId
            {
                get
                {
                    return _versionSeriesId.HasValue ? _versionSeriesId.Value : _owner.VersionSeriesId.Value;
                }
                set
                {
                    _versionSeriesId = value;
                }
            }

            /// <summary>
         /// Converts a date to a DateTimeOffset respecting the MinValue-restriction of DateTimeOffset
         /// (lower and upper bounds given in utc; avoid ArgumentOutOfRangeException)
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            private static DateTimeOffset ToDateTimeOffset(DateTime value)
            {
                try
                {
                    if (value.Kind == DateTimeKind.Utc)
                    {
                        return value;
                    }
                    else if (value <= DateTimeOffset.MinValue.Date)
                    {
                        return DateTimeOffset.MinValue;
                    }
                    else if (value >= DateTimeOffset.MaxValue.Date)
                    {
                        return DateTimeOffset.MaxValue;
                    }
                    else
                    {
                        return value;
                    }
                }
                catch (Exception ex)
                {
                    return value < DateTime.Now ? DateTimeOffset.MinValue : DateTimeOffset.MaxValue;
                }
            }

        }
        #endregion

        #region IServiceModelObject
        public cmisObjectType Object
        {
            get
            {
                return this;
            }
        }

        public string PathSegment
        {
            get
            {
                return null;
            }
        }

        public string RelativePathSegment
        {
            get
            {
                return null;
            }
        }

        private readonly ServiceModelExtension _serviceModel;
        public ServiceModelExtension ServiceModel
        {
            get
            {
                return _serviceModel;
            }
        }
        //A remplacer
        //private BulkUpdatePropertiesExtension _BulkUpdateProperties_retVal = new BulkUpdatePropertiesExtension(this);
        private BulkUpdatePropertiesExtension _BulkUpdateProperties_retVal = null;
        #endregion

        /// <summary>
        /// Extension when cmisObjectType is used in BulkUpdateProperties call
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public BulkUpdatePropertiesExtension BulkUpdateProperties
        {
            get
            {
                return _BulkUpdateProperties_retVal;
            }
        }

    }
}