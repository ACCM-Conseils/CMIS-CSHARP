using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Net;
using System.ServiceModel;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Common
{
    /// <summary>
   /// Common collection of generic method proxies which allow the call of generic methods at runtime, if only 
   /// information about the RuntimeType but not the generic type argument itself can be provided.
   /// </summary>
   /// <remarks>
   /// Sample.
   /// Assuming the module MyModule contains the generic method:
   ///    Public Function MyFunc(Of T)(argumentList) As ReturnType
   /// 
   /// If T is known at compiletime (for example: String) then the call is easy:
   ///    MyModule.MyFunc(Of String)(argumentList)
   /// 
   /// But what is to do, if the type isn't known at compiletime but at runtime as GetType(T)? In this case widen the
   /// GenericRuntimeHelper:
   ///    Public MustOverride Function Call_MyModule_MyFunc(argumentList) As ReturnType
   /// 
   /// and override this method in the nested GenericTypeHelper(Of T) class:
   ///    Public Overrides Function Call_MyModule_MyFunc(argumentList) As ReturnType
   ///       Return MyModule.MyFunc(Of T)(argumentList)
   ///    End Function
   /// 
   /// Now You can call MyModule.MyFunc(Of String)() at runtime by:
   ///    CmisObjectModelLibrary.Common.GenericRuntimeHelper.GetInstance(GetType(String)).CallMyModule_MyFunc(argumentList)
   /// </remarks>
    public abstract class GenericRuntimeHelper
    {

        #region Constructors
        /// <summary>
      /// Creation only possible from nested classes
      /// </summary>
      /// <remarks></remarks>
        private GenericRuntimeHelper()
        {
        }

        private static Dictionary<Type, GenericRuntimeHelper> _runtimeHelpers = new Dictionary<Type, GenericRuntimeHelper>();
        /// <summary>
      /// Creates a GenericTypeHelper-instance for specified type
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static GenericRuntimeHelper GetInstance(Type type)
        {
            if (type is null)
            {
                return null;
            }
            else
            {
                lock (_runtimeHelpers)
                {
                    if (_runtimeHelpers.ContainsKey(type))
                    {
                        return _runtimeHelpers[type];
                    }
                    else if (typeof(Enum).IsAssignableFrom(type))
                    {
                        // create a GenericTypeHelper-instance for enums using reflection
                        var genericTypeDefinitionType = typeof(GenericEnumTypeHelper<enumClientType>).GetGenericTypeDefinition();
                        var genericType = genericTypeDefinitionType.MakeGenericType(type);
                        var ci = genericType.GetConstructor(new Type[] { });
                        GenericRuntimeHelper retVal = (GenericRuntimeHelper)ci.Invoke(new object[] { });

                        _runtimeHelpers.Add(type, retVal);
                        return retVal;
                    }
                    else if (type.IsValueType)
                    {
                        // create a GenericTypeHelper-instance for value type using reflection
                        var genericTypeDefinitionType = typeof(GenericValueTypeHelper<bool>).GetGenericTypeDefinition();
                        var genericType = genericTypeDefinitionType.MakeGenericType(type);
                        var ci = genericType.GetConstructor(new Type[] { });
                        GenericRuntimeHelper retVal = (GenericRuntimeHelper)ci.Invoke(new object[] { });

                        _runtimeHelpers.Add(type, retVal);
                        return retVal;
                    }
                    else
                    {
                        // create an unspecific GenericTypeHelper-instance using reflection
                        var genericTypeDefinitionType = typeof(GenericTypeHelper<object>).GetGenericTypeDefinition();
                        var genericType = genericTypeDefinitionType.MakeGenericType(type);
                        var ci = genericType.GetConstructor(new Type[] { });
                        GenericRuntimeHelper retVal = (GenericRuntimeHelper)ci.Invoke(new object[] { });

                        _runtimeHelpers.Add(type, retVal);
                        return retVal;
                    }
                }
            }
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// GenericTypeHelper only for enums
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <remarks></remarks>
        private class GenericEnumTypeHelper<T> : GenericValueTypeHelper<T> where T : struct
        {

            public override string Convert(object value)
            {
                return ((Enum)value).GetName();
            }

            public override object ConvertBack(string value, object defaultValue)
            {
                var result = default(T);
                return value.TryParse(ref result, true) ? result : defaultValue;
            }

            /// <summary>
         /// TryParse() for type T (enum type)
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public override bool TryParseGeneric(string name, ref Enum genericValue, bool ignoreCase = false)
            {
                T value = Conversions.ToGenericParameter<T>(genericValue);

                if (name.TryParse(ref value, ignoreCase))
                {
                    genericValue = (Enum)(object)value;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
      /// GenericTypeHelper for valueType
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <remarks></remarks>
        private class GenericValueTypeHelper<T> : GenericTypeHelper<T> where T : struct
        {

            public override object GetValue(object nullable)
            {
                if (nullable is T?)
                {
                    {
                        var withBlock = Conversions.ToGenericParameter<T>(nullable);
                        try
                        {
                            return (object)withBlock;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }

                // failure
                return null;
            }

            public override bool HasValue(object nullable)
            {
                bool hasVal = true;
                try
                {
                    T test = Conversions.ToGenericParameter<T>(nullable);
                }
                catch
                {
                    hasVal = false;
                }
                return nullable is T? && hasVal;
            }
        }

        /// <summary>
      /// Unspecific GenericTypeHelper
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <remarks></remarks>
        private class GenericTypeHelper<T> : GenericRuntimeHelper
        {

            /// <summary>
         /// Common.Convert(Of T)()
         /// </summary>
            public override string Convert(object value)
            {
                return CommonFunctions.Convert(Conversions.ToGenericParameter<T>(value));
            }

            /// <summary>
         /// Common.ConvertBack(Of T)()
         /// </summary>
            public override object ConvertBack(string value, object defaultValue)
            {
                return CommonFunctions.ConvertBack(value, Conversions.ToGenericParameter<T>(defaultValue));
            }

            /// <summary>
         /// Converts dictionary to IDictionary(Of TKey, TValue)
         /// </summary>
            public override IDictionary<TKey, TValue> ConvertDictionary<TKey, TValue>(object dictionary)
            {
                if (dictionary is null)
                {
                    return null;
                }
                else if (dictionary is IDictionary<TKey, TValue>)
                {
                    return (IDictionary<TKey, TValue>)dictionary;
                }
                else
                {
                    var dictionaryType = typeof(IDictionary<TKey, TValue>);
                    var genericIDictionaryType = dictionaryType.GetGenericTypeDefinition();

                    // search for IDictionary(Of)
                    foreach (Type interfaceType in dictionary.GetType().GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && ReferenceEquals(interfaceType.GetGenericTypeDefinition(), genericIDictionaryType))
                        {
                            var genericArguments = interfaceType.GetGenericArguments();
                            var keyType = genericArguments[0];
                            var valueType = genericArguments[1];

                            // this instance can be used to define TSourceKey
                            if (ReferenceEquals(typeof(T), keyType))
                            {
                                return ConvertDictionaryCore<TKey, TValue, T>(valueType, dictionary);
                            }
                            else
                            {
                                return GetInstance(keyType).ConvertDictionaryCore<TKey, TValue>(valueType, dictionary);
                            }
                        }
                    }

                    if (dictionary is IDictionary)
                    {
                        // IDictionary
                        var retVal = new Dictionary<TKey, TValue>();

                        foreach (DictionaryEntry de in (IDictionary)dictionary)
                        {
                            if (de.Key is TKey && (de.Value is null || de.Value is TValue))
                            {
                                retVal.Add(Conversions.ToGenericParameter<TKey>(de.Key), Conversions.ToGenericParameter<TValue>(de.Value));
                            }
                        }
                        return retVal;
                    }
                    else
                    {
                        // no IDictionary interface found
                        return null;
                    }
                }
            }

            protected override IDictionary<TKey, TValue> ConvertDictionaryCore<TKey, TValue>(Type sourceValueType, object dictionary)
            {
                return ConvertDictionaryCore<TKey, TValue, T>(sourceValueType, dictionary);
            }
            protected override IDictionary<TKey, TValue> ConvertDictionaryCore<TKey, TValue, TSourceKey>(object dictionary)
            {
                var retVal = new Dictionary<TKey, TValue>();

                foreach (KeyValuePair<TSourceKey, T> de in (IDictionary<TSourceKey, T>)dictionary)
                {
                    if (Conversions.ToBoolean(de.Key is TKey) && (de.Value is null || Conversions.ToBoolean(de.Value is TValue)))
                    {
                        retVal.Add(Conversions.ToGenericParameter<TKey>(de.Key), Conversions.ToGenericParameter<TValue>(de.Value));
                    }
                }
                return retVal;
            }
            private IDictionary<TKey, TValue> ConvertDictionaryCore<TKey, TValue, TSourceKey>(Type sourceValueType, object dictionary)
            {
                return GetInstance(sourceValueType).ConvertDictionaryCore<TKey, TValue, T>(dictionary);
            }

            /// <summary>
         /// Creates a cmisFaultType to encapsulate an exception
         /// </summary>
            public override Messaging.cmisFaultType CreateCmisFaultType(Exception ex)
            {
                if (ex is System.ServiceModel.Web.WebFaultException<T>)
                {
                    return Messaging.cmisFaultType.CreateInstance((System.ServiceModel.Web.WebFaultException<T>)ex);
                }
                else if (ex is System.ServiceModel.Web.WebFaultException)
                {
                    return Messaging.cmisFaultType.CreateInstance((System.ServiceModel.Web.WebFaultException)ex);
                }
                else
                {
                    return Messaging.cmisFaultType.CreateInstance(ex);
                }
            }

            /// <summary>
         /// Returns the count property of ICollection or ICollection(Of) instances
         /// </summary>
            public override int GetCount(object collection)
            {
                if (collection is null)
                {
                    return 0;
                }
                else if (collection is ICollection)
                {
                    return ((ICollection)collection).Count;
                }
                else if (collection is ICollection<T>)
                {
                    return ((ICollection<T>)collection).Count;
                }
                else
                {
                    var collectionType = typeof(ICollection<T>);
                    var genericICollectionType = collectionType.GetGenericTypeDefinition();
                    foreach (Type interfaceType in collection.GetType().GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && ReferenceEquals(interfaceType.GetGenericTypeDefinition(), genericICollectionType))
                        {
                            return GetInstance(interfaceType.GetGenericArguments()[0]).GetCount(collection);
                        }
                    }

                    // no ICollection interface found
                    return 0;
                }
            }

            /// <summary>
         /// Extracts StatusCode from WebFaultException and generic WebFaultException instances
         /// </summary>
         /// <param name="exception"></param>
         /// <returns></returns>
            protected override HttpStatusCode GetStatusCodeCore(FaultException exception)
            {
                if (exception is System.ServiceModel.Web.WebFaultException<T>)
                {
                    return ((System.ServiceModel.Web.WebFaultException<T>)exception).StatusCode;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
            }

            /// <summary>
         /// Object types cannot be designed as nullable
         /// </summary>
            public override object GetValue(object nullable)
            {
                return nullable;
            }

            /// <summary>
         /// Object types cannot be designed as nullable
         /// </summary>
            public override bool HasValue(object nullable)
            {
                return nullable is not null;
            }

            /// <summary>
         /// Returns True if dictionary could be converted to IDictionary(Of TKey, TValue)
         /// </summary>
         /// <param name="dictionary">If successful the value is changed to IDictionary(Of TKey, TValue) type</param>
            public override bool TryConvertDictionary<TKey, TValue>(ref object dictionary)
            {
                if (dictionary is null)
                {
                    return false;
                }
                else if (dictionary is IDictionary<TKey, TValue>)
                {
                    return true;
                }
                else
                {
                    var dictionaryType = typeof(IDictionary<TKey, TValue>);
                    var genericIDictionaryType = dictionaryType.GetGenericTypeDefinition();

                    foreach (Type interfaceType in dictionary.GetType().GetInterfaces())
                    {
                        if (interfaceType.IsGenericType && ReferenceEquals(interfaceType.GetGenericTypeDefinition(), genericIDictionaryType))
                        {
                            var genericArguments = interfaceType.GetGenericArguments();
                            var keyType = genericArguments[0];
                            var valueType = genericArguments[1];
                            var result = ReferenceEquals(typeof(T), keyType) ? ConvertDictionaryCore<TKey, TValue, T>(valueType, dictionary) : GetInstance(keyType).ConvertDictionaryCore<TKey, TValue>(valueType, dictionary);
                            if (result is not null && result.Count == GetCount(dictionary))
                            {
                                dictionary = result;
                                return true;
                            }
                        }
                    }
                    // expected interface not found
                    return false;
                }
            }

            /// <summary>
         /// for implementation requirements only
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public override bool TryParseGeneric(string name, ref Enum genericValue, bool ignoreCase = false)
            {
                // TryParse() can only be called for enum types; type T IS NOT an enum type (see GenericRuntimeHelper.GetInstance())
                return false;
            }
        }
        #endregion

        #region Support for generic calls
        /// <summary>
      /// Convert version for unspecific value
      /// </summary>
        public abstract string Convert(object value);

        /// <summary>
      /// ConvertBack version for unspecific defaultValue
      /// </summary>
        public abstract object ConvertBack(string value, object defaultValue);

        /// <summary>
      /// Converts dictionary to IDictionary(Of TKey, TValue)
      /// </summary>
        public abstract IDictionary<TKey, TValue> ConvertDictionary<TKey, TValue>(object dictionary);

        /// <summary>
      /// internal use
      /// </summary>
        protected abstract IDictionary<TKey, TValue> ConvertDictionaryCore<TKey, TValue>(Type sourceValueType, object dictionary);
        /// <summary>
      /// internal use
      /// </summary>
        protected abstract IDictionary<TKey, TValue> ConvertDictionaryCore<TKey, TValue, TSourceKey>(object dictionary);

        /// <summary>
      /// Creates a cmisFaultType to encapsulate an exception
      /// </summary>
        public abstract Messaging.cmisFaultType CreateCmisFaultType(Exception ex);

        /// <summary>
      /// Returns the count property of ICollection or ICollection(Of) instances
      /// </summary>
        public abstract int GetCount(object collection);

        /// <summary>
      /// Returns the HttpStatusCode of WebFaultExceptions
      /// </summary>
        public static HttpStatusCode GetStatusCode(FaultException exception)
        {
            if (exception is null)
            {
                return HttpStatusCode.OK;
            }
            else if (exception is System.ServiceModel.Web.WebFaultException)
            {
                // System.ServiceModel.Web.WebFaultException
                return ((System.ServiceModel.Web.WebFaultException)exception).StatusCode;
            }
            else
            {
                // System.ServiceModel.Web.WebFaultException(Of T)
                var exceptionType = exception.GetType();

                while (exceptionType is not null)
                {
                    if (exceptionType.IsGenericType && ReferenceEquals(exceptionType.GetGenericTypeDefinition(), _genericWebFaultExceptionTypeDefinition))
                    {
                        var genericArguments = exceptionType.GetGenericArguments();
                        return GetInstance(genericArguments[0]).GetStatusCodeCore(exception);
                    }
                    exceptionType = exceptionType.BaseType;
                }

                return HttpStatusCode.InternalServerError;
            }
        }
        protected abstract HttpStatusCode GetStatusCodeCore(FaultException exception);

        /// <summary>
      /// Returns the value of a nullable
      /// </summary>
        public abstract object GetValue(object nullable);

        /// <summary>
      /// Returns true if nullable has a value
      /// </summary>
        public abstract bool HasValue(object nullable);

        /// <summary>
      /// Returns True if dictionary could be converted to IDictionary(Of TKey, TValue)
      /// </summary>
      /// <param name="dictionary">If successful the value is changed to IDictionary(Of TKey, TValue) type</param>
        public abstract bool TryConvertDictionary<TKey, TValue>(ref object dictionary);

        /// <summary>
      /// TryParse version for unspecific enum type
      /// </summary>
        public abstract bool TryParseGeneric(string name, ref Enum genericValue, bool ignoreCase = false);
        #endregion

        private static Type _genericWebFaultExceptionTypeDefinition = typeof(System.ServiceModel.Web.WebFaultException<string>).GetGenericTypeDefinition();

    }
}