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
   /// see cmisObjectType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisObjectType : Serialization.XmlSerializable
    {

        public cmisObjectType() : base(true)
        {
            _properties = new Collections.cmisPropertiesType();
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisObjectType(bool? initClassSupported) : base(initClassSupported)
        {
            _properties = new Collections.cmisPropertiesType();
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisObjectType, string>> _setter = new Dictionary<string, Action<cmisObjectType, string>>() { }; // _setter

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
            _properties = Read(reader, attributeOverrides, "properties", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Collections.cmisPropertiesType>) ?? new Collections.cmisPropertiesType();
            _allowableActions = Read(reader, attributeOverrides, "allowableActions", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisAllowableActionsType>);
            _relationships = ReadArray(reader, attributeOverrides, "relationship", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisObjectType>);
            _changeEventInfo = Read(reader, attributeOverrides, "changeEventInfo", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisChangeEventType>);
            _acl = Read(reader, attributeOverrides, "acl", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Security.cmisAccessControlListType>);
            _exactACL = Read(reader, attributeOverrides, "exactACL", Constants.Namespaces.cmis, _exactACL);
            _policyIds = Read(reader, attributeOverrides, "policyIds", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Collections.cmisListOfIdsType>);
            _renditions = ReadArray(reader, attributeOverrides, "rendition", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisRenditionType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "properties", Constants.Namespaces.cmis, _properties);
            WriteElement(writer, attributeOverrides, "allowableActions", Constants.Namespaces.cmis, _allowableActions);
            WriteArray(writer, attributeOverrides, "relationship", Constants.Namespaces.cmis, _relationships);
            WriteElement(writer, attributeOverrides, "changeEventInfo", Constants.Namespaces.cmis, _changeEventInfo);
            WriteElement(writer, attributeOverrides, "acl", Constants.Namespaces.cmis, _acl);
            if (_exactACL.HasValue)
                WriteElement(writer, attributeOverrides, "exactACL", Constants.Namespaces.cmis, CommonFunctions.Convert(_exactACL));
            WriteElement(writer, attributeOverrides, "policyIds", Constants.Namespaces.cmis, _policyIds);
            WriteArray(writer, attributeOverrides, "rendition", Constants.Namespaces.cmis, _renditions);
        }
        #endregion

        protected Security.cmisAccessControlListType _acl;
        public virtual Security.cmisAccessControlListType Acl
        {
            get
            {
                return _acl;
            }
            set
            {
                if (!ReferenceEquals(value, _acl))
                {
                    var oldValue = _acl;
                    _acl = value;
                    OnPropertyChanged("Acl", value, oldValue);
                }
            }
        } // Acl

        protected cmisAllowableActionsType _allowableActions;
        public virtual cmisAllowableActionsType AllowableActions
        {
            get
            {
                return _allowableActions;
            }
            set
            {
                if (!ReferenceEquals(value, _allowableActions))
                {
                    var oldValue = _allowableActions;
                    _allowableActions = value;
                    OnPropertyChanged("AllowableActions", value, oldValue);
                }
            }
        } // AllowableActions

        protected cmisChangeEventType _changeEventInfo;
        public virtual cmisChangeEventType ChangeEventInfo
        {
            get
            {
                return _changeEventInfo;
            }
            set
            {
                if (!ReferenceEquals(value, _changeEventInfo))
                {
                    var oldValue = _changeEventInfo;
                    _changeEventInfo = value;
                    OnPropertyChanged("ChangeEventInfo", value, oldValue);
                }
            }
        } // ChangeEventInfo

        protected bool? _exactACL;
        public virtual bool? ExactACL
        {
            get
            {
                return _exactACL;
            }
            set
            {
                if (!_exactACL.Equals(value))
                {
                    var oldValue = _exactACL;
                    _exactACL = value;
                    OnPropertyChanged("ExactACL", value, oldValue);
                }
            }
        } // ExactACL

        protected Collections.cmisListOfIdsType _policyIds;
        public virtual Collections.cmisListOfIdsType PolicyIds
        {
            get
            {
                return _policyIds;
            }
            set
            {
                if (!ReferenceEquals(value, _policyIds))
                {
                    var oldValue = _policyIds;
                    _policyIds = value;
                    OnPropertyChanged("PolicyIds", value, oldValue);
                }
            }
        } // PolicyIds

        protected Collections.cmisPropertiesType _properties;
        public virtual Collections.cmisPropertiesType Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (!ReferenceEquals(value, _properties))
                {
                    var oldValue = _properties;
                    if (oldValue is not null)
                    {
                        oldValue.PropertyChanged -= xmlSerializable_PropertyChanged;
                    }
                    if (value is null)
                    {
                        _properties = new Collections.cmisPropertiesType();
                    }
                    else
                    {
                        _properties = value;
                    }
                    _properties.PropertyChanged += xmlSerializable_PropertyChanged;
                    OnPropertyChanged("Properties", value, oldValue);
                }
            }
        } // Properties

        protected static cmisObjectType[] _relationships;
        public virtual cmisObjectType[] Relationships
        {
            get
            {
                return _relationships;
            }
            set
            {
                if (!ReferenceEquals(value, _relationships))
                {
                    var oldValue = _relationships;
                    _relationships = value;
                    OnPropertyChanged("Relationships", value, oldValue);
                }
            }
        } // Relationships

        protected cmisRenditionType[] _renditions;
        public virtual cmisRenditionType[] Renditions
        {
            get
            {
                return _renditions;
            }
            set
            {
                if (!ReferenceEquals(value, _renditions))
                {
                    var oldValue = _renditions;
                    _renditions = value;
                    OnPropertyChanged("Renditions", value, oldValue);
                }
            }
        } // Renditions

    }
}