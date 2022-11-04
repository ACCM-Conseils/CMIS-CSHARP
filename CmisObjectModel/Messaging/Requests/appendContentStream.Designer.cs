﻿using System;
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
   /// see appendContentStream
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("appendContentStream", Namespace = Constants.Namespaces.cmism)]
    public partial class appendContentStream : RequestBase
    {

        public appendContentStream()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected appendContentStream(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<appendContentStream, string>> _setter = new Dictionary<string, Action<appendContentStream, string>>() { }; // _setter

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
            _objectId = Read(reader, attributeOverrides, "objectId", Constants.Namespaces.cmism, _objectId);
            _isLastChunk = Read(reader, attributeOverrides, "isLastChunk", Constants.Namespaces.cmism, _isLastChunk);
            _changeToken = Read(reader, attributeOverrides, "changeToken", Constants.Namespaces.cmism, _changeToken);
            _contentStream = Read(reader, attributeOverrides, "contentStream", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisContentStreamType>);
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
            WriteElement(writer, attributeOverrides, "objectId", Constants.Namespaces.cmism, _objectId);
            if (_isLastChunk.HasValue)
                WriteElement(writer, attributeOverrides, "isLastChunk", Constants.Namespaces.cmism, CommonFunctions.Convert(_isLastChunk));
            if (!string.IsNullOrEmpty(_changeToken))
                WriteElement(writer, attributeOverrides, "changeToken", Constants.Namespaces.cmism, _changeToken);
            WriteElement(writer, attributeOverrides, "contentStream", Constants.Namespaces.cmism, _contentStream);
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected string _changeToken;
        public virtual string ChangeToken
        {
            get
            {
                return _changeToken;
            }
            set
            {
                if ((_changeToken ?? "") != (value ?? ""))
                {
                    string oldValue = _changeToken;
                    _changeToken = value;
                    OnPropertyChanged("ChangeToken", value, oldValue);
                }
            }
        } // ChangeToken

        protected cmisContentStreamType _contentStream;
        public virtual cmisContentStreamType ContentStream
        {
            get
            {
                return _contentStream;
            }
            set
            {
                if (!ReferenceEquals(value, _contentStream))
                {
                    var oldValue = _contentStream;
                    _contentStream = value;
                    OnPropertyChanged("ContentStream", value, oldValue);
                }
            }
        } // ContentStream

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

        protected bool? _isLastChunk;
        public virtual bool? IsLastChunk
        {
            get
            {
                return _isLastChunk;
            }
            set
            {
                if (!_isLastChunk.Equals(value))
                {
                    var oldValue = _isLastChunk;
                    _isLastChunk = value;
                    OnPropertyChanged("IsLastChunk", value, oldValue);
                }
            }
        } // IsLastChunk

        protected string _objectId;
        public virtual string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                if ((_objectId ?? "") != (value ?? ""))
                {
                    string oldValue = _objectId;
                    _objectId = value;
                    OnPropertyChanged("ObjectId", value, oldValue);
                }
            }
        } // ObjectId

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

    }
}