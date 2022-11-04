using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using cjs = CmisObjectModel.JSON.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Collections.Generic
{
    /// <summary>
   /// Allows to access the elements of an array of XmlSerializable-instances via a customizable key
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapper<TOwner, TItem> : ArrayMapper<TOwner, TItem, string>
   where TOwner : Serialization.XmlSerializable
   where TItem : Serialization.XmlSerializable
    {

        public ArrayMapper(TOwner owner, string arrayPropertyName, Func<TItem[]> getArray, string keyPropertyName, Func<TItem, string> getKey) : base(owner, new Common.Generic.DynamicProperty<TItem[]>(getArray, arrayPropertyName), new Common.Generic.DynamicProperty<TItem, string>(getKey, keyPropertyName))
        {
        }

        /// <summary>
      /// Maps a new item
      /// </summary>
        protected override bool Add(ArrayMapperItem<TItem, string> value)
        {
            if (base.Add(value))
            {
                string key = (value.Key ?? string.Empty).ToLowerInvariant();
                if (!_mapIgnoreCase.ContainsKey(key))
                    _mapIgnoreCase.Add(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Dictionary<string, ArrayMapperItem<TItem, string>> _mapIgnoreCase = new Dictionary<string, ArrayMapperItem<TItem, string>>();

        public TItem this[string key, bool ignoreCase]
        {
            get
            {
                if (key is null)
                {
                    return null;
                }
                else if (ignoreCase)
                {
                    // build maps
                    if (_items is null)
                        Refresh();
                    key = key.ToLowerInvariant();
                    return _mapIgnoreCase.ContainsKey(key) ? _mapIgnoreCase[key].Item : null;
                }
                else
                {
                    return base[key];
                }
            }
        }

        /// <summary>
      /// Removes item
      /// </summary>
        protected override void Remove(TItem item)
        {
            string key = (_keyProperty.get_Value(item) ?? string.Empty).ToLowerInvariant();

            if (_mapIgnoreCase.ContainsKey(key) && ReferenceEquals(_mapIgnoreCase[key].Item, item))
                _mapIgnoreCase.Remove(key);
            base.Remove(item);
        }

        /// <summary>
      /// Ensures that the maps will be rebuild before the next access on array items
      /// </summary>
      /// <remarks></remarks>
        protected override void Reset()
        {
            base.Reset();
            _mapIgnoreCase.Clear();
        }

    }

    /// <summary>
   /// Allows to access the elements of an array of XmlSerializable-instances via a customizable key
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapper<TOwner, TItem, TKey> : ArrayMapperBase<TOwner, TItem, TKey, ArrayMapperItem<TItem, TKey>>
where TOwner : Serialization.XmlSerializable
where TItem : Serialization.XmlSerializable
    {

        public ArrayMapper(TOwner owner, Common.Generic.DynamicProperty<TItem[]> arrayProperty, Common.Generic.DynamicProperty<TItem, TKey> keyProperty) : base(owner, arrayProperty, keyProperty)
        {
        }

        /// <summary>
      /// Creates an ArrayMapperItem suitable for item
      /// </summary>
        protected override ArrayMapperItem<TItem, TKey> CreateArrayMapperItem(TItem item, int index)
        {
            return new ArrayMapperItem<TItem, TKey>(item, _keyProperty.get_Value(item), index);
        }
    }

    /// <summary>
   /// Allows to access the elements of an array of XmlSerializable-instances via a customizable key
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapper<TOwner, TItem, TKey, TValue> : ArrayMapper<TOwner, TItem, TKey, TValue, cjs.Generic.DefaultObjectResolver<TItem>>
   where TOwner : Serialization.XmlSerializable
   where TItem : Serialization.XmlSerializable, new()
    {

        public ArrayMapper(TOwner owner, Common.Generic.DynamicProperty<TItem[]> arrayProperty, Common.Generic.DynamicProperty<TItem, TKey> keyProperty, Common.Generic.DynamicProperty<TItem, TValue> valueProperty) : base(owner, arrayProperty, keyProperty, valueProperty)
        {
        }
    }

    /// <summary>
   /// Allows to access the elements of an array of XmlSerializable-instances via a customizable key
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapper<TOwner, TItem, TKey, TValue, TObjectResolver> : ArrayMapperBase<TOwner, TItem, TKey, ArrayMapperItem<TItem, TKey, TValue>>
where TOwner : Serialization.XmlSerializable
where TItem : Serialization.XmlSerializable
where TObjectResolver : cjs.Generic.ObjectResolver<TItem>, new()
    {

        public ArrayMapper(TOwner owner, Common.Generic.DynamicProperty<TItem[]> arrayProperty, Common.Generic.DynamicProperty<TItem, TKey> keyProperty, Common.Generic.DynamicProperty<TItem, TValue> valueProperty) : base(owner, arrayProperty, keyProperty)
        {
            _valueProperty = valueProperty;
            _objectObserver = new TObjectResolver();
        }

        #region IJavaSerializationProvider
        /// <summary>
      /// Loads data from serialized java-map
      /// </summary>
        public override object JavaImport(object source, cjs.JavaScriptSerializer serializer)
        {
            var javaScriptConverter = serializer is null ? null : serializer.GetJavaScriptConverter(typeof(TValue));

            if (javaScriptConverter is null)
            {
                if (source is IDictionary<TKey, TValue>)
                {
                    Load((IDictionary<TKey, TValue>)source);
                }
                else
                {
                    Reset();
                }
            }
            else if (CommonFunctions.TryConvertDictionary<TKey, IDictionary<string, object>>(ref source))
            {
                var data = new Dictionary<TKey, TValue>();

                foreach (KeyValuePair<TKey, IDictionary<string, object>> de in (IDictionary<TKey, IDictionary<string, object>>)source)
                    data.Add(de.Key, Conversions.ToGenericParameter<TValue>(javaScriptConverter.Deserialize(de.Value, typeof(TValue), serializer)));
                Load(data);
            }
            else
            {
                Reset();
            }

            return _items;
        }

        /// <summary>
      /// Serializes the content as a java-map
      /// </summary>
        public override object JavaExport(object obj, cjs.JavaScriptSerializer serializer)
        {
            var javaScriptConverter = serializer is null ? null : serializer.GetJavaScriptConverter(typeof(TValue));

            if (_items is null)
                Refresh();
            if (_map.Count == 0)
            {
                return null;
            }
            else if (javaScriptConverter is null)
            {
                var valueType = typeof(TValue);
                if (ReferenceEquals(valueType, typeof(DateTime)))
                {
                    return _map.ToDictionary(de => de.Key, de => Conversions.ToLong(JavaExportDateTime(de.Value.Value)));
                }
                else if (ReferenceEquals(valueType, typeof(DateTime[])))
                {
                    return _map.ToDictionary(de => de.Key, de => (long[])JavaExportDateTimeArray(de.Value.Value));
                }
                else if (ReferenceEquals(valueType, typeof(DateTimeOffset)))
                {
                    return _map.ToDictionary(de => de.Key, de => Conversions.ToLong(JavaExportDateTimeOffset(de.Value.Value)));
                }
                else if (ReferenceEquals(valueType, typeof(DateTimeOffset[])))
                {
                    return _map.ToDictionary(de => de.Key, de => (long[])JavaExportDateTimeOffsetArray(de.Value.Value));
                }
                else if (ReferenceEquals(valueType, typeof(string[])))
                {
                    return _map.ToDictionary(de => de.Key, de => (string[])JavaExportStringArray(de.Value.Value));
                }
                else
                {
                    return _map.ToDictionary(de => de.Key, de =>
{
var currentValueType = de.Value.Value is null ? valueType : de.Value.Value.GetType();
if (ReferenceEquals(currentValueType, typeof(DateTime)))
{
return Conversions.ToGenericParameter<TValue>(JavaExportDateTime(de.Value.Value));
}
else if (ReferenceEquals(currentValueType, typeof(DateTime[])))
{
return Conversions.ToGenericParameter<TValue>(JavaExportDateTimeArray(de.Value.Value));
}
else if (ReferenceEquals(currentValueType, typeof(DateTimeOffset)))
{
return Conversions.ToGenericParameter<TValue>(JavaExportDateTimeOffset(de.Value.Value));
}
else if (ReferenceEquals(currentValueType, typeof(DateTimeOffset[])))
{
return Conversions.ToGenericParameter<TValue>(JavaExportDateTimeOffsetArray(de.Value.Value));
}
else if (ReferenceEquals(currentValueType, typeof(string[])))
{
return Conversions.ToGenericParameter<TValue>(JavaExportStringArray(de.Value.Value));
}
else
{
return de.Value.Value;
}
});
                }
            }
            else
            {
                return _map.ToDictionary(de => de.Key, de => javaScriptConverter.Serialize(de.Value.Value, serializer));
            }
        }

        /// <summary>
      /// Support for DateTime objects
      /// </summary>
        private object JavaExportDateTime(object value)
        {
            return Conversions.ToDate(value).ToJSONTime();
        }
        /// <summary>
      /// Support for DateTime arrays
      /// </summary>
        private object JavaExportDateTimeArray(object values)
        {
            return (from value in (IEnumerable<string>)values
                    select JavaExportDateTime(value)).ToArray();
        }
        /// <summary>
      /// Support for DateTimeOffset objects
      /// </summary>
        private object JavaExportDateTimeOffset(object value)
        {
            return ((DateTimeOffset)value).DateTime.ToJSONTime();
        }
        /// <summary>
      /// Support for DateTimeOffset arrays
      /// </summary>
        private object JavaExportDateTimeOffsetArray(object values)
        {
            return (from value in (IEnumerable<string>)values
                    select JavaExportDateTimeOffset(value)).ToArray();
        }
        /// <summary>
      /// Support for String arrays (converts null elements to empty elements)
      /// </summary>
        private object JavaExportStringArray(object values)
        {
            return (from value in (IEnumerable<string>)values
                    select Conversions.ToString(value) ?? string.Empty).ToArray();
        }
        #endregion

        protected override ArrayMapperItem<TItem, TKey, TValue> CreateArrayMapperItem(TItem item, int index)
        {
            return new ArrayMapperItem<TItem, TKey, TValue>(item, _keyProperty.get_Value(item), _valueProperty.get_Value(item), index);
        }

        /// <summary>
      /// Replaces the current content of the arraymapper with data
      /// </summary>
      /// <param name="data"></param>
      /// <remarks></remarks>
        public virtual void Load(IDictionary<TKey, TValue> data)
        {
            if (_arrayProperty.CanWrite && _keyProperty.CanWrite && _valueProperty.CanWrite)
            {
                try
                {
                    _modifierThread = System.Threading.Thread.CurrentThread;
                    Reset();
                    if (data is not null)
                    {
                        var items = new List<TItem>(data.Count);

                        foreach (KeyValuePair<TKey, TValue> de in data)
                        {
                            var item = _objectObserver.CreateInstance(de.Value);

                            _keyProperty.set_Value(item, de.Key);
                            _valueProperty.set_Value(item, de.Value);
                            items.Add(item);
                        }
                        _items = items.ToArray();
                    }
                    _arrayProperty.Value = _items;
                }
                finally
                {
                    _modifierThread = null;
                }
            }
        }

        /// <summary>
      /// Mounts key/value-pair to the collection. If the specified key is already in the
      /// collection, item replaces that entry, otherwise a new entry is copied to the collection
      /// </summary>
      /// <param name="key"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual bool Mount(TKey key, TValue value)
        {
            if (_arrayProperty.CanWrite && key is not null && _keyProperty.CanWrite && _valueProperty.CanWrite)
            {
                var item = base[key];

                if (item is null)
                {
                    item = _objectObserver.CreateInstance(value);
                    _keyProperty.set_Value(item, key);
                    _valueProperty.set_Value(item, value);

                    return base.Mount(item);
                }
                else
                {
                    _keyProperty.set_Value(item, key);
                    _valueProperty.set_Value(item, value);

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        protected TObjectResolver _objectObserver;

        public virtual new TValue this[int index]
        {
            get
            {
                if (_items is null)
                    Refresh();
                if (_items[index] is null)
                {
                    return default;
                }
                else
                {
                    return this[_keyProperty.get_Value(_items[index])];
                }
            }
            set
            {
                if (_items is null)
                    Refresh();
                if (_valueProperty.CanWrite && _items[index] is not null)
                {
                    _valueProperty.set_Value(_items[index], value);
                }
            }
        }
        public virtual new TValue this[TKey key]
        {
            get
            {
                if (_items is null)
                    Refresh();
                return key is not null && _map.ContainsKey(key) ? _map[key].Value : default;
            }
        }

        protected Common.Generic.DynamicProperty<TItem, TValue> _valueProperty;
    }

    /// <summary>
   /// Allows to access the elements of an array of XmlSerializable-instances via a customizable key
   /// </summary>
   /// <remarks></remarks>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public abstract class ArrayMapperBase<TOwner, TItem, TKey, TArrayMapperItem> : Contracts.IJavaSerializationProvider
         where TOwner : Serialization.XmlSerializable
         where TItem : Serialization.XmlSerializable
         where TArrayMapperItem : ArrayMapperItem<TItem, TKey>
    {

        public ArrayMapperBase(TOwner owner, Common.Generic.DynamicProperty<TItem[]> arrayProperty, Common.Generic.DynamicProperty<TItem, TKey> keyProperty)
        {
            _owner = owner;
            _arrayProperty = arrayProperty;
            if (_owner is not null && _arrayProperty is not null)
                _owner.AddHandler(_xmlSerializable_PropertyChanged, _arrayProperty.PropertyName);
            _keyProperty = keyProperty;
        }

        #region IJavaSerializationProvider
        /// <summary>
      /// Loads data from serialized java-map
      /// </summary>
        public virtual object JavaImport(object source, cjs.JavaScriptSerializer serializer)
        {
            var javaScriptConverter = serializer is null ? null : serializer.GetJavaScriptConverter(typeof(TItem));

            if (javaScriptConverter is not null && CommonFunctions.TryConvertDictionary<TKey, IDictionary<string, object>>(ref source))
            {
                var data = new Dictionary<TKey, TItem>();

                foreach (KeyValuePair<TKey, IDictionary<string, object>> de in (IDictionary<TKey, IDictionary<string, object>>)source)
                    data.Add(de.Key, (TItem)javaScriptConverter.Deserialize(de.Value, typeof(TItem), serializer));
                Load(data);
            }
            else
            {
                Reset();
            }

            return _items;
        }

        /// <summary>
      /// Serializes the content as a java-map
      /// </summary>
        public virtual object JavaExport(object obj, cjs.JavaScriptSerializer serializer)
        {
            var javaScriptConverter = serializer is null ? null : serializer.GetJavaScriptConverter(typeof(TItem));

            if (javaScriptConverter is null)
            {
                return null;
            }
            else
            {
                if (_items is null)
                    Refresh();
                return _map.ToDictionary(de => de.Key, de => javaScriptConverter.Serialize(de.Value.Item, serializer));
            }
        }
        #endregion

        /// <summary>
      /// Maps a new item
      /// </summary>
        protected virtual bool Add(TArrayMapperItem value)
        {
            var key = value is null ? default : value.Key;

            if (!(key is null || _map.ContainsKey(key)))
            {
                _map.Add(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected Common.Generic.DynamicProperty<TItem[]> _arrayProperty;

        public int Count
        {
            get
            {
                if (_items is null)
                    Refresh();
                return _items.Length;
            }
        }

        /// <summary>
      /// Creates an ArrayMapperItem suitable for item
      /// </summary>
        protected abstract TArrayMapperItem CreateArrayMapperItem(TItem item, int index);

        /// <summary>
      /// Returns the index of the item corresponding with key
      /// </summary>
        public int get_IndexOf(TKey key)
        {
            if (_items is null)
                Refresh();
            return key is not null && _map.ContainsKey(key) ? _map[key].Index : -1;
        }

        protected TItem[] _items;
        public TItem[] Items
        {
            get
            {
                return _items;
            }
        }

        public virtual TItem this[int index]
        {
            get
            {
                if (_items is null)
                    Refresh();
                return _items[index];
            }
            set
            {
                TItem oldValue;

                if (_items is null)
                    Refresh();
                oldValue = _items[index];
                if (_arrayProperty.CanWrite && !ReferenceEquals(oldValue, value))
                {
                    try
                    {
                        int length = _items.Length;
                        TItem[] items = (TItem[])Array.CreateInstance(typeof(TItem), _items.Length);
                        Array.Copy(_items, items, length);

                        // replace item
                        _modifierThread = System.Threading.Thread.CurrentThread;
                        items[index] = value;
                        if (oldValue is not null)
                            Remove(oldValue);
                        if (value is not null)
                            Add(CreateArrayMapperItem(value, index));
                        _arrayProperty.Value = items;
                        _items = items;
                    }
                    finally
                    {
                        _modifierThread = null;
                    }
                }
            }
        }
        public virtual TItem this[TKey key]
        {
            get
            {
                if (_items is null)
                    Refresh();
                return key is not null && _map.ContainsKey(key) ? _map[key].Item : null;
            }
        }

        protected Common.Generic.DynamicProperty<TItem, TKey> _keyProperty;

        public TKey[] Keys
        {
            get
            {
                return _map.Keys.ToArray();
            }
        }

        /// <summary>
      /// Replaces the current content of the arraymapper with data
      /// </summary>
      /// <param name="data"></param>
      /// <remarks></remarks>
        public virtual void Load(IDictionary<TKey, TItem> data)
        {
            if (_arrayProperty.CanWrite && _keyProperty.CanWrite)
            {
                try
                {
                    _modifierThread = System.Threading.Thread.CurrentThread;
                    Reset();
                    if (data is not null)
                    {
                        var items = new List<TItem>(data.Count);

                        foreach (KeyValuePair<TKey, TItem> de in data)
                        {
                            _keyProperty.set_Value(de.Value, de.Key);
                            items.Add(de.Value);
                        }
                        _items = items.ToArray();
                    }
                    _arrayProperty.Value = _items;
                }
                finally
                {
                    _modifierThread = null;
                }
            }
        }
        /// <summary>
      /// Replaces the current content of the arraymapper with data
      /// </summary>
      /// <param name="data"></param>
      /// <remarks></remarks>
        public virtual void Load(IList<TItem> data)
        {
            if (_arrayProperty.CanWrite)
            {
                try
                {
                    _modifierThread = System.Threading.Thread.CurrentThread;
                    Reset();
                    if (data is not null)
                        _items = data.ToArray();
                    _arrayProperty.Value = _items;
                }
                finally
                {
                    _modifierThread = null;
                }
            }
        }

        protected Dictionary<TKey, TArrayMapperItem> _map = new Dictionary<TKey, TArrayMapperItem>();
        protected System.Threading.Thread _modifierThread;

        /// <summary>
      /// Mounts item to the collection. If the key specified by item is already in the
      /// collection, item replaces that entry, otherwise a new entry is copied to the collection
      /// </summary>
        public virtual bool Mount(TItem item)
        {
            if (_items is null)
                Refresh();
            if (_arrayProperty.CanWrite && item is not null)
            {
                var key = _keyProperty.get_Value(item);
                var oldValue = key is not null && _map.ContainsKey(key) ? _map[key] : null;

                if (oldValue is null)
                {
                    // add a new item
                    try
                    {
                        int length = _items.Length;
                        TItem[] items = (TItem[])Array.CreateInstance(typeof(TItem), length + 1);

                        if (length > 0)
                            Array.Copy(_items, items, length);
                        items[length] = item;
                        _modifierThread = System.Threading.Thread.CurrentThread;
                        Add(CreateArrayMapperItem(item, length));
                        _arrayProperty.Value = items;
                        _items = items;
                    }
                    finally
                    {
                        _modifierThread = null;
                    }
                }
                else
                {
                    // replacement
                    this[oldValue.Index] = item;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void _xmlSerializable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // ignore property changed events if caused by an action within this instance
            if (!ReferenceEquals(System.Threading.Thread.CurrentThread, _modifierThread))
            {
                Reset();
            }
        }

        protected TOwner _owner;
        public TOwner Owner
        {
            get
            {
                return _owner;
            }
        }

        /// <summary>
      /// Rebuilds the maps
      /// </summary>
      /// <remarks></remarks>
        protected virtual void Refresh()
        {
            Reset();
            // get the current items
            _items = _arrayProperty.Value ?? (new TItem[] { });

            for (int index = 0, loopTo = _items.Length - 1; index <= loopTo; index++)
            {
                var item = _items[index];

                if (item is not null)
                {
                    Add(CreateArrayMapperItem(item, index));
                    if (_keyProperty is not null)
                    {
                        item.AddHandler(_xmlSerializable_PropertyChanged, _keyProperty.PropertyName);
                    }
                }
            }
        }

        /// <summary>
      /// Removes item
      /// </summary>
        protected virtual void Remove(TItem item)
        {
            var key = _keyProperty.get_Value(item);

            if (key is not null && _map.ContainsKey(key) && ReferenceEquals(_map[key].Item, item))
                _map.Remove(key);
            if (_keyProperty is not null)
            {
                item.RemoveHandler(_xmlSerializable_PropertyChanged, _keyProperty.PropertyName);
            }
        }

        /// <summary>
      /// Ensures that the maps will be rebuild before the next access on array items
      /// </summary>
      /// <remarks></remarks>
        protected virtual void Reset()
        {
            // remove items from event handler
            if (_items is not null)
            {
                if (_keyProperty is not null)
                {
                    string propertyName = _keyProperty.PropertyName;

                    foreach (TItem item in _items)
                        item.RemoveHandler(_xmlSerializable_PropertyChanged, propertyName);
                }
                _items = null;
                _map.Clear();
            }
        }

    }

    /// <summary>
   /// Item of an ArrayMapper-instance
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapperItem<TItem, TKey> where TItem : Serialization.XmlSerializable
    {
        public ArrayMapperItem(TItem item, TKey key, int index)
        {
            Item = item;
            Key = key;
            Index = index;
        }

        public readonly int Index;
        public readonly TItem Item;
        public readonly TKey Key;
    }

    /// <summary>
   /// Item of an ArrayMapper-instance
   /// </summary>
   /// <remarks></remarks>
    public class ArrayMapperItem<TItem, TKey, TValue> : ArrayMapperItem<TItem, TKey> where TItem : Serialization.XmlSerializable
    {

        public ArrayMapperItem(TItem item, TKey key, TValue value, int index) : base(item, key, index)
        {
            Value = value;
        }

        public readonly TValue Value;
    }
}