using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
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
using CmisObjectModel.Constants;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Extensions.Data
{
    [sxs.XmlRoot("converterDefinition", Namespace = Namespaces.com)]
    [cac("com:converterDefinition", null, "converterDefinition")]
    [Attributes.JavaScriptConverter(typeof(JSON.Extensions.Data.ConverterDefinitionConverter))]
    public class ConverterDefinition : Extension
    {

        public ConverterDefinition()
        {
        }

        public ConverterDefinition(string propertyDefinitionId, params ConverterDefinitionItem[] items)
        {
            if (items is not null && items.Length > 0)
            {
                _items.AddRange(items);
            }
            _propertyDefinitionId = propertyDefinitionId;
        }

        public ConverterDefinition(string propertyDefinitionId, string nullValueMapping, params ConverterDefinitionItem[] items) : this(propertyDefinitionId, items)
        {
            _nullValueMapping = nullValueMapping;
        }

        public ConverterDefinition(string propertyDefinitionId, string nullValueMapping, string converterIdentifier, params ConverterDefinitionItem[] items) : this(propertyDefinitionId, nullValueMapping, items)
        {
            _converterIdentifier = converterIdentifier;
        }

        #region Helper classes
        /// <summary>
      /// Base class to create a PropertyValueConverter
      /// </summary>
      /// <remarks></remarks>
        private abstract class ConverterFactory
        {

            #region Constructors
            private static Dictionary<Type, Dictionary<Type, ConverterFactory>> _factories = new Dictionary<Type, Dictionary<Type, ConverterFactory>>();
            public static ConverterFactory CreateInstance(Type localType, Type remoteType)
            {
                lock (_factories)
                {
                    Dictionary<Type, ConverterFactory> factories;

                    if (_factories.ContainsKey(localType))
                    {
                        factories = _factories[localType];
                    }
                    else
                    {
                        factories = new Dictionary<Type, ConverterFactory>();
                        _factories.Add(localType, factories);
                    }
                    if (factories.ContainsKey(remoteType))
                    {
                        return factories[remoteType];
                    }
                    else
                    {
                        var genericTypeDefinition = typeof(ConverterFactoryType<string, string>).GetGenericTypeDefinition();
                        var genericType = genericTypeDefinition.MakeGenericType(localType, remoteType);
                        var ci = genericType.GetConstructor(new Type[] { });
                        ConverterFactory retVal = (ConverterFactory)ci.Invoke(new object[] { });

                        factories.Add(remoteType, retVal);
                        return retVal;
                    }
                }
            }
            #endregion

            #region Helper classes
            /// <summary>
         /// Factory for typesafe converters
         /// </summary>
         /// <typeparam name="TLocal"></typeparam>
         /// <typeparam name="TRemote"></typeparam>
         /// <remarks></remarks>
            private class ConverterFactoryType<TLocal, TRemote> : ConverterFactory
            {

                /// <summary>
            /// Creates a typesafe converter
            /// </summary>
            /// <param name="Definition"></param>
            /// <returns></returns>
            /// <remarks></remarks>
                public override CmisObjectModel.Data.PropertyValueConverter CreateConverter(ConverterDefinition definition)
                {
                    var map = new Dictionary<TLocal, TRemote>();
                    var nullValueMapping = default(CmisObjectModel.Common.Generic.Nullable<TRemote>);

                    if (!string.IsNullOrEmpty(definition._nullValueMapping))
                    {
                        nullValueMapping = new CmisObjectModel.Common.Generic.Nullable<TRemote>(Conversions.ToGenericParameter<TRemote>(definition._nullValueMapping));
                    }
                    foreach (ConverterDefinitionItem item in definition._items)
                    {
                        try
                        {
                            map.Add(Conversions.ToGenericParameter<TLocal>(item.Key), Conversions.ToGenericParameter<TRemote>(item.Value));
                        }
                        catch
                        {
                        }
                    }

                    return new CmisObjectModel.Data.Generic.PropertyValueConverter<TLocal, TRemote>(map, nullValueMapping);
                }
            }
            #endregion

            public abstract CmisObjectModel.Data.PropertyValueConverter CreateConverter(ConverterDefinition definition);
        }
        #endregion

        #region IXmlSerializable
        private static Dictionary<string, Action<ConverterDefinition, string>> _setter = new Dictionary<string, Action<ConverterDefinition, string>>() { { "converteridentifier", SetConverterIdentifier }, { "propertydefinitionid", SetPropertyDefinitionId } }; // _setter

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

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _localType = ReadOptionalEnum(reader, attributeOverrides, "localType", _localType);
            _remoteType = ReadOptionalEnum(reader, attributeOverrides, "remoteType", _remoteType);
            _nullValueMapping = Read(reader, attributeOverrides, "nullValueMapping", null);

            var items = ReadArray(reader, attributeOverrides, "item", Namespaces.com, GenericXmlSerializableFactory<ConverterDefinitionItem>);
            if (items is null || items.Length == 0)
            {
                _items = new List<ConverterDefinitionItem>();
            }
            else
            {
                _items = new List<ConverterDefinitionItem>(items);
            }
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (!string.IsNullOrEmpty(_converterIdentifier))
                WriteAttribute(writer, attributeOverrides, "converterIdentifier", null, _converterIdentifier);
            WriteAttribute(writer, attributeOverrides, "propertyDefinitionId", null, _propertyDefinitionId);
            if (_localType.HasValue)
                WriteElement(writer, attributeOverrides, "localType", Namespaces.com, _localType.Value.GetName());
            if (_remoteType.HasValue)
                WriteElement(writer, attributeOverrides, "remoteType", Namespaces.com, _remoteType.Value.GetName());
            if (!string.IsNullOrEmpty(_nullValueMapping))
                WriteElement(writer, attributeOverrides, "nullValueMapping", Namespaces.com, _nullValueMapping);
            if (_items.Count > 0)
                WriteArray(writer, attributeOverrides, "item", Namespaces.com, _items.ToArray());
        }
        #endregion

        #region Converter by identifier
        private static Dictionary<string, Func<ConverterDefinitionItem[], CmisObjectModel.Data.PropertyValueConverter>> _converterFactories = new Dictionary<string, Func<ConverterDefinitionItem[], CmisObjectModel.Data.PropertyValueConverter>>();

        /// <summary>
      /// Returns a registered factory if available, otherwise null
      /// </summary>
      /// <param name="instance"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static Func<ConverterDefinitionItem[], CmisObjectModel.Data.PropertyValueConverter> GetStoredFactory(ConverterDefinition instance)
        {
            lock (_converterFactories)
            {
                if (!string.IsNullOrEmpty(instance._converterIdentifier) && _converterFactories.ContainsKey(instance._converterIdentifier))
                {
                    return _converterFactories[instance._converterIdentifier];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
      /// Registers a factory for converterIdentifier
      /// </summary>
      /// <param name="converterIdentifier"></param>
      /// <param name="factory"></param>
      /// <remarks></remarks>
        public static void RegisterConverterFactory(string converterIdentifier, Func<ConverterDefinitionItem[], CmisObjectModel.Data.PropertyValueConverter> factory)
        {
            lock (_converterFactories)
            {
                if (!(string.IsNullOrEmpty(converterIdentifier) || _converterFactories.ContainsKey(converterIdentifier)))
                {
                    _converterFactories.Add(converterIdentifier, factory);
                }
            }
        }

        /// <summary>
      /// Unregisters a factory for converterIdentifier
      /// </summary>
      /// <param name="converterIdentifier"></param>
      /// <param name="factory"></param>
      /// <remarks></remarks>
        public static void UnRegisterConverterFactory(string converterIdentifier, Func<ConverterDefinitionItem[], CmisObjectModel.Data.PropertyValueConverter> factory)
        {
            lock (_converterFactories)
            {
                var storedFactory = string.IsNullOrEmpty(converterIdentifier) || !_converterFactories.ContainsKey(converterIdentifier) ? null : _converterFactories[converterIdentifier];
                if (storedFactory is not null && factory is not null && ReferenceEquals(storedFactory.Target, factory.Target) && storedFactory.Method.MethodHandle.Value.Equals(factory.Method.MethodHandle.Value))
                {
                    _converterFactories.Remove(converterIdentifier);
                }
            }
        }
        #endregion

        /// <summary>
      /// Adds a new item to the items collection
      /// </summary>
      /// <param name="item"></param>
      /// <remarks></remarks>
        public void Add(ConverterDefinitionItem item)
        {
            if (item is not null)
            {
                _items.Add(item);
                OnPropertyChanged("Items");
            }
        }

        private string _converterIdentifier;
        public string ConverterIdentifier
        {
            get
            {
                return _converterIdentifier;
            }
            set
            {
                if ((_converterIdentifier ?? "") != (value ?? ""))
                {
                    string oldValue = _converterIdentifier;
                    _converterIdentifier = value;
                    OnPropertyChanged("ConverterIdentifier", value, oldValue);
                }
            }
        } // ConverterIdentifier
        private static void SetConverterIdentifier(ConverterDefinition instance, string value)
        {
            instance._converterIdentifier = value;
        }

        /// <summary>
      /// Creates a converter depending on local and remote type
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObjectModel.Data.PropertyValueConverter CreateConverter()
        {
            var factory = GetStoredFactory(this);
            return factory is null ? ConverterFactory.CreateInstance(GetLocalType(), GetRemoteType()).CreateConverter(this) : factory(_items.ToArray());
        }

        public Type GetLocalType()
        {
            return GetType(_localType);
        }
        public Type GetRemoteType()
        {
            return GetType(_remoteType);
        }
        private Type GetType(enumConverterSupportedTypes? type)
        {
            switch (type.HasValue ? type.Value : enumConverterSupportedTypes.@string)
            {
                case enumConverterSupportedTypes.boolean:
                    {
                        return typeof(bool);
                    }
                case enumConverterSupportedTypes.@decimal:
                    {
                        return CommonFunctions.DecimalRepresentation == enumDecimalRepresentation.@decimal ? typeof(decimal) : typeof(double);
                    }
                case enumConverterSupportedTypes.integer:
                    {
                        return typeof(long);
                    }

                default:
                    {
                        return typeof(string);
                    }
            }
        }

        private List<ConverterDefinitionItem> _items = new List<ConverterDefinitionItem>();
        public ConverterDefinitionItem[] Items
        {
            get
            {
                return _items.ToArray();
            }
            private set
            {
                _items.Clear();
                if (value is not null && value.Length > 0)
                    _items.AddRange(value);
                OnPropertyChanged("Items");
            }
        } // Items

        private enumConverterSupportedTypes? _localType;
        public enumConverterSupportedTypes? LocalType
        {
            get
            {
                return _localType;
            }
            set
            {
                if (!_localType.Equals(value))
                {
                    var oldValue = _localType;
                    _localType = value;
                    OnPropertyChanged("LocalType", value, oldValue);
                }
            }
        } // LocalType

        private string _nullValueMapping;
        public string NullValueMapping
        {
            get
            {
                return _nullValueMapping;
            }
            set
            {
                if ((_nullValueMapping ?? "") != (value ?? ""))
                {
                    string oldValue = _nullValueMapping;
                    _nullValueMapping = value;
                    OnPropertyChanged("NullValueMapping", value, oldValue);
                }
            }
        } // NullValueMapping

        protected string _propertyDefinitionId;
        public virtual string PropertyDefinitionId
        {
            get
            {
                return _propertyDefinitionId;
            }
            set
            {
                if ((_propertyDefinitionId ?? "") != (value ?? ""))
                {
                    string oldValue = _propertyDefinitionId;
                    _propertyDefinitionId = value;
                    OnPropertyChanged("PropertyDefinitionId", value, oldValue);
                }
            }
        } // PropertyDefinitionId
        private static void SetPropertyDefinitionId(ConverterDefinition instance, string value)
        {
            instance._propertyDefinitionId = value;
        }

        private enumConverterSupportedTypes? _remoteType;
        public enumConverterSupportedTypes? RemoteType
        {
            get
            {
                return _remoteType;
            }
            set
            {
                if (!_remoteType.Equals(value))
                {
                    var oldValue = _remoteType;
                    _remoteType = value;
                    OnPropertyChanged("RemoteType", value, oldValue);
                }
            }
        } // RemoteType

        /// <summary>
      /// Removes the specified item from the items collection
      /// </summary>
      /// <param name="item"></param>
      /// <remarks></remarks>
        public void RemoveItem(ConverterDefinitionItem item)
        {
            if (item is not null && _items.Remove(item))
                OnPropertyChanged("Items");
        }
    }
}