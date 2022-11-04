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
   /// see cmisRenditionType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisRenditionType : Serialization.XmlSerializable
    {

        public cmisRenditionType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisRenditionType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisRenditionType, string>> _setter = new Dictionary<string, Action<cmisRenditionType, string>>() { }; // _setter

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
            _streamId = Read(reader, attributeOverrides, "streamId", Constants.Namespaces.cmis, _streamId);
            _mimetype = Read(reader, attributeOverrides, "mimetype", Constants.Namespaces.cmis, _mimetype);
            _length = Read(reader, attributeOverrides, "length", Constants.Namespaces.cmis, _length);
            _kind = Read(reader, attributeOverrides, "kind", Constants.Namespaces.cmis, _kind);
            _title = Read(reader, attributeOverrides, "title", Constants.Namespaces.cmis, _title);
            _height = Read(reader, attributeOverrides, "height", Constants.Namespaces.cmis, _height);
            _width = Read(reader, attributeOverrides, "width", Constants.Namespaces.cmis, _width);
            _renditionDocumentId = Read(reader, attributeOverrides, "renditionDocumentId", Constants.Namespaces.cmis, _renditionDocumentId);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "streamId", Constants.Namespaces.cmis, _streamId);
            WriteElement(writer, attributeOverrides, "mimetype", Constants.Namespaces.cmis, _mimetype);
            WriteElement(writer, attributeOverrides, "length", Constants.Namespaces.cmis, CommonFunctions.Convert(_length));
            WriteElement(writer, attributeOverrides, "kind", Constants.Namespaces.cmis, _kind);
            if (!string.IsNullOrEmpty(_title))
                WriteElement(writer, attributeOverrides, "title", Constants.Namespaces.cmis, _title);
            if (_height.HasValue)
                WriteElement(writer, attributeOverrides, "height", Constants.Namespaces.cmis, CommonFunctions.Convert(_height));
            if (_width.HasValue)
                WriteElement(writer, attributeOverrides, "width", Constants.Namespaces.cmis, CommonFunctions.Convert(_width));
            if (!string.IsNullOrEmpty(_renditionDocumentId))
                WriteElement(writer, attributeOverrides, "renditionDocumentId", Constants.Namespaces.cmis, _renditionDocumentId);
        }
        #endregion

        protected long? _height;
        public virtual long? Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (!_height.Equals(value))
                {
                    var oldValue = _height;
                    _height = value;
                    OnPropertyChanged("Height", value, oldValue);
                }
            }
        } // Height

        protected string _kind;
        public virtual string Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                if ((_kind ?? "") != (value ?? ""))
                {
                    string oldValue = _kind;
                    _kind = value;
                    OnPropertyChanged("Kind", value, oldValue);
                }
            }
        } // Kind

        protected long _length;
        public virtual long Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (_length != value)
                {
                    long oldValue = _length;
                    _length = value;
                    OnPropertyChanged("Length", value, oldValue);
                }
            }
        } // Length

        protected string _mimetype;
        public virtual string Mimetype
        {
            get
            {
                return _mimetype;
            }
            set
            {
                if ((_mimetype ?? "") != (value ?? ""))
                {
                    string oldValue = _mimetype;
                    _mimetype = value;
                    OnPropertyChanged("Mimetype", value, oldValue);
                }
            }
        } // Mimetype

        protected string _renditionDocumentId;
        public virtual string RenditionDocumentId
        {
            get
            {
                return _renditionDocumentId;
            }
            set
            {
                if ((_renditionDocumentId ?? "") != (value ?? ""))
                {
                    string oldValue = _renditionDocumentId;
                    _renditionDocumentId = value;
                    OnPropertyChanged("RenditionDocumentId", value, oldValue);
                }
            }
        } // RenditionDocumentId

        protected string _streamId;
        public virtual string StreamId
        {
            get
            {
                return _streamId;
            }
            set
            {
                if ((_streamId ?? "") != (value ?? ""))
                {
                    string oldValue = _streamId;
                    _streamId = value;
                    OnPropertyChanged("StreamId", value, oldValue);
                }
            }
        } // StreamId

        protected string _title;
        public virtual string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if ((_title ?? "") != (value ?? ""))
                {
                    string oldValue = _title;
                    _title = value;
                    OnPropertyChanged("Title", value, oldValue);
                }
            }
        } // Title

        protected long? _width;
        public virtual long? Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (!_width.Equals(value))
                {
                    var oldValue = _width;
                    _width = value;
                    OnPropertyChanged("Width", value, oldValue);
                }
            }
        } // Width

    }
}