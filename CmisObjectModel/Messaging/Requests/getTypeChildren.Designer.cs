using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
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
namespace CmisObjectModel.Messaging.Requests
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see getTypeChildren
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("getTypeChildren", Namespace = Constants.Namespaces.cmism)]
    public partial class getTypeChildren : RequestBase
    {

        public getTypeChildren()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getTypeChildren(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getTypeChildren, string>> _setter = new Dictionary<string, Action<getTypeChildren, string>>() { }; // _setter

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
            _repositoryId = Read(reader, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            _typeId = Read(reader, attributeOverrides, "typeId", Constants.Namespaces.cmism, _typeId);
            _includePropertyDefinitions = Read(reader, attributeOverrides, "includePropertyDefinitions", Constants.Namespaces.cmism, _includePropertyDefinitions);
            _maxItems = Read(reader, attributeOverrides, "maxItems", Constants.Namespaces.cmism, _maxItems);
            _skipCount = Read(reader, attributeOverrides, "skipCount", Constants.Namespaces.cmism, _skipCount);
            _extension = Read(reader, attributeOverrides, "extension", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisExtensionType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            if (!string.IsNullOrEmpty(_typeId))
                WriteElement(writer, attributeOverrides, "typeId", Constants.Namespaces.cmism, _typeId);
            if (_includePropertyDefinitions.HasValue)
                WriteElement(writer, attributeOverrides, "includePropertyDefinitions", Constants.Namespaces.cmism, CommonFunctions.Convert(_includePropertyDefinitions));
            if (_maxItems.HasValue)
                WriteElement(writer, attributeOverrides, "maxItems", Constants.Namespaces.cmism, CommonFunctions.Convert(_maxItems));
            if (_skipCount.HasValue)
                WriteElement(writer, attributeOverrides, "skipCount", Constants.Namespaces.cmism, CommonFunctions.Convert(_skipCount));
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected cmisExtensionType _extension;
        public virtual cmisExtensionType Extension
        {
            get
            {
                return _extension;
            }
            set
            {
                if (!ReferenceEquals(value, _extension))
                {
                    var oldValue = _extension;
                    _extension = value;
                    OnPropertyChanged("Extension", value, oldValue);
                }
            }
        } // Extension

        protected bool? _includePropertyDefinitions;
        public virtual bool? IncludePropertyDefinitions
        {
            get
            {
                return _includePropertyDefinitions;
            }
            set
            {
                if (!_includePropertyDefinitions.Equals(value))
                {
                    var oldValue = _includePropertyDefinitions;
                    _includePropertyDefinitions = value;
                    OnPropertyChanged("IncludePropertyDefinitions", value, oldValue);
                }
            }
        } // IncludePropertyDefinitions

        protected long? _maxItems;
        public virtual long? MaxItems
        {
            get
            {
                return _maxItems;
            }
            set
            {
                if (!_maxItems.Equals(value))
                {
                    var oldValue = _maxItems;
                    _maxItems = value;
                    OnPropertyChanged("MaxItems", value, oldValue);
                }
            }
        } // MaxItems

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

        protected long? _skipCount;
        public virtual long? SkipCount
        {
            get
            {
                return _skipCount;
            }
            set
            {
                if (!_skipCount.Equals(value))
                {
                    var oldValue = _skipCount;
                    _skipCount = value;
                    OnPropertyChanged("SkipCount", value, oldValue);
                }
            }
        } // SkipCount

        protected string _typeId;
        public virtual string TypeId
        {
            get
            {
                return _typeId;
            }
            set
            {
                if ((_typeId ?? "") != (value ?? ""))
                {
                    string oldValue = _typeId;
                    _typeId = value;
                    OnPropertyChanged("TypeId", value, oldValue);
                }
            }
        } // TypeId

    }
}