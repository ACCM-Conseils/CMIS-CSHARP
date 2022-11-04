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
namespace CmisObjectModel.Messaging
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisACLType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class cmisACLType : Serialization.XmlSerializable
    {

        public cmisACLType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisACLType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisACLType, string>> _setter = new Dictionary<string, Action<cmisACLType, string>>() { }; // _setter

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
            _aCL = Read(reader, attributeOverrides, "ACL", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
            _exact = Read(reader, attributeOverrides, "exact", Constants.Namespaces.cmism, _exact);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "ACL", Constants.Namespaces.cmism, _aCL);
            if (_exact.HasValue)
                WriteElement(writer, attributeOverrides, "exact", Constants.Namespaces.cmism, CommonFunctions.Convert(_exact));
        }
        #endregion

        protected Core.Security.cmisAccessControlListType _aCL;
        public virtual Core.Security.cmisAccessControlListType ACL
        {
            get
            {
                return _aCL;
            }
            set
            {
                if (!ReferenceEquals(value, _aCL))
                {
                    var oldValue = _aCL;
                    _aCL = value;
                    OnPropertyChanged("ACL", value, oldValue);
                }
            }
        } // ACL

        protected bool? _exact;
        public virtual bool? Exact
        {
            get
            {
                return _exact;
            }
            set
            {
                if (!_exact.Equals(value))
                {
                    var oldValue = _exact;
                    _exact = value;
                    OnPropertyChanged("Exact", value, oldValue);
                }
            }
        } // Exact

    }
}