using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// * Author: auto-generated code
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Requests
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see getChildren
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("getChildren", Namespace = Constants.Namespaces.cmism)]
    public partial class getChildren : RequestBase
    {

        public getChildren()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getChildren(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getChildren, string>> _setter = new Dictionary<string, Action<getChildren, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in attributes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadAttributes(sx.XmlReader reader)
        {
            // at least one property is serialized in an attribute-value
            if (_setter.Count > 0)
            {
                for (int attributeIndex = 0, loopTo = reader.AttributeCount - 1; attributeIndex <= loopTo; attributeIndex++)
                {
                    reader.MoveToAttribute(attributeIndex);
                    // attribute name
                    string key = reader.Name.ToLowerInvariant();
                    if (_setter.ContainsKey(key))
                        _setter[key].Invoke(this, reader.GetAttribute(attributeIndex));
                }
            }
        }

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _repositoryId = Read(reader, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            _folderId = Read(reader, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            _filter = Read(reader, attributeOverrides, "filter", Constants.Namespaces.cmism, _filter);
            _orderBy = Read(reader, attributeOverrides, "orderBy", Constants.Namespaces.cmism, _orderBy);
            _includeAllowableActions = Read(reader, attributeOverrides, "includeAllowableActions", Constants.Namespaces.cmism, _includeAllowableActions);
            _includeRelationships = ReadOptionalEnum(reader, attributeOverrides, "includeRelationships", Constants.Namespaces.cmism, _includeRelationships);
            _renditionFilter = Read(reader, attributeOverrides, "renditionFilter", Constants.Namespaces.cmism, _renditionFilter);
            _includePathSegment = Read(reader, attributeOverrides, "includePathSegment", Constants.Namespaces.cmism, _includePathSegment);
            _maxItems = Read(reader, attributeOverrides, "maxItems", Constants.Namespaces.cmism, _maxItems);
            _skipCount = Read(reader, attributeOverrides, "skipCount", Constants.Namespaces.cmism, _skipCount);
            _extension = Read(reader, attributeOverrides, "extension", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisExtensionType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            WriteElement(writer, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            if (!string.IsNullOrEmpty(_filter))
                WriteElement(writer, attributeOverrides, "filter", Constants.Namespaces.cmism, _filter);
            if (!string.IsNullOrEmpty(_orderBy))
                WriteElement(writer, attributeOverrides, "orderBy", Constants.Namespaces.cmism, _orderBy);
            if (_includeAllowableActions.HasValue)
                WriteElement(writer, attributeOverrides, "includeAllowableActions", Constants.Namespaces.cmism, CommonFunctions.Convert(_includeAllowableActions));
            if (_includeRelationships.HasValue)
                WriteElement(writer, attributeOverrides, "includeRelationships", Constants.Namespaces.cmism, _includeRelationships.Value.GetName());
            if (!string.IsNullOrEmpty(_renditionFilter))
                WriteElement(writer, attributeOverrides, "renditionFilter", Constants.Namespaces.cmism, _renditionFilter);
            if (_includePathSegment.HasValue)
                WriteElement(writer, attributeOverrides, "includePathSegment", Constants.Namespaces.cmism, CommonFunctions.Convert(_includePathSegment));
            if (_maxItems.HasValue)
                WriteElement(writer, attributeOverrides, "maxItems", Constants.Namespaces.cmism, CommonFunctions.Convert(_maxItems));
            if (_skipCount.HasValue)
                WriteElement(writer, attributeOverrides, "skipCount", Constants.Namespaces.cmism, CommonFunctions.Convert(_skipCount));
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected cmisExtensionType _extension;
        public virtual cmisExtensionType Extension
        {
            get
            {
                return _extension;
            }
            set
            {
                if (!ReferenceEquals(value, _extension))
                {
                    var oldValue = _extension;
                    _extension = value;
                    OnPropertyChanged("Extension", value, oldValue);
                }
            }
        } // Extension

        protected string _filter;
        public virtual string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if ((_filter ?? "") != (value ?? ""))
                {
                    string oldValue = _filter;
                    _filter = value;
                    OnPropertyChanged("Filter", value, oldValue);
                }
            }
        } // Filter

        protected string _folderId;
        public virtual string FolderId
        {
            get
            {
                return _folderId;
            }
            set
            {
                if ((_folderId ?? "") != (value ?? ""))
                {
                    string oldValue = _folderId;
                    _folderId = value;
                    OnPropertyChanged("FolderId", value, oldValue);
                }
            }
        } // FolderId

        protected bool? _includeAllowableActions;
        public virtual bool? IncludeAllowableActions
        {
            get
            {
                return _includeAllowableActions;
            }
            set
            {
                if (!_includeAllowableActions.Equals(value))
                {
                    var oldValue = _includeAllowableActions;
                    _includeAllowableActions = value;
                    OnPropertyChanged("IncludeAllowableActions", value, oldValue);
                }
            }
        } // IncludeAllowableActions

        protected bool? _includePathSegment;
        public virtual bool? IncludePathSegment
        {
            get
            {
                return _includePathSegment;
            }
            set
            {
                if (!_includePathSegment.Equals(value))
                {
                    var oldValue = _includePathSegment;
                    _includePathSegment = value;
                    OnPropertyChanged("IncludePathSegment", value, oldValue);
                }
            }
        } // IncludePathSegment

        protected Core.enumIncludeRelationships? _includeRelationships;
        public virtual Core.enumIncludeRelationships? IncludeRelationships
        {
            get
            {
                return _includeRelationships;
            }
            set
            {
                if (!_includeRelationships.Equals(value))
                {
                    var oldValue = _includeRelationships;
                    _includeRelationships = value;
                    OnPropertyChanged("IncludeRelationships", value, oldValue);
                }
            }
        } // IncludeRelationships

        protected long? _maxItems;
        public virtual long? MaxItems
        {
            get
            {
                return _maxItems;
            }
            set
            {
                if (!_maxItems.Equals(value))
                {
                    var oldValue = _maxItems;
                    _maxItems = value;
                    OnPropertyChanged("MaxItems", value, oldValue);
                }
            }
        } // MaxItems

        protected string _orderBy;
        public virtual string OrderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                if ((_orderBy ?? "") != (value ?? ""))
                {
                    string oldValue = _orderBy;
                    _orderBy = value;
                    OnPropertyChanged("OrderBy", value, oldValue);
                }
            }
        } // OrderBy

        protected string _renditionFilter;
        public virtual string RenditionFilter
        {
            get
            {
                return _renditionFilter;
            }
            set
            {
                if ((_renditionFilter ?? "") != (value ?? ""))
                {
                    string oldValue = _renditionFilter;
                    _renditionFilter = value;
                    OnPropertyChanged("RenditionFilter", value, oldValue);
                }
            }
        } // RenditionFilter

        protected string _repositoryId;
        public virtual string RepositoryId
        {
            get
            {
                return _repositoryId;
            }
            set
            {
                if ((_repositoryId ?? "") != (value ?? ""))
                {
                    string oldValue = _repositoryId;
                    _repositoryId = value;
                    OnPropertyChanged("RepositoryId", value, oldValue);
                }
            }
        } // RepositoryId

        protected long? _skipCount;
        public virtual long? SkipCount
        {
            get
            {
                return _skipCount;
            }
            set
            {
                if (!_skipCount.Equals(value))
                {
                    var oldValue = _skipCount;
                    _skipCount = value;
                    OnPropertyChanged("SkipCount", value, oldValue);
                }
            }
        } // SkipCount

    }
}