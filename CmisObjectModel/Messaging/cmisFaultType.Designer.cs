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
   /// see cmisFaultType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public partial class cmisFaultType : Serialization.XmlSerializable
    {

        public cmisFaultType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisFaultType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisFaultType, string>> _setter = new Dictionary<string, Action<cmisFaultType, string>>() { }; // _setter

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
            _type = ReadEnum(reader, attributeOverrides, "type", Constants.Namespaces.cmism, _type);
            _code = Read(reader, attributeOverrides, "code", Constants.Namespaces.cmism, _code);
            _message = Read(reader, attributeOverrides, "message", Constants.Namespaces.cmism, _message);
            _extensions = ReadArray(reader, attributeOverrides, null, CmisObjectModel.Extensions.Extension.CreateInstance);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "type", Constants.Namespaces.cmism, _type.GetName());
            WriteElement(writer, attributeOverrides, "code", Constants.Namespaces.cmism, CommonFunctions.Convert(_code));
            WriteElement(writer, attributeOverrides, "message", Constants.Namespaces.cmism, _message);
            WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmism, _extensions);
        }
        #endregion

        protected long _code;
        public virtual long Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    long oldValue = _code;
                    _code = value;
                    OnPropertyChanged("Code", value, oldValue);
                }
            }
        } // Code

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

        protected string _message;
        public virtual string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if ((_message ?? "") != (value ?? ""))
                {
                    string oldValue = _message;
                    _message = value;
                    OnPropertyChanged("Message", value, oldValue);
                }
            }
        } // Message

        protected enumServiceException _type;
        public virtual enumServiceException Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    var oldValue = _type;
                    _type = value;
                    OnPropertyChanged("Type", value, oldValue);
                }
            }
        } // Type

    }
}