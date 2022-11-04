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
   /// see cmisObjectInFolderListType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class cmisObjectInFolderListType : Serialization.XmlSerializable
    {

        public cmisObjectInFolderListType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisObjectInFolderListType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisObjectInFolderListType, string>> _setter = new Dictionary<string, Action<cmisObjectInFolderListType, string>>() { }; // _setter

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
            _objects = ReadArray(reader, attributeOverrides, "objects", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisObjectInFolderType>);
            _hasMoreItems = Read(reader, attributeOverrides, "hasMoreItems", Constants.Namespaces.cmism, _hasMoreItems);
            _numItems = Read(reader, attributeOverrides, "numItems", Constants.Namespaces.cmism, _numItems);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "objects", Constants.Namespaces.cmism, _objects);
            WriteElement(writer, attributeOverrides, "hasMoreItems", Constants.Namespaces.cmism, CommonFunctions.Convert(_hasMoreItems));
            if (_numItems.HasValue)
                WriteElement(writer, attributeOverrides, "numItems", Constants.Namespaces.cmism, CommonFunctions.Convert(_numItems));
        }
        #endregion

        protected bool _hasMoreItems;
        public virtual bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
            set
            {
                if (_hasMoreItems != value)
                {
                    bool oldValue = _hasMoreItems;
                    _hasMoreItems = value;
                    OnPropertyChanged("HasMoreItems", value, oldValue);
                }
            }
        } // HasMoreItems

        protected long? _numItems;
        public virtual long? NumItems
        {
            get
            {
                return _numItems;
            }
            set
            {
                if (!_numItems.Equals(value))
                {
                    var oldValue = _numItems;
                    _numItems = value;
                    OnPropertyChanged("NumItems", value, oldValue);
                }
            }
        } // NumItems

        protected cmisObjectInFolderType[] _objects;
        public virtual cmisObjectInFolderType[] Objects
        {
            get
            {
                return _objects;
            }
            set
            {
                if (!ReferenceEquals(value, _objects))
                {
                    var oldValue = _objects;
                    _objects = value;
                    OnPropertyChanged("Objects", value, oldValue);
                }
            }
        } // Objects

    }
}