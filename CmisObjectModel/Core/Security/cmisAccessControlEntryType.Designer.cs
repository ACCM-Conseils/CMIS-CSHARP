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
   /// see cmisAccessControlEntryType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisAccessControlEntryType : Serialization.XmlSerializable
    {

        public cmisAccessControlEntryType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisAccessControlEntryType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisAccessControlEntryType, string>> _setter = new Dictionary<string, Action<cmisAccessControlEntryType, string>>() { }; // _setter

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
            _principal = Read(reader, attributeOverrides, "principal", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisAccessControlPrincipalType>);
            _permissions = ReadArray<string>(reader, attributeOverrides, "permission", Constants.Namespaces.cmis);
            _direct = Read(reader, attributeOverrides, "direct", Constants.Namespaces.cmis, _direct);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "principal", Constants.Namespaces.cmis, _principal);
            WriteArray(writer, attributeOverrides, "permission", Constants.Namespaces.cmis, _permissions);
            WriteElement(writer, attributeOverrides, "direct", Constants.Namespaces.cmis, CommonFunctions.Convert(_direct));
        }
        #endregion

        protected bool _direct;
        public virtual bool Direct
        {
            get
            {
                return _direct;
            }
            set
            {
                if (_direct != value)
                {
                    bool oldValue = _direct;
                    _direct = value;
                    OnPropertyChanged("Direct", value, oldValue);
                }
            }
        } // Direct

        protected string[] _permissions;
        public virtual string[] Permissions
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

        protected cmisAccessControlPrincipalType _principal;
        public virtual cmisAccessControlPrincipalType Principal
        {
            get
            {
                return _principal;
            }
            set
            {
                if (!ReferenceEquals(value, _principal))
                {
                    var oldValue = _principal;
                    _principal = value;
                    OnPropertyChanged("Principal", value, oldValue);
                }
            }
        } // Principal

    }
}