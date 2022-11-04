using System;
using System.Collections.Generic;
using sx = System.Xml;
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
namespace CmisObjectModel.Core
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisRepositoryInfoType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisRepositoryInfoType : Serialization.XmlSerializable
    {

        public cmisRepositoryInfoType() : base(true)
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisRepositoryInfoType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisRepositoryInfoType, string>> _setter = new Dictionary<string, Action<cmisRepositoryInfoType, string>>() { }; // _setter

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
            _repositoryId = Read(reader, attributeOverrides, "repositoryId", Constants.Namespaces.cmis, _repositoryId);
            _repositoryName = Read(reader, attributeOverrides, "repositoryName", Constants.Namespaces.cmis, _repositoryName);
            _repositoryDescription = Read(reader, attributeOverrides, "repositoryDescription", Constants.Namespaces.cmis, _repositoryDescription);
            _vendorName = Read(reader, attributeOverrides, "vendorName", Constants.Namespaces.cmis, _vendorName);
            _productName = Read(reader, attributeOverrides, "productName", Constants.Namespaces.cmis, _productName);
            _productVersion = Read(reader, attributeOverrides, "productVersion", Constants.Namespaces.cmis, _productVersion);
            _rootFolderId = Read(reader, attributeOverrides, "rootFolderId", Constants.Namespaces.cmis, _rootFolderId);
            _latestChangeLogToken = Read(reader, attributeOverrides, "latestChangeLogToken", Constants.Namespaces.cmis, _latestChangeLogToken);
            _capabilities = Read(reader, attributeOverrides, "capabilities", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisRepositoryCapabilitiesType>);
            _aclCapability = Read(reader, attributeOverrides, "aclCapability", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Security.cmisACLCapabilityType>);
            _cmisVersionSupported = Read(reader, attributeOverrides, "cmisVersionSupported", Constants.Namespaces.cmis, _cmisVersionSupported);
            _thinClientURI = Read(reader, attributeOverrides, "thinClientURI", Constants.Namespaces.cmis, _thinClientURI);
            _changesIncomplete = Read(reader, attributeOverrides, "changesIncomplete", Constants.Namespaces.cmis, _changesIncomplete);
            _changesOnTypes = ReadEnumArray<enumBaseObjectTypeIds>(reader, attributeOverrides, "changesOnType", Constants.Namespaces.cmis);
            _principalAnonymous = Read(reader, attributeOverrides, "principalAnonymous", Constants.Namespaces.cmis, _principalAnonymous);
            _principalAnyone = Read(reader, attributeOverrides, "principalAnyone", Constants.Namespaces.cmis, _principalAnyone);
            _extendedFeatures = ReadArray(reader, attributeOverrides, "extendedFeatures", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisExtensionFeatureType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "repositoryId", Constants.Namespaces.cmis, _repositoryId);
            WriteElement(writer, attributeOverrides, "repositoryName", Constants.Namespaces.cmis, _repositoryName);
            WriteElement(writer, attributeOverrides, "repositoryDescription", Constants.Namespaces.cmis, _repositoryDescription);
            WriteElement(writer, attributeOverrides, "vendorName", Constants.Namespaces.cmis, _vendorName);
            WriteElement(writer, attributeOverrides, "productName", Constants.Namespaces.cmis, _productName);
            WriteElement(writer, attributeOverrides, "productVersion", Constants.Namespaces.cmis, _productVersion);
            WriteElement(writer, attributeOverrides, "rootFolderId", Constants.Namespaces.cmis, _rootFolderId);
            if (!string.IsNullOrEmpty(_latestChangeLogToken))
                WriteElement(writer, attributeOverrides, "latestChangeLogToken", Constants.Namespaces.cmis, _latestChangeLogToken);
            WriteElement(writer, attributeOverrides, "capabilities", Constants.Namespaces.cmis, _capabilities);
            WriteElement(writer, attributeOverrides, "aclCapability", Constants.Namespaces.cmis, _aclCapability);
            WriteElement(writer, attributeOverrides, "cmisVersionSupported", Constants.Namespaces.cmis, _cmisVersionSupported);
            if (!string.IsNullOrEmpty(_thinClientURI))
                WriteElement(writer, attributeOverrides, "thinClientURI", Constants.Namespaces.cmis, _thinClientURI);
            if (_changesIncomplete.HasValue)
                WriteElement(writer, attributeOverrides, "changesIncomplete", Constants.Namespaces.cmis, CommonFunctions.Convert(_changesIncomplete));
            WriteArray(writer, attributeOverrides, "changesOnType", Constants.Namespaces.cmis, _changesOnTypes);
            if (!string.IsNullOrEmpty(_principalAnonymous))
                WriteElement(writer, attributeOverrides, "principalAnonymous", Constants.Namespaces.cmis, _principalAnonymous);
            if (!string.IsNullOrEmpty(_principalAnyone))
                WriteElement(writer, attributeOverrides, "principalAnyone", Constants.Namespaces.cmis, _principalAnyone);
            WriteArray(writer, attributeOverrides, "extendedFeatures", Constants.Namespaces.cmis, _extendedFeatures);
        }
        #endregion

        protected Security.cmisACLCapabilityType _aclCapability;
        public virtual Security.cmisACLCapabilityType AclCapability
        {
            get
            {
                return _aclCapability;
            }
            set
            {
                if (!ReferenceEquals(value, _aclCapability))
                {
                    var oldValue = _aclCapability;
                    _aclCapability = value;
                    OnPropertyChanged("AclCapability", value, oldValue);
                }
            }
        } // AclCapability

        protected cmisRepositoryCapabilitiesType _capabilities;
        public virtual cmisRepositoryCapabilitiesType Capabilities
        {
            get
            {
                return _capabilities;
            }
            set
            {
                if (!ReferenceEquals(value, _capabilities))
                {
                    var oldValue = _capabilities;
                    _capabilities = value;
                    OnPropertyChanged("Capabilities", value, oldValue);
                }
            }
        } // Capabilities

        protected bool? _changesIncomplete;
        public virtual bool? ChangesIncomplete
        {
            get
            {
                return _changesIncomplete;
            }
            set
            {
                if (!_changesIncomplete.Equals(value))
                {
                    var oldValue = _changesIncomplete;
                    _changesIncomplete = value;
                    OnPropertyChanged("ChangesIncomplete", value, oldValue);
                }
            }
        } // ChangesIncomplete

        protected enumBaseObjectTypeIds[] _changesOnTypes;
        public virtual enumBaseObjectTypeIds[] ChangesOnTypes
        {
            get
            {
                return _changesOnTypes;
            }
            set
            {
                if (!ReferenceEquals(value, _changesOnTypes))
                {
                    var oldValue = _changesOnTypes;
                    _changesOnTypes = value;
                    OnPropertyChanged("ChangesOnTypes", value, oldValue);
                }
            }
        } // ChangesOnTypes

        protected string _cmisVersionSupported;
        public virtual string CmisVersionSupported
        {
            get
            {
                return _cmisVersionSupported;
            }
            set
            {
                if ((_cmisVersionSupported ?? "") != (value ?? ""))
                {
                    string oldValue = _cmisVersionSupported;
                    _cmisVersionSupported = value;
                    OnPropertyChanged("CmisVersionSupported", value, oldValue);
                }
            }
        } // CmisVersionSupported

        protected cmisExtensionFeatureType[] _extendedFeatures;
        public virtual cmisExtensionFeatureType[] ExtendedFeatures
        {
            get
            {
                return _extendedFeatures;
            }
            set
            {
                if (!ReferenceEquals(value, _extendedFeatures))
                {
                    var oldValue = _extendedFeatures;
                    _extendedFeatures = value;
                    OnPropertyChanged("ExtendedFeatures", value, oldValue);
                }
            }
        } // ExtendedFeatures

        protected string _latestChangeLogToken;
        public virtual string LatestChangeLogToken
        {
            get
            {
                return _latestChangeLogToken;
            }
            set
            {
                if ((_latestChangeLogToken ?? "") != (value ?? ""))
                {
                    string oldValue = _latestChangeLogToken;
                    _latestChangeLogToken = value;
                    OnPropertyChanged("LatestChangeLogToken", value, oldValue);
                }
            }
        } // LatestChangeLogToken

        protected string _principalAnonymous;
        public virtual string PrincipalAnonymous
        {
            get
            {
                return _principalAnonymous;
            }
            set
            {
                if ((_principalAnonymous ?? "") != (value ?? ""))
                {
                    string oldValue = _principalAnonymous;
                    _principalAnonymous = value;
                    OnPropertyChanged("PrincipalAnonymous", value, oldValue);
                }
            }
        } // PrincipalAnonymous

        protected string _principalAnyone;
        public virtual string PrincipalAnyone
        {
            get
            {
                return _principalAnyone;
            }
            set
            {
                if ((_principalAnyone ?? "") != (value ?? ""))
                {
                    string oldValue = _principalAnyone;
                    _principalAnyone = value;
                    OnPropertyChanged("PrincipalAnyone", value, oldValue);
                }
            }
        } // PrincipalAnyone

        protected string _productName;
        public virtual string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                if ((_productName ?? "") != (value ?? ""))
                {
                    string oldValue = _productName;
                    _productName = value;
                    OnPropertyChanged("ProductName", value, oldValue);
                }
            }
        } // ProductName

        protected string _productVersion;
        public virtual string ProductVersion
        {
            get
            {
                return _productVersion;
            }
            set
            {
                if ((_productVersion ?? "") != (value ?? ""))
                {
                    string oldValue = _productVersion;
                    _productVersion = value;
                    OnPropertyChanged("ProductVersion", value, oldValue);
                }
            }
        } // ProductVersion

        protected string _repositoryDescription;
        public virtual string RepositoryDescription
        {
            get
            {
                return _repositoryDescription;
            }
            set
            {
                if ((_repositoryDescription ?? "") != (value ?? ""))
                {
                    string oldValue = _repositoryDescription;
                    _repositoryDescription = value;
                    OnPropertyChanged("RepositoryDescription", value, oldValue);
                }
            }
        } // RepositoryDescription

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

        protected string _repositoryName;
        public virtual string RepositoryName
        {
            get
            {
                return _repositoryName;
            }
            set
            {
                if ((_repositoryName ?? "") != (value ?? ""))
                {
                    string oldValue = _repositoryName;
                    _repositoryName = value;
                    OnPropertyChanged("RepositoryName", value, oldValue);
                }
            }
        } // RepositoryName

        protected string _rootFolderId;
        public virtual string RootFolderId
        {
            get
            {
                return _rootFolderId;
            }
            set
            {
                if ((_rootFolderId ?? "") != (value ?? ""))
                {
                    string oldValue = _rootFolderId;
                    _rootFolderId = value;
                    OnPropertyChanged("RootFolderId", value, oldValue);
                }
            }
        } // RootFolderId

        protected string _thinClientURI;
        public virtual string ThinClientURI
        {
            get
            {
                return _thinClientURI;
            }
            set
            {
                if ((_thinClientURI ?? "") != (value ?? ""))
                {
                    string oldValue = _thinClientURI;
                    _thinClientURI = value;
                    OnPropertyChanged("ThinClientURI", value, oldValue);
                }
            }
        } // ThinClientURI

        protected string _vendorName;
        public virtual string VendorName
        {
            get
            {
                return _vendorName;
            }
            set
            {
                if ((_vendorName ?? "") != (value ?? ""))
                {
                    string oldValue = _vendorName;
                    _vendorName = value;
                    OnPropertyChanged("VendorName", value, oldValue);
                }
            }
        } // VendorName

    }
}