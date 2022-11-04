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
namespace CmisObjectModel.Core.Definitions.Types
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisTypeDefinitionType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public abstract partial class cmisTypeDefinitionType : DefinitionBase
    {

        protected cmisTypeDefinitionType() : base(true)
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisTypeDefinitionType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisTypeDefinitionType, string>> _setter = new Dictionary<string, Action<cmisTypeDefinitionType, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _id = Read(reader, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            _localName = Read(reader, attributeOverrides, "localName", Constants.Namespaces.cmis, _localName);
            _localNamespace = Read(reader, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, _localNamespace);
            _displayName = Read(reader, attributeOverrides, "displayName", Constants.Namespaces.cmis, _displayName);
            _queryName = Read(reader, attributeOverrides, "queryName", Constants.Namespaces.cmis, _queryName);
            _description = Read(reader, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            // baseId is readonly
            ReadEnum(reader, attributeOverrides, "baseId", Constants.Namespaces.cmis, _baseId);
            _parentId = Read(reader, attributeOverrides, "parentId", Constants.Namespaces.cmis, _parentId);
            _creatable = Read(reader, attributeOverrides, "creatable", Constants.Namespaces.cmis, _creatable);
            _fileable = Read(reader, attributeOverrides, "fileable", Constants.Namespaces.cmis, _fileable);
            _queryable = Read(reader, attributeOverrides, "queryable", Constants.Namespaces.cmis, _queryable);
            _fulltextIndexed = Read(reader, attributeOverrides, "fulltextIndexed", Constants.Namespaces.cmis, _fulltextIndexed);
            _includedInSupertypeQuery = Read(reader, attributeOverrides, "includedInSupertypeQuery", Constants.Namespaces.cmis, _includedInSupertypeQuery);
            _controllablePolicy = Read(reader, attributeOverrides, "controllablePolicy", Constants.Namespaces.cmis, _controllablePolicy);
            _controllableACL = Read(reader, attributeOverrides, "controllableACL", Constants.Namespaces.cmis, _controllableACL);
            _typeMutability = Read(reader, attributeOverrides, "typeMutability", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisTypeMutabilityCapabilitiesType>);
            _propertyDefinitions = ReadArray(reader, attributeOverrides, null, Properties.cmisPropertyDefinitionType.CreateInstance);
            _extensions = ReadArray(reader, attributeOverrides, null, CmisObjectModel.Extensions.Extension.CreateInstance);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            WriteElement(writer, attributeOverrides, "localName", Constants.Namespaces.cmis, _localName);
            WriteElement(writer, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, _localNamespace);
            if (!string.IsNullOrEmpty(_displayName))
                WriteElement(writer, attributeOverrides, "displayName", Constants.Namespaces.cmis, _displayName);
            if (!string.IsNullOrEmpty(_queryName))
                WriteElement(writer, attributeOverrides, "queryName", Constants.Namespaces.cmis, _queryName);
            if (!string.IsNullOrEmpty(_description))
                WriteElement(writer, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            WriteElement(writer, attributeOverrides, "baseId", Constants.Namespaces.cmis, _baseId.GetName());
            if (!string.IsNullOrEmpty(_parentId))
                WriteElement(writer, attributeOverrides, "parentId", Constants.Namespaces.cmis, _parentId);
            WriteElement(writer, attributeOverrides, "creatable", Constants.Namespaces.cmis, CommonFunctions.Convert(_creatable));
            WriteElement(writer, attributeOverrides, "fileable", Constants.Namespaces.cmis, CommonFunctions.Convert(_fileable));
            WriteElement(writer, attributeOverrides, "queryable", Constants.Namespaces.cmis, CommonFunctions.Convert(_queryable));
            WriteElement(writer, attributeOverrides, "fulltextIndexed", Constants.Namespaces.cmis, CommonFunctions.Convert(_fulltextIndexed));
            WriteElement(writer, attributeOverrides, "includedInSupertypeQuery", Constants.Namespaces.cmis, CommonFunctions.Convert(_includedInSupertypeQuery));
            WriteElement(writer, attributeOverrides, "controllablePolicy", Constants.Namespaces.cmis, CommonFunctions.Convert(_controllablePolicy));
            WriteElement(writer, attributeOverrides, "controllableACL", Constants.Namespaces.cmis, CommonFunctions.Convert(_controllableACL));
            WriteElement(writer, attributeOverrides, "typeMutability", Constants.Namespaces.cmis, _typeMutability);
            WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmis, _propertyDefinitions);
            WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmis, _extensions);
        }
        #endregion

        protected bool _controllableACL;
        public virtual bool ControllableACL
        {
            get
            {
                return _controllableACL;
            }
            set
            {
                if (_controllableACL != value)
                {
                    bool oldValue = _controllableACL;
                    _controllableACL = value;
                    OnPropertyChanged("ControllableACL", value, oldValue);
                }
            }
        } // ControllableACL

        protected bool _controllablePolicy;
        public virtual bool ControllablePolicy
        {
            get
            {
                return _controllablePolicy;
            }
            set
            {
                if (_controllablePolicy != value)
                {
                    bool oldValue = _controllablePolicy;
                    _controllablePolicy = value;
                    OnPropertyChanged("ControllablePolicy", value, oldValue);
                }
            }
        } // ControllablePolicy

        protected bool _creatable;
        public virtual bool Creatable
        {
            get
            {
                return _creatable;
            }
            set
            {
                if (_creatable != value)
                {
                    bool oldValue = _creatable;
                    _creatable = value;
                    OnPropertyChanged("Creatable", value, oldValue);
                }
            }
        } // Creatable

        protected Extensions.Extension[] _extensions;
        public virtual Extensions.Extension[] Extensions
        {
            get
            {
                return _extensions;
            }
            set
            {
                if (!ReferenceEquals(value, _extensions))
                {
                    var oldValue = _extensions;
                    _extensions = value;
                    OnPropertyChanged("Extensions", value, oldValue);
                }
            }
        } // Extensions

        protected bool _fileable;
        public virtual bool Fileable
        {
            get
            {
                return _fileable;
            }
            set
            {
                if (_fileable != value)
                {
                    bool oldValue = _fileable;
                    _fileable = value;
                    OnPropertyChanged("Fileable", value, oldValue);
                }
            }
        } // Fileable

        protected bool _fulltextIndexed;
        public virtual bool FulltextIndexed
        {
            get
            {
                return _fulltextIndexed;
            }
            set
            {
                if (_fulltextIndexed != value)
                {
                    bool oldValue = _fulltextIndexed;
                    _fulltextIndexed = value;
                    OnPropertyChanged("FulltextIndexed", value, oldValue);
                }
            }
        } // FulltextIndexed

        protected bool _includedInSupertypeQuery;
        public virtual bool IncludedInSupertypeQuery
        {
            get
            {
                return _includedInSupertypeQuery;
            }
            set
            {
                if (_includedInSupertypeQuery != value)
                {
                    bool oldValue = _includedInSupertypeQuery;
                    _includedInSupertypeQuery = value;
                    OnPropertyChanged("IncludedInSupertypeQuery", value, oldValue);
                }
            }
        } // IncludedInSupertypeQuery

        protected string _parentId;
        public virtual string ParentId
        {
            get
            {
                return _parentId;
            }
            set
            {
                if ((_parentId ?? "") != (value ?? ""))
                {
                    string oldValue = _parentId;
                    _parentId = value;
                    OnPropertyChanged("ParentId", value, oldValue);
                }
            }
        } // ParentId

        protected static Properties.cmisPropertyDefinitionType[] _propertyDefinitions;
        public virtual Properties.cmisPropertyDefinitionType[] PropertyDefinitions
        {
            get
            {
                return _propertyDefinitions;
            }
            set
            {
                if (!ReferenceEquals(value, _propertyDefinitions))
                {
                    var oldValue = _propertyDefinitions;
                    _propertyDefinitions = value;
                    OnPropertyChanged("PropertyDefinitions", value, oldValue);
                }
            }
        } // PropertyDefinitions

        protected cmisTypeMutabilityCapabilitiesType _typeMutability;
        public virtual cmisTypeMutabilityCapabilitiesType TypeMutability
        {
            get
            {
                return _typeMutability;
            }
            set
            {
                if (!ReferenceEquals(value, _typeMutability))
                {
                    var oldValue = _typeMutability;
                    _typeMutability = value;
                    OnPropertyChanged("TypeMutability", value, oldValue);
                }
            }
        } // TypeMutability

    }
}