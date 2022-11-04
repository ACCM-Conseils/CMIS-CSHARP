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
   /// see deleteTree
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("deleteTree", Namespace = Constants.Namespaces.cmism)]
    public partial class deleteTree : RequestBase
    {

        public deleteTree()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected deleteTree(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<deleteTree, string>> _setter = new Dictionary<string, Action<deleteTree, string>>() { }; // _setter

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
            _folderId = Read(reader, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            _allVersions = Read(reader, attributeOverrides, "allVersions", Constants.Namespaces.cmism, _allVersions);
            _unfileObjects = ReadOptionalEnum(reader, attributeOverrides, "unfileObjects", Constants.Namespaces.cmism, _unfileObjects);
            _continueOnFailure = Read(reader, attributeOverrides, "continueOnFailure", Constants.Namespaces.cmism, _continueOnFailure);
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
            WriteElement(writer, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            if (_allVersions.HasValue)
                WriteElement(writer, attributeOverrides, "allVersions", Constants.Namespaces.cmism, CommonFunctions.Convert(_allVersions));
            if (_unfileObjects.HasValue)
                WriteElement(writer, attributeOverrides, "unfileObjects", Constants.Namespaces.cmism, _unfileObjects.Value.GetName());
            if (_continueOnFailure.HasValue)
                WriteElement(writer, attributeOverrides, "continueOnFailure", Constants.Namespaces.cmism, CommonFunctions.Convert(_continueOnFailure));
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected bool? _allVersions;
        public virtual bool? AllVersions
        {
            get
            {
                return _allVersions;
            }
            set
            {
                if (!_allVersions.Equals(value))
                {
                    var oldValue = _allVersions;
                    _allVersions = value;
                    OnPropertyChanged("AllVersions", value, oldValue);
                }
            }
        } // AllVersions

        protected bool? _continueOnFailure;
        public virtual bool? ContinueOnFailure
        {
            get
            {
                return _continueOnFailure;
            }
            set
            {
                if (!_continueOnFailure.Equals(value))
                {
                    var oldValue = _continueOnFailure;
                    _continueOnFailure = value;
                    OnPropertyChanged("ContinueOnFailure", value, oldValue);
                }
            }
        } // ContinueOnFailure

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

        protected string _folderId;
        public virtual string FolderId
        {
            get
            {
                return _folderId;
            }
            set
            {
                if ((_folderId ?? "") != (value ?? ""))
                {
                    string oldValue = _folderId;
                    _folderId = value;
                    OnPropertyChanged("FolderId", value, oldValue);
                }
            }
        } // FolderId

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

        protected Core.enumUnfileObject? _unfileObjects;
        public virtual Core.enumUnfileObject? UnfileObjects
        {
            get
            {
                return _unfileObjects;
            }
            set
            {
                if (!_unfileObjects.Equals(value))
                {
                    var oldValue = _unfileObjects;
                    _unfileObjects = value;
                    OnPropertyChanged("UnfileObjects", value, oldValue);
                }
            }
        } // UnfileObjects

    }
}