using System;
using System.Collections.Generic;
using sc = System.ComponentModel;
using System.Data;
using System.Linq;
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

namespace CmisObjectModel.Serialization
{
    /// <summary>
   /// Baseclass for all Cmis-Types
   /// </summary>
   /// <remarks>
   /// Attributes.JavaScriptObjectResolver(GetType(JSON.Serialization.Generic.DefaultObjectResolver(Of Core.cmisObjectType)), "{"""":""T""}")
   /// declares a fallback mechanism for Cmis-Types to allow JavaScript-serialization. If there is no other ObjectResolver defined for a
   /// type then the current type is used for the generic typeargument of DefaultObjectResolver(Of T) (empty string """" is mapped to ""T"")
   /// </remarks>
    [Attributes.JavaScriptConverter(typeof(JSON.Serialization.Generic.XmlSerializerConverter<Core.cmisObjectType>), "{\"\":\"TSerializable\"}")]
    [Attributes.JavaScriptObjectResolver(typeof(JSON.Serialization.Generic.DefaultObjectResolver<Core.cmisObjectType>), "{\"\":\"T\"}")]

    public abstract class XmlSerializable : sc.INotifyPropertyChanged, sxs.IXmlSerializable
    {

        protected XmlSerializable()
        {
        }
        protected XmlSerializable(bool? initClassSupported)
        {
            if (initClassSupported.HasValue && initClassSupported.Value)
                InitClass();
        }
        protected virtual void InitClass()
        {
        }

        /// <summary>
      /// Creates a new instance
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual XmlSerializable Copy()
        {
            XmlSerializable retVal = (XmlSerializable)GetType().GetConstructor(new Type[] { }).Invoke(new object[] { });
            CopyPropertiesTo(retVal);
            return retVal;
        }
        /// <summary>
      /// Copies the serializable properties to destination-object
      /// </summary>
      /// <remarks>destination and current instance MUST be of the same type</remarks>
        protected virtual void CopyPropertiesTo(XmlSerializable destination)
        {
            var currentType = GetType();
            var objectType = typeof(object);
            var stringType = typeof(string);
            var xmlSerializableType = typeof(XmlSerializable);
            var propertyNames = new HashSet<string>();
            var emptyIndex = new object[] { };

            // To support possible property overloads correct the algorithm starts with the currentType (that is the type of the new destination-object)
            // and then handles the properties of the baseclasses until XmlSerializable-Type is reached.
            // Copied properties are primitives properties, enums, strings and properties of type XmlSerializable or derived from it, furthermore arrays which
            // elementtypes accords to the same list.
            do
            {
                foreach (System.Reflection.PropertyInfo pi in currentType.GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    var propertyType = pi.PropertyType;
                    bool isArray = propertyType.IsArray;
                    var elementType = isArray ? propertyType.GetElementType() : propertyType;
                    bool isXmlSerializable = xmlSerializableType.IsAssignableFrom(elementType);
                    var indexParameters = pi.GetIndexParameters();

                    if (pi.CanRead && pi.CanWrite && (indexParameters is null || indexParameters.Length == 0) && propertyNames.Add(pi.Name) && (elementType.IsEnum || elementType.IsPrimitive || ReferenceEquals(elementType, stringType) || isXmlSerializable))
                    {
                        var value = pi.GetValue(this, emptyIndex);

                        if (value is null)
                        {
                            pi.SetValue(destination, value, emptyIndex);
                        }
                        else if (isArray)
                        {
                            {
                                var withBlock = (Array)value;
                                int length = withBlock.Length;
                                var newArray = Array.CreateInstance(elementType, length);

                                if (isXmlSerializable)
                                {
                                    for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                                    {
                                        XmlSerializable element = (XmlSerializable)withBlock.GetValue(index);
                                        if (element is not null)
                                            newArray.SetValue(element.Copy(), index);
                                    }
                                }
                                else
                                {
                                    for (int index = 0, loopTo1 = length - 1; index <= loopTo1; index++)
                                        newArray.SetValue(withBlock.GetValue(index), index);
                                }
                                pi.SetValue(destination, newArray, emptyIndex);
                            }
                        }
                        else if (isXmlSerializable)
                        {
                            pi.SetValue(destination, ((XmlSerializable)value).Copy(), emptyIndex);
                        }
                        else
                        {
                            pi.SetValue(destination, value, emptyIndex);
                        }
                    }
                }
                if (ReferenceEquals(currentType, xmlSerializableType))
                {
                    return;
                }
                else
                {
                    currentType = currentType.BaseType;
                }
            }
            while (true);
        }

        #region INotifyPropertyChanged
        /// <summary>
      /// AddHandler for specified propertyName
      /// </summary>
        public virtual void AddHandler(sc.PropertyChangedEventHandler handler, string propertyName)
        {
            sc.PropertyChangedEventHandler propertyChangedHandler;

            if (string.IsNullOrEmpty(propertyName))
                propertyName = "*";
            propertyChangedHandler = get_PropertyChangedHandler(propertyName);
            if (propertyChangedHandler is null)
            {
                set_PropertyChangedHandler(propertyName, handler);
            }
            else
            {
                set_PropertyChangedHandler(propertyName, (sc.PropertyChangedEventHandler)Delegate.Combine(propertyChangedHandler, handler));
            }
        }
        /// <summary>
      /// AddHandler for specified propertyNames
      /// </summary>
        public virtual void AddHandler(sc.PropertyChangedEventHandler handler, params string[] propertyNames)
        {
            if (propertyNames is null || propertyNames.Length == 0)
            {
                AddHandler(handler, "*");
            }
            else
            {
                foreach (string propertyName in propertyNames)
                    AddHandler(handler, propertyName);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new sc.PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged<TProperty>(string propertyName, TProperty newValue, TProperty oldValue)
        {
            OnPropertyChanged(propertyName.ToPropertyChangedEventArgs(newValue, oldValue));
        }
        protected virtual void OnPropertyChanged(sc.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(this, e);
        }

        public event sc.PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                AddHandler(value, "*");
            }
            remove
            {
                RemoveHandler(value, "*");
            }
        }
        void OnPropertyChanged(object sender, sc.PropertyChangedEventArgs e)
        {
            foreach (string propertyName in new string[] { e.PropertyName ?? string.Empty, "*" })
            {
                var propertyChangedHandler = get_PropertyChangedHandler(propertyName);

                if (propertyChangedHandler is not null)
                {
                    foreach (sc.PropertyChangedEventHandler handler in propertyChangedHandler.GetInvocationList())
                    {
                        try
                        {
                            handler.Invoke(sender, e);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }

        protected Dictionary<string, sc.PropertyChangedEventHandler> _propertyChangedHandlers = new Dictionary<string, sc.PropertyChangedEventHandler>();
        protected sc.PropertyChangedEventHandler get_PropertyChangedHandler(string propertyName)
        {
            return _propertyChangedHandlers.ContainsKey(propertyName) ? _propertyChangedHandlers[propertyName] : null;
        }

        protected void set_PropertyChangedHandler(string propertyName, sc.PropertyChangedEventHandler value)
        {
            _propertyChangedHandlers.Remove(propertyName);
            if (value is not null)
            {
                _propertyChangedHandlers.Add(propertyName, value);
            }
        }

        /// <summary>
      /// RemoveHandler for specified propertyName
      /// </summary>
        public virtual void RemoveHandler(sc.PropertyChangedEventHandler handler, string propertyName)
        {
            sc.PropertyChangedEventHandler propertyChangedHandler;

            if (string.IsNullOrEmpty(propertyName))
                propertyName = "*";
            propertyChangedHandler = get_PropertyChangedHandler(propertyName);
            if (propertyChangedHandler is not null)
            {
                propertyChangedHandler = (sc.PropertyChangedEventHandler)Delegate.Remove(propertyChangedHandler, handler);
                if (propertyChangedHandler is not null)
                {
                    var delegates = propertyChangedHandler.GetInvocationList();
                    if (delegates is null || delegates.Length == 0)
                    {
                        set_PropertyChangedHandler(propertyName, null);
                    }
                    else
                    {
                        set_PropertyChangedHandler(propertyName, propertyChangedHandler);
                    }
                }
            }
        }
        /// <summary>
      /// RemoveHandler for specified propertyNames
      /// </summary>
        public virtual void RemoveHandler(sc.PropertyChangedEventHandler handler, params string[] propertyNames)
        {
            if (propertyNames is null || propertyNames.Length == 0)
            {
                RemoveHandler(handler, "*");
            }
            else
            {
                foreach (string propertyName in propertyNames)
                    RemoveHandler(handler, propertyName);
            }
        }
        #endregion

        #region IXmlSerializable
        public virtual sx.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
      /// Reads the next ElementString from the XmlReader
      /// </summary>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual string Read(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string defaultValue)
        {
            return ReadCore(reader, attributeOverrides, nodeName, defaultValue, () => reader.ReadElementString());
        }
        /// <summary>
      /// Read the next ElementString from the XmlReader
      /// </summary>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual string Read(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, string defaultValue)
        {
            return ReadCore(reader, attributeOverrides, nodeName, @namespace, defaultValue, () => reader.ReadElementString());
        }

        /// <summary>
      /// Reads the next primitive property from the XmlReader
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual T Read<T>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, T defaultValue)
        {
            return ReadCore(reader, attributeOverrides, nodeName, defaultValue, () => CommonFunctions.ConvertBack(reader.ReadElementString(), defaultValue));
        }
        /// <summary>
      /// Read the next primitive property from the XmlReader
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TResult Read<TResult>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, TResult defaultValue)
        {
            return ReadCore(reader, attributeOverrides, nodeName, @namespace, defaultValue, () => CommonFunctions.ConvertBack(reader.ReadElementString(), defaultValue));
        }

        /// <summary>
      /// Reads the next serializable property from the XmlReader
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TResult Read<TResult>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, Func<sx.XmlReader, TResult> factory) where TResult : XmlSerializable
        {
            return ReadCore(reader, attributeOverrides, nodeName, null, () => factory.Invoke(reader));
        }
        /// <summary>
      /// Read the next serializable property from the XmlReader
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TResult Read<TResult>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, Func<sx.XmlReader, TResult> factory) where TResult : XmlSerializable
        {
            return ReadCore(reader, attributeOverrides, nodeName, @namespace, null, () => factory.Invoke(reader));
        }

        /// <summary>
      /// Reads the next array of primitives or strings from the XmlReader
      /// </summary>
      /// <typeparam name="TElement"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TElement[] ReadArray<TElement>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName)
        {
            var result = new List<TElement>();

            while (string.Compare(nodeName, CommonFunctions.GetCurrentStartElementLocalName(reader), true) == 0)
            {
                try
                {
                    result.Add(CommonFunctions.ConvertBack<TElement>(reader.ReadElementString(), default(TElement)));
                }
                catch
                {
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }
        /// <summary>
      /// Read the next array of primitives or strings from the XmlReader
      /// </summary>
      /// <typeparam name="TElement"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TElement[] ReadArray<TElement>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace)
        {
            var result = new List<TElement>();

            if (attributeOverrides is not null)
            {
                var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                if (xmlRootAttribute is not null && xmlRootAttribute.Namespace is not null)
                    @namespace = xmlRootAttribute.Namespace;
                if (xmlElementAttribute is not null)
                {
                    nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                    if (xmlElementAttribute.Namespace is not null)
                        @namespace = xmlElementAttribute.Namespace;
                }
            }
            while (string.Compare(nodeName, CommonFunctions.GetCurrentStartElementLocalName(reader), true) == 0 && (string.IsNullOrEmpty(@namespace) || string.Compare(reader.NamespaceURI, @namespace, true) == 0))
            {
                try
                {
                    result.Add(CommonFunctions.ConvertBack<TElement>(reader.ReadElementString(), default(TElement)));
                }
                catch
                {
                }
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        /// <summary>
      /// Reads the next array of serializables from the XmlReader
      /// </summary>
      /// <typeparam name="TElement"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TElement[] ReadArray<TElement>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, Func<sx.XmlReader, TElement> factory) where TElement : XmlSerializable
        {
            var result = new List<TElement>();

            do
            {
                // wenn Factory Nothing zurückgibt, ist das Ende des Arrays erreicht
                var factoryResult = string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(CommonFunctions.GetCurrentStartElementLocalName(reader), nodeName, true) == 0 ? factory.Invoke(reader) : null;

                if (factoryResult is not null)
                {
                    result.Add(factoryResult);
                }
                else if (result.Count == 0)
                {
                    return null;
                }
                else
                {
                    return result.ToArray();
                }
            }
            while (true);
        }
        /// <summary>
      /// Read the next array of serializables from the XmlReader
      /// </summary>
      /// <typeparam name="TElement"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TElement[] ReadArray<TElement>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, Func<sx.XmlReader, TElement> factory) where TElement : XmlSerializable
        {
            var result = new List<TElement>();

            if (attributeOverrides is not null)
            {
                var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                if (xmlRootAttribute is not null && xmlRootAttribute.Namespace is not null)
                    @namespace = xmlRootAttribute.Namespace;
                if (xmlElementAttribute is not null)
                {
                    nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                    if (xmlElementAttribute.Namespace is not null)
                        @namespace = xmlElementAttribute.Namespace;
                }
            }

            do
            {
                // wenn Factory Nothing zurückgibt, ist das Ende des Arrays erreicht
                var factoryResult = string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(CommonFunctions.GetCurrentStartElementLocalName(reader), nodeName, true) == 0 && (string.IsNullOrEmpty(@namespace) || string.Compare(reader.NamespaceURI, @namespace, true) == 0) ? factory.Invoke(reader) : null;

                if (factoryResult is not null)
                {
                    result.Add(factoryResult);
                }
                else if (result.Count == 0)
                {
                    return null;
                }
                else
                {
                    return result.ToArray();
                }
            }
            while (true);
        }

        /// <summary>
      /// Read properties represented in attributes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected abstract void ReadAttributes(sx.XmlReader reader);

        /// <summary>
      /// Embeds the getResult-call within a try-catch-block and checks the current nodename of the
      /// reader object if the name matches the given nodeName
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="defaultValue"></param>
      /// <param name="itemFactory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TResult ReadCore<TResult>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, TResult defaultValue, Func<TResult> itemFactory)
        {
            try
            {
                if (string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(nodeName, CommonFunctions.GetCurrentStartElementLocalName(reader), true) == 0)
                {
                    return itemFactory.Invoke();
                }
                else
                {
                    return defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
      /// Embeds the getResult-call within a try-catch-block and checks the current nodename of the
      /// reader object if the name matches the given nodeName and namespace
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="defaultValue"></param>
      /// <param name="itemFactory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TResult ReadCore<TResult>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, TResult defaultValue, Func<TResult> itemFactory)
        {
            try
            {
                if (attributeOverrides is not null)
                {
                    var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                    var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                    if (xmlRootAttribute is not null && xmlRootAttribute.Namespace is not null)
                        @namespace = xmlRootAttribute.Namespace;
                    if (xmlElementAttribute is not null)
                    {
                        nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                        if (xmlElementAttribute.Namespace is not null)
                            @namespace = xmlElementAttribute.Namespace;
                    }
                }
                if (string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(nodeName, CommonFunctions.GetCurrentStartElementLocalName(reader), true) == 0 && (string.IsNullOrEmpty(@namespace) || string.Compare(reader.NamespaceURI, @namespace, true) == 0))
                {
                    return itemFactory.Invoke();
                }
                else
                {
                    return defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
      /// Reads the next enum from XmlReader
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum ReadEnum<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, TEnum defaultValue) where TEnum : struct
        {
            return ReadCore(reader, attributeOverrides, nodeName, defaultValue, () =>
{
var value = default(TEnum);
return CommonFunctions.TryParse(reader.ReadElementString(), ref value, true) ? value : defaultValue;
});
        }
        /// <summary>
      /// Read the next enum from XmlReader
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum ReadEnum<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, TEnum defaultValue) where TEnum : struct
        {
            return ReadCore(reader, attributeOverrides, nodeName, @namespace, defaultValue, () =>
{
var value = default(TEnum);
return CommonFunctions.TryParse(reader.ReadElementString(), ref value, true) ? value : defaultValue;
});
        }

        /// <summary>
      /// Reads the next array of enums
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum[] ReadEnumArray<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName) where TEnum : struct
        {
            var result = new List<TEnum>();
            var value = default(TEnum);

            while (string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(CommonFunctions.GetCurrentStartElementLocalName(reader), nodeName, true) == 0)
            {
                if (CommonFunctions.TryParse(reader.ReadElementString(), ref value, true))
                    result.Add(value);
            }

            return result.Count == 0 ? null : result.ToArray();
        }
        /// <summary>
      /// Read the next array of enums
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum[] ReadEnumArray<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace) where TEnum : struct
        {
            var result = new List<TEnum>();
            var value = default(TEnum);

            if (attributeOverrides is not null)
            {
                var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                if (xmlRootAttribute is not null && xmlRootAttribute.Namespace is not null)
                    @namespace = xmlRootAttribute.Namespace;
                if (xmlElementAttribute is not null)
                {
                    nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                    if (xmlElementAttribute.Namespace is not null)
                        @namespace = xmlElementAttribute.Namespace;
                }
            }

            while (string.IsNullOrEmpty(nodeName) && reader.MoveToContent() == sx.XmlNodeType.Element || string.Compare(CommonFunctions.GetCurrentStartElementLocalName(reader), nodeName, true) == 0 && (string.IsNullOrEmpty(@namespace) || string.Compare(reader.NamespaceURI, @namespace, true) == 0))
            {
                if (CommonFunctions.TryParse(reader.ReadElementString(), ref value, true))
                    result.Add(value);
            }

            return result.Count == 0 ? null : result.ToArray();
        }

        /// <summary>
      /// Reads the next enum from XmlReader
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum? ReadOptionalEnum<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, TEnum? defaultValue) where TEnum : struct
        {
            return ReadCore(reader, attributeOverrides, nodeName, defaultValue, () =>
{
var value = default(TEnum);
return CommonFunctions.TryParse(reader.ReadElementString(), ref value, true) ? value : defaultValue;
});
        }
        /// <summary>
      /// Read the next enum from XmlReader
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="reader"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual TEnum? ReadOptionalEnum<TEnum>(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, TEnum? defaultValue) where TEnum : struct
        {
            return ReadCore(reader, attributeOverrides, nodeName, @namespace, defaultValue, () =>
{
var value = default(TEnum);
return CommonFunctions.TryParse(reader.ReadElementString(), ref value, true) ? value : defaultValue;
});
        }

        /// <summary>
      /// Deserialize properties
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        public virtual void ReadXml(sx.XmlReader reader)
        {
            bool isEmptyElement;

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            ReadAttributes(reader);
            // open tag of current instance
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                // give derived classes the chance to deserialize their properties
                ReadXmlCore(reader, XmlAttributeOverrides.GetInstance(reader));
                // skip next child nodes including the EndElement belongs to reader.ReadStartElement()
                reader.ReadToEndElement(true);
            }
        }

        /// <summary>
      /// Permits derived classes to read their properties
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected abstract void ReadXmlCore(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides);
        protected virtual bool ReadXmlCoreFuzzy(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, bool callReadXmlCoreFuzzy2)
        {
            return false;
        }
        protected virtual bool ReadXmlCoreFuzzy2(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides, bool callReadOuterXml)
        {
            return false;
        }

        /// <summary>
      /// Serializes values
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="values"></param>
      /// <remarks></remarks>
        protected virtual void WriteArray(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, string[] values)
        {
            if (values is not null)
            {
                if (attributeOverrides is not null)
                {
                    var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                    var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                    if (!(xmlRootAttribute is null || string.IsNullOrEmpty(@namespace)))
                        @namespace = xmlRootAttribute.Namespace;
                    if (xmlElementAttribute is not null)
                    {
                        nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                        if (!string.IsNullOrEmpty(@namespace))
                        {
                            if (xmlElementAttribute.Namespace is not null)
                                @namespace = xmlElementAttribute.Namespace;
                        }
                    }
                }
                if (string.IsNullOrEmpty(@namespace))
                {
                    foreach (string value in values)
                        writer.WriteElementString(nodeName, value);
                }
                else
                {
                    foreach (string value in values)
                        writer.WriteElementString(nodeName, @namespace, value);
                }
            }
        }
        /// <summary>
      /// Serializes values
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="values"></param>
      /// <remarks></remarks>
        protected virtual void WriteArray(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, XmlSerializable[] values)
        {
            if (values is not null)
            {
                foreach (XmlSerializable value in values)
                    WriteElement(writer, attributeOverrides, nodeName, @namespace, value);
            }
        }
        /// <summary>
      /// Serializes values
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="values"></param>
      /// <remarks></remarks>
        protected virtual void WriteArray(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, Extensions.Extension[] values)
        {
            if (values is not null)
            {
                foreach (Extensions.Extension value in values)
                {
                    if (value is not null)
                    {
                        var xmlRoot = value.GetXmlRootAttribute();

                        if (xmlRoot is null)
                        {
                            WriteElement(writer, attributeOverrides, nodeName, @namespace, value);
                        }
                        else
                        {
                            WriteElement(writer, attributeOverrides, xmlRoot.ElementName ?? nodeName, xmlRoot.Namespace ?? @namespace, value);
                        }
                    }
                }
            }
        }
        /// <summary>
      /// Serializes values
      /// </summary>
      /// <typeparam name="TItem"></typeparam>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="values"></param>
      /// <remarks>Detects type of values and delegates the call to a specific WriteArray() method</remarks>
        protected virtual void WriteArray<TItem>(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, TItem[] values)
        {
            if (values is not null)
            {
                if (typeof(Extensions.Extension).IsAssignableFrom(typeof(TItem)))
                {
                    WriteArray(writer, attributeOverrides, nodeName, @namespace, (from item in values
                                                                                  let serializable = (Extensions.Extension)(object)item
                                                                                  select serializable).ToArray());
                }
                else if (typeof(XmlSerializable).IsAssignableFrom(typeof(TItem)))
                {
                    WriteArray(writer, attributeOverrides, nodeName, @namespace, (from item in values
                                                                                  let serializable = (XmlSerializable)(object)item
                                                                                  select serializable).ToArray());
                }
                else if (typeof(TItem).IsEnum)
                {
                    WriteArray(writer, attributeOverrides, nodeName, @namespace, (from item in values
                                                                                  select ((Enum)(object)item).GetName()).ToArray());
                }
                else
                {
                    WriteArray(writer, attributeOverrides, nodeName, @namespace, (from item in values
                                                                                  select CommonFunctions.Convert(item)).ToArray());
                }
            }
        }

        /// <summary>
      /// Serializes value as attribute
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="value"></param>
      /// <remarks></remarks>
        protected virtual void WriteAttribute(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, string value)
        {
            if (attributeOverrides is not null)
            {
                var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                if (!(xmlRootAttribute is null || string.IsNullOrEmpty(@namespace)))
                    @namespace = xmlRootAttribute.Namespace;
                if (xmlElementAttribute is not null)
                {
                    nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                    if (!string.IsNullOrEmpty(@namespace))
                    {
                        if (xmlElementAttribute.Namespace is not null)
                            @namespace = xmlElementAttribute.Namespace;
                    }
                }
            }
            if (string.IsNullOrEmpty(@namespace))
            {
                writer.WriteAttributeString(nodeName, value);
            }
            else
            {
                writer.WriteAttributeString(nodeName, @namespace, value);
            }
        }
        /// <summary>
      /// Serializes value as element
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="value"></param>
      /// <remarks></remarks>
        protected virtual void WriteElement(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, string value)
        {
            if (attributeOverrides is not null)
            {
                var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                if (!(xmlRootAttribute is null || string.IsNullOrEmpty(@namespace)))
                    @namespace = xmlRootAttribute.Namespace;
                if (xmlElementAttribute is not null)
                {
                    nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                    if (!string.IsNullOrEmpty(@namespace))
                    {
                        if (xmlElementAttribute.Namespace is not null)
                            @namespace = xmlElementAttribute.Namespace;
                    }
                }
            }
            if (string.IsNullOrEmpty(@namespace))
            {
                writer.WriteElementString(nodeName, value);
            }
            else
            {
                writer.WriteElementString(nodeName, @namespace, value);
            }
        }
        /// <summary>
      /// Serializes value as element
      /// </summary>
      /// <param name="writer"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName">If this parameter is not set the system determines the nodeName from the XmlRootAttribute of the value-class</param>
      /// <param name="namespace"></param>
      /// <param name="value"></param>
      /// <remarks></remarks>
        protected virtual void WriteElement(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, XmlSerializable value)
        {
            if (value is not null)
            {
                if (attributeOverrides is not null)
                {
                    var xmlRootAttribute = attributeOverrides.get_XmlRoot(GetType());
                    var xmlElementAttribute = string.IsNullOrEmpty(nodeName) ? null : attributeOverrides.get_XmlElement(GetType(), nodeName);
                    if (!(xmlRootAttribute is null || string.IsNullOrEmpty(@namespace)))
                        @namespace = xmlRootAttribute.Namespace;
                    if (xmlElementAttribute is not null)
                    {
                        nodeName = xmlElementAttribute.ElementName.NVL(nodeName);
                        if (!string.IsNullOrEmpty(@namespace))
                        {
                            if (xmlElementAttribute.Namespace is not null)
                                @namespace = xmlElementAttribute.Namespace;
                        }
                    }
                }
                // nodeName must be read from the XmlRootAttribute of the value-instance
                if (string.IsNullOrEmpty(nodeName))
                    nodeName = value.GetXmlRootAttribute(exactNonNullResult: true).ElementName;
                if (string.IsNullOrEmpty(@namespace))
                {
                    writer.WriteStartElement(nodeName);
                }
                else
                {
                    writer.WriteStartElement(nodeName, @namespace);
                }
                value.WriteXmlCore(writer, attributeOverrides);
                writer.WriteEndElement();
            }
        }

        /// <summary>
      /// Serialization of this instance
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        public virtual void WriteXml(sx.XmlWriter writer)
        {
            WriteXmlCore(writer, XmlAttributeOverrides.GetInstance(writer));
        }
        protected abstract void WriteXmlCore(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides);
        #endregion

        protected Dictionary<string, object> _extendedProperties;
        /// <summary>
      /// ExtendProperty - support
      /// </summary>
      /// <remarks></remarks>
        public virtual Dictionary<string, object> get_ExtendedProperties(bool ensureInstance = true)
        {
            if (_extendedProperties is null && ensureInstance)
                _extendedProperties = new Dictionary<string, object>();
            return _extendedProperties;
        }

        /// <summary>
      /// Creates a new serializable-instance and initializes it via ReadXml()
      /// </summary>
      /// <typeparam name="TClass"></typeparam>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static TClass GenericXmlSerializableFactory<TClass>(sx.XmlReader reader) where TClass : XmlSerializable, new()
        {
            var retVal = new TClass();
            retVal.ReadXml(reader);
            return retVal;
        }

    }

    /// <summary>
    /// XmlSerializable class with aspects before and after ReadXmlCore() and WriteXmlCore()
    /// </summary>
    /// <remarks></remarks>
    /// 
    public abstract class XmlSerializableWithIOAspects : XmlSerializable
    {

        #region Constructors
        protected XmlSerializableWithIOAspects() : base()
        {
        }
        protected XmlSerializableWithIOAspects(bool? initClassSupported) : base(initClassSupported)
        {
        }
        #endregion

        protected abstract void BeginReadXmlCore(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides);
        protected abstract void BeginWriteXmlCore(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides);
        protected abstract void EndReadXmlCore(sx.XmlReader reader, XmlAttributeOverrides attributeOverrides);
        protected abstract void EndWriteXmlCore(sx.XmlWriter writer, XmlAttributeOverrides attributeOverrides);

        public override void ReadXml(sx.XmlReader reader)
        {
            bool isEmptyElement;

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            ReadAttributes(reader);
            // open tag of current instance
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                var attributeOverrides = XmlAttributeOverrides.GetInstance(reader);

                BeginReadXmlCore(reader, attributeOverrides);
                // give derived classes the chance to deserialize their properties
                ReadXmlCore(reader, attributeOverrides);
                EndReadXmlCore(reader, attributeOverrides);
                // skip next child nodes including the EndElement belongs to reader.ReadStartElement()
                reader.ReadToEndElement(true);
            }
        }

        public override void WriteXml(sx.XmlWriter writer)
        {
            var attributeOverrides = XmlAttributeOverrides.GetInstance(writer);
            BeginWriteXmlCore(writer, attributeOverrides);
            WriteXmlCore(writer, attributeOverrides);
            EndWriteXmlCore(writer, attributeOverrides);
        }
    }
}