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
   /// see cmisContentStreamType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class cmisContentStreamType : Serialization.XmlSerializable
    {

        public cmisContentStreamType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisContentStreamType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisContentStreamType, string>> _setter = new Dictionary<string, Action<cmisContentStreamType, string>>() { }; // _setter

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
            _length = Read(reader, attributeOverrides, "length", Constants.Namespaces.cmism, _length);
            _mimeType = Read(reader, attributeOverrides, "mimeType", Constants.Namespaces.cmism, _mimeType);
            _filename = Read(reader, attributeOverrides, "filename", Constants.Namespaces.cmism, _filename);
            _stream = Read(reader, attributeOverrides, "stream", Constants.Namespaces.cmism, _stream);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (_length.HasValue)
                WriteElement(writer, attributeOverrides, "length", Constants.Namespaces.cmism, CommonFunctions.Convert(_length));
            if (!string.IsNullOrEmpty(_mimeType))
                WriteElement(writer, attributeOverrides, "mimeType", Constants.Namespaces.cmism, _mimeType);
            if (!string.IsNullOrEmpty(_filename))
                WriteElement(writer, attributeOverrides, "filename", Constants.Namespaces.cmism, _filename);
            WriteElement(writer, attributeOverrides, "stream", Constants.Namespaces.cmism, _stream);
        }
        #endregion

        protected string _filename;
        public virtual string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                if ((_filename ?? "") != (value ?? ""))
                {
                    string oldValue = _filename;
                    _filename = value;
                    OnPropertyChanged("Filename", value, oldValue);
                }
            }
        } // Filename

        protected long? _length;
        public virtual long? Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (!_length.Equals(value))
                {
                    var oldValue = _length;
                    _length = value;
                    OnPropertyChanged("Length", value, oldValue);
                }
            }
        } // Length

        protected string _mimeType;
        public virtual string MimeType
        {
            get
            {
                return _mimeType;
            }
            set
            {
                if ((_mimeType ?? "") != (value ?? ""))
                {
                    string oldValue = _mimeType;
                    _mimeType = value;
                    OnPropertyChanged("MimeType", value, oldValue);
                }
            }
        } // MimeType

        protected string _stream;
        public virtual string Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if ((_stream ?? "") != (value ?? ""))
                {
                    string oldValue = _stream;
                    _stream = value;
                    OnPropertyChanged("Stream", value, oldValue);
                }
            }
        } // Stream

    }
}