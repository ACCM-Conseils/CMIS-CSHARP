using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CmisObjectModel.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Data
{
    /// <summary>
   /// A simple converter to adapt values from current system to remote system and vice versa
   /// </summary>
   /// <remarks></remarks>
    public class PropertyValueConverter
    {

        protected Hashtable _map;
        protected Hashtable _mapReverse;
        protected Common.Generic.Nullable<object> _nullValueMapping;
        protected Common.Generic.Nullable<object> _nullValueReverseMapping;

        #region Constructors
        protected PropertyValueConverter(Type localType, Type remoteType)
        {
            LocalType = localType;
            RemoteType = remoteType;
        }
        /// <summary>
      /// </summary>
      /// <param name="map">keys: local system objects, values: remote system objects</param>
      /// <param name="nullValueMapping"></param>
      /// <remarks></remarks>
        public PropertyValueConverter(Hashtable map, Common.Generic.Nullable<object> nullValueMapping = default) : this(map, typeof(object), typeof(object), nullValueMapping)
        {
        }
        public PropertyValueConverter(Hashtable map, Type localType, Type remoteType, Common.Generic.Nullable<object> nullValueMapping = default) : this(localType, remoteType)
        {
            _map = map;
            _nullValueMapping = nullValueMapping;
            if (map is not null)
            {
                _mapReverse = new Hashtable();
                if (nullValueMapping.Value is null)
                {
                    _nullValueReverseMapping = nullValueMapping;
                }
                else
                {
                    _mapReverse.Add(nullValueMapping.Value, null);
                }
                foreach (DictionaryEntry de in _map)
                {
                    if (de.Value is not null)
                    {
                        if (!_mapReverse.ContainsKey(de.Value))
                            _mapReverse.Add(de.Value, de.Key);
                    }
                    else if (!_nullValueReverseMapping.HasValue)
                    {
                        _nullValueReverseMapping = new Common.Generic.Nullable<object>(de.Key);
                    }
                }
            }
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Skips localType and remoteType of a propertyValueConverter
      /// </summary>
      /// <remarks></remarks>
        private class InversPropertyValueConverter : PropertyValueConverter
        {

            private PropertyValueConverter _innerConverter;
            public InversPropertyValueConverter(PropertyValueConverter innerConverter) : base(innerConverter.RemoteType, innerConverter.LocalType)
            {
                _innerConverter = innerConverter;
            }

            public override object Convert(object value)
            {
                return _innerConverter.ConvertBack(value);
            }

            public override object ConvertBack(object value)
            {
                return _innerConverter.Convert(value);
            }

            public override PropertyValueConverter MakeReverse()
            {
                return _innerConverter;
            }
        }
        #endregion

        /// <summary>
      /// Converts a value-array from the remote system (server/client) to the local system (client/server)
      /// </summary>
      /// <param name="values"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public object[] Convert(params object[] values)
        {
            if (values is null)
            {
                return null;
            }
            else
            {
                return (from value in values
                        select Convert(value)).ToArray();
            }
        }
        /// <summary>
      /// Converts a value from the remote system (server/client) to the local system (client/server)
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual object Convert(object value)
        {
            if (_mapReverse is null)
            {
                return value;
            }
            else
            {
                return value is null ? _nullValueReverseMapping.Value : _mapReverse.ContainsKey(value) ? _mapReverse[value] : value;
            }
        }

        /// <summary>
      /// Converts back a value-array from the local system (client/server) to the remote system (server/client)
      /// </summary>
      /// <param name="values"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public object[] ConvertBack(params object[] values)
        {
            if (values is null)
            {
                return null;
            }
            else
            {
                return (from value in values
                        select ConvertBack(value)).ToArray();
            }
        }
        /// <summary>
      /// Converts back a value from the local system (client/server) to the remote system (server/client)
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual object ConvertBack(object value)
        {
            if (_map is not null)
            {
                return value is null ? _nullValueMapping.Value : _map.ContainsKey(value) ? _map[value] : value;
            }
            else if (value is null)
            {
                return _nullValueMapping.Value;
            }
            else
            {
                return value;
            }
        }
        private InversPropertyValueConverter _MakeReverse_retVal = new InversPropertyValueConverter(null);

        public virtual PropertyValueConverter MakeReverse()
        {
            return _MakeReverse_retVal;
        }

        public readonly Type LocalType;
        public readonly Type RemoteType;

    }

    namespace Generic
    {
        /// <summary>
      /// Provides convert method from TValue to TResult
      /// </summary>
      /// <typeparam name="TValue"></typeparam>
      /// <typeparam name="TResult"></typeparam>
      /// <remarks></remarks>
        internal class ConvertDynamicHelper<TValue, TResult>
        {

            private static Func<TValue, TResult> _converter;
            private static object _syncObject = new object();

            public static TResult ConvertDynamic(TValue value)
            {
                {
                    var withBlock = _converter ?? GetConverter();
                    return withBlock.Invoke(value);
                }
            }

            /// <summary>
         /// Returns a valid converter from TValue to TResult
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public static Func<TValue, TResult> GetConverter()
            {
                lock (_syncObject)
                {
                    // search only once to generate a lambda
                    if (_converter is null)
                    {
                        var type = typeof(TResult);
                        var converters = new List<Func<TValue, TResult>>();

                        // 1. chance: TypeConverter (ConvertFrom)
                        foreach (Attribute attr in type.GetCustomAttributes(true))
                        {
                            if (attr is System.ComponentModel.TypeConverterAttribute)
                            {
                                System.ComponentModel.TypeConverterAttribute tca = (System.ComponentModel.TypeConverterAttribute)attr;
                                var converterType = Type.GetType(tca.ConverterTypeName, false, true);

                                if (converterType is not null)
                                {
                                    System.ComponentModel.TypeConverter converter = (System.ComponentModel.TypeConverter)Activator.CreateInstance(converterType);
                                    if (converter is not null && converter.CanConvertFrom(typeof(TValue)))
                                    {
                                        converters.Add(value => Conversions.ToGenericParameter<TResult>(converter.ConvertFrom(value)));
                                    }
                                }
                            }
                        }

                        // 2. chance: TypeConverter (ConvertTo)
                        foreach (Attribute attr in typeof(TValue).GetCustomAttributes(true))
                        {
                            if (attr is System.ComponentModel.TypeConverterAttribute)
                            {
                                System.ComponentModel.TypeConverterAttribute tca = (System.ComponentModel.TypeConverterAttribute)attr;
                                var converterType = Type.GetType(tca.ConverterTypeName, false, true);

                                if (converterType is not null)
                                {
                                    System.ComponentModel.TypeConverter converter = (System.ComponentModel.TypeConverter)Activator.CreateInstance(converterType);
                                    if (converter is not null && converter.CanConvertTo(type))
                                    {
                                        converters.Add(value => Conversions.ToGenericParameter<TResult>(converter.ConvertTo(value, type)));
                                    }
                                }
                            }
                        }

                        // 3. chance: direkt
                        if (type.IsEnum)
                        {
                            converters.Add(value =>
                                   {
                                       string enumValue = Conversions.ToString(value);
                                       return Conversions.ToGenericParameter<TResult>(Enum.Parse(type, enumValue, true));
                                   });
                        }
                        else
                        {
                            converters.Add(value => Conversion.CTypeDynamic<TResult>((object)value));
                        }

                        // at least there is one converter defined
                        if (converters.Count == 1)
                        {
                            var converter = converters[0];

                            _converter = new Func<TValue, TResult>(value => { try { return converter(value); } catch { return default; } });
                        }
                        else
                        {
                            _converter = new Func<TValue, TResult>(value =>
                                   {
                                       foreach (Func<TValue, TResult> converter in converters)
                                       {
                                           try
                                           {
                                               return converter(value);
                                           }
                                           catch
                                           {
                                           }
                                       }

                                       // not to convert
                                       return default;
                                   });
                        }
                    }

                    return _converter;
                }
            }
        }

        /// <summary>
      /// A simple typesafe converter to adapt values from remote system to local system and vice versa
      /// </summary>
      /// <typeparam name="TValue"></typeparam>
      /// <remarks></remarks>
        public class PropertyValueConverter<TValue> : PropertyValueConverter<TValue, TValue>
        {

            /// <summary>
         /// </summary>
         /// <param name="map">keys: local system objects, values: remote system objects</param>
         /// <param name="nullValueMapping"></param>
         /// <remarks></remarks>
            public PropertyValueConverter(Dictionary<TValue, TValue> map, Common.Generic.Nullable<TValue> nullValueMapping = default) : base(map, nullValueMapping)
            {
            }

            protected override TValue ConvertBackDynamic(TValue value)
            {
                return value;
            }

            protected override TValue ConvertDynamic(TValue value)
            {
                return value;
            }
        }

        /// <summary>
      /// A simple typesafe converter to adapt values from remote system to local system and vice versa
      /// </summary>
      /// <typeparam name="TLocal"></typeparam>
      /// <typeparam name="TRemote"></typeparam>
      /// <remarks></remarks>
        public class PropertyValueConverter<TLocal, TRemote> : PropertyValueConverter
        {

            protected new Dictionary<TLocal, TRemote> _map;
            protected new Dictionary<TRemote, TLocal> _mapReverse;
            protected new Common.Generic.Nullable<TRemote> _nullValueMapping;
            protected new Common.Generic.Nullable<TLocal> _nullValueReverseMapping;

            #region Constructors
            private PropertyValueConverter() : base(typeof(TLocal), typeof(TRemote))
            {
            }
            /// <summary>
         /// </summary>
         /// <param name="map">keys: local system objects, values: remote system objects</param>
         /// <param name="nullValueMapping"></param>
         /// <remarks></remarks>
            public PropertyValueConverter(Dictionary<TLocal, TRemote> map, Common.Generic.Nullable<TRemote> nullValueMapping = default) : this()
            {

                _map = map;
                _nullValueMapping = nullValueMapping;
                if (map is not null)
                {
                    _mapReverse = new Dictionary<TRemote, TLocal>();
                    if (nullValueMapping.HasValue)
                    {
                        if (nullValueMapping.Value is null)
                        {
                            _nullValueReverseMapping = new Common.Generic.Nullable<TLocal>(default);
                        }
                        else
                        {
                            _mapReverse.Add(nullValueMapping.Value, default);
                        }
                    }

                    foreach (KeyValuePair<TLocal, TRemote> de in _map)
                    {
                        if (de.Value is not null)
                        {
                            if (!_mapReverse.ContainsKey(de.Value))
                                _mapReverse.Add(de.Value, de.Key);
                        }
                        else if (!_nullValueReverseMapping.HasValue)
                        {
                            _nullValueReverseMapping = new Common.Generic.Nullable<TLocal>(de.Key);
                        }
                    }
                }
            }
            #endregion

            #region Helper classes
            /// <summary>
         /// Skips localType and remoteType of a propertyValueConverter
         /// </summary>
         /// <remarks></remarks>
            private class InversPropertyValueConverter : PropertyValueConverter<TRemote, TLocal>
            {

                private PropertyValueConverter<TLocal, TRemote> _innerPropertyValueConverter;
                public InversPropertyValueConverter(PropertyValueConverter<TLocal, TRemote> innerPropertyValueConverter)
                {
                    _innerPropertyValueConverter = innerPropertyValueConverter;
                }

                public override TRemote Convert(TLocal value)
                {
                    return _innerPropertyValueConverter.ConvertBack(value);
                }

                public override TLocal ConvertBack(TRemote value)
                {
                    return _innerPropertyValueConverter.Convert(value);
                }

                public override PropertyValueConverter MakeReverse()
                {
                    return _innerPropertyValueConverter;
                }
            }
            #endregion

            /// <summary>
         /// Converts a value-array from the remote system (server/client) to the local system (client/server)
         /// </summary>
         /// <param name="values"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public TLocal[] Convert(params TRemote[] values)
            {
                if (values is null)
                {
                    return null;
                }
                else
                {
                    return (from value in values
                            select Convert(value)).ToArray();
                }
            }
            /// <summary>
         /// Converts a value from the remote system (server/client) to the local system (client/server)
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public sealed override object Convert(object value)
            {
                return value is TRemote ? Convert(Conversions.ToGenericParameter<TRemote>(value)) : value;
            }
            /// <summary>
         /// Converts a value from the remote system (server/client) to the local system (client/server)
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public virtual TLocal Convert(TRemote value)
            {
                if (value is null)
                {
                    return _nullValueReverseMapping.Value;
                }
                else if (_mapReverse is null || !_mapReverse.ContainsKey(value))
                {
                    return ConvertDynamic(value);
                }
                else
                {
                    return _mapReverse[value];
                }
            }

            /// <summary>
         /// Converts back a value-array from the local system (client/server) to the remote system (server/client)
         /// </summary>
         /// <param name="values"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public TRemote[] ConvertBack(params TLocal[] values)
            {
                if (values is null)
                {
                    return null;
                }
                else
                {
                    return (from value in values
                            select ConvertBack(value)).ToArray();
                }
            }
            /// <summary>
         /// Converts back a value from the local system (client/server) to the remote system (server/client)
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public sealed override object ConvertBack(object value)
            {
                return value is TLocal ? ConvertBack(Conversions.ToGenericParameter<TLocal>(value)) : value;
            }
            /// <summary>
         /// Converts back a value from the local system (client/server) to the remote system (server/client)
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public virtual TRemote ConvertBack(TLocal value)
            {
                if (value is null)
                {
                    return _nullValueMapping.Value;
                }
                else if (_map is null || !_map.ContainsKey(value))
                {
                    return ConvertBackDynamic(value);
                }
                else
                {
                    return _map[value];
                }
            }

            protected virtual TRemote ConvertBackDynamic(TLocal value)
            {
                return ConvertDynamic<TLocal, TRemote>(value);
            }
            protected virtual TLocal ConvertDynamic(TRemote value)
            {
                return ConvertDynamic<TRemote, TLocal>(value);
            }
            private TResult ConvertDynamic<TValue, TResult>(TValue value)
            {
                return ConvertDynamicHelper<TValue, TResult>.ConvertDynamic(value);
            }
            private InversPropertyValueConverter _MakeReverse_retVal1 = new InversPropertyValueConverter(null);

            public override PropertyValueConverter MakeReverse()
            {
                return _MakeReverse_retVal1;
            }

        }
    }
}