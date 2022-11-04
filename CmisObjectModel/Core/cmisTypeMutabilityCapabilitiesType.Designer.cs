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
   /// see cmisTypeMutabilityCapabilitiesType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public partial class cmisTypeMutabilityCapabilitiesType : Serialization.XmlSerializable
    {

        public cmisTypeMutabilityCapabilitiesType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisTypeMutabilityCapabilitiesType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisTypeMutabilityCapabilitiesType, string>> _setter = new Dictionary<string, Action<cmisTypeMutabilityCapabilitiesType, string>>() { }; // _setter

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
            _create = Read(reader, attributeOverrides, "create", Constants.Namespaces.cmis, _create);
            _update = Read(reader, attributeOverrides, "update", Constants.Namespaces.cmis, _update);
            _delete = Read(reader, attributeOverrides, "delete", Constants.Namespaces.cmis, _delete);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "create", Constants.Namespaces.cmis, CommonFunctions.Convert(_create));
            WriteElement(writer, attributeOverrides, "update", Constants.Namespaces.cmis, CommonFunctions.Convert(_update));
            WriteElement(writer, attributeOverrides, "delete", Constants.Namespaces.cmis, CommonFunctions.Convert(_delete));
        }
        #endregion

        protected bool _create;
        public virtual bool Create
        {
            get
            {
                return _create;
            }
            set
            {
                if (_create != value)
                {
                    bool oldValue = _create;
                    _create = value;
                    OnPropertyChanged("Create", value, oldValue);
                }
            }
        } // Create

        protected bool _delete;
        public virtual bool Delete
        {
            get
            {
                return _delete;
            }
            set
            {
                if (_delete != value)
                {
                    bool oldValue = _delete;
                    _delete = value;
                    OnPropertyChanged("Delete", value, oldValue);
                }
            }
        } // Delete

        protected bool _update;
        public virtual bool Update
        {
            get
            {
                return _update;
            }
            set
            {
                if (_update != value)
                {
                    bool oldValue = _update;
                    _update = value;
                    OnPropertyChanged("Update", value, oldValue);
                }
            }
        } // Update

    }
}