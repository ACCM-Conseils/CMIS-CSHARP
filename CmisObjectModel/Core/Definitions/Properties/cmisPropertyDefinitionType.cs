using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ccg = CmisObjectModel.Collections.Generic;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// *
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
using ccc = CmisObjectModel.Core.Choices;
using cccg = CmisObjectModel.Core.Choices.Generic;
using ccp = CmisObjectModel.Core.Properties;
using cjcdp = CmisObjectModel.JSON.Core.Definitions.Properties;
using cjcdpg = CmisObjectModel.JSON.Core.Definitions.Properties.Generic;
using cjs = CmisObjectModel.JSON.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Core.Definitions.Properties
{
    [Attributes.JavaScriptObjectResolver(typeof(cjs.CmisPropertyDefinitionResolver))]
    [Attributes.JavaScriptConverter(typeof(cjcdp.cmisPropertyDefinitionTypeConverter))]
    public partial class cmisPropertyDefinitionType : Contracts.IPropertyDefinition
    {

        #region Constructors
        protected cmisPropertyDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability) : base(id, localName, localNamespace, displayName, queryName, queryable)
        {
            Required = required;
            Inherited = inherited;
            Cardinality = cardinality;
            Updatability = updatability;
            Orderable = orderable;
        }

        /// <summary>
      /// Creates a new instance suitable for the current node of the reader
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks>
      /// Node "propertyType" is responsible for type of returned PropertyDefinition-instance
      /// </remarks>
        public static cmisPropertyDefinitionType CreateInstance(System.Xml.XmlReader reader)
        {
            // first chance: from current node name
            string nodeName = CommonFunctions.GetCurrentStartElementLocalName(reader);
            if (!string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeName.ToLowerInvariant();
                if (_factories.ContainsKey(nodeName))
                {
                    cmisPropertyDefinitionType retVal = _factories[nodeName].CreateInstance() as cmisPropertyDefinitionType;

                    if (retVal is not null)
                    {
                        retVal.ReadXml(reader);
                        return retVal;
                    }
                }
                else if (nodeName == "cmisproperty" || nodeName == "propertydefinition" || nodeName == "cmispropertydefinitiontype")
                {
                    // second chance: child element named 'propertyType'
                    return CreateInstance<cmisPropertyDefinitionType>(reader, "propertyType");
                }
            }

            // unable to interpret node as cmisproperty
            return null;
        }
        #endregion

        #region IPropertyDefinition mirrored properties
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private enumCardinality IPropertyDefinition_Cardinality
        {
            get
            {
                return _cardinality;
            }
            set
            {
                Cardinality = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        enumCardinality Contracts.IPropertyDefinition.Cardinality { get => IPropertyDefinition_Cardinality; set => IPropertyDefinition_Cardinality = value; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private bool? IPropertyDefinition_Inherited
        {
            get
            {
                return _inherited;
            }
            set
            {
                Inherited = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool? Contracts.IPropertyDefinition.Inherited { get => IPropertyDefinition_Inherited; set => IPropertyDefinition_Inherited = value; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private bool? IPropertyDefinition_OpenChoice
        {
            get
            {
                return _openChoice;
            }
            set
            {
                OpenChoice = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool? Contracts.IPropertyDefinition.OpenChoice { get => IPropertyDefinition_OpenChoice; set => IPropertyDefinition_OpenChoice = value; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private bool IPropertyDefinition_Orderable
        {
            get
            {
                return _orderable;
            }
            set
            {
                Orderable = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool Contracts.IPropertyDefinition.Orderable { get => IPropertyDefinition_Orderable; set => IPropertyDefinition_Orderable = value; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private bool IPropertyDefinition_Required
        {
            get
            {
                return _required;
            }
            set
            {
                Required = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        bool Contracts.IPropertyDefinition.Required { get => IPropertyDefinition_Required; set => IPropertyDefinition_Required = value; }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private enumUpdatability IPropertyDefinition_Updatability
        {
            get
            {
                return _updatability;
            }
            set
            {
                Updatability = value;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        enumUpdatability Contracts.IPropertyDefinition.Updatability { get => IPropertyDefinition_Updatability; set => IPropertyDefinition_Updatability = value; }
        #endregion

        #region IXmlSerialization
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            // support browser binding properties
            get_ExtendedProperties().Add(Constants.ExtendedProperties.Cardinality, _cardinality.GetName());
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Compare(e.PropertyName, "Cardinality", true) == 0)
            {
                var extendedProperties = get_ExtendedProperties();

                if (extendedProperties.ContainsKey(Constants.ExtendedProperties.Cardinality))
                {
                    extendedProperties[Constants.ExtendedProperties.Cardinality] = _cardinality.GetName();
                }
                else
                {
                    extendedProperties.Add(Constants.ExtendedProperties.Cardinality, _cardinality.GetName());
                }
            }
            base.OnPropertyChanged(e);
        }
        #endregion

        /// <summary>
      /// Creates a property-instance for this property-definition
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public abstract ccp.cmisProperty CreateProperty();
        public abstract ccp.cmisProperty CreateProperty(params object[] values);

        public ccc.cmisChoice[] Choices
        {
            get
            {
                return ChoicesCore;
            }
            set
            {
                ChoicesCore = value;
            }
        }
        protected static ccc.cmisChoice[] ChoicesCore { get; set; }

        private ccg.ArrayMapper<cmisPropertyDefinitionType, ccc.cmisChoice> initial_choicesAsReadOnly() => new ccg.ArrayMapper<cmisPropertyDefinitionType, ccc.cmisChoice>(this, "Choices", () => ChoicesCore, "DisplayName", choice => choice.DisplayName);

        private ccg.ArrayMapper<cmisPropertyDefinitionType, ccc.cmisChoice> _choicesAsReadOnly;
        /// <summary>
      /// Access to choices via index or DisplayName
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg.ArrayMapper<cmisPropertyDefinitionType, ccc.cmisChoice> ChoicesAsReadOnly
        {
            get
            {
                return _choicesAsReadOnly;
            }
        }

        public abstract Type ChoiceType { get; }
        public abstract Type CreatePropertyResultType { get; }

        public ccp.cmisProperty DefaultValue
        {
            get
            {
                return DefaultValueCore;
            }
            set
            {
                DefaultValueCore = value;
            }
        }
        protected abstract ccp.cmisProperty DefaultValueCore { get; set; }

        public enumPropertyType PropertyType
        {
            get
            {
                return _propertyType;
            }
        }
        protected abstract enumPropertyType _propertyType { get; }

        public abstract Type PropertyValueType { get; }

    }

    namespace Generic
    {
        /// <summary>
      /// Generic version of cmisPropertyDefinitionType
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <typeparam name="TChoice"></typeparam>
      /// <typeparam name="TDefaultValue"></typeparam>
      /// <remarks>Baseclass of all typesafe cmisPropertyDefinitionType-classes</remarks>
        [Attributes.JavaScriptConverter(typeof(cjcdpg.cmisPropertyDefinitionTypeConverter<bool, ccc.cmisChoiceBoolean, ccp.cmisPropertyBoolean, cmisPropertyBooleanDefinitionType>), "{\"\":\"TPropertyDefinition\",\"TProperty\":\"TProperty\",\"TChoice\":\"TChoice\",\"TDefaultValue\":\"TDefaultValue\"}")]
        public abstract class cmisPropertyDefinitionType<TProperty, TChoice, TDefaultValue> : cmisPropertyDefinitionType
where TChoice : cccg.cmisChoice<TProperty, TChoice>, new()
where TDefaultValue : ccp.Generic.cmisProperty<TProperty>, new()
        {

            protected cmisPropertyDefinitionType()
            {

                _choicesAsReadOnly = new ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }
            /// <summary>
         /// this constructor is only used if derived classes from this class needs an InitClass()-call
         /// </summary>
         /// <param name="initClassSupported"></param>
         /// <remarks></remarks>
            protected cmisPropertyDefinitionType(bool? initClassSupported) : base(initClassSupported)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
            }
            protected cmisPropertyDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params TChoice[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
                _choices = choices;
            }
            protected cmisPropertyDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, TDefaultValue defaultValue, params TChoice[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability)
            {
                _choicesAsReadOnly = new ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice>(this, "Choices", () => _choices, "DisplayName", choice => choice.DisplayName);
                _defaultValue = defaultValue;
                _choices = choices;
            }

            /// <summary>
         /// Creates a property-instance for this property definition
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public override ccp.cmisProperty CreateProperty()
            {
                var retVal = new TDefaultValue()
                {
                    Cardinality = Cardinality,
                    DisplayName = DisplayName,
                    LocalName = LocalName,
                    PropertyDefinition = this,
                    PropertyDefinitionId = Id,
                    QueryName = QueryName
                };
                if (_extendedProperties is not null)
                {
                    var extendedProperties = retVal.get_ExtendedProperties();
                    foreach (KeyValuePair<string, object> de in _extendedProperties)
                    {
                        if (!extendedProperties.ContainsKey(de.Key))
                            extendedProperties.Add(de.Key, de.Value);
                    }
                }
                return retVal;
            }
            public override ccp.cmisProperty CreateProperty(params object[] values)
            {
                if (values is null || values.Length == 0)
                {
                    return CreateProperty();
                }
                else
                {
                    var retVal = new TDefaultValue()
                    {
                        Cardinality = Cardinality,
                        DisplayName = DisplayName,
                        LocalName = LocalName,
                        PropertyDefinition = this,
                        PropertyDefinitionId = Id,
                        QueryName = QueryName,
                        Values = (from value in values
                                  where value is null || value is TProperty
                                  select (Conversions.ToGenericParameter<TProperty>(value))).ToArray()
                    };
                    if (_extendedProperties is not null)
                    {
                        var extendedProperties = retVal.get_ExtendedProperties();
                        foreach (KeyValuePair<string, object> de in _extendedProperties)
                        {
                            if (!extendedProperties.ContainsKey(de.Key))
                                extendedProperties.Add(de.Key, de.Value);
                        }
                    }
                    return retVal;
                }
            }

            protected TChoice[] _choices;
            public virtual new TChoice[] Choices
            {
                get
                {
                    return _choices;
                }
                set
                {
                    if (!ReferenceEquals(value, _choices))
                    {
                        var oldValue = _choices;
                        _choices = value;
                        OnPropertyChanged("Choices", value, oldValue);
                    }
                }
            } // Choices
            protected virtual ccc.cmisChoice[] ChoicesCore
            {
                get
                {
                    if (_choices is null)
                    {
                        return null;
                    }
                    else
                    {
                        return (from choice in _choices
                                select choice).ToArray();
                    }
                }
                set
                {
                    if (value is null)
                    {
                        Choices = null;
                    }
                    else
                    {
                        Choices = (from choice in value
                                   where choice is TChoice
                                   select ((TChoice)choice)).ToArray();
                    }
                }
            }

            private ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice> _choicesAsReadOnly;
            /// <summary>
         /// Access to choices via index or DisplayName
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks></remarks>
            public new ccg.ArrayMapper<cmisPropertyDefinitionType, TChoice> ChoicesAsReadOnly
            {
                get
                {
                    return _choicesAsReadOnly;
                }
            }

            public override Type ChoiceType
            {
                get
                {
                    return typeof(TChoice);
                }
            }

            public override Type CreatePropertyResultType
            {
                get
                {
                    return typeof(TDefaultValue);
                }
            }

            protected TDefaultValue _defaultValue;
            public virtual new TDefaultValue DefaultValue
            {
                get
                {
                    return _defaultValue;
                }
                set
                {
                    if (!ReferenceEquals(value, _defaultValue))
                    {
                        var oldValue = _defaultValue;
                        _defaultValue = value;
                        OnPropertyChanged("DefaultValue", value, oldValue);
                    }
                }
            } // DefaultValue
            protected override ccp.cmisProperty DefaultValueCore
            {
                get
                {
                    return _defaultValue;
                }
                set
                {
                    DefaultValue = value is TDefaultValue ? (TDefaultValue)value : null;
                }
            }

            public override Type PropertyValueType
            {
                get
                {
                    return typeof(TProperty);
                }
            }

            public override string ToString()
            {
                return typeof(TProperty).Name + "/" + Id;
            }

        }
    }
}