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
namespace CmisObjectModel.Core.Definitions.Properties
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPropertyDefinitionType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public abstract partial class cmisPropertyDefinitionType : DefinitionBase
    {

        protected cmisPropertyDefinitionType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyDefinitionType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyDefinitionType, string>> _setter = new Dictionary<string, Action<cmisPropertyDefinitionType, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _id = Read(reader, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            _localName = Read(reader, attributeOverrides, "localName", Constants.Namespaces.cmis, _localName);
            _localNamespace = Read(reader, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, _localNamespace);
            _displayName = Read(reader, attributeOverrides, "displayName", Constants.Namespaces.cmis, _displayName);
            _queryName = Read(reader, attributeOverrides, "queryName", Constants.Namespaces.cmis, _queryName);
            _description = Read(reader, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            // propertyType is readonly
            ReadEnum(reader, attributeOverrides, "propertyType", Constants.Namespaces.cmis, _propertyType);
            _cardinality = ReadEnum(reader, attributeOverrides, "cardinality", Constants.Namespaces.cmis, _cardinality);
            _updatability = ReadEnum(reader, attributeOverrides, "updatability", Constants.Namespaces.cmis, _updatability);
            _inherited = Read(reader, attributeOverrides, "inherited", Constants.Namespaces.cmis, _inherited);
            _required = Read(reader, attributeOverrides, "required", Constants.Namespaces.cmis, _required);
            _queryable = Read(reader, attributeOverrides, "queryable", Constants.Namespaces.cmis, _queryable);
            _orderable = Read(reader, attributeOverrides, "orderable", Constants.Namespaces.cmis, _orderable);
            _openChoice = Read(reader, attributeOverrides, "openChoice", Constants.Namespaces.cmis, _openChoice);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            WriteElement(writer, attributeOverrides, "localName", Constants.Namespaces.cmis, _localName);
            if (!string.IsNullOrEmpty(_localNamespace))
                WriteElement(writer, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, _localNamespace);
            if (!string.IsNullOrEmpty(_displayName))
                WriteElement(writer, attributeOverrides, "displayName", Constants.Namespaces.cmis, _displayName);
            if (!string.IsNullOrEmpty(_queryName))
                WriteElement(writer, attributeOverrides, "queryName", Constants.Namespaces.cmis, _queryName);
            if (!string.IsNullOrEmpty(_description))
                WriteElement(writer, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            WriteElement(writer, attributeOverrides, "propertyType", Constants.Namespaces.cmis, _propertyType.GetName());
            WriteElement(writer, attributeOverrides, "cardinality", Constants.Namespaces.cmis, _cardinality.GetName());
            WriteElement(writer, attributeOverrides, "updatability", Constants.Namespaces.cmis, _updatability.GetName());
            if (_inherited.HasValue)
                WriteElement(writer, attributeOverrides, "inherited", Constants.Namespaces.cmis, CommonFunctions.Convert(_inherited));
            WriteElement(writer, attributeOverrides, "required", Constants.Namespaces.cmis, CommonFunctions.Convert(_required));
            WriteElement(writer, attributeOverrides, "queryable", Constants.Namespaces.cmis, CommonFunctions.Convert(_queryable));
            WriteElement(writer, attributeOverrides, "orderable", Constants.Namespaces.cmis, CommonFunctions.Convert(_orderable));
            if (_openChoice.HasValue)
                WriteElement(writer, attributeOverrides, "openChoice", Constants.Namespaces.cmis, CommonFunctions.Convert(_openChoice));
        }
        #endregion

        protected enumCardinality _cardinality;
        public virtual enumCardinality Cardinality
        {
            get
            {
                return _cardinality;
            }
            set
            {
                if (_cardinality != value)
                {
                    var oldValue = _cardinality;
                    _cardinality = value;
                    OnPropertyChanged("Cardinality", value, oldValue);
                }
            }
        } // Cardinality

        protected bool? _inherited;
        public virtual bool? Inherited
        {
            get
            {
                return _inherited;
            }
            set
            {
                if (!_inherited.Equals(value))
                {
                    var oldValue = _inherited;
                    _inherited = value;
                    OnPropertyChanged("Inherited", value, oldValue);
                }
            }
        } // Inherited

        protected bool? _openChoice;
        public virtual bool? OpenChoice
        {
            get
            {
                return _openChoice;
            }
            set
            {
                if (!_openChoice.Equals(value))
                {
                    var oldValue = _openChoice;
                    _openChoice = value;
                    OnPropertyChanged("OpenChoice", value, oldValue);
                }
            }
        } // OpenChoice

        protected bool _orderable;
        public virtual bool Orderable
        {
            get
            {
                return _orderable;
            }
            set
            {
                if (_orderable != value)
                {
                    bool oldValue = _orderable;
                    _orderable = value;
                    OnPropertyChanged("Orderable", value, oldValue);
                }
            }
        } // Orderable

        protected bool _required;
        public virtual bool Required
        {
            get
            {
                return _required;
            }
            set
            {
                if (_required != value)
                {
                    bool oldValue = _required;
                    _required = value;
                    OnPropertyChanged("Required", value, oldValue);
                }
            }
        } // Required

        protected enumUpdatability _updatability;
        public virtual enumUpdatability Updatability
        {
            get
            {
                return _updatability;
            }
            set
            {
                if (_updatability != value)
                {
                    var oldValue = _updatability;
                    _updatability = value;
                    OnPropertyChanged("Updatability", value, oldValue);
                }
            }
        } // Updatability

    }
}