using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Properties
{
    /// <summary>
   /// Implementation of a propertyDefinition for cmisPropertyDouble
   /// </summary>
   /// <remarks>In CMIS specification there is no cmisPropertyDoubleDefinitionType defined, but some CMIS servers
   /// (i.e. alfresco) send double values instead of decimal values</remarks>
    [sxs.XmlRoot(cmisPropertyDecimalDefinitionType.DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyDoubleDefinitionType : Generic.cmisPropertyDefinitionType<double, Choices.cmisChoiceDouble, Core.Properties.cmisPropertyDouble>
    {

        public cmisPropertyDoubleDefinitionType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyDoubleDefinitionType(bool? initClassSupported) : base(initClassSupported)
        {
        }
        public cmisPropertyDoubleDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, params Choices.cmisChoiceDouble[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, choices)
        {
        }
        public cmisPropertyDoubleDefinitionType(string id, string localName, string localNamespace, string displayName, string queryName, bool required, bool inherited, bool queryable, bool orderable, enumCardinality cardinality, enumUpdatability updatability, Core.Properties.cmisPropertyDouble defaultValue, params Choices.cmisChoiceDouble[] choices) : base(id, localName, localNamespace, displayName, queryName, required, inherited, queryable, orderable, cardinality, updatability, defaultValue, choices)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyDoubleDefinitionType";
        public const string TargetTypeName = Core.Properties.cmisPropertyDouble.CmisTypeName;
        public const string DefaultElementName = "propertyDoubleDefinition";
        #endregion

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPropertyDoubleDefinitionType, string>> _setter = new Dictionary<string, Action<cmisPropertyDoubleDefinitionType, string>>() { }; // _setter

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
            base.ReadXmlCore(reader, attributeOverrides);
            _defaultValue = Read(reader, attributeOverrides, "defaultValue", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Core.Properties.cmisPropertyDouble>);
            _maxValue = Read(reader, attributeOverrides, "maxValue", Constants.Namespaces.cmis, _maxValue);
            _minValue = Read(reader, attributeOverrides, "minValue", Constants.Namespaces.cmis, _minValue);
            _precision = ReadOptionalEnum(reader, attributeOverrides, "precision", Constants.Namespaces.cmis, _precision);
            _choices = ReadArray(reader, attributeOverrides, "choice", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Choices.cmisChoiceDouble>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.WriteXmlCore(writer, attributeOverrides);
            WriteElement(writer, attributeOverrides, "defaultValue", Constants.Namespaces.cmis, _defaultValue);
            if (_maxValue.HasValue)
                WriteElement(writer, attributeOverrides, "maxValue", Constants.Namespaces.cmis, CommonFunctions.Convert(_maxValue));
            if (_minValue.HasValue)
                WriteElement(writer, attributeOverrides, "minValue", Constants.Namespaces.cmis, CommonFunctions.Convert(_minValue));
            if (_precision.HasValue)
                WriteElement(writer, attributeOverrides, "precision", Constants.Namespaces.cmis, _precision.Value.GetName());
            WriteArray(writer, attributeOverrides, "choice", Constants.Namespaces.cmis, _choices);
        }
        #endregion

        protected double? _maxValue;
        public virtual double? MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (!_maxValue.Equals(value))
                {
                    var oldValue = _maxValue;
                    _maxValue = value;
                    OnPropertyChanged("MaxValue", value, oldValue);
                }
            }
        } // MaxValue

        protected double? _minValue;
        public virtual double? MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (!_minValue.Equals(value))
                {
                    var oldValue = _minValue;
                    _minValue = value;
                    OnPropertyChanged("MinValue", value, oldValue);
                }
            }
        } // MinValue

        protected enumDecimalPrecision? _precision;
        public virtual enumDecimalPrecision? Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                if (!_precision.Equals(value))
                {
                    var oldValue = _precision;
                    _precision = value;
                    OnPropertyChanged("Precision", value, oldValue);
                }
            }
        } // Precision

        protected override enumPropertyType _propertyType
        {
            get
            {
                return enumPropertyType.@decimal;
            }
        }

    }
}