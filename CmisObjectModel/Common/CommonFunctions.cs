using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using srs = System.Runtime.Serialization;
using ssw = System.ServiceModel.Web;
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
using CmisObjectModel.Constants;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Common
{
    /// <summary>
   /// a set of common functions
   /// </summary>
   /// <remarks></remarks>
    [HideModuleName()]
    public static class CommonFunctions
    {

        static CommonFunctions()
        {
            try
            {
                string defaultLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];

                if (!string.IsNullOrEmpty(defaultLogFile) && EnsureDirectory(System.IO.Path.GetDirectoryName(defaultLogFile)))
                {
                    if (!System.IO.File.Exists(defaultLogFile))
                    {
                        try
                        {
                            System.IO.File.Create(defaultLogFile).Close();
                            _defaultLogFile = defaultLogFile;
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        _defaultLogFile = defaultLogFile;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            _defaultNamespacePrefixes = new Dictionary<string, string>();
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in _namespaces)
            {
                string key = de.Value.ToLowerInvariant();

                if (!_defaultNamespacePrefixes.ContainsKey(key))
                {
                    _defaultNamespacePrefixes.Add(key, de.Key.Name);
                }
            }
        }

        #region Helper classes
        /// <summary>
      /// Allows a quick map between the name and the value of enum-members
      /// </summary>
      /// <remarks></remarks>
        private class EnumInspector
        {

            private EnumInspector(Type enumType)
            {
                var names = new Dictionary<Enum, string>();
                var values = new Dictionary<string, Enum>();
                var members = (from fi in enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                               select fi).ToDictionary(current => current.Name, current => current);

                foreach (KeyValuePair<string, System.Reflection.FieldInfo> de in members)
                {
                    Enum value = (Enum)de.Value.GetValue(null);
                    var attrs = de.Value.GetCustomAttributes(typeof(srs.EnumMemberAttribute), false);

                    if (attrs is not null && attrs.Length > 0)
                    {
                        // alias via EnumMemberAttribute defined
                        string aliasName = ((srs.EnumMemberAttribute)attrs[0]).Value;

                        // alias has priority
                        AppendName(_names, value, aliasName);
                        AppendValue(_values, aliasName, value);
                    }
                    else
                    {
                        AppendName(names, value, de.Key);
                    }
                    AppendValue(values, de.Key, value);
                }
                // append original entries if possible
                foreach (KeyValuePair<Enum, string> de in names)
                {
                    if (!_names.ContainsKey(de.Key))
                        _names.Add(de.Key, de.Value);
                }
                foreach (KeyValuePair<string, Enum> de in values)
                {
                    if (!_values.ContainsKey(de.Key))
                        _values.Add(de.Key, de.Value);
                }
            }

            private void AppendName(Dictionary<Enum, string> names, Enum key, string name)
            {
                if (!names.ContainsKey(key) && !string.IsNullOrEmpty(name))
                    names.Add(key, name);
            }
            private void AppendValue(Dictionary<string, Enum> values, string key, Enum value)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (!values.ContainsKey(key))
                        values.Add(key, value);
                    key = key.ToLowerInvariant();
                    if (!values.ContainsKey(key))
                        values.Add(key, value);
                    // enumeration value
                    var arr = Array.CreateInstance(Enum.GetUnderlyingType(value.GetType()), 1);
                    arr.SetValue(value, 0);
                    key = arr.GetValue(0).ToString();
                    if (!values.ContainsKey(key))
                        values.Add(key, value);
                }
            }

            /// <summary>
         /// Gets singleton for specified enumType
         /// </summary>
         /// <param name="enumType"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            private static EnumInspector GetInstance(Type enumType)
            {
                lock (_instances)
                {
                    if (_instances.ContainsKey(enumType))
                    {
                        return _instances[enumType];
                    }
                    else
                    {
                        var retVal = new EnumInspector(enumType);
                        _instances.Add(enumType, retVal);
                        return retVal;
                    }
                }
            }

            /// <summary>
         /// Returns the name of value regarding EnumMemberAttribute
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public static string GetName(Enum value)
            {
                return GetInstance(value.GetType()).GetNameCore(value);
            }
            private string GetNameCore(Enum value)
            {
                return _names.ContainsKey(value) ? _names[value] : value.ToString();
            }

            /// <summary>
         /// Returns True if name is a valid expression
         /// </summary>
         /// <typeparam name="TEnum"></typeparam>
         /// <param name="name"></param>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public static bool TryParse<TEnum>(string name, ref TEnum value, bool ignoreCase) where TEnum : struct
            {
                var enumType = typeof(TEnum);
                return enumType.IsEnum && !string.IsNullOrEmpty(name) ? GetInstance(typeof(TEnum)).TryParseCore(name, ref value, ignoreCase) : false;
            }
            private bool TryParseCore<TEnum>(string name, ref TEnum value, bool ignoreCase) where TEnum : struct
            {
                if (ignoreCase)
                    name = name.ToLowerInvariant();
                if (_values.ContainsKey(name))
                {
                    value = Conversions.ToGenericParameter<TEnum>(_values[name]);
                    return true;
                }
                else
                {
                    return Enum.TryParse(name, ignoreCase, out value);
                }
            }

            private static Dictionary<Type, EnumInspector> _instances = new Dictionary<Type, EnumInspector>();
            private Dictionary<Enum, string> _names = new Dictionary<Enum, string>();
            private Dictionary<string, Enum> _values = new Dictionary<string, Enum>();

        }
        #endregion

        #region XmlSerialization
        /// <summary>
      /// Convert and ConvertBack from base types to string and vice versa
      /// </summary>
      /// <remarks></remarks>
        public static Dictionary<Type, object> DefaultXmlConverter = new Dictionary<Type, object>() { { typeof(bool), new Tuple<Func<bool, string>, Func<string, bool>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToBoolean(item)) }, { typeof(bool?), new Tuple<Func<bool?, string>, Func<string, bool?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToBoolean(item)) }, { typeof(byte), new Tuple<Func<byte, string>, Func<string, byte>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToByte(item)) }, { typeof(byte?), new Tuple<Func<byte?, string>, Func<string, byte?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToByte(item)) }, { typeof(DateTime), new Tuple<Func<DateTime, string>, Func<string, DateTime>>(item => sx.XmlConvert.ToString(item, sx.XmlDateTimeSerializationMode.Utc), item => sx.XmlConvert.ToDateTime(item, sx.XmlDateTimeSerializationMode.Utc)) }, { typeof(DateTime?), new Tuple<Func<DateTime?, string>, Func<string, DateTime?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value, sx.XmlDateTimeSerializationMode.Utc) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToDateTime(item, sx.XmlDateTimeSerializationMode.Utc)) }, { typeof(DateTimeOffset), new Tuple<Func<DateTimeOffset, string>, Func<string, DateTimeOffset>>(item => sx.XmlConvert.ToString(item), item => CreateDateTimeOffset(item)) }, { typeof(DateTimeOffset?), new Tuple<Func<DateTimeOffset?, string>, Func<string, DateTimeOffset?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : CreateDateTimeOffset(item)) }, { typeof(decimal), new Tuple<Func<decimal, string>, Func<string, decimal>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToDecimal(item)) }, { typeof(decimal?), new Tuple<Func<decimal?, string>, Func<string, decimal?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToDecimal(item)) }, { typeof(double), new Tuple<Func<double, string>, Func<string, double>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToDouble(item)) }, { typeof(double?), new Tuple<Func<double?, string>, Func<string, double?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToDouble(item)) }, { typeof(int), new Tuple<Func<int, string>, Func<string, int>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToInt32(item)) }, { typeof(int?), new Tuple<Func<int?, string>, Func<string, int?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToInt32(item)) }, { typeof(long), new Tuple<Func<long, string>, Func<string, long>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToInt64(item)) }, { typeof(long?), new Tuple<Func<long?, string>, Func<string, long?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToInt64(item)) }, { typeof(float), new Tuple<Func<float, string>, Func<string, float>>(item => sx.XmlConvert.ToString(item), item => sx.XmlConvert.ToSingle(item)) }, { typeof(float?), new Tuple<Func<float?, string>, Func<string, float?>>(item => item.HasValue ? sx.XmlConvert.ToString(item.Value) : null, item => string.IsNullOrEmpty(item) ? default : sx.XmlConvert.ToSingle(item)) }, { typeof(string), new Tuple<Func<string, string>, Func<string, string>>(value => value, value => value) } };

        /// <summary>
      /// Converts a primitive object into its string-representation
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string Convert<T>(T value)
        {
            return DefaultXmlConverter.ContainsKey(typeof(T)) ? ((Tuple<Func<T, string>, Func<string, T>>)DefaultXmlConverter[typeof(T)]).Item1(value) : value is null ? null : value.ToString();
        }

        /// <summary>
      /// Converts value into primitive object
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static T ConvertBack<T>(string value, T defaultValue)
        {
            try
            {
                return DefaultXmlConverter.ContainsKey(typeof(T)) ? ((Tuple<Func<T, string>, Func<string, T>>)DefaultXmlConverter[typeof(T)]).Item2(value) : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
      /// Returns Nothing if the current element is not a startelement, otherwise the name of the element
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetCurrentStartElementLocalName(sx.XmlReader reader)
        {
            reader.MoveToContent();
            return reader.IsStartElement() ? reader.LocalName : null;
        }

        /// <summary>
      /// Reads to EndElement of the current node skipping the rest of the childnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        public static void ReadToEndElement(this sx.XmlReader reader, bool removeEndElement)
        {
            while (reader.MoveToContent() != sx.XmlNodeType.EndElement)
                reader.Skip();
            if (removeEndElement)
                reader.ReadEndElement();
        }
        #endregion

        #region Enum-Functions
        /// <summary>
      /// Returns the name of value regarding EnumMemberAttribute
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetName(this Enum value)
        {
            return EnumInspector.GetName(value);
        }

        /// <summary>
      /// Returns True if the name expression is valid for TEnum-objects
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="name"></param>
      /// <param name="value"></param>
      /// <param name="ignoreCase"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool TryParse<TEnum>(this string name, ref TEnum value, bool ignoreCase = false) where TEnum : struct
        {
            return EnumInspector.TryParse(name, ref value, ignoreCase);
        }

        /// <summary>
      /// Returns True if the name expression is valid for enum-type specified by value.GetType
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool TryParseGeneric(this string name, ref Enum value, bool ignoreCase = false)
        {
            {
                var withBlock = GenericRuntimeHelper.GetInstance(value.GetType());
                return withBlock.TryParseGeneric(name, ref value, ignoreCase);
            }
        }

        /// <summary>
      /// Converts enumServiceException to HttpStatusCode
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static System.Net.HttpStatusCode ToHttpStatusCode(this Messaging.enumServiceException value)
        {
            switch (value)
            {
                case Messaging.enumServiceException.constraint:
                    {
                        return System.Net.HttpStatusCode.Conflict;
                    }
                case Messaging.enumServiceException.contentAlreadyExists:
                    {
                        return System.Net.HttpStatusCode.Conflict;
                    }
                case Messaging.enumServiceException.nameConstraintViolation:
                    {
                        return System.Net.HttpStatusCode.Conflict;
                    }
                case Messaging.enumServiceException.filterNotValid:
                    {
                        return System.Net.HttpStatusCode.BadRequest;
                    }
                case Messaging.enumServiceException.invalidArgument:
                    {
                        return System.Net.HttpStatusCode.BadRequest;
                    }
                case Messaging.enumServiceException.notSupported:
                    {
                        return System.Net.HttpStatusCode.MethodNotAllowed;
                    }
                case Messaging.enumServiceException.objectNotFound:
                    {
                        return System.Net.HttpStatusCode.NotFound;
                    }
                case Messaging.enumServiceException.permissionDenied:
                    {
                        return System.Net.HttpStatusCode.Forbidden;
                    }
                case Messaging.enumServiceException.runtime:
                    {
                        return System.Net.HttpStatusCode.InternalServerError;
                    }
                case Messaging.enumServiceException.storage:
                    {
                        return System.Net.HttpStatusCode.InternalServerError;
                    }
                case Messaging.enumServiceException.streamNotSupported:
                    {
                        return System.Net.HttpStatusCode.Forbidden;
                    }
                case Messaging.enumServiceException.updateConflict:
                    {
                        return System.Net.HttpStatusCode.Conflict;
                    }
                case Messaging.enumServiceException.versioning:
                    {
                        return System.Net.HttpStatusCode.Conflict;
                    }

                default:
                    {
                        return System.Net.HttpStatusCode.InternalServerError;
                    }
            }
        }

        /// <summary>
      /// Conversion between enumRelationshipDirection and enumIncludeRelationships
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Core.enumIncludeRelationships ToIncludeRelationships(this Core.enumRelationshipDirection value)
        {
            switch (value)
            {
                case Core.enumRelationshipDirection.either:
                    {
                        return Core.enumIncludeRelationships.both;
                    }
                case Core.enumRelationshipDirection.source:
                    {
                        return Core.enumIncludeRelationships.source;
                    }
                case Core.enumRelationshipDirection.target:
                    {
                        return Core.enumIncludeRelationships.target;
                    }

                default:
                    {
                        return Core.enumIncludeRelationships.none;
                    }
            }
        }

        /// <summary>
      /// Converts HttpStatusCode to enumServiceException (lossy)
      /// </summary>
        public static Messaging.enumServiceException ToServiceException(this System.Net.HttpStatusCode value)
        {
            switch (value)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    {
                        return Messaging.enumServiceException.invalidArgument;
                    }
                case System.Net.HttpStatusCode.Conflict:
                    {
                        return Messaging.enumServiceException.constraint;
                    }
                case System.Net.HttpStatusCode.Forbidden:
                    {
                        return Messaging.enumServiceException.permissionDenied;
                    }
                case System.Net.HttpStatusCode.InternalServerError:
                    {
                        return Messaging.enumServiceException.runtime;
                    }
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    {
                        return Messaging.enumServiceException.notSupported;
                    }
                case System.Net.HttpStatusCode.NotFound:
                    {
                        return Messaging.enumServiceException.objectNotFound;
                    }

                default:
                    {
                        return Messaging.enumServiceException.runtime;
                    }
            }
        }
        #endregion

        #region ServiceUri-Support
        /// <summary>
      /// Creates a new Uri
      /// </summary>
        public static Uri Combine(this Uri baseUri, string relativeUri)
        {
            if (string.IsNullOrEmpty(relativeUri))
            {
                return baseUri;
            }
            else if (relativeUri == "/")
            {
                if (baseUri.OriginalString.EndsWith(Conversions.ToString('/')))
                {
                    return baseUri;
                }
                else
                {
                    return new Uri(baseUri.OriginalString + "/");
                }
            }
            else if (relativeUri.StartsWith(Conversions.ToString('?')))
            {
                return new Uri(baseUri, relativeUri);
            }
            else if (relativeUri.StartsWith(Conversions.ToString('/')))
            {
                relativeUri = relativeUri.Substring(1);
            }
            if (baseUri.OriginalString.EndsWith(Conversions.ToString('/')))
            {
                return new Uri(baseUri, relativeUri);
            }
            else
            {
                return new Uri(new Uri(baseUri.OriginalString + "/"), relativeUri);
            }
        }

        /// <summary>
        /// Searches the serviceUri for placeHolders and replaces them with given replacements.
        /// </summary>
        public static string ReplaceUri(string serviceUri, Dictionary<string, string> replacements)
        {
            bool stringQueryParameters = false;

            if (serviceUri == "" || replacements.Count == 0)
                return serviceUri;
            else
            {
                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(\A\/|\?|\{(?<placeHolder>[A-Z][A-Z0-9]*)\})", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                System.Text.RegularExpressions.MatchEvaluator evaluator = match =>
                {
                    System.Text.RegularExpressions.Group group = match.Groups["placeHolder"];

                    // placeHolder or start of stringqueryparameters found
                    if (match.Value == "?")
                    {
                        stringQueryParameters = true;
                        return "?";
                    }
                    else if (match.Value == "/")
                        // starting slash has to be removed (\A\/)
                        return "";
                    else if (replacements.ContainsKey(group.Value))
                    {
                        // Return If(stringQueryParameters, System.Web.HttpUtility.UrlEncode(pairs(group.Value)), System.Web.HttpUtility.UrlPathEncode(pairs(group.Value)))
                        string replacement = replacements[group.Value];
                        return string.IsNullOrEmpty(replacement) ? replacement : System.Uri.EscapeDataString(replacement);
                    }
                    else if (replacements.ContainsKey("*"))
                    {
                        // undefined placeHolder with defined generalReplacement
                        // Return If(stringQueryParameters, System.Web.HttpUtility.UrlEncode(pairs("*")), System.Web.HttpUtility.UrlPathEncode(pairs("*")))
                        string replacement = replacements["*"];
                        return string.IsNullOrEmpty(replacement) ? replacement : System.Uri.EscapeDataString(replacement);
                    }
                    else
                        return match.Value;
                };

                return regEx.Replace(serviceUri, evaluator);
            }
        }
        /// <summary>
        /// Searches the serviceUri for placeHolders and replaces them with given replacements.
        /// </summary>
        /// <param name="serviceUri"></param>
        /// <param name="searchAndReplace">pairs of replacements: the first value defines the name of
        /// the placeholder, the second value defines the replacement. If the length of this
        /// parameter is odd, the last value defines the replacement for all placeHolders not
        /// found in the search and replace pairs before</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceUri(this string serviceUri, params string[] searchAndReplace)
        {
            return ReplaceUri(serviceUri, ConvertSearchAndReplace(searchAndReplace));
        }

        /// <summary>
        /// Searches the serviceUri for placeHolders and replaces them with given replacements.
        /// Unused querystring parameters will be removed
        /// </summary>
        /// <param name="serviceUri"></param>
        /// <param name="searchAndReplace"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceUriTemplate(string serviceUri, params string[] searchAndReplace)
        {
            // expression detects empty querystring-parameters
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"((?<Query>\?)[^\=\&]+\=(\&[^\=\&]+\=(?=($|\&)))*(?=($|\&))(?<Eos>$)?|\&[^\=\&]+=(?=($|\&)))", System.Text.RegularExpressions.RegexOptions.Singleline);
            System.Text.RegularExpressions.MatchEvaluator evaluator = match =>
            {
                System.Text.RegularExpressions.Group group = match.Groups["Eos"];
                return match.Value.StartsWith("?") && (group == null || !group.Success) ? "?" : null;
            };
            {
                var withBlock = new List<string>();
                if (searchAndReplace != null)
                    withBlock.AddRange(searchAndReplace);
                // add null-replacement for undefined parameters
                if (((withBlock.Count & 1) == 0))
                    withBlock.Add(null);
                return regEx.Replace(ReplaceUri(serviceUri, withBlock.ToArray()), evaluator);
            }
        }

        /// <summary>
        /// Returns the searchAndReplace definitions in the KeyValuePair-form
        /// </summary>
        /// <param name="searchAndReplace">If the length of searchAndReplace is odd, the last
        /// element is interpreted as generalReplacement for all placeHolders not defined before</param>
        /// <returns></returns>
        /// <remarks>The generalReplacement is stored with the key '*'</remarks>
        public static Dictionary<string, string> ConvertSearchAndReplace(string[] searchAndReplace)
        {
            int length = searchAndReplace == null ? 0 : searchAndReplace.Length;
            Dictionary<string, string> retVal = new Dictionary<string, string>(Math.Max(1, (length + 1) >> 1));

            if (length > 0)
            {
                Queue<string> pairs = new Queue<string>(searchAndReplace);

                while (pairs.Count > 1)
                {
                    string placeHolderName = pairs.Dequeue();
                    string replacement = pairs.Dequeue();
                    if (placeHolderName != "" && !retVal.ContainsKey(placeHolderName))
                        retVal.Add(placeHolderName, replacement);
                }
                if (pairs.Count > 0 && !retVal.ContainsKey("*"))
                    retVal.Add("*", pairs.Dequeue());
            }

            return retVal;
        }

        #endregion

        #region ElementName-Support
        /// <summary>
        /// Returns the XmlRootAttribute for class T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inherit"></param>
        /// <param name="exactNonNullResult">If True: if there is no XmlRootAttribute defined for class T,
        /// an empty XmlRootAttribute is returned</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static sxs.XmlRootAttribute GetXmlRootAttribute<T>(bool inherit = false, bool exactNonNullResult = false)
        {
            return typeof(T).GetXmlRootAttribute(inherit, exactNonNullResult);
        }
        /// <summary>
      /// Returns the XmlRootAttribute for instanceOrType
      /// </summary>
      /// <param name="instanceOrType"></param>
      /// <param name="inherit"></param>
      /// <param name="exactNonNullResult">If True: if there is no XmlRootAttribute defined for instanceOrType,
      /// an empty XmlRootAttribute is returned</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static sxs.XmlRootAttribute GetXmlRootAttribute(this object instanceOrType, bool inherit = false, bool exactNonNullResult = false)
        {
            if (instanceOrType is not null)
            {
                var type = instanceOrType is Type ? (Type)instanceOrType : instanceOrType.GetType();
                var attrs = type.GetCustomAttributes(typeof(sxs.XmlRootAttribute), inherit);
                if (attrs is not null && attrs.Length > 0)
                {
                    return (sxs.XmlRootAttribute)attrs[0];
                }
                else if (exactNonNullResult)
                {
                    return new sxs.XmlRootAttribute();
                }
                else
                {
                    return null;
                }
            }
            else if (exactNonNullResult)
            {
                return new sxs.XmlRootAttribute();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Returns the root elementname defined by System.Xml.Serialization.XmlRootAttribute
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetXmlRootElementName<T>(bool inherit = false)
        {
            return GetXmlRootAttribute<T>(inherit, true).ElementName;
        }
        /// <summary>
      /// Returns the root elementname defined by System.Xml.Serialization.XmlRootAttribute
      /// </summary>
      /// <param name="instanceOrType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetXmlRootElementName(this object instanceOrType, bool inherit = false)
        {
            return instanceOrType.GetXmlRootAttribute(inherit, true).ElementName;
        }

        /// <summary>
      /// Returns the root namespace defined by System.Xml.Serialization.XmlRootAttribute
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="inherit"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetXmlRootNamespace<T>(bool inherit = false)
        {
            return GetXmlRootAttribute<T>(inherit, true).Namespace;
        }
        /// <summary>
      /// Returns the root namespace defined by System.Xml.Serialization.XmlRootAttribute
      /// </summary>
      /// <param name="instanceOrType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetXmlRootNamespace(this Type instanceOrType, bool inherit = false)
        {
            return instanceOrType.GetXmlRootAttribute(inherit, true).Namespace;
        }

        /// <summary>
      /// Returns the nodename without namespace information
      /// </summary>
      /// <param name="node"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetNodeNameWithoutNamespace(this sx.XmlNode node)
        {
            string name = node.Name;
            int indexOf = name.IndexOf(':');
            return indexOf < 0 ? name : name.Substring(indexOf + 1);
        }

        /// <summary>
      /// Returns the namespace of node
      /// </summary>
      /// <param name="node"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetNodeNamespace(this sx.XmlNode node)
        {
            string name = node.Name;
            int indexOf = name.IndexOf(':');
            return indexOf <= 0 ? null : name.Substring(0, indexOf);
        }
        #endregion

        #region Parse Nullables
        /// <summary>
      /// Converts a string to a nullable integer.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static long? ParseInteger(string value)
        {
            long intValue;

            if (string.IsNullOrEmpty(value) || !long.TryParse(value, out intValue))
            {
                return default;
            }
            else
            {
                return intValue;
            }
        }
        /// <summary>
      /// Converts a string to a nullable boolean.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool? ParseBoolean(string value)
        {
            bool boolValue;

            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out boolValue))
            {
                return default;
            }
            else
            {
                return boolValue;
            }
        }
        /// <summary>
      /// Converts a string to a nullable enum.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static T? ParseEnum<T>(string value) where T : struct
        {
            T enumValue = default;

            if (string.IsNullOrEmpty(value) || !Enum.TryParse(value, out enumValue))
            {
                return default;
            }
            else
            {
                return enumValue;
            }
        }
        #endregion

        #region DateTime
        private static System.Text.RegularExpressions.Regex _toDateTimeRegEx = new System.Text.RegularExpressions.Regex(@"-?\d{4,}(-\d{2}){2}T\d{2}(\:\d{2}){2}(\.\d+)?([\+\-]\d{2}\:\d{2}|Z)?", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        /// <summary>
      /// Evaluates the current node as a DateTimeOffset-value
      /// </summary>
      /// <param name="dateTimeOffset"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static DateTimeOffset CreateDateTimeOffset(string dateTimeOffset)
        {
            lock (_toDateTimeRegEx)
            {
                try
                {
                    bool isNullOrEmpty = string.IsNullOrEmpty(dateTimeOffset);
                    var match = isNullOrEmpty ? null : _toDateTimeRegEx.Match(dateTimeOffset);

                    return isNullOrEmpty ? default : match is null || !match.Success ? new DateTimeOffset(Conversions.ToDate(dateTimeOffset)) : sx.XmlConvert.ToDateTimeOffset(dateTimeOffset);
                }
                catch
                {
                }
            }

            return default;
        }

        private static DateTime _jsonReferenceDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromJSONTime(this long value)
        {
            return _jsonReferenceDate.Add(TimeSpan.FromMilliseconds(value)).ToLocalTime();
        }

        public static long ToJSONTime(this DateTime value)
        {
            return (long)Math.Round(value.ToUniversalTime().Subtract(_jsonReferenceDate).TotalMilliseconds);
        }
        #endregion

        #region Change-Token
        /// <summary>
      /// ChangeToken-Generator
      /// </summary>
      /// <remarks></remarks>
        public abstract class ChangeToken
        {
            private ChangeToken()
            {
            }
            public abstract class Oracle
            {
                private Oracle()
                {
                }
                /// <summary>
            /// Expression for current UTC-time
            /// </summary>
            /// <remarks></remarks>
                public const string NextChangeTokenExpression = "To_Char(sys_extract_utc(systimestamp),'YYYYMMDDHH24MISSFF')";
            }
            public static string NextChangeToken
            {
                get
                {
                    return Strings.Format(DateTime.UtcNow, "yyyyMMddHHmmssffffff000");
                }
            }
        }
        #endregion

        #region Support cmisPropertyDecimal
        private static HashSet<enumDecimalRepresentation> _allowedDecimalRepresentations = new HashSet<enumDecimalRepresentation>() { enumDecimalRepresentation.@decimal, enumDecimalRepresentation.@double };
        private static enumDecimalRepresentation _decimalRepresentation = enumDecimalRepresentation.@decimal;
        public static enumDecimalRepresentation DecimalRepresentation
        {
            get
            {
                return _decimalRepresentation;
            }
            set
            {
                if (value != _decimalRepresentation && _allowedDecimalRepresentations.Contains(value))
                {
                    _decimalRepresentation = value;
                    DecimalRepresentationChanged?.Invoke(value);
                }
            }
        }

        public static event DecimalRepresentationChangedEventHandler DecimalRepresentationChanged;

        public delegate void DecimalRepresentationChangedEventHandler(enumDecimalRepresentation newValue);
        #endregion

        #region GenericType-Support
        /// <summary>
      /// Returns true if genericTypeDefinition is the direct or indirect generic type definition of type
      /// </summary>
      /// <param name="type"></param>
      /// <param name="genericTypeDefinition"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool BasedOnGenericTypeDefinition(this Type type, Type genericTypeDefinition)
        {
            var nearestGenericTypeDefinition = type.FindGenericTypeDefinition();

            if (ReferenceEquals(nearestGenericTypeDefinition, genericTypeDefinition))
            {
                return true;
            }
            else if (nearestGenericTypeDefinition is null)
            {
                return false;
            }
            else
            {
                return nearestGenericTypeDefinition.BaseType.BasedOnGenericTypeDefinition(genericTypeDefinition);
            }
        }

        /// <summary>
      /// Returns the generic type definition which type is based on, or nothing, if no generic type definition could be found
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Type FindGenericTypeDefinition(this Type type)
        {
            if (type is null || ReferenceEquals(type, typeof(object)))
            {
                return null;
            }
            else if (type.IsGenericType)
            {
                return type.GetGenericTypeDefinition();
            }
            else
            {
                return type.BaseType.FindGenericTypeDefinition();
            }
        }

        /// <summary>
      /// Converts dictionary to IDictionary(Of TKey, TValue)
      /// </summary>
      /// <remarks>dictionary must be a IDictionary(Of) instance and the keys must be of type TKey or derived from TKey
      /// and the values must be of type TValue or derived from TValue</remarks>
        public static IDictionary<TKey, TValue> GeneralizeDictionary<TKey, TValue>(object dictionary)
        {
            // suppose keys are of type string (if not the genericRuntimeHelper will select the correct keyType)
            return GenericRuntimeHelper.GetInstance(typeof(string)).ConvertDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
      /// Returns True if dictionary could be converted to IDictionary(Of TKey, TValue)
      /// </summary>
      /// <param name="dictionary">If successful the value is changed to IDictionary(Of TKey, TValue) type</param>
        public static bool TryConvertDictionary<TKey, TValue>(ref object dictionary)
        {
            // suppose keys are of type string (if not the genericRuntimeHelper will select the correct keyType)
            return GenericRuntimeHelper.GetInstance(typeof(string)).TryConvertDictionary<TKey, TValue>(ref dictionary);
        }
        #endregion

        #region Request-Support
        /// <summary>
      /// Returns the queryStringParameter that corresponds with the name of enumValue
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="enumValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetRequestParameter<TEnum>(TEnum enumValue) where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (enumType.IsEnum)
            {
                return GetRequestParameter(ServiceURIs.GetValues(enumType)[Conversions.ToInteger(enumValue)].Item1);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
      /// Returns the queryStringParameter that corresponds with parameterName
      /// </summary>
      /// <param name="parameterName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string GetRequestParameter(string parameterName)
        {
            var requestParams = ssw.WebOperationContext.Current is null ? null : ssw.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
            if (requestParams is not null)
            {
                string retVal = requestParams[parameterName];
                return string.IsNullOrEmpty(retVal) ? null : retVal;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Returns the query-parameters of the incoming request
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Dictionary<string, string> GetRequestParameters()
        {
            var retVal = new Dictionary<string, string>();

            if (ssw.WebOperationContext.Current is not null)
            {
                var queryParameters = ssw.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
                if (queryParameters is not null && queryParameters.Count > 0)
                {
                    for (int index = 0, loopTo = queryParameters.Count - 1; index <= loopTo; index++)
                    {
                        string key = queryParameters.GetKey(index) ?? "";
                        if (!retVal.ContainsKey(key))
                            retVal.Add(key, queryParameters.Get(index));
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region PWCRemovedHandler-Support
        /// <summary>
      /// Creates WeakListeners to detect the end of a checkedOut-state.
      /// </summary>
        public static void AddPWCRemovedListeners(this EventBus.WeakListenerCallback handler, ref EventBus.WeakListener[] listeners, string absoluteUri, string repositoryId, string pwcId)
        {
            var eventNames = new string[] { EventBus.BuiltInEventNames.EndCancelCheckout, EventBus.BuiltInEventNames.EndCheckIn, EventBus.BuiltInEventNames.EndDeleteObject };
            int length = eventNames.Length;

            lock (handler)
            {
                handler.RemovePWCRemovedListeners(ref listeners);
                listeners = (EventBus.WeakListener[])Array.CreateInstance(typeof(EventBus.WeakListener), length);
                for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                    listeners[index] = EventBus.WeakListener.CreateInstance(handler, absoluteUri, repositoryId, eventNames[index], pwcId);
            }
        }

        /// <summary>
      /// Releases listeners
      /// </summary>
        public static void RemovePWCRemovedListeners(this EventBus.WeakListenerCallback handler, ref EventBus.WeakListener[] listeners)
        {
            lock (handler)
            {
                if (listeners is not null)
                {
                    for (int index = 0, loopTo = listeners.Length - 1; index <= loopTo; index++)
                    {
                        var listener = listeners[index];
                        if (listener is not null)
                            listener.RemoveListener();
                    }
                    listeners = null;
                }
            }
        }
        #endregion

        /// <summary>
      /// Copies given array starting with the element on position index
      /// </summary>
      /// <typeparam name="TItem"></typeparam>
      /// <param name="array"></param>
      /// <param name="index"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static TItem[] Copy<TItem>(this TItem[] array, int index = 0)
        {
            int length = Math.Max(0, array is null ? 0 : array.Length - Math.Max(0, index));
            TItem[] retVal = (TItem[])Array.CreateInstance(typeof(TItem), length);

            if (length > 0)
                array.CopyTo(retVal, index);
            return retVal;
        }
        private static System.Text.RegularExpressions.MatchEvaluator _CreateRegExPattern_matchEvaluator = new System.Text.RegularExpressions.MatchEvaluator(CreateRegExPatternReplace);

        /// <summary>
      /// Creates valid regular expression pattern for text
      /// </summary>
      /// <param name="text"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string CreateRegExPattern(string text)
        {
            var regEx = new System.Text.RegularExpressions.Regex(@"((?<CrLf>(\r\n|\r|\n))|(?<Blank>\s)|\\|\/|\.|\*|\+|\-|\(|\)|\[|\]|\<|\>|\||\?|\$|\{|\}|\^)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(text, _CreateRegExPattern_matchEvaluator);
        }
        private static string CreateRegExPatternReplace(System.Text.RegularExpressions.Match match)
        {
            var grBlank = match.Groups["Blank"];
            var grCrLf = match.Groups["CrLf"];
            if (grBlank is not null && grBlank.Success)
            {
                return @"\s";
            }
            else if (grCrLf is not null && grCrLf.Success)
            {
                switch (grCrLf.Value ?? "")
                {
                    case Microsoft.VisualBasic.Constants.vbLf:
                        {
                            return @"\n";
                        }
                    case Microsoft.VisualBasic.Constants.vbCr:
                        {
                            return @"\r";
                        }

                    default:
                        {
                            return @"(\r\n|\r|\n)";
                        }
                }
            }
            else
            {
                return @"\" + match.Value;
            }
        }

        /// <summary>
      /// Creates a typesafe array of type
      /// </summary>
        public static Array CreateValuesArray(this Type type, params object[] values)
        {
            int length = values is null ? 0 : values.Length;
            var retVal = Array.CreateInstance(type, length);

            if (length > 0)
            {
                for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                    retVal.SetValue(values[index].TryCastDynamic(type, null), index);
            }

            return retVal;
        }

        private static string _defaultLogFile;

        /// <summary>
      /// Mapping between the used namespaces in the cmis-environment and their prefixes in
      /// xml - documents.
      /// </summary>
      /// <remarks></remarks>
        private readonly static Dictionary<string, string> _defaultNamespacePrefixes;
        public static string GetDefaultPrefix(string namespaceUri, ref int unknownUriIndex)
        {
            string GetDefaultPrefixRet = default;
            if (string.IsNullOrEmpty(namespaceUri))
            {
                return null;
            }
            else
            {
                string key = namespaceUri.ToLowerInvariant();

                if (_defaultNamespacePrefixes.ContainsKey(key))
                {
                    return _defaultNamespacePrefixes[key];
                }
                else
                {
                    GetDefaultPrefixRet = "ns" + unknownUriIndex;
                    unknownUriIndex += 1;
                }
            }

            return GetDefaultPrefixRet;
        }

        /// <summary>
      /// If the directory doesn't exist, the method tries to create it and
      /// returns True, if the directoryPath points to an existing directory.
      /// </summary>
      /// <param name="directoryPath"></param>
      /// <remarks></remarks>
        public static bool EnsureDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath) || System.IO.Directory.Exists(directoryPath))
            {
                // if directoryPath = "" then the current directory is meant
                return true;
            }
            else if (EnsureDirectory(System.IO.Path.GetDirectoryName(directoryPath)))
            {
                // parent directory exists
                try
                {
                    return System.IO.Directory.CreateDirectory(directoryPath) is not null;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static Dictionary<Type, Type> _jsonTypes = new Dictionary<Type, Type>() { { typeof(DateTime), typeof(long) }, { typeof(DateTimeOffset), typeof(long) }, { typeof(DateTime?), typeof(long?) }, { typeof(DateTimeOffset?), typeof(long?) }, { typeof(DateTime[]), typeof(long[]) }, { typeof(DateTimeOffset[]), typeof(long[]) } };
        /// <summary>
      /// Returns the System.Type which is used to represent type in JavaScript-serialization
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Type GetJSONType(Type type)
        {
            lock (_jsonTypes)
                return _jsonTypes.ContainsKey(type) ? _jsonTypes[type] : type;
        }

        /// <summary>
      /// Used namespaces in the cmis-environment
      /// </summary>
      /// <remarks></remarks>
        private static Dictionary<sx.XmlQualifiedName, string> _namespaces = new Dictionary<sx.XmlQualifiedName, string>() { { new sx.XmlQualifiedName("cmis", Namespaces.xmlns), Namespaces.cmis }, { new sx.XmlQualifiedName("cmism", Namespaces.xmlns), Namespaces.cmism }, { new sx.XmlQualifiedName("atom", Namespaces.xmlns), Namespaces.atom }, { new sx.XmlQualifiedName("app", Namespaces.xmlns), Namespaces.app }, { new sx.XmlQualifiedName("cmisra", Namespaces.xmlns), Namespaces.cmisra }, { new sx.XmlQualifiedName("xsi", Namespaces.xmlns), Namespaces.w3instance }, { new sx.XmlQualifiedName("cmisl", Namespaces.xmlns), Namespaces.cmisl }, { new sx.XmlQualifiedName("cmisw", Namespaces.xmlns), Namespaces.cmisw }, { new sx.XmlQualifiedName("com", Namespaces.xmlns), Namespaces.com }, { new sx.XmlQualifiedName("alf", Namespaces.xmlns), Namespaces.alf }, { new sx.XmlQualifiedName("browser", Namespaces.xmlns), Namespaces.browser } };
        public static Dictionary<sx.XmlQualifiedName, string> CmisNamespaces
        {
            get
            {
                return get_CmisNamespaces("cmis", "cmism", "atom", "app", "cmisra", "xsi");
            }
        }
        public static Dictionary<sx.XmlQualifiedName, string> get_CmisNamespaces(params string[] namespaces)
        {
            if (namespaces is null || namespaces.Length == 0)
            {
                return _namespaces;
            }
            else
            {
                var verify = new HashSet<string>(namespaces);
                return (from de in _namespaces
                        where verify.Contains(de.Key.Name)
                        select de).ToDictionary(item => item.Key, item => item.Value);
            }
        }

        /// <summary>
      /// Returns True if type is nullable
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool IsNullableType(this Type type)
        {
            return type.IsValueType && type.IsGenericType && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(bool?).GetGenericTypeDefinition());
        }

        /// <summary>
      /// Logs a message into the logfile configured in AppSettings("LogFile")
      /// </summary>
      /// <param name="message"></param>
      /// <remarks></remarks>
        public static void LogMessage(string message)
        {
            if (!string.IsNullOrEmpty(_defaultLogFile) && !string.IsNullOrEmpty(message))
            {
                try
                {
                    string prefix = DateTime.Now.ToString() + " ";
                    string indent = new string(' ', prefix.Length);

                    System.IO.File.AppendAllText(_defaultLogFile, prefix + message.Replace(Microsoft.VisualBasic.Constants.vbNewLine, indent + Microsoft.VisualBasic.Constants.vbNewLine) + Microsoft.VisualBasic.Constants.vbNewLine);
                }
                catch
                {
                }
            }
        }

        /// <summary>
      /// Logs an error into the logfile configured in AppSettings("LogFile")
      /// </summary>
      /// <param name="ex"></param>
      /// <remarks></remarks>
        public static void LogError(Exception ex)
        {
            string indent = "";
            var sb = new System.Text.StringBuilder();

            while (ex is not null)
            {
                sb.AppendLine(indent + ex.Message.Replace(Microsoft.VisualBasic.Constants.vbNewLine, Microsoft.VisualBasic.Constants.vbNewLine + indent));
                sb.AppendLine(indent + "StackTrace:");
                indent += "  ";
                sb.AppendLine(indent + ex.StackTrace.Replace(Microsoft.VisualBasic.Constants.vbNewLine, Microsoft.VisualBasic.Constants.vbNewLine + indent));
                ex = ex.InnerException;
            }
            if (sb.Length > 0)
                sb.Length -= Microsoft.VisualBasic.Constants.vbNewLine.Length;
            LogMessage(sb.ToString());
        }

        /// <summary>
      /// Returns a non nullOrEmpty-String if available
      /// Preference: arg1, arg2
      /// </summary>
      /// <param name="arg1"></param>
      /// <param name="arg2"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string NVL(this string arg1, string arg2)
        {
            if (!string.IsNullOrEmpty(arg1))
            {
                return arg1;
            }
            else
            {
                return arg2 ?? arg1;
            }
        }
        /// <summary>
        /// Returns a non nullOrEmpty-String if available
        /// Preference: arg1, arg2, alternate
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="alternate"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NVL(string arg1, string arg2, string alternate)
        {
            if (!string.IsNullOrEmpty(arg1))
                return arg1;
            else
                return NVL(arg2, alternate) ?? arg1;
        }

        /// <summary>
        /// Returns instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <remarks>Useful in With-End With-expressions</remarks>
        public static T Self<T>(this T instance)
        {
            return instance;
        }

        /// <summary>
      /// Returns a typesafe PropertyChangedEventArgs-object
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <param name="propertyName"></param>
      /// <param name="newValue"></param>
      /// <param name="oldValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ComponentModel.PropertyChangedEventArgs ToPropertyChangedEventArgs<TProperty>(this string propertyName, TProperty newValue, TProperty oldValue)
        {
            return new ComponentModel.Generic.PropertyChangedEventArgs<TProperty>(propertyName, newValue, oldValue);
        }

        private static Dictionary<Type, Dictionary<Type, bool>> _tryCastDynamics = new Dictionary<Type, Dictionary<Type, bool>>();
        /// <summary>
      /// Try to convert value to targetType
      /// </summary>
        public static object TryCastDynamic(this object value, Type targetType, object defaultValue = null)
        {
            var sourceType = value is null ? typeof(void) : value.GetType();

            if (value is not null && value.GetType().IsArray && targetType.IsArray)
            {
                var targetElementType = targetType.GetElementType();
                var sourceElementType = sourceType.GetElementType();

                if (ReferenceEquals(sourceElementType, targetElementType))
                {
                    // nothing to do
                    return value;
                }
                else
                {
                    // create a new array type
                    Array source = (Array)value;
                    int length = source.Length;
                    var retVal = Array.CreateInstance(targetElementType, length);

                    for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                        retVal.SetValue(source.GetValue(index).TryCastDynamic(targetElementType), index);
                    return retVal;
                }
            }
            else
            {
                // try to convert via CTypeDynamic
                // if it is the first try to convert from sourceType to targetType
                // the framework stores if an exception was thrown when CTypeDynamic
                // was called. In this case this function will not allow further
                // tries in the future.
                Dictionary<Type, bool> tryCastDynamics;
                bool canConvert;

                // minimize lock time for all types
                lock (_tryCastDynamics)
                {
                    if (_tryCastDynamics.ContainsKey(sourceType))
                    {
                        tryCastDynamics = _tryCastDynamics[sourceType];
                    }
                    else
                    {
                        tryCastDynamics = new Dictionary<Type, bool>();
                        _tryCastDynamics.Add(sourceType, tryCastDynamics);
                    }
                }
                // minimize lock time for targetType
                lock (tryCastDynamics)
                {
                    if (!tryCastDynamics.ContainsKey(targetType))
                    {
                        // optimistic
                        tryCastDynamics.Add(targetType, true);
                        canConvert = true;
                    }
                    else
                    {
                        canConvert = tryCastDynamics[targetType];
                    }
                }

                if (canConvert)
                {
                    try
                    {
                        return Conversion.CTypeDynamic(value, targetType);
                    }
                    catch
                    {
                        if (ReferenceEquals(targetType, typeof(DateTimeOffset)) && value is DateTime)
                        {
                            return defaultValue;
                        }
                        // if at least one try fails, there will be
                        // no conversations from sourceType to
                        // targetType in future calls
                        lock (tryCastDynamics)
                            tryCastDynamics[targetType] = false;
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
      /// Try to convert value to targetType
      /// </summary>
        public static TResult TryCastDynamic<TResult>(this object value, TResult defaultValue = default)
        {
            return Conversions.ToGenericParameter<TResult>(value.TryCastDynamic(typeof(TResult), defaultValue));
        }

        /// <summary>
      /// Unwraps the content of the given xmlDoc instance if the DocumentElement contains a base64-string
      /// </summary>
      /// <param name="xmlDoc"></param>
      /// <remarks></remarks>
        public static void UnWrap(this sx.XmlDocument xmlDoc)
        {
            if (xmlDoc is not null && xmlDoc.DocumentElement is not null && string.Compare(xmlDoc.DocumentElement.Name, "Binary", true) == 0)
            {
                try
                {
                    xmlDoc.LoadXml(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(xmlDoc.DocumentElement.InnerText)));
                }
                catch
                {
                }
            }
        }

        /// <summary>
      /// Query names MUST NOT contain " ", ",", """, "'", "\", ".", "(" or ")". This function eliminates these chars.
      /// </summary>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string ValidateQueryName(this string queryName)
        {
            if (string.IsNullOrEmpty(queryName))
            {
                return queryName;
            }
            else
            {
                return new System.Text.RegularExpressions.Regex(@"[\s,""'\\\.\(\)]", System.Text.RegularExpressions.RegexOptions.Singleline).Replace(queryName, "");
            }
        }

    }
}