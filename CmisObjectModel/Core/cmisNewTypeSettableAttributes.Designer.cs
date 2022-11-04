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
   /// see cmisNewTypeSettableAttributes
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisNewTypeSettableAttributes : Serialization.XmlSerializable
    {

        public cmisNewTypeSettableAttributes()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisNewTypeSettableAttributes(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisNewTypeSettableAttributes, string>> _setter = new Dictionary<string, Action<cmisNewTypeSettableAttributes, string>>() { }; // _setter

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
            _id = Read(reader, attributeOverrides, "id", Constants.Namespaces.cmis, _id);
            _localName = Read(reader, attributeOverrides, "localName", Constants.Namespaces.cmis, _localName);
            _localNamespace = Read(reader, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, _localNamespace);
            _displayName = Read(reader, attributeOverrides, "displayName", Constants.Namespaces.cmis, _displayName);
            _queryName = Read(reader, attributeOverrides, "queryName", Constants.Namespaces.cmis, _queryName);
            _description = Read(reader, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
            _creatable = Read(reader, attributeOverrides, "creatable", Constants.Namespaces.cmis, _creatable);
            _fileable = Read(reader, attributeOverrides, "fileable", Constants.Namespaces.cmis, _fileable);
            _queryable = Read(reader, attributeOverrides, "queryable", Constants.Namespaces.cmis, _queryable);
            _fulltextIndexed = Read(reader, attributeOverrides, "fulltextIndexed", Constants.Namespaces.cmis, _fulltextIndexed);
            _includedInSupertypeQuery = Read(reader, attributeOverrides, "includedInSupertypeQuery", Constants.Namespaces.cmis, _includedInSupertypeQuery);
            _controllablePolicy = Read(reader, attributeOverrides, "controllablePolicy", Constants.Namespaces.cmis, _controllablePolicy);
            _controllableACL = Read(reader, attributeOverrides, "controllableACL", Constants.Namespaces.cmis, _controllableACL);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "id", Constants.Namespaces.cmis, CommonFunctions.Convert(_id));
            WriteElement(writer, attributeOverrides, "localName", Constants.Namespaces.cmis, CommonFunctions.Convert(_localName));
            WriteElement(writer, attributeOverrides, "localNamespace", Constants.Namespaces.cmis, CommonFunctions.Convert(_localNamespace));
            WriteElement(writer, attributeOverrides, "displayName", Constants.Namespaces.cmis, CommonFunctions.Convert(_displayName));
            WriteElement(writer, attributeOverrides, "queryName", Constants.Namespaces.cmis, CommonFunctions.Convert(_queryName));
            WriteElement(writer, attributeOverrides, "description", Constants.Namespaces.cmis, CommonFunctions.Convert(_description));
            WriteElement(writer, attributeOverrides, "creatable", Constants.Namespaces.cmis, CommonFunctions.Convert(_creatable));
            WriteElement(writer, attributeOverrides, "fileable", Constants.Namespaces.cmis, CommonFunctions.Convert(_fileable));
            WriteElement(writer, attributeOverrides, "queryable", Constants.Namespaces.cmis, CommonFunctions.Convert(_queryable));
            WriteElement(writer, attributeOverrides, "fulltextIndexed", Constants.Namespaces.cmis, CommonFunctions.Convert(_fulltextIndexed));
            WriteElement(writer, attributeOverrides, "includedInSupertypeQuery", Constants.Namespaces.cmis, CommonFunctions.Convert(_includedInSupertypeQuery));
            WriteElement(writer, attributeOverrides, "controllablePolicy", Constants.Namespaces.cmis, CommonFunctions.Convert(_controllablePolicy));
            WriteElement(writer, attributeOverrides, "controllableACL", Constants.Namespaces.cmis, CommonFunctions.Convert(_controllableACL));
        }
        #endregion

        protected bool _controllableACL;
        public virtual bool ControllableACL
        {
            get
            {
                return _controllableACL;
            }
            set
            {
                if (_controllableACL != value)
                {
                    bool oldValue = _controllableACL;
                    _controllableACL = value;
                    OnPropertyChanged("ControllableACL", value, oldValue);
                }
            }
        } // ControllableACL

        protected bool _controllablePolicy;
        public virtual bool ControllablePolicy
        {
            get
            {
                return _controllablePolicy;
            }
            set
            {
                if (_controllablePolicy != value)
                {
                    bool oldValue = _controllablePolicy;
                    _controllablePolicy = value;
                    OnPropertyChanged("ControllablePolicy", value, oldValue);
                }
            }
        } // ControllablePolicy

        protected bool _creatable;
        public virtual bool Creatable
        {
            get
            {
                return _creatable;
            }
            set
            {
                if (_creatable != value)
                {
                    bool oldValue = _creatable;
                    _creatable = value;
                    OnPropertyChanged("Creatable", value, oldValue);
                }
            }
        } // Creatable

        protected bool _description;
        public virtual bool Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    bool oldValue = _description;
                    _description = value;
                    OnPropertyChanged("Description", value, oldValue);
                }
            }
        } // Description

        protected bool _displayName;
        public virtual bool DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName != value)
                {
                    bool oldValue = _displayName;
                    _displayName = value;
                    OnPropertyChanged("DisplayName", value, oldValue);
                }
            }
        } // DisplayName

        protected bool _fileable;
        public virtual bool Fileable
        {
            get
            {
                return _fileable;
            }
            set
            {
                if (_fileable != value)
                {
                    bool oldValue = _fileable;
                    _fileable = value;
                    OnPropertyChanged("Fileable", value, oldValue);
                }
            }
        } // Fileable

        protected bool _fulltextIndexed;
        public virtual bool FulltextIndexed
        {
            get
            {
                return _fulltextIndexed;
            }
            set
            {
                if (_fulltextIndexed != value)
                {
                    bool oldValue = _fulltextIndexed;
                    _fulltextIndexed = value;
                    OnPropertyChanged("FulltextIndexed", value, oldValue);
                }
            }
        } // FulltextIndexed

        protected bool _id;
        public virtual bool Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    bool oldValue = _id;
                    _id = value;
                    OnPropertyChanged("Id", value, oldValue);
                }
            }
        } // Id

        protected bool _includedInSupertypeQuery;
        public virtual bool IncludedInSupertypeQuery
        {
            get
            {
                return _includedInSupertypeQuery;
            }
            set
            {
                if (_includedInSupertypeQuery != value)
                {
                    bool oldValue = _includedInSupertypeQuery;
                    _includedInSupertypeQuery = value;
                    OnPropertyChanged("IncludedInSupertypeQuery", value, oldValue);
                }
            }
        } // IncludedInSupertypeQuery

        protected bool _localName;
        public virtual bool LocalName
        {
            get
            {
                return _localName;
            }
            set
            {
                if (_localName != value)
                {
                    bool oldValue = _localName;
                    _localName = value;
                    OnPropertyChanged("LocalName", value, oldValue);
                }
            }
        } // LocalName

        protected bool _localNamespace;
        public virtual bool LocalNamespace
        {
            get
            {
                return _localNamespace;
            }
            set
            {
                if (_localNamespace != value)
                {
                    bool oldValue = _localNamespace;
                    _localNamespace = value;
                    OnPropertyChanged("LocalNamespace", value, oldValue);
                }
            }
        } // LocalNamespace

        protected bool _queryable;
        public virtual bool Queryable
        {
            get
            {
                return _queryable;
            }
            set
            {
                if (_queryable != value)
                {
                    bool oldValue = _queryable;
                    _queryable = value;
                    OnPropertyChanged("Queryable", value, oldValue);
                }
            }
        } // Queryable

        protected bool _queryName;
        public virtual bool QueryName
        {
            get
            {
                return _queryName;
            }
            set
            {
                if (_queryName != value)
                {
                    bool oldValue = _queryName;
                    _queryName = value;
                    OnPropertyChanged("QueryName", value, oldValue);
                }
            }
        } // QueryName

    }
}