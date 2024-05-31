using System;
using System.Collections.Generic;
using sc = System.ComponentModel;
using System.Data;
using System.Linq;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
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
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;

namespace CmisObjectModel.Core.Properties
{
    [Attributes.JavaScriptObjectResolver(typeof(JSON.Serialization.CmisPropertyResolver))]
    public partial class cmisProperty : IComparable, IComparable<cmisProperty>
    {

        #region Constructors
        static cmisProperty()
        {
            // search for all types supporting cmisProperties ...
            if (!ExploreAssembly(typeof(cmisProperty).Assembly))
            {
                // ... failed.
                // At least register well-known cmisProperty-classes
                cac.ExploreTypes(_factories, _genericTypeDefinition, typeof(cmisPropertyBoolean), typeof(cmisPropertyDateTime), typeof(cmisPropertyDecimal), typeof(cmisPropertyDouble), typeof(cmisPropertyHtml), typeof(cmisPropertyId), typeof(cmisPropertyInteger), typeof(cmisPropertyObject), typeof(cmisPropertyString), typeof(cmisPropertyUri));
                // update the FromType()-support
                ExploreFactories();
            }

            CommonFunctions.DecimalRepresentationChanged += OnDecimalRepresentationChanged;
            if (CommonFunctions.DecimalRepresentation != enumDecimalRepresentation.@decimal)
            {
                OnDecimalRepresentationChanged(CommonFunctions.DecimalRepresentation);
            }
        }
        protected cmisProperty(string propertyDefinitionId, string localName, string displayName, string queryName)
        {
            PropertyDefinitionId = propertyDefinitionId;
            LocalName = localName;
            DisplayName = displayName;
            QueryName = queryName;
        }

        /// <summary>
      /// Creates a CmisProperty-instance from the current node of the reader-object using the
      /// name of the current node to determine the suitable type
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static cmisProperty CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();

            string nodeName = reader.LocalName;
            if (!string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeName.ToLowerInvariant();
                if (_factories.ContainsKey(nodeName))
                {
                    var retVal = _factories[nodeName].CreateInstance();

                    if (retVal is not null)
                    {
                        retVal.ReadXml(reader);
                        return retVal;
                    }
                }
            }

            // current node doesn't describe a CmisProperty-instance
            return null;
        }

        /// <summary>
      /// Searches in assembly for types supporting cmisProperties
      /// </summary>
      /// <param name="assembly"></param>
      /// <remarks></remarks>
        public static bool ExploreAssembly(System.Reflection.Assembly assembly)
        {
            try
            {
                // explore the complete assembly if possible
                if (assembly is not null)
                    cac.ExploreTypes(_factories, _genericTypeDefinition, assembly.GetTypes());
                // update FromType-support
                ExploreFactories();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
      /// Updates the FromType() support
      /// </summary>
      /// <remarks></remarks>
        private static void ExploreFactories()
        {
            try
            {
                var factories = new HashSet<Common.Generic.Factory<cmisProperty>>(_fromTypeFactories.Values);
                Action<Common.Generic.Factory<cmisProperty>> addMethod = factory =>
     {
         var property = factories.Add(factory) ? factory.CreateInstance() : null;
         var propertyType = property is null ? null : property.PropertyType;

         if (!(propertyType is null || _fromTypeFactories.ContainsKey(propertyType)))
         {
             _fromTypeFactories.Add(propertyType, factory);
             _fromTypeFactories.Add(propertyType.MakeArrayType(), factory);
         }
     };

                // preferred factories
                foreach (string priorityFactory in _priorityFactories)
                {
                    if (_factories.ContainsKey(priorityFactory))
                        addMethod.Invoke(_factories[priorityFactory]);
                }
                // check the rest
                foreach (Common.Generic.Factory<cmisProperty> factory in _factories.Values)
                    addMethod.Invoke(factory);
                // support for DateTime,Int32/Int64-class
                if (!_fromTypeFactories.ContainsKey(typeof(DateTime)) && _fromTypeFactories.ContainsKey(typeof(DateTimeOffset)))
                {
                    _fromTypeFactories.Add(typeof(DateTime), _fromTypeFactories[typeof(DateTimeOffset)]);
                    _fromTypeFactories.Add(typeof(DateTime[]), _fromTypeFactories[typeof(DateTimeOffset[])]);
                }
                if (!_fromTypeFactories.ContainsKey(typeof(int)) && _fromTypeFactories.ContainsKey(typeof(long)))
                {
                    _fromTypeFactories.Add(typeof(int), _fromTypeFactories[typeof(long)]);
                    _fromTypeFactories.Add(typeof(int[]), _fromTypeFactories[typeof(long[])]);
                }
                if (!_fromTypeFactories.ContainsKey(typeof(long)) && _fromTypeFactories.ContainsKey(typeof(int)))
                {
                    _fromTypeFactories.Add(typeof(long), _fromTypeFactories[typeof(int)]);
                    _fromTypeFactories.Add(typeof(long[]), _fromTypeFactories[typeof(int[])]);
                }
            }
            catch
            {
            }
        }

        protected static Dictionary<string, Common.Generic.Factory<cmisProperty>> _factories = new Dictionary<string, Common.Generic.Factory<cmisProperty>>();
        protected static Dictionary<Type, Common.Generic.Factory<cmisProperty>> _fromTypeFactories = new Dictionary<Type, Common.Generic.Factory<cmisProperty>>();

        /// <summary>
      /// Returns a cmisProperty-instance for given type
      /// </summary>
        public static cmisProperty FromType(Type type)
        {
            while (type is not null)
            {
                if (_fromTypeFactories.ContainsKey(type))
                {
                    return _fromTypeFactories[type].CreateInstance();
                }
                else
                {
                    type = type.BaseType;
                }
            }

            // no cmisProperty can represent this type
            return null;
        }

        /// <summary>
      /// GetType(Generic.Factory(Of CmisProperty, TDerivedFromCmisProperty))
      /// </summary>
      /// <remarks></remarks>
        private static Type _genericTypeDefinition = typeof(Common.Generic.Factory<cmisProperty, cmisPropertyBoolean>).GetGenericTypeDefinition();
        private static string[] _priorityFactories = new string[] { cmisPropertyString.CmisTypeName.ToLowerInvariant(), cmisPropertyString.DefaultElementName.ToLowerInvariant(), cmisPropertyString.TargetTypeName.ToLowerInvariant() };
        #endregion

        #region INotifyPropertiesChanged
        public event ExtendedPropertyChangedEventHandler ExtendedPropertyChanged;

        public delegate void ExtendedPropertyChangedEventHandler(object sender, sc.PropertyChangedEventArgs e);
        protected void OnExtendedPropertyChanged(string propertyName)
        {
            ExtendedPropertyChanged?.Invoke(this, new sc.PropertyChangedEventArgs(propertyName));
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            OnExtendedPropertyChanged(propertyName);
            base.OnPropertyChanged(propertyName);
        }
        protected override void OnPropertyChanged<TProperty>(string propertyName, TProperty newValue, TProperty oldValue)
        {
            OnExtendedPropertyChanged(propertyName, newValue, oldValue);
            base.OnPropertyChanged(propertyName, newValue, oldValue);
        }
        protected void OnExtendedPropertyChanged<TProperty>(string propertyName, TProperty newValue, TProperty oldValue)
        {
            ExtendedPropertyChanged?.Invoke(this, propertyName.ToPropertyChangedEventArgs(newValue, oldValue));
        }
        #endregion

        #region IComparable
        public static int Compare(cmisProperty first, cmisProperty second)
        {
            if (ReferenceEquals(first, second))
            {
                return 0;
            }
            else if (first is null)
            {
                return -1;
            }
            else if (second is null)
            {
                return 1;
            }
            else
            {
                return first.CompareTo(second);
            }
        }
        protected abstract int CompareTo(object other);
        int IComparable.CompareTo(object other) => CompareTo(other);
        protected abstract int CompareTo(cmisProperty other);
        int IComparable<cmisProperty>.CompareTo(cmisProperty other) => CompareTo(other);
        #endregion

        /// <summary>
      /// Cardinality returns the correct value if this instance of a cmisProperty is created by a cmisPropertyDefinitionType-instance.
      /// If this instance has been created during deserialization the returned value is enumCardinality.multi (default) until the property
      /// is changed by custom code.
      /// </summary>
        public enumCardinality Cardinality { get; set; } = enumCardinality.multi;

        private static void OnDecimalRepresentationChanged(enumDecimalRepresentation newValue)
        {
            var attrs = typeof(cmisPropertyDecimal).GetCustomAttributes(typeof(cac), false);
            var attr = attrs is not null && attrs.Length > 0 ? (cac)attrs[0] : null;

            switch (newValue)
            {
                case enumDecimalRepresentation.@decimal:
                    {
                        cac.AppendTypeFactory(_factories, _genericTypeDefinition, typeof(cmisPropertyDecimal), attr);
                        break;
                    }
                case enumDecimalRepresentation.@double:
                    {
                        cac.AppendTypeFactory(_factories, _genericTypeDefinition, typeof(cmisPropertyDouble), attr);
                        break;
                    }
            }
        }

        public abstract Type PropertyType { get; }

        protected Definitions.Properties.cmisPropertyDefinitionType _propertyDefinition;
        /// <summary>
      /// The cmisPropertyDefinitionType-instance created this cmisPropertyType
      /// (the value is null if the current instance is created by deserialization)
      /// </summary>
        public Definitions.Properties.cmisPropertyDefinitionType PropertyDefinition
        {
            get
            {
                return _propertyDefinition;
            }
            set
            {
                _propertyDefinition = value;
            }
        }

        public abstract enumPropertyType Type { get; }

        public object Value
        {
            get
            {
                return ValueCore;
            }
            set
            {
                ValueCore = value;
            }
        }
        protected abstract object ValueCore { get; set; }
        /// <summary>
      /// Sets Value without raising the PropertyChanged-Event
      /// </summary>
      /// <param name="value"></param>
      /// <remarks></remarks>
        public abstract object SetValueSilent(object value);
        public object[] Values
        {
            get
            {
                return ValuesCore;
            }
            set
            {
                ValuesCore = value;
            }
        }
        protected abstract object[] ValuesCore { get; set; }
        /// <summary>
      /// Sets Values without raising the PropertyChanged-Event
      /// </summary>
      /// <param name="values"></param>
      /// <remarks></remarks>
        public abstract object[] SetValuesSilent(object[] values);

    }

    /// <summary>
   /// Base-class for all string containing properties avoids null values within property Values
   /// </summary>
   /// <remarks>Null values result in a failure when doing a query from the apache workbench</remarks>
    public abstract class cmisPropertyStringBase : Generic.cmisProperty<string>
    {

        protected cmisPropertyStringBase() : base()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPropertyStringBase(bool? initClassSupported) : base(initClassSupported)
        {
        }
        protected cmisPropertyStringBase(string propertyDefinitionId, string localName, string displayName, string queryName, params string[] values) : base(propertyDefinitionId, localName, displayName, queryName)
        {
            _values = values;
        }

        public override string[] Values
        {
            get
            {
                return base.Values;
            }
            set
            {
                if (value is null)
                {
                    base.Values = value;
                }
                else
                {
                    base.Values = (from currentValue in value
                                   select currentValue ?? string.Empty).ToArray();
                }
            }
        }

    }

    namespace Generic
    {
        /// <summary>
      /// Generic version of cmisProperty-class
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <remarks>Baseclass of all typesafe cmisProperty-classes</remarks>
        public abstract class cmisProperty<TProperty> : cmisProperty, IComparable<cmisProperty<TProperty>>
        {

            protected cmisProperty() : base()
            {
            }
            /// <summary>
         /// this constructor is only used if derived classes from this class needs an InitClass()-call
         /// </summary>
         /// <param name="initClassSupported"></param>
         /// <remarks></remarks>
            protected cmisProperty(bool? initClassSupported) : base(initClassSupported)
            {
            }
            protected cmisProperty(string propertyDefinitionId, string localName, string displayName, string queryName, params TProperty[] values) : base(propertyDefinitionId, localName, displayName, queryName)
            {
                _values = values;
            }

            #region IComparable
            /// <summary>
         /// Compares the content of the current instance with the content of the other-instance
         /// </summary>
         /// <param name="other"></param>
         /// <returns></returns>
         /// <remarks>comparisons only defined for following other-types:
         /// cmisProperty(Of TProperty), TProperty, IEnumerable(Of TProperty)</remarks>
            protected override int CompareTo(object other)
            {
                if (other is null)
                {
                    return 1;
                }
                else if (other is cmisProperty<TProperty>)
                {
                    return CompareTo((cmisProperty<TProperty>)other);
                }
                else if (other is TProperty)
                {
                    return CompareTo(Conversions.ToGenericParameter<TProperty>(other));
                }
                else if (other is IEnumerable<TProperty>)
                {
                    return CompareTo(((IEnumerable<TProperty>)other).ToArray());
                }
                else
                {
                    // unable to compare
                    return 0;
                }
            }
            /// <summary>
         /// Compares the content of the current instance with the content of the other-instance
         /// </summary>
         /// <param name="other"></param>
         /// <returns></returns>
         /// <remarks>comparisons only defined for other-objects of type cmisProperty(Of TProperty)</remarks>
            protected override int CompareTo(cmisProperty other)
            {
                if (other is cmisProperty<TProperty>)
                {
                    return CompareTo((cmisProperty<TProperty>)other);
                }
                else
                {
                    // unable to compare
                    return 0;
                }
            }
            protected int CompareTo(cmisProperty<TProperty> other)
            {
                if (other is null)
                {
                    return 1;
                }
                else if (ReferenceEquals(other, this))
                {
                    return 0;
                }
                else
                {
                    return CompareTo(other._values);
                }
            }

            int IComparable<cmisProperty<TProperty>>.CompareTo(cmisProperty<TProperty> other) => CompareTo(other);
            protected abstract int CompareTo(params TProperty[] other);
            #endregion

            /// <summary>
         /// Returns the runtimetype of the cmisProperty (enumCardinality.single).
         /// Note: if this cmisProperty supports multiple entries (array), the
         /// returned value is the elementType of the arrayType.
         /// </summary>
            public override Type PropertyType
            {
                get
                {
                    return typeof(TProperty);
                }
            }

            public virtual new TProperty Value
            {
                get
                {
                    return _values is null || _values.Length == 0 ? default : _values[0];
                }
                set
                {
                    var oldValue = _values;
                    _values = new TProperty[] { value };
                    OnPropertyChanged("Values", _values, oldValue);
                }
            }
            protected override object ValueCore
            {
                get
                {
                    return Value;
                }
                set
                {
                    Value = Conversions.ToGenericParameter<TProperty>(value is null || value is TProperty ? value : value.TryCastDynamic(typeof(TProperty)));
                }
            }
            /// <summary>
         /// Sets _values without raising the PropertyChanged-Event
         /// </summary>
         /// <param name="value"></param>
         /// <remarks></remarks>
            public override object SetValueSilent(object value)
            {
                object retVal = Value;

                if (value is TProperty || value is null)
                {
                    _values = new TProperty[] { Conversions.ToGenericParameter<TProperty>(value) };
                    OnExtendedPropertyChanged("Value", value, retVal);
                }
                return retVal;
            }

            protected TProperty[] _values;
            public virtual new TProperty[] Values
            {
                get
                {
                    return _values;
                }
                set
                {
                    if (!ReferenceEquals(value, _values))
                    {
                        var oldValue = _values;
                        _values = value;
                        OnPropertyChanged("Values", value, oldValue);
                    }
                }
            } // Values
            protected override object[] ValuesCore
            {
                get
                {
                    if (_values == null)
                        return null;
                    else
                        /*return ((from value in _values
                                select value).Cast<TProperty[]>().ToArray());*/
                        return (from value in _values
                                select (object)value).ToArray();
                }
                set
                {
                    if (value == null)
                        Values = null;
                    else
                        Values = (from rawItem in value
                                  let item = rawItem == null || rawItem is TProperty ? rawItem : (TProperty)(rawItem)
                                  select (TProperty)item).ToArray();
                }
            }
            /// <summary>
            /// Sets _values without raising the PropertyChanged-Event
            /// </summary>
            /// <param name="values"></param>
            /// <remarks></remarks>
            public override object[] SetValuesSilent(object[] values)
            {
                var retVal = ValuesCore;

                _values = (from rawValue in values
                           let value = rawValue is null || rawValue is TProperty ? rawValue : rawValue.TryCastDynamic(typeof(TProperty))
                           select (Conversions.ToGenericParameter<TProperty>(value))).ToArray();
                OnExtendedPropertyChanged("Values", ValuesCore, retVal);
                return retVal;
            }

            public override string ToString()
            {
                return typeof(TProperty).Name + "/" + _propertyDefinitionId + " = " + (_values is null ? "null" : string.Join(", ", _values));
            }

        }
    }
}