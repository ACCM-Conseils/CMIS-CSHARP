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
   /// see getContentChanges
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("getContentChanges", Namespace = Constants.Namespaces.cmism)]
    public partial class getContentChanges : RequestBase
    {

        public getContentChanges()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getContentChanges(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getContentChanges, string>> _setter = new Dictionary<string, Action<getContentChanges, string>>() { }; // _setter

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
            _changeLogToken = Read(reader, attributeOverrides, "changeLogToken", Constants.Namespaces.cmism, _changeLogToken);
            _includeProperties = Read(reader, attributeOverrides, "includeProperties", Constants.Namespaces.cmism, _includeProperties);
            _filter = Read(reader, attributeOverrides, "filter", Constants.Namespaces.cmism, _filter);
            _includePolicyIds = Read(reader, attributeOverrides, "includePolicyIds", Constants.Namespaces.cmism, _includePolicyIds);
            _includeACL = Read(reader, attributeOverrides, "includeACL", Constants.Namespaces.cmism, _includeACL);
            _maxItems = Read(reader, attributeOverrides, "maxItems", Constants.Namespaces.cmism, _maxItems);
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
            if (!string.IsNullOrEmpty(_changeLogToken))
                WriteElement(writer, attributeOverrides, "changeLogToken", Constants.Namespaces.cmism, _changeLogToken);
            if (_includeProperties.HasValue)
                WriteElement(writer, attributeOverrides, "includeProperties", Constants.Namespaces.cmism, CommonFunctions.Convert(_includeProperties));
            if (!string.IsNullOrEmpty(_filter))
                WriteElement(writer, attributeOverrides, "filter", Constants.Namespaces.cmism, _filter);
            if (_includePolicyIds.HasValue)
                WriteElement(writer, attributeOverrides, "includePolicyIds", Constants.Namespaces.cmism, CommonFunctions.Convert(_includePolicyIds));
            if (_includeACL.HasValue)
                WriteElement(writer, attributeOverrides, "includeACL", Constants.Namespaces.cmism, CommonFunctions.Convert(_includeACL));
            if (_maxItems.HasValue)
                WriteElement(writer, attributeOverrides, "maxItems", Constants.Namespaces.cmism, CommonFunctions.Convert(_maxItems));
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected string _changeLogToken;
        public virtual string ChangeLogToken
        {
            get
            {
                return _changeLogToken;
            }
            set
            {
                if ((_changeLogToken ?? "") != (value ?? ""))
                {
                    string oldValue = _changeLogToken;
                    _changeLogToken = value;
                    OnPropertyChanged("ChangeLogToken", value, oldValue);
                }
            }
        } // ChangeLogToken

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

        protected bool? _includeACL;
        public virtual bool? IncludeACL
        {
            get
            {
                return _includeACL;
            }
            set
            {
                if (!_includeACL.Equals(value))
                {
                    var oldValue = _includeACL;
                    _includeACL = value;
                    OnPropertyChanged("IncludeACL", value, oldValue);
                }
            }
        } // IncludeACL

        protected bool? _includePolicyIds;
        public virtual bool? IncludePolicyIds
        {
            get
            {
                return _includePolicyIds;
            }
            set
            {
                if (!_includePolicyIds.Equals(value))
                {
                    var oldValue = _includePolicyIds;
                    _includePolicyIds = value;
                    OnPropertyChanged("IncludePolicyIds", value, oldValue);
                }
            }
        } // IncludePolicyIds

        protected bool? _includeProperties;
        public virtual bool? IncludeProperties
        {
            get
            {
                return _includeProperties;
            }
            set
            {
                if (!_includeProperties.Equals(value))
                {
                    var oldValue = _includeProperties;
                    _includeProperties = value;
                    OnPropertyChanged("IncludeProperties", value, oldValue);
                }
            }
        } // IncludeProperties

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

    }
}