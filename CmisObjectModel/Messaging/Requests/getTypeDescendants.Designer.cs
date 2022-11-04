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
   /// see getTypeDescendants
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("getTypeDescendants", Namespace = Constants.Namespaces.cmism)]
    public partial class getTypeDescendants : RequestBase
    {

        public getTypeDescendants()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getTypeDescendants(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getTypeDescendants, string>> _setter = new Dictionary<string, Action<getTypeDescendants, string>>() { }; // _setter

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
            _depth = Read(reader, attributeOverrides, "depth", Constants.Namespaces.cmism, _depth);
            _includePropertyDefinitions = Read(reader, attributeOverrides, "includePropertyDefinitions", Constants.Namespaces.cmism, _includePropertyDefinitions);
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
            if (_depth.HasValue)
                WriteElement(writer, attributeOverrides, "depth", Constants.Namespaces.cmism, CommonFunctions.Convert(_depth));
            if (_includePropertyDefinitions.HasValue)
                WriteElement(writer, attributeOverrides, "includePropertyDefinitions", Constants.Namespaces.cmism, CommonFunctions.Convert(_includePropertyDefinitions));
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected long? _depth;
        public virtual long? Depth
        {
            get
            {
                return _depth;
            }
            set
            {
                if (!_depth.Equals(value))
                {
                    var oldValue = _depth;
                    _depth = value;
                    OnPropertyChanged("Depth", value, oldValue);
                }
            }
        } // Depth

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