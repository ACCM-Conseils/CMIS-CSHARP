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
   /// see query
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("query", Namespace = Constants.Namespaces.cmism)]
    public partial class query : RequestBase
    {

        public query()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected query(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<query, string>> _setter = new Dictionary<string, Action<query, string>>() { }; // _setter

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
            _statement = Read(reader, attributeOverrides, "statement", Constants.Namespaces.cmism, _statement);
            _searchAllVersions = Read(reader, attributeOverrides, "searchAllVersions", Constants.Namespaces.cmism, _searchAllVersions);
            _includeAllowableActions = Read(reader, attributeOverrides, "includeAllowableActions", Constants.Namespaces.cmism, _includeAllowableActions);
            _includeRelationships = ReadOptionalEnum(reader, attributeOverrides, "includeRelationships", Constants.Namespaces.cmism, _includeRelationships);
            _renditionFilter = Read(reader, attributeOverrides, "renditionFilter", Constants.Namespaces.cmism, _renditionFilter);
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
            WriteElement(writer, attributeOverrides, "statement", Constants.Namespaces.cmism, _statement);
            if (_searchAllVersions.HasValue)
                WriteElement(writer, attributeOverrides, "searchAllVersions", Constants.Namespaces.cmism, CommonFunctions.Convert(_searchAllVersions));
            if (_includeAllowableActions.HasValue)
                WriteElement(writer, attributeOverrides, "includeAllowableActions", Constants.Namespaces.cmism, CommonFunctions.Convert(_includeAllowableActions));
            if (_includeRelationships.HasValue)
                WriteElement(writer, attributeOverrides, "includeRelationships", Constants.Namespaces.cmism, _includeRelationships.Value.GetName());
            if (!string.IsNullOrEmpty(_renditionFilter))
                WriteElement(writer, attributeOverrides, "renditionFilter", Constants.Namespaces.cmism, _renditionFilter);
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

        protected bool? _searchAllVersions;
        public virtual bool? SearchAllVersions
        {
            get
            {
                return _searchAllVersions;
            }
            set
            {
                if (!_searchAllVersions.Equals(value))
                {
                    var oldValue = _searchAllVersions;
                    _searchAllVersions = value;
                    OnPropertyChanged("SearchAllVersions", value, oldValue);
                }
            }
        } // SearchAllVersions

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

        protected string _statement;
        public virtual string Statement
        {
            get
            {
                return _statement;
            }
            set
            {
                if ((_statement ?? "") != (value ?? ""))
                {
                    string oldValue = _statement;
                    _statement = value;
                    OnPropertyChanged("Statement", value, oldValue);
                }
            }
        } // Statement

    }
}