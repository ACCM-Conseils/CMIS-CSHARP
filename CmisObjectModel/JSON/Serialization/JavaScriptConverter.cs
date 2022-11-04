using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using swss = System.Web.Script.Serialization;
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
using cs = CmisObjectModel.Serialization;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Serialization
{
    public abstract class JavaScriptConverter : swss.JavaScriptConverter
    {

        protected JavaScriptConverter(ObjectResolver objectResolver)
        {
            ObjectResolverCore = objectResolver;
        }

        #region Helper-classes
        public interface IExtendedDeserialization
        {
            bool DeserializeProperties(object @object, IDictionary<string, object> dictionary, JavaScriptSerializer serializer);
        }
        #endregion

        /// <summary>
      /// non generic variant
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ObjectResolver ObjectResolver
        {
            get
            {
                return ObjectResolverCore;
            }
        }
        protected abstract ObjectResolver ObjectResolverCore { get; set; }

    }

    namespace Generic
    {
        /// <summary>
      /// Baseclass for typesafe JavaScriptConverter with overloaded serialization-methods
      /// </summary>
      /// <typeparam name="TSerializable"></typeparam>
      /// <remarks></remarks>
        public abstract class JavaScriptConverter<TSerializable> : JavaScriptConverter<TSerializable, ObjectResolver<TSerializable>>, JavaScriptConverter.IExtendedDeserialization where TSerializable : cs.XmlSerializable
        {

            #region Constructors
            protected JavaScriptConverter(ObjectResolver<TSerializable> objectResolver) : base(objectResolver)
            {
            }
            #endregion

            /// <summary>
         /// Converts dictionary to a XmlSerializable-instance of type SupportedType
         /// </summary>
         /// <param name="serializer">MUST be a CmisObjectModelLibrary.JSON.JavaScriptSerializer</param>
         /// <param name="type">ignored, should be nothing</param>
            public sealed override object Deserialize(IDictionary<string, object> dictionary, Type type, swss.JavaScriptSerializer serializer)
            {
                if (dictionary is null)
                {
                    return null;
                }
                else
                {
                    var retVal = _objectResolver.CreateInstance(dictionary);
                    if (retVal is not null)
                    {
                        var context = new SerializationContext(retVal, dictionary, (JavaScriptSerializer)serializer);
                        Deserialize(context);
                    }
                    return retVal;
                }
            }
            protected abstract void Deserialize(SerializationContext context);
            public bool DeserializeProperties(object @object, IDictionary<string, object> dictionary, JavaScriptSerializer serializer)
            {
                if (@object is TSerializable)
                {
                    var context = new SerializationContext((TSerializable)@object, dictionary, serializer);
                    Deserialize(context);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
         /// Converts XmlSerializable-instance to IDictionary(Of String, Object)
         /// </summary>
         /// <param name="serializer">MUST be a CmisObjectModelLibrary.JSON.JavaScriptSerializer</param>
            public sealed override IDictionary<string, object> Serialize(object obj, swss.JavaScriptSerializer serializer)
            {
                if (obj is null)
                {
                    return null;
                }
                else
                {
                    var retVal = new Dictionary<string, object>();
                    var context = new SerializationContext((TSerializable)obj, retVal, (JavaScriptSerializer)serializer);
                    Serialize(context);
                    return retVal;
                }
            }
            protected abstract void Serialize(SerializationContext context);
        }
        /// <summary>
      /// Baseclass for typesafe JavaScriptConverter
      /// </summary>
      /// <typeparam name="TSerializable"></typeparam>
      /// <remarks></remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public abstract class JavaScriptConverter<TSerializable, TObjectResolver> : JavaScriptConverter
              where TSerializable : cs.XmlSerializable
              where TObjectResolver : ObjectResolver
        {

            #region Constructors
            protected JavaScriptConverter(TObjectResolver objectResolver) : base(objectResolver)
            {
            }
            #endregion

            #region Helper classes
            /// <summary>
         /// Context for complex Read- and all Write-calls
         /// </summary>
         /// <remarks></remarks>
            protected class SerializationContext
            {
                public SerializationContext(TSerializable @object, IDictionary<string, object> dictionary, JavaScriptSerializer serializer)
                {
                    Object = @object;
                    Dictionary = dictionary;
                    Serializer = serializer;
                }

                public void Add(string key, object value)
                {
                    Dictionary.Add(key, value);
                }
                public readonly IDictionary<string, object> Dictionary;
                public readonly TSerializable Object;
                public readonly JavaScriptSerializer Serializer;
            }
            #endregion

            /// <summary>
         /// Returns the value-property from value or, if not set, returns insteadOfNull
         /// </summary>
            protected TStructure NVL<TStructure>(TStructure? value, TStructure insteadOfNull = default) where TStructure : struct
            {
                return value.HasValue ? value.Value : insteadOfNull;
            }

            protected TObjectResolver _objectResolver;
            protected override ObjectResolver ObjectResolverCore
            {
                get
                {
                    return _objectResolver;
                }
                set
                {
                    if (_objectResolver is null)
                        _objectResolver = value as TObjectResolver;
                }
            }
            public new TObjectResolver ObjectResolver
            {
                get
                {
                    return _objectResolver;
                }
                set
                {
                    if (_objectResolver is null)
                        _objectResolver = value;
                }
            }

            /// <summary>
         /// Reads datetime
         /// </summary>
         /// <remarks>JSON representation of DateTimeOffset-instance as long</remarks>
            protected virtual DateTimeOffset Read(IDictionary<string, object> dictionary, string propertyName, DateTimeOffset defaultValue)
            {
                if (dictionary.ContainsKey(propertyName))
                {
                    return dictionary[propertyName].TryCastDynamic((long)0).FromJSONTime();
                }
                else
                {
                    return default;
                }
            }

            /// <summary>
         /// Reads simple property from dictionary
         /// </summary>
            protected virtual T Read<T>(IDictionary<string, object> dictionary, string propertyName, T defaultValue)
            {
                if (dictionary.ContainsKey(propertyName))
                {
                    return dictionary[propertyName].TryCastDynamic(defaultValue);
                }
                else
                {
                    return defaultValue;
                }
            }

            /// <summary>
         /// Reads complex property from dictionary
         /// </summary>
            protected virtual T Read<T>(SerializationContext context, string propertyName, T defaultValue) where T : cs.XmlSerializable
            {
                IDictionary<string, object> innerDictionary = null;
                swss.JavaScriptConverter javaScriptConverter = null;

                if (context.Dictionary.ContainsKey(propertyName))
                {
                    // default: dictionary contains the expected property
                    innerDictionary = context.Dictionary[propertyName] as IDictionary<string, object>;
                    if (innerDictionary is not null)
                    {
                        javaScriptConverter = context.Serializer.GetJavaScriptConverter(typeof(T));
                    }
                }
                else
                {
                    // second chance: perhaps there exists an AttributesOverrides-information for the property
                    var elementAttribute = context.Serializer.AttributesOverrides.get_ElementAttribute(SupportedType, propertyName);
                    if (elementAttribute is not null && context.Dictionary.ContainsKey(elementAttribute.AliasName))
                    {
                        innerDictionary = context.Dictionary[elementAttribute.AliasName] as IDictionary<string, object>;
                        if (innerDictionary is not null)
                            javaScriptConverter = elementAttribute.ElementConverter;
                    }
                }

                if (javaScriptConverter is null)
                {
                    return defaultValue;
                }
                else
                {
                    return (T)javaScriptConverter.Deserialize(innerDictionary, typeof(T), context.Serializer);
                }
            }

            /// <summary>
            /// Reads an array of datetime-objects
            /// </summary>
            /// <remarks>JSON representation of DateTimeOffset-instance as long</remarks>
            protected virtual DateTimeOffset[] ReadArray(IDictionary<string, object> dictionary, string propertyName, DateTimeOffset[] defaultValues)
            {
                /*if (dictionary.ContainsKey(propertyName))
                {
                    object values = dictionary[propertyName];

                    if (values == null)
                        return null;
                    else if (values is ICollection)
                        return (from value in (ICollection)values
                                select (DateTimeOffset)Common.TryCastDynamic<long>(value, 0).FromJSONTime()).ToArray();
                    else
                        return new DateTimeOffset[] { TryCastDynamic<long>(values, 0).FromJSONTime() };
                }
                else*/
                    return defaultValues;
            }

            /// <summary>
            /// Reads an array of primitives or strings
            /// </summary>
            protected virtual T[] ReadArray<T>(IDictionary<string, object> dictionary, string propertyName, T[] defaultValues)
            {
                /*if (dictionary.ContainsKey(propertyName))
                {
                    var values = dictionary[propertyName];

                    if (values is null)
                    {
                        return null;
                    }
                    else if (values is ICollection)
                    {
                        return (from value in (ICollection)values
                                select CommonFunctions.TryCastDynamic<T>(value, default(T))).ToArray();
                    }
                    else
                    {
                        return new T[] { values.TryCastDynamic(default(T)) };
                    }
                }
                else
                {*/
                    return defaultValues;
                //}
            }

            /// <summary>
         /// Reads an array of complex types
         /// </summary>
            protected virtual T[] ReadArray<T>(SerializationContext context, string propertyName, T[] defaultValues) where T : cs.XmlSerializable
            {
                swss.JavaScriptConverter javaScriptConverter = null;
                ICollection values = null;
                IDictionary<string, object> emptyDictionary = new Dictionary<string, object>();

                if (context.Dictionary.ContainsKey(propertyName))
                {
                    javaScriptConverter = context.Serializer.GetJavaScriptConverter(typeof(T));
                    values = context.Dictionary[propertyName] as ICollection;
                }
                else
                {
                    // second chance: perhaps there exists an AttributesOverrides-information for the property
                    var elementAttribute = context.Serializer.AttributesOverrides.get_ElementAttribute(SupportedType, propertyName);
                    if (elementAttribute is not null && context.Dictionary.ContainsKey(elementAttribute.AliasName))
                    {
                        values = context.Dictionary[elementAttribute.AliasName] as ICollection;
                        javaScriptConverter = elementAttribute.ElementConverter;
                    }
                }

                if (javaScriptConverter is null)
                {
                    return defaultValues;
                }
                else if (values is null)
                {
                    return null;
                }
                else
                {
                    /*return (from value in values
                            let innerDictionary = value as IDictionary<string, object> ?? emptyDictionary
                            select javaScriptConverter.Deserialize(innerDictionary, typeof(T), context.Serializer).TryCastDynamic<T>(null)).ToArray();*/

                    return defaultValues;
                }
            }

            /// <summary>
         /// Reads enum-property from dictionary
         /// </summary>
            protected virtual TEnum ReadEnum<TEnum>(IDictionary<string, object> dictionary, string propertyName, TEnum defaultValue) where TEnum : struct
            {
                var value = default(TEnum);
                if (dictionary.ContainsKey(propertyName))
                {
                    return CommonFunctions.TryParse(Conversions.ToString(dictionary[propertyName]), ref value, true) ? value : defaultValue;
                }
                else
                {
                    return defaultValue;
                }
            }

            /// <summary>
            /// Reads an array of enums
            /// </summary>
            protected virtual TEnum[] ReadEnumArray<TEnum>(IDictionary<string, object> dictionary, string propertyName, TEnum[] defaultValue) where TEnum : struct
            {
                /*if (dictionary.ContainsKey(propertyName))
                {
                    IEnumerable values = dictionary[propertyName] as IEnumerable;

                    if (values == null)
                        return null;
                    else
                    {
                        TEnum enumValue;

                        return (from value in values select TryParse(System.Convert.ToString(value), enumValue, true) ? enumValue : default(TEnum)).ToArray();
                    }
                }
                else*/
                    return defaultValue;
            }

            /// <summary>
            /// Reads datetime nullable property from dictionary
            /// </summary>
            /// <remarks>JSON representation of DateTimeOffset-instance as long</remarks>
            protected virtual DateTimeOffset? ReadNullable(IDictionary<string, object> dictionary, string propertyName, DateTimeOffset? defaultValue)
            {
                if (dictionary.ContainsKey(propertyName))
                {
                    return Read(dictionary, propertyName, defaultValue.HasValue ? defaultValue.Value : default);
                }
                else
                {
                    return defaultValue;
                }
            }

            /// <summary>
         /// Reads simple nullable property from dictionary
         /// </summary>
            protected virtual T? ReadNullable<T>(IDictionary<string, object> dictionary, string propertyName, T? defaultValue) where T : struct
            {
                if (dictionary.ContainsKey(propertyName))
                {
                    return Read(dictionary, propertyName, defaultValue.HasValue ? defaultValue.Value : default);
                }
                else
                {
                    return defaultValue;
                }
            }

            /// <summary>
         /// Reads optional enum-property from dictionary
         /// </summary>
            protected virtual TEnum? ReadOptionalEnum<TEnum>(IDictionary<string, object> dictionary, string propertyName, TEnum? defaultValue) where TEnum : struct
            {
                var value = default(TEnum);
                if (dictionary.ContainsKey(propertyName))
                {
                    if (CommonFunctions.TryParse(Conversions.ToString(dictionary[propertyName]), ref value, true))
                    {
                        return value;
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }

            public static readonly Type SupportedType = typeof(TSerializable);
            public sealed override IEnumerable<Type> SupportedTypes
            {
                get
                {
                    return new Type[] { SupportedType };
                }
            }

            /// <summary>
         /// Writes a complex property to dictionary
         /// </summary>
            protected virtual void Write(SerializationContext context, cs.XmlSerializable value, string propertyName)
            {
                if (value is not null)
                {
                    var elementAttribute = context.Serializer is null ? null : context.Serializer.AttributesOverrides.get_ElementAttribute(SupportedType, propertyName);

                    if (elementAttribute is null)
                    {
                        var javaScriptConverter = context.Serializer.GetJavaScriptConverter(value.GetType());
                        if (javaScriptConverter is not null)
                            context.Dictionary.Add(propertyName, javaScriptConverter.Serialize(value, context.Serializer));
                    }
                    else
                    {
                        context.Dictionary.Add(elementAttribute.AliasName, elementAttribute.ElementConverter.Serialize(value, context.Serializer));
                    }
                }
            }

            /// <summary>
         /// Writes a datetime
         /// </summary>
         /// <remarks>JSON representation of DateTimeOffset-instance as long</remarks>
            protected virtual void Write(SerializationContext context, DateTimeOffset value, string propertyName)
            {
                if (!DateTime.MinValue.Equals(value.DateTime))
                {
                    context.Dictionary.Add(propertyName, value.DateTime.ToJSONTime());
                }
            }

            /// <summary>
         /// Writes an array of datetime-objects
         /// </summary>
         /// <remarks>JSON representation of DateTimeOffset-instance as long</remarks>
            protected virtual void WriteArray(SerializationContext context, DateTimeOffset[] values, string propertyName)
            {
                if (values is not null)
                {
                    WriteArray(context, (from value in values
                                         select value.DateTime.ToJSONTime()).ToArray(), propertyName);
                }
            }

            /// <summary>
         /// Writes an array to dictionary
         /// </summary>
            protected virtual void WriteArray<TItem>(SerializationContext context, TItem[] values, string propertyName)
            {
                if (values is not null)
                {
                    if (typeof(TItem).IsEnum)
                    {
                        // enums
                        context.Dictionary.Add(propertyName, (from value in values
                                                              select ((Enum)(object)value).GetName()).ToArray());
                    }
                    else if (typeof(cs.XmlSerializable).IsAssignableFrom(typeof(TItem)))
                    {
                        // complex types
                        var elementAttribute = context.Serializer is null ? null : context.Serializer.AttributesOverrides.get_ElementAttribute(SupportedType, propertyName);
                        if (elementAttribute is not null)
                            propertyName = elementAttribute.AliasName;
                        context.Dictionary.Add(propertyName, (from value in values
                                                              let valueType = value is null ? typeof(TItem) : value.GetType()
                                                              let javaScriptConverter = (elementAttribute is null ? null : elementAttribute.ElementConverter) ?? context.Serializer.GetJavaScriptConverter(valueType)
                                                              where javaScriptConverter is not null
                                                              select javaScriptConverter.Serialize(value, context.Serializer)).ToArray());
                    }
                    else if (ReferenceEquals(typeof(string), typeof(TItem)))
                    {
                        context.Add(propertyName, (from rawValue in values
                                                   let value = Conversions.ToString(rawValue)
                                                   select value ?? string.Empty).ToArray());
                    }
                    else
                    {
                        // others
                        context.Dictionary.Add(propertyName, values);
                    }
                }
            }

        }

        /// <summary>
      /// Fallback mechanism: just create the type, but no deserialization/serialization-support
      /// </summary>
      /// <typeparam name="TSerializable"></typeparam>
      /// <remarks>Unspecific converter using reflection to (de-)serialize a XmlSerializable-instance</remarks>
        public class XmlSerializerConverter<TSerializable> : JavaScriptConverter<TSerializable> where TSerializable : cs.XmlSerializable
        {

            public XmlSerializerConverter(ObjectResolver<TSerializable> objectResolver) : base(objectResolver)
            {
            }

            #region Helper classes
            /// <summary>
         /// Helper class to allow access to generic methods of the base class
         /// </summary>
         /// <remarks></remarks>
            private abstract class IOHelper
            {
                public abstract object Read(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue);
                public abstract object ReadArray(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue);
                public void Write(XmlSerializerConverter<TSerializable> caller, SerializationContext context, object value, string propertyName)
                {
                    caller.Write(context, (cs.XmlSerializable)value, propertyName);
                }
                public abstract void WriteArray(XmlSerializerConverter<TSerializable> caller, SerializationContext context, object value, string propertyName);
            }

            /// <summary>
         /// Extension of IOHelper to handle value types
         /// </summary>
         /// <remarks></remarks>
            private abstract class IOValueTypeHelper : IOHelper
            {
                public abstract object GetValue(object value);
                public abstract bool HasValue(object value);
                public abstract object ReadNullable(XmlSerializerConverter<TSerializable> caller, IDictionary<string, object> dictionary, string propertyName, object defaultValue);
            }

            /// <summary>
         /// Used for enum-type
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <remarks></remarks>
            private class IOEnumTypeHelper<T> : IOValueTypeHelper<T> where T : struct
            {

                #region Read
                public override object Read(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.ReadEnum((IDictionary<string, object>)source, propertyName, Conversions.ToGenericParameter<T>(defaultValue));
                }
                public override object ReadArray(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.ReadEnumArray((IDictionary<string, object>)source, propertyName, (T[])defaultValue);
                }
                public override object ReadNullable(XmlSerializerConverter<TSerializable> caller, IDictionary<string, object> dictionary, string propertyName, object defaultValue)
                {
                    return caller.ReadOptionalEnum<T>(dictionary, propertyName, Conversions.ToGenericParameter<T>(defaultValue));
                }
                #endregion
            }

            /// <summary>
         /// Used for value-type types that are not enum-type
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <remarks></remarks>
            private class IOValueTypeHelper<T> : IOValueTypeHelper where T : struct
            {
                public override object GetValue(object value)
                {
                    return (object)Conversions.ToGenericParameter<T>(value);
                }
                public override bool HasValue(object value)
                {
                    try
                    {
                        Conversions.ToGenericParameter<T>(value);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                #region Read
                public override object Read(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.Read((IDictionary<string, object>)source, propertyName, Conversions.ToGenericParameter<T>(defaultValue));
                }
                public override object ReadArray(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.ReadArray((IDictionary<string, object>)source, propertyName, (T[])defaultValue);
                }
                public override object ReadNullable(XmlSerializerConverter<TSerializable> caller, IDictionary<string, object> dictionary, string propertyName, object defaultValue)
                {
                    return caller.ReadNullable<T>(dictionary, propertyName, Conversions.ToGenericParameter<T>(defaultValue));
                }
                #endregion

                #region Write
                public override void WriteArray(XmlSerializerConverter<TSerializable> caller, SerializationContext context, object value, string propertyName)
                {
                    caller.WriteArray(context, (T[])value, propertyName);
                }
                #endregion
            }

            /// <summary>
         /// Used for non value-type types that are not derived from XmlSerializable
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <remarks></remarks>
            private class IOUnspecificTypeHelper<T> : IOHelper
            {
                #region Read
                public override object Read(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.Read((IDictionary<string, object>)source, propertyName, Conversions.ToGenericParameter<T>(defaultValue));
                }
                public override object ReadArray(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.ReadArray((IDictionary<string, object>)source, propertyName, (T[])defaultValue);
                }
                #endregion
                #region Write
                public override void WriteArray(XmlSerializerConverter<TSerializable> caller, SerializationContext context, object value, string propertyName)
                {
                    caller.WriteArray(context, (T[])value, propertyName);
                }
                #endregion
            }

            /// <summary>
         /// Used for XmlSerializable-types
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <remarks></remarks>
            private class IOXmlSerializableTypeHelper<T> : IOHelper where T : cs.XmlSerializable
            {
                #region Read
                public override object Read(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.Read((SerializationContext)source, propertyName, (T)defaultValue);
                }
                public override object ReadArray(XmlSerializerConverter<TSerializable> caller, object source, string propertyName, object defaultValue)
                {
                    return caller.ReadArray((SerializationContext)source, propertyName, (T[])defaultValue);
                }
                #endregion
                #region Write
                public override void WriteArray(XmlSerializerConverter<TSerializable> caller, SerializationContext context, object value, string propertyName)
                {
                    caller.WriteArray(context, (T[])value, propertyName);
                }
                #endregion
            }
            #endregion

            protected override void Deserialize(SerializationContext context)
            {
                var objectType = context.Object.GetType();

                if (!ReferenceEquals(objectType, typeof(TSerializable)))
                {
                    IExtendedDeserialization moreSuitableConverter = context.Serializer.GetJavaScriptConverter(objectType) as IExtendedDeserialization;
                    // a better deserialization method is found
                    if (moreSuitableConverter is not null && !(moreSuitableConverter is XmlSerializerConverter<TSerializable>) && moreSuitableConverter.DeserializeProperties(context.Object, context.Dictionary, context.Serializer))
                        return;
                }

                var props = GetPropertyInfos(objectType, pi => pi.CanWrite);

                foreach (KeyValuePair<string, object> de in context.Dictionary)
                {
                    string key = de.Key.ToLowerInvariant();

                    if (props.ContainsKey(key))
                    {
                        var pi = props[key];
                        var propertyType = pi.PropertyType;
                        var elementType = propertyType.IsArray ? propertyType.GetElementType() : propertyType;
                        var ioHelper = GetOrCreateIOHelper(elementType);
                        var defaultValue = pi.GetValue(context.Object, null);
                        var value = de.Value;

                        if (value is not null)
                        {
                            if (propertyType.IsArray)
                            {
                                if (ReferenceEquals(elementType, typeof(DateTimeOffset)))
                                {
                                    value = ReadArray(context.Dictionary, de.Key, null);
                                }
                                else if (IsXmlSerializableType(elementType))
                                {
                                    value = ioHelper.ReadArray(this, context, de.Key, defaultValue);
                                }
                                else
                                {
                                    value = ioHelper.ReadArray(this, context.Dictionary, de.Key, defaultValue);
                                }
                            }
                            else if (ReferenceEquals(propertyType, typeof(DateTimeOffset)))
                            {
                                value = Read(context.Dictionary, de.Key, default);
                            }
                            else if (propertyType.IsNullableType())
                            {
                                if (ReferenceEquals(propertyType, typeof(DateTimeOffset?)))
                                {
                                    value = ReadNullable(context.Dictionary, de.Key, (DateTimeOffset?)defaultValue);
                                }
                                else
                                {
                                    value = ((IOValueTypeHelper)ioHelper).ReadNullable(this, context.Dictionary, de.Key, defaultValue);
                                }
                            }
                            else if (IsXmlSerializableType(propertyType))
                            {
                                value = ioHelper.Read(this, context, de.Key, defaultValue);
                            }
                            else
                            {
                                value = ioHelper.Read(this, context.Dictionary, de.Key, defaultValue);
                            }
                        }
                        pi.SetValue(context.Object, value, null);
                    }
                }
            }

            /// <summary>
         /// Returns an ioHelper-instance suitable for type
         /// </summary>
            private IOHelper GetOrCreateIOHelper(Type type)
            {
                if (type.IsNullableType())
                {
                    type = type.GetGenericArguments()[0];
                }
                // create an ioHelper for specified type
                if (!_ioHelpers.ContainsKey(type))
                {
                    if (type.IsEnum)
                    {
                        _ioHelpers.Add(type, (IOHelper)typeof(IOEnumTypeHelper<AcceptRejectRule>).GetGenericTypeDefinition().MakeGenericType(typeof(TSerializable), type).GetConstructor(new Type[] { }).Invoke(new object[] { }));
                    }
                    else if (type.IsValueType)
                    {
                        _ioHelpers.Add(type, (IOHelper)typeof(IOValueTypeHelper<AcceptRejectRule>).GetGenericTypeDefinition().MakeGenericType(typeof(TSerializable), type).GetConstructor(new Type[] { }).Invoke(new object[] { }));
                    }
                    else if (IsXmlSerializableType(type))
                    {
                        _ioHelpers.Add(type, (IOHelper)typeof(IOXmlSerializableTypeHelper<cs.XmlSerializable>).GetGenericTypeDefinition().MakeGenericType(typeof(TSerializable), type).GetConstructor(new Type[] { }).Invoke(new object[] { }));
                    }
                    else
                    {
                        _ioHelpers.Add(type, (IOHelper)typeof(IOUnspecificTypeHelper<string>).GetGenericTypeDefinition().MakeGenericType(typeof(TSerializable), type).GetConstructor(new Type[] { }).Invoke(new object[] { }));
                    }
                }

                return _ioHelpers[type];
            }

            /// <summary>
         /// Returns a collection of propertyInfos declared in XmlSerializable-classes
         /// </summary>
            private Dictionary<string, System.Reflection.PropertyInfo> GetPropertyInfos(Type type, Func<System.Reflection.PropertyInfo, bool> selector)
            {
                var retVal = new Dictionary<string, System.Reflection.PropertyInfo>();

                while (typeof(cs.XmlSerializable).IsAssignableFrom(type))
                {
                    foreach (System.Reflection.PropertyInfo pi in type.GetProperties())
                    {
                        var parameters = pi.GetIndexParameters();
                        string key = pi.Name.ToLowerInvariant();

                        if (!retVal.ContainsKey(key) && (parameters is null || parameters.Length == 0) && selector(pi))
                        {
                            retVal.Add(key, pi);
                        }
                    }
                    type = type.BaseType;
                }

                return retVal;
            }

            /// <summary>
         /// Returns True if the property should be serialized
         /// </summary>
            private bool GetPropertyInfosReadSelector(System.Reflection.PropertyInfo pi)
            {
                if (pi.CanRead)
                {
                    var attrs = pi.GetCustomAttributes(typeof(swss.ScriptIgnoreAttribute), true);
                    return attrs is null || attrs.Length == 0;
                }
                else
                {
                    return false;
                }
            }

            private Dictionary<Type, IOHelper> _ioHelpers = new Dictionary<Type, IOHelper>();

            /// <summary>
         /// Returns True if type is derived from XmlSerializable
         /// </summary>
            private bool IsXmlSerializableType(Type type)
            {
                return typeof(cs.XmlSerializable).IsAssignableFrom(type);
            }

            protected override void Serialize(SerializationContext context)
            {
                var props = GetPropertyInfos(context.Object.GetType(), GetPropertyInfosReadSelector).Values.ToArray();
                var emptyIndexParameters = new object[] { };

                foreach (System.Reflection.PropertyInfo pi in props)
                {
                    var propertyType = pi.PropertyType;
                    var elementType = propertyType.IsArray ? propertyType.GetElementType() : propertyType;
                    var ioHelper = GetOrCreateIOHelper(elementType);
                    var value = pi.GetValue(context.Object, emptyIndexParameters);

                    if (propertyType.IsEnum)
                    {
                        if (propertyType.IsNullableType())
                        {
                            if (((IOValueTypeHelper)ioHelper).HasValue(value))
                            {
                                value = ((IOValueTypeHelper)ioHelper).GetValue(value);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        value = ((Enum)value).GetName();
                    }
                    else if (propertyType.IsNullableType())
                    {
                        if (((IOValueTypeHelper)ioHelper).HasValue(value))
                        {
                            value = ((IOValueTypeHelper)ioHelper).GetValue(value);
                            if (ReferenceEquals(propertyType, typeof(DateTimeOffset?)))
                            {
                                Write(context, (DateTimeOffset)value, pi.Name);
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (IsXmlSerializableType(propertyType))
                    {
                        ioHelper.Write(this, context, value, pi.Name);
                        continue;
                    }
                    else if (propertyType.IsArray)
                    {
                        if (ReferenceEquals(elementType, typeof(DateTimeOffset)))
                        {
                            WriteArray(context, (DateTimeOffset[])value, pi.Name);
                        }
                        else
                        {
                            ioHelper.WriteArray(this, context, value, pi.Name);
                        }
                        continue;
                    }
                    else if (ReferenceEquals(propertyType, typeof(DateTimeOffset)))
                    {
                        Write(context, (DateTimeOffset)value, pi.Name);
                        continue;
                    }

                    // write
                    context.Dictionary.Add(pi.Name, value);
                }
            }
        }
    }
}