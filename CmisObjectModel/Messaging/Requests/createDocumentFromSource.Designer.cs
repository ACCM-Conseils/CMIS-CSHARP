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
   /// see createDocumentFromSource
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("createDocumentFromSource", Namespace = Constants.Namespaces.cmism)]
    public partial class createDocumentFromSource : RequestBase
    {

        public createDocumentFromSource()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected createDocumentFromSource(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<createDocumentFromSource, string>> _setter = new Dictionary<string, Action<createDocumentFromSource, string>>() { }; // _setter

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
            _sourceId = Read(reader, attributeOverrides, "sourceId", Constants.Namespaces.cmism, _sourceId);
            _properties = Read(reader, attributeOverrides, "properties", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Collections.cmisPropertiesType>);
            _folderId = Read(reader, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            _versioningState = ReadOptionalEnum(reader, attributeOverrides, "versioningState", Constants.Namespaces.cmism, _versioningState);
            _policies = ReadArray<string>(reader, attributeOverrides, "policies", Constants.Namespaces.cmism);
            _addACEs = Read(reader, attributeOverrides, "addACEs", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
            _removeACEs = Read(reader, attributeOverrides, "removeACEs", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
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
            WriteElement(writer, attributeOverrides, "sourceId", Constants.Namespaces.cmism, _sourceId);
            WriteElement(writer, attributeOverrides, "properties", Constants.Namespaces.cmism, _properties);
            if (!string.IsNullOrEmpty(_folderId))
                WriteElement(writer, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            if (_versioningState.HasValue)
                WriteElement(writer, attributeOverrides, "versioningState", Constants.Namespaces.cmism, _versioningState.Value.GetName());
            WriteArray(writer, attributeOverrides, "policies", Constants.Namespaces.cmism, _policies);
            WriteElement(writer, attributeOverrides, "addACEs", Constants.Namespaces.cmism, _addACEs);
            WriteElement(writer, attributeOverrides, "removeACEs", Constants.Namespaces.cmism, _removeACEs);
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected Core.Security.cmisAccessControlListType _addACEs;
        public virtual Core.Security.cmisAccessControlListType AddACEs
        {
            get
            {
                return _addACEs;
            }
            set
            {
                if (!ReferenceEquals(value, _addACEs))
                {
                    var oldValue = _addACEs;
                    _addACEs = value;
                    OnPropertyChanged("AddACEs", value, oldValue);
                }
            }
        } // AddACEs

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

        protected string[] _policies;
        public virtual string[] Policies
        {
            get
            {
                return _policies;
            }
            set
            {
                if (!ReferenceEquals(value, _policies))
                {
                    var oldValue = _policies;
                    _policies = value;
                    OnPropertyChanged("Policies", value, oldValue);
                }
            }
        } // Policies

        protected Core.Collections.cmisPropertiesType _properties;
        public virtual Core.Collections.cmisPropertiesType Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (!ReferenceEquals(value, _properties))
                {
                    var oldValue = _properties;
                    _properties = value;
                    OnPropertyChanged("Properties", value, oldValue);
                }
            }
        } // Properties

        protected Core.Security.cmisAccessControlListType _removeACEs;
        public virtual Core.Security.cmisAccessControlListType RemoveACEs
        {
            get
            {
                return _removeACEs;
            }
            set
            {
                if (!ReferenceEquals(value, _removeACEs))
                {
                    var oldValue = _removeACEs;
                    _removeACEs = value;
                    OnPropertyChanged("RemoveACEs", value, oldValue);
                }
            }
        } // RemoveACEs

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

        protected string _sourceId;
        public virtual string SourceId
        {
            get
            {
                return _sourceId;
            }
            set
            {
                if ((_sourceId ?? "") != (value ?? ""))
                {
                    string oldValue = _sourceId;
                    _sourceId = value;
                    OnPropertyChanged("SourceId", value, oldValue);
                }
            }
        } // SourceId

        protected Core.enumVersioningState? _versioningState;
        public virtual Core.enumVersioningState? VersioningState
        {
            get
            {
                return _versioningState;
            }
            set
            {
                if (!_versioningState.Equals(value))
                {
                    var oldValue = _versioningState;
                    _versioningState = value;
                    OnPropertyChanged("VersioningState", value, oldValue);
                }
            }
        } // VersioningState

    }
}