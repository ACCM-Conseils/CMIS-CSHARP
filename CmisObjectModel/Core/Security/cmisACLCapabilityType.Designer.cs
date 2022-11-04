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
namespace CmisObjectModel.Core.Security
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisACLCapabilityType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisACLCapabilityType : Serialization.XmlSerializable
    {

        public cmisACLCapabilityType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisACLCapabilityType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisACLCapabilityType, string>> _setter = new Dictionary<string, Action<cmisACLCapabilityType, string>>() { }; // _setter

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
            _supportedPermissions = ReadEnum(reader, attributeOverrides, "supportedPermissions", Constants.Namespaces.cmis, _supportedPermissions);
            _propagation = ReadEnum(reader, attributeOverrides, "propagation", Constants.Namespaces.cmis, _propagation);
            _permissions = ReadArray(reader, attributeOverrides, "permissions", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisPermissionDefinition>);
            _mappings = ReadArray(reader, attributeOverrides, "mapping", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisPermissionMapping>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "supportedPermissions", Constants.Namespaces.cmis, _supportedPermissions.GetName());
            WriteElement(writer, attributeOverrides, "propagation", Constants.Namespaces.cmis, _propagation.GetName());
            WriteArray(writer, attributeOverrides, "permissions", Constants.Namespaces.cmis, _permissions);
            WriteArray(writer, attributeOverrides, "mapping", Constants.Namespaces.cmis, _mappings);
        }
        #endregion

        protected cmisPermissionMapping[] _mappings;
        public virtual cmisPermissionMapping[] Mappings
        {
            get
            {
                return _mappings;
            }
            set
            {
                if (!ReferenceEquals(value, _mappings))
                {
                    var oldValue = _mappings;
                    _mappings = value;
                    OnPropertyChanged("Mappings", value, oldValue);
                }
            }
        } // Mappings

        protected cmisPermissionDefinition[] _permissions;
        public virtual cmisPermissionDefinition[] Permissions
        {
            get
            {
                return _permissions;
            }
            set
            {
                if (!ReferenceEquals(value, _permissions))
                {
                    var oldValue = _permissions;
                    _permissions = value;
                    OnPropertyChanged("Permissions", value, oldValue);
                }
            }
        } // Permissions

        protected enumACLPropagation _propagation;
        public virtual enumACLPropagation Propagation
        {
            get
            {
                return _propagation;
            }
            set
            {
                if (_propagation != value)
                {
                    var oldValue = _propagation;
                    _propagation = value;
                    OnPropertyChanged("Propagation", value, oldValue);
                }
            }
        } // Propagation

        protected enumSupportedPermissions _supportedPermissions;
        public virtual enumSupportedPermissions SupportedPermissions
        {
            get
            {
                return _supportedPermissions;
            }
            set
            {
                if (_supportedPermissions != value)
                {
                    var oldValue = _supportedPermissions;
                    _supportedPermissions = value;
                    OnPropertyChanged("SupportedPermissions", value, oldValue);
                }
            }
        } // SupportedPermissions

    }
}