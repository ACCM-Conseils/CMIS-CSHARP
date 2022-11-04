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
   /// see cmisRepositoryCapabilitiesType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisRepositoryCapabilitiesType : Serialization.XmlSerializable
    {

        public cmisRepositoryCapabilitiesType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisRepositoryCapabilitiesType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisRepositoryCapabilitiesType, string>> _setter = new Dictionary<string, Action<cmisRepositoryCapabilitiesType, string>>() { }; // _setter

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
            _capabilityACL = ReadEnum(reader, attributeOverrides, "capabilityACL", Constants.Namespaces.cmis, _capabilityACL);
            _capabilityAllVersionsSearchable = Read(reader, attributeOverrides, "capabilityAllVersionsSearchable", Constants.Namespaces.cmis, _capabilityAllVersionsSearchable);
            _capabilityChanges = ReadEnum(reader, attributeOverrides, "capabilityChanges", Constants.Namespaces.cmis, _capabilityChanges);
            _capabilityContentStreamUpdatability = ReadEnum(reader, attributeOverrides, "capabilityContentStreamUpdatability", Constants.Namespaces.cmis, _capabilityContentStreamUpdatability);
            _capabilityGetDescendants = Read(reader, attributeOverrides, "capabilityGetDescendants", Constants.Namespaces.cmis, _capabilityGetDescendants);
            _capabilityGetFolderTree = Read(reader, attributeOverrides, "capabilityGetFolderTree", Constants.Namespaces.cmis, _capabilityGetFolderTree);
            _capabilityOrderBy = ReadEnum(reader, attributeOverrides, "capabilityOrderBy", Constants.Namespaces.cmis, _capabilityOrderBy);
            _capabilityMultifiling = Read(reader, attributeOverrides, "capabilityMultifiling", Constants.Namespaces.cmis, _capabilityMultifiling);
            _capabilityPWCSearchable = Read(reader, attributeOverrides, "capabilityPWCSearchable", Constants.Namespaces.cmis, _capabilityPWCSearchable);
            _capabilityPWCUpdatable = Read(reader, attributeOverrides, "capabilityPWCUpdatable", Constants.Namespaces.cmis, _capabilityPWCUpdatable);
            _capabilityQuery = ReadEnum(reader, attributeOverrides, "capabilityQuery", Constants.Namespaces.cmis, _capabilityQuery);
            _capabilityRenditions = ReadEnum(reader, attributeOverrides, "capabilityRenditions", Constants.Namespaces.cmis, _capabilityRenditions);
            _capabilityUnfiling = Read(reader, attributeOverrides, "capabilityUnfiling", Constants.Namespaces.cmis, _capabilityUnfiling);
            _capabilityVersionSpecificFiling = Read(reader, attributeOverrides, "capabilityVersionSpecificFiling", Constants.Namespaces.cmis, _capabilityVersionSpecificFiling);
            _capabilityJoin = ReadEnum(reader, attributeOverrides, "capabilityJoin", Constants.Namespaces.cmis, _capabilityJoin);
            _capabilityCreatablePropertyTypes = Read(reader, attributeOverrides, "capabilityCreatablePropertyTypes", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisCreatablePropertyTypesType>);
            _capabilityNewTypeSettableAttributes = Read(reader, attributeOverrides, "capabilityNewTypeSettableAttributes", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisNewTypeSettableAttributes>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "capabilityACL", Constants.Namespaces.cmis, _capabilityACL.GetName());
            WriteElement(writer, attributeOverrides, "capabilityAllVersionsSearchable", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityAllVersionsSearchable));
            WriteElement(writer, attributeOverrides, "capabilityChanges", Constants.Namespaces.cmis, _capabilityChanges.GetName());
            WriteElement(writer, attributeOverrides, "capabilityContentStreamUpdatability", Constants.Namespaces.cmis, _capabilityContentStreamUpdatability.GetName());
            WriteElement(writer, attributeOverrides, "capabilityGetDescendants", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityGetDescendants));
            WriteElement(writer, attributeOverrides, "capabilityGetFolderTree", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityGetFolderTree));
            WriteElement(writer, attributeOverrides, "capabilityOrderBy", Constants.Namespaces.cmis, _capabilityOrderBy.GetName());
            WriteElement(writer, attributeOverrides, "capabilityMultifiling", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityMultifiling));
            WriteElement(writer, attributeOverrides, "capabilityPWCSearchable", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityPWCSearchable));
            WriteElement(writer, attributeOverrides, "capabilityPWCUpdatable", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityPWCUpdatable));
            WriteElement(writer, attributeOverrides, "capabilityQuery", Constants.Namespaces.cmis, _capabilityQuery.GetName());
            WriteElement(writer, attributeOverrides, "capabilityRenditions", Constants.Namespaces.cmis, _capabilityRenditions.GetName());
            WriteElement(writer, attributeOverrides, "capabilityUnfiling", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityUnfiling));
            WriteElement(writer, attributeOverrides, "capabilityVersionSpecificFiling", Constants.Namespaces.cmis, CommonFunctions.Convert(_capabilityVersionSpecificFiling));
            WriteElement(writer, attributeOverrides, "capabilityJoin", Constants.Namespaces.cmis, _capabilityJoin.GetName());
            WriteElement(writer, attributeOverrides, "capabilityCreatablePropertyTypes", Constants.Namespaces.cmis, _capabilityCreatablePropertyTypes);
            WriteElement(writer, attributeOverrides, "capabilityNewTypeSettableAttributes", Constants.Namespaces.cmis, _capabilityNewTypeSettableAttributes);
        }
        #endregion

        protected enumCapabilityACL _capabilityACL;
        public virtual enumCapabilityACL CapabilityACL
        {
            get
            {
                return _capabilityACL;
            }
            set
            {
                if (_capabilityACL != value)
                {
                    var oldValue = _capabilityACL;
                    _capabilityACL = value;
                    OnPropertyChanged("CapabilityACL", value, oldValue);
                }
            }
        } // CapabilityACL

        protected bool _capabilityAllVersionsSearchable;
        public virtual bool CapabilityAllVersionsSearchable
        {
            get
            {
                return _capabilityAllVersionsSearchable;
            }
            set
            {
                if (_capabilityAllVersionsSearchable != value)
                {
                    bool oldValue = _capabilityAllVersionsSearchable;
                    _capabilityAllVersionsSearchable = value;
                    OnPropertyChanged("CapabilityAllVersionsSearchable", value, oldValue);
                }
            }
        } // CapabilityAllVersionsSearchable

        protected enumCapabilityChanges _capabilityChanges;
        public virtual enumCapabilityChanges CapabilityChanges
        {
            get
            {
                return _capabilityChanges;
            }
            set
            {
                if (_capabilityChanges != value)
                {
                    var oldValue = _capabilityChanges;
                    _capabilityChanges = value;
                    OnPropertyChanged("CapabilityChanges", value, oldValue);
                }
            }
        } // CapabilityChanges

        protected enumCapabilityContentStreamUpdates _capabilityContentStreamUpdatability;
        public virtual enumCapabilityContentStreamUpdates CapabilityContentStreamUpdatability
        {
            get
            {
                return _capabilityContentStreamUpdatability;
            }
            set
            {
                if (_capabilityContentStreamUpdatability != value)
                {
                    var oldValue = _capabilityContentStreamUpdatability;
                    _capabilityContentStreamUpdatability = value;
                    OnPropertyChanged("CapabilityContentStreamUpdatability", value, oldValue);
                }
            }
        } // CapabilityContentStreamUpdatability

        protected cmisCreatablePropertyTypesType _capabilityCreatablePropertyTypes;
        public virtual cmisCreatablePropertyTypesType CapabilityCreatablePropertyTypes
        {
            get
            {
                return _capabilityCreatablePropertyTypes;
            }
            set
            {
                if (!ReferenceEquals(value, _capabilityCreatablePropertyTypes))
                {
                    var oldValue = _capabilityCreatablePropertyTypes;
                    _capabilityCreatablePropertyTypes = value;
                    OnPropertyChanged("CapabilityCreatablePropertyTypes", value, oldValue);
                }
            }
        } // CapabilityCreatablePropertyTypes

        protected bool _capabilityGetDescendants;
        public virtual bool CapabilityGetDescendants
        {
            get
            {
                return _capabilityGetDescendants;
            }
            set
            {
                if (_capabilityGetDescendants != value)
                {
                    bool oldValue = _capabilityGetDescendants;
                    _capabilityGetDescendants = value;
                    OnPropertyChanged("CapabilityGetDescendants", value, oldValue);
                }
            }
        } // CapabilityGetDescendants

        protected bool _capabilityGetFolderTree;
        public virtual bool CapabilityGetFolderTree
        {
            get
            {
                return _capabilityGetFolderTree;
            }
            set
            {
                if (_capabilityGetFolderTree != value)
                {
                    bool oldValue = _capabilityGetFolderTree;
                    _capabilityGetFolderTree = value;
                    OnPropertyChanged("CapabilityGetFolderTree", value, oldValue);
                }
            }
        } // CapabilityGetFolderTree

        protected enumCapabilityJoin _capabilityJoin;
        public virtual enumCapabilityJoin CapabilityJoin
        {
            get
            {
                return _capabilityJoin;
            }
            set
            {
                if (_capabilityJoin != value)
                {
                    var oldValue = _capabilityJoin;
                    _capabilityJoin = value;
                    OnPropertyChanged("CapabilityJoin", value, oldValue);
                }
            }
        } // CapabilityJoin

        protected bool _capabilityMultifiling;
        public virtual bool CapabilityMultifiling
        {
            get
            {
                return _capabilityMultifiling;
            }
            set
            {
                if (_capabilityMultifiling != value)
                {
                    bool oldValue = _capabilityMultifiling;
                    _capabilityMultifiling = value;
                    OnPropertyChanged("CapabilityMultifiling", value, oldValue);
                }
            }
        } // CapabilityMultifiling

        protected cmisNewTypeSettableAttributes _capabilityNewTypeSettableAttributes;
        public virtual cmisNewTypeSettableAttributes CapabilityNewTypeSettableAttributes
        {
            get
            {
                return _capabilityNewTypeSettableAttributes;
            }
            set
            {
                if (!ReferenceEquals(value, _capabilityNewTypeSettableAttributes))
                {
                    var oldValue = _capabilityNewTypeSettableAttributes;
                    _capabilityNewTypeSettableAttributes = value;
                    OnPropertyChanged("CapabilityNewTypeSettableAttributes", value, oldValue);
                }
            }
        } // CapabilityNewTypeSettableAttributes

        protected enumCapabilityOrderBy _capabilityOrderBy;
        public virtual enumCapabilityOrderBy CapabilityOrderBy
        {
            get
            {
                return _capabilityOrderBy;
            }
            set
            {
                if (_capabilityOrderBy != value)
                {
                    var oldValue = _capabilityOrderBy;
                    _capabilityOrderBy = value;
                    OnPropertyChanged("CapabilityOrderBy", value, oldValue);
                }
            }
        } // CapabilityOrderBy

        protected bool _capabilityPWCSearchable;
        public virtual bool CapabilityPWCSearchable
        {
            get
            {
                return _capabilityPWCSearchable;
            }
            set
            {
                if (_capabilityPWCSearchable != value)
                {
                    bool oldValue = _capabilityPWCSearchable;
                    _capabilityPWCSearchable = value;
                    OnPropertyChanged("CapabilityPWCSearchable", value, oldValue);
                }
            }
        } // CapabilityPWCSearchable

        protected bool _capabilityPWCUpdatable;
        public virtual bool CapabilityPWCUpdatable
        {
            get
            {
                return _capabilityPWCUpdatable;
            }
            set
            {
                if (_capabilityPWCUpdatable != value)
                {
                    bool oldValue = _capabilityPWCUpdatable;
                    _capabilityPWCUpdatable = value;
                    OnPropertyChanged("CapabilityPWCUpdatable", value, oldValue);
                }
            }
        } // CapabilityPWCUpdatable

        protected enumCapabilityQuery _capabilityQuery;
        public virtual enumCapabilityQuery CapabilityQuery
        {
            get
            {
                return _capabilityQuery;
            }
            set
            {
                if (_capabilityQuery != value)
                {
                    var oldValue = _capabilityQuery;
                    _capabilityQuery = value;
                    OnPropertyChanged("CapabilityQuery", value, oldValue);
                }
            }
        } // CapabilityQuery

        protected enumCapabilityRendition _capabilityRenditions;
        public virtual enumCapabilityRendition CapabilityRenditions
        {
            get
            {
                return _capabilityRenditions;
            }
            set
            {
                if (_capabilityRenditions != value)
                {
                    var oldValue = _capabilityRenditions;
                    _capabilityRenditions = value;
                    OnPropertyChanged("CapabilityRenditions", value, oldValue);
                }
            }
        } // CapabilityRenditions

        protected bool _capabilityUnfiling;
        public virtual bool CapabilityUnfiling
        {
            get
            {
                return _capabilityUnfiling;
            }
            set
            {
                if (_capabilityUnfiling != value)
                {
                    bool oldValue = _capabilityUnfiling;
                    _capabilityUnfiling = value;
                    OnPropertyChanged("CapabilityUnfiling", value, oldValue);
                }
            }
        } // CapabilityUnfiling

        protected bool _capabilityVersionSpecificFiling;
        public virtual bool CapabilityVersionSpecificFiling
        {
            get
            {
                return _capabilityVersionSpecificFiling;
            }
            set
            {
                if (_capabilityVersionSpecificFiling != value)
                {
                    bool oldValue = _capabilityVersionSpecificFiling;
                    _capabilityVersionSpecificFiling = value;
                    OnPropertyChanged("CapabilityVersionSpecificFiling", value, oldValue);
                }
            }
        } // CapabilityVersionSpecificFiling

    }
}