using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sw = System.Web;
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
using ccs = CmisObjectModel.Core.Security;
using Microsoft.VisualBasic.CompilerServices;
using System.Reflection;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON
{
    /// <summary>
   /// Class handles Multipart-content in browser binding
   /// </summary>
   /// <remarks></remarks>
    public class MultipartFormDataContent : HttpContent
    {

        #region Constructors
        public MultipartFormDataContent(string contentType) : base(null)
        {

            if (!string.IsNullOrEmpty(contentType))
            {
                var match = _regExContentType.Match(contentType);

                if (match is not null && match.Success)
                {
                    var grBoundary = match.Groups["Boundary"];
                    var grUrlEncoded = match.Groups["UrlEncoded"];
                    var grCharset = match.Groups[charsetGroupName];

                    IsUrlEncoded = grUrlEncoded is not null && grUrlEncoded.Success;
                    Boundary = grBoundary is not null && grBoundary.Success ? grBoundary.Value : IsUrlEncoded ? string.Empty : Guid.NewGuid().ToString();
                    if (grCharset is not null && grCharset.Success)
                    {
                        try
                        {
                            UrlEncoding = System.Text.Encoding.GetEncoding(grCharset.Value);
                        }
                        catch
                        {
                        }
                    }
                    ContentType = contentType;
                }
            }
        }

        public MultipartFormDataContent(string contentType, string cmisAction) : this(ProcessContentType(contentType, cmisAction))
        {
            Add("cmisaction", cmisAction);
        }

        public MultipartFormDataContent(string contentType, string cmisAction, CmisObjectModel.Messaging.Requests.RequestBase request) : this(contentType, cmisAction)
        {
            if (request.BrowserBinding.Succinct)
                Add("succinct", CommonFunctions.Convert(request.BrowserBinding.Succinct));
        }

        private static string ProcessContentType(string contentType, string cmisAction)
        {
            if (string.Compare(contentType, Constants.MediaTypes.MultipartFormData, true) == 0)
            {
                string affix = "CmisObjectModel" + Guid.NewGuid().ToString("N");
                return contentType + "; boundary=" + affix + cmisAction + affix;
            }
            else
            {
                return contentType;
            }
        }
        #endregion

        #region Helper-classes
        /// <summary>
      /// A simple multi dimensional matrix
      /// </summary>
      /// <remarks></remarks>
        public class Matrix<T> : List<Matrix<T>>
        {

            /// <summary>
         /// Encapsulates value with a Matrix(Of T) instance and puts it to the end of the list
         /// </summary>
            public Matrix<T> Add(T value)
            {
                var retVal = new Matrix<T>() { _value = value };

                Add(retVal);
                return retVal;
            }

            /// <summary>
         /// Encapsulates values with Matrix(Of T) instances and puts them to the end of the list
         /// </summary>
         /// <param name="values"></param>
         /// <remarks></remarks>
            public void AddRange(IEnumerable<T> values)
            {
                if (values is not null)
                {
                    AddRange(from value in values
                             select new Matrix<T>() { _value = value });
                }
            }

            /// <summary>
         /// Ensures a valid item at given position
         /// </summary>
            private Matrix<T> GetSafeItem(int index)
            {
                Matrix<T> retVal;

                if (Count <= index)
                {
                    AddRange((Matrix<T>[])Array.CreateInstance(typeof(Matrix<T>), 1 + index - Count));
                }
                retVal = base[index];
                if (retVal is null)
                {
                    retVal = new Matrix<T>();
                    base[index] = retVal;
                }

                return retVal;
            }

            /// <summary>
         /// Returns the values of all items
         /// </summary>
            public T[] get_ItemsValues(params int[] indexes)
            {
                var instance = this;

                if (indexes is not null)
                {
                    foreach (int index in indexes)
                        instance = instance.GetSafeItem(index);
                }

                if (instance.Count == 0)
                {
                    return null;
                }
                else
                {
                    return (from matrix in instance
                            let value = matrix is null ? default : matrix._value
                            select value).ToArray();
                }
            }

            private T _value;
            public T get_Value(params int[] indexes)
            {
                var instance = this;

                if (indexes is not null)
                {
                    foreach (int index in indexes)
                        instance = instance.GetSafeItem(index);
                }
                return instance._value;
            }

            public void set_Value(int index, T value)
            {
                var instance = this;
                instance = instance.GetSafeItem(index);

                instance._value = value;
            }
        }
        #endregion

        private Dictionary<Enums.enumCollectionAction, Matrix<string>> _aces;
        private CmisObjectModel.Core.enumACLPropagation? _aclPropagation;

        /// <summary>
      /// Adds another content
      /// </summary>
      /// <param name="value"></param>
      /// <remarks></remarks>
        public Exception Add(HttpContent value)
        {
            string key = value is null ? null : value.ContentDisposition;
            string lowerKey = (key ?? string.Empty).ToLowerInvariant();

            if (value is null)
            {
                return new ArgumentNullException("value", "Parameter MUST NOT be null.");
            }
            else if (string.IsNullOrEmpty(key))
            {
                return new ArgumentException("Headers", "value.Headers MUST contain '" + RFC2231Helper.ContentDispositionHeaderName + "' or the value MUST NOT be null or empty.");
            }
            else if (_contentMap.ContainsKey(lowerKey))
            {
                return new ArgumentException("Content '" + key + "' already exists.");
            }
            else
            {
                _contents.Add(value);
                _contentMap.Add(lowerKey, value);
                return null;
            }
        }

        /// <summary>
      /// Adds a simple parameter
      /// </summary>
        public Exception Add(string parameterName, string parameterValue)
        {
            var content = new HttpContent(string.IsNullOrEmpty(parameterValue) ? (new byte[] { }) : System.Text.Encoding.UTF8.GetBytes(parameterValue));

            content.Headers.Add(RFC2231Helper.ContentDispositionHeaderName, "form-data; name=\"" + parameterName + "\"");
            content.Headers.Add(RFC2231Helper.ContentTypeHeaderName, Constants.MediaTypes.PlainText + "; charset=utf-8");
            return Add(content);
        }

        /// <summary>
      /// Adds extensions of cmisPropertiesType
      /// </summary>
        public void Add(CmisObjectModel.Core.cmisObjectType.PropertiesExtensions propertiesExtensions)
        {
            var serializer = new Serialization.JavaScriptSerializer();
            Add("propertiesExtension", serializer.Serialize(propertiesExtensions));
        }

        /// <summary>
      /// Adds a property
      /// </summary>
      /// <remarks>Follows "5.4.4.3.11 Single-value Properties" and "5.4.4.3.12 Multi-value Properties"</remarks>
        public void Add(CmisObjectModel.Core.Properties.cmisProperty property)
        {
            if (_properties is null)
                _properties = new Matrix<string>();

            string propertyDefinitionId = property.PropertyDefinitionId;
            int index = _properties.Count;
            var propertyType = property.PropertyType;
            // JSON transfers date properties as longs
            var grh = GenericRuntimeHelper.GetInstance(CommonFunctions.GetJSONType(propertyType));
            Func<object, string> fnConvert = value =>
   {
       if (value is DateTimeOffset)
       {
           value = ((DateTimeOffset)value).DateTime.ToJSONTime();
       }
       else if (value is DateTimeOffset?)
       {
           {
               var withBlock = (DateTimeOffset?)value;
               if (withBlock.HasValue)
               {
                   value = withBlock.Value.DateTime.ToJSONTime();
               }
               else
               {
                   value = default;
               }
           }
       }
       else if (value is DateTime)
       {
           value = Conversions.ToDate(value).ToJSONTime();
       }
       else if (value is DateTime?)
       {
           {
               var withBlock1 = (DateTime?)value;
               if (withBlock1.HasValue)
               {
                   value = withBlock1.Value.ToJSONTime();
               }
               else
               {
                   value = default;
               }
           }
       }
       if (!(value is null || value is string))
           value = grh.Convert(value);

       return Conversions.ToString(value);
   };
            var values = property.Values is null ? null : (from value in property.Values
                                                           select fnConvert(value)).ToList();

            _properties.Add(propertyDefinitionId).AddRange(values);
            Add("propertyId[" + index + "]", propertyDefinitionId);
            if (values is not null)
            {
                string prefix = "propertyValue[" + index + "]";

                if (property.Cardinality == CmisObjectModel.Core.enumCardinality.multi || values.Count > 1)
                {
                    for (int subIndex = 0, loopTo = values.Count - 1; subIndex <= loopTo; subIndex++)
                        Add(prefix + "[" + subIndex + "]", values[subIndex]);
                }
                else if (values.Count == 1)
                {
                    Add(prefix, values[0]);
                }
            }
        }

        /// <summary>
      /// Adds a typedefinition
      /// </summary>
      /// <param name="type"></param>
      /// <remarks>chapter 5.4.4.3.31 Type in cmis documentation</remarks>
        public void Add(CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType type)
        {
            var serializer = new Serialization.JavaScriptSerializer();
            Add("type", serializer.Serialize(type));
        }

        /// <summary>
      /// Adds information about a secondary type id
      /// </summary>
        public void Add(string secondaryTypeId, Enums.enumCollectionAction action)
        {
            List<string> secondaryTypeIds;

            if (_secondaryTypeIds is null)
                _secondaryTypeIds = new Dictionary<Enums.enumCollectionAction, List<string>>();
            secondaryTypeIds = GetSafeDictionaryValue(_secondaryTypeIds, action);
            Add(action.GetName() + "SecondaryTypeId[" + secondaryTypeIds.Count + "]", secondaryTypeId);
            secondaryTypeIds.Add(secondaryTypeId);
        }

        /// <summary>
      /// Adds information about an accessControlEntry
      /// </summary>
        public void Add(ccs.cmisAccessControlEntryType ace, Enums.enumCollectionAction action)
        {
            if (ace is not null && ace.Principal is not null)
            {


                if (_aces is null)
                    _aces = new Dictionary<Enums.enumCollectionAction, Matrix<string>>();

                var aces = GetSafeDictionaryValue(_aces, action);
                string prefix = action.GetName() + "ACEPermission[" + aces.Count + "]";
                var permissions = ace.Permissions ?? (new string[] { });

                Add(action.GetName() + "ACEPrincipal[" + aces.Count + "]", ace.Principal.PrincipalId);
                for (int index = 0, loopTo = permissions.Length; index <= loopTo; index++)
                    Add(prefix + "[" + index + "]", permissions[index]);
                aces.Add(ace.Principal.PrincipalId).AddRange(permissions);
            }
        }

        /// <summary>
      /// Adds aclPropagation information
      /// </summary>
      /// <param name="propagation"></param>
      /// <remarks></remarks>
        public void Add(CmisObjectModel.Core.enumACLPropagation propagation)
        {
            _aclPropagation = propagation;
            Add("ACLPropagation", propagation.GetName());
        }

        /// <summary>
      /// Adds autoindexed value-information
      /// </summary>
        public void Add(string value, Enums.enumValueType type)
        {
            List<string> autoIndexedValues;

            if (_autoIndexedValues is null)
                _autoIndexedValues = new Dictionary<Enums.enumValueType, List<string>>();
            autoIndexedValues = GetSafeDictionaryValue(_autoIndexedValues, type);
            Add(type.GetName() + "[" + autoIndexedValues.Count + "]", value);
            autoIndexedValues.Add(value);
        }

        private Dictionary<Enums.enumValueType, List<string>> _autoIndexedValues;
        public readonly string Boundary;

        /// <summary>
      /// List of embedded contents
      /// </summary>
      /// <remarks></remarks>
        private readonly List<HttpContent> _contents = new List<HttpContent>();
        private readonly Dictionary<string, HttpContent> _contentMap = new Dictionary<string, HttpContent>();
        public HttpContent get_Content(string key)
        {
            if (key is null)
            {
                return null;
            }
            else
            {
                key = key.ToLowerInvariant();
                return _contentMap.ContainsKey(key) ? _contentMap[key] : null;
            }
        }
        public HttpContent[] Contents
        {
            get
            {
                return _contents.ToArray();
            }
        }

        /// <summary>
      /// Searches all contents for aces, aclPropagation, autoIndexed values, properties and secondary type ids
      /// </summary>
      /// <remarks></remarks>
        private void ExtractAll()
        {
            var regEx = new System.Text.RegularExpressions.Regex(@"((?<action>\S+)(?<Type>(ACEPrincipal|SecondaryTypeId))\[(?<Index>\d+)\]" + @"|(?<action>\S+)(?<Type>ACEPermission)\[(?<Index>\d+)\]\[(?<SubIndex>\d+)\]" + @"|(?<Type>[^\[]+)\[(?<Index>\d+)\](\[(?<SubIndex>\d+)\])?" + "|(?<Type>(ACLPropagation|PropertiesExtension|Type))" + ")", System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            _aces = new Dictionary<Enums.enumCollectionAction, Matrix<string>>();
            _autoIndexedValues = new Dictionary<Enums.enumValueType, List<string>>();
            _properties = new Matrix<string>();
            _secondaryTypeIds = new Dictionary<Enums.enumCollectionAction, List<string>>();
            _typeDefinition = string.Empty;

            var valueType = default(Enums.enumValueType);
            foreach (KeyValuePair<string, HttpContent> de in _contentMap)
            {
                var match = regEx.Match(de.Key);

                if (match is not null && match.Success)
                {
                    var action = Enums.enumCollectionAction.add;
                    int index = 0;
                    int subIndex = 0;
                    System.Text.RegularExpressions.Group group;
                    string value = de.Value.ToString();

                    group = match.Groups["action"];
                    if (group is not null && group.Success)
                        CommonFunctions.TryParse(group.Value, ref action, true);
                    group = match.Groups["Index"];
                    if (group is not null && group.Success)
                        index = Conversions.ToInteger(group.Value);
                    group = match.Groups["SubIndex"];
                    if (group is not null && group.Success)
                        subIndex = Conversions.ToInteger(group.Value);

                    switch (match.Groups["Type"].Value.ToLowerInvariant() ?? "")
                    {
                        case "aceprincipal":
                            {
                                GetSafeDictionaryValue(_aces, action).set_Value(index, value);
                                break;
                            }
                        case "acepermission":
                            {
                                GetSafeDictionaryValue(_aces, action).set_Value(index, value: value);
                                break;
                            }
                        case "aclpropagation":
                            {
                                _aclPropagation = (CmisObjectModel.Core.enumACLPropagation?)Enum.Parse(typeof(CmisObjectModel.Core.enumACLPropagation), value);
                                break;
                            }
                        case "propertiesextension":
                            {
                                _propertiesExtensions = value ?? string.Empty;
                                break;
                            }
                        case "propertyid":
                            {
                                _properties.set_Value(index, value);
                                break;
                            }
                        case "propertyvalue":
                            {
                                _properties.set_Value(index, value:value);
                                break;
                            }
                        case "secondarytypeid":
                            {
                                SetAt(GetSafeDictionaryValue(_secondaryTypeIds, action), value, index);
                                break;
                            }
                        case "type":
                            {
                                // auto indexed values
                                _typeDefinition = value ?? string.Empty;
                                break;
                            }

                        default:
                            {
                                //A reprendre 
                                /*string key = "ph16bbbc88f741405f871096b494ecc30a";
                                if (CommonFunctions.TryParse(match.Groups["Type"].Value, ref key, true))
                                {
                                    SetAt(this.GetSafeDictionaryValue(_autoIndexedValues, key), value, index);
                                }*/

                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
      /// Extracts multipart content from stream
      /// </summary>
      /// <param name="stream"></param>
      /// <param name="contentType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static MultipartFormDataContent FromStream(System.IO.Stream stream, string contentType)
        {
            var retVal = new MultipartFormDataContent(contentType);

            if (!(stream is null || string.IsNullOrEmpty(contentType)))
            {
                if (retVal.IsUrlEncoded)
                {
                    // simple format: urlencoded
                    using (var ms = new System.IO.MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ms.Position = 0L;
                        var nvc = sw.HttpUtility.ParseQueryString(retVal.UrlEncoding.GetString(ms.ToArray()));
                        foreach (string name in nvc)
                            retVal.Add(name, nvc[name]);
                        ms.Close();
                    }
                }
                else
                {
                    // multipart/form-data
                    // 8-bit codepage (may interpret utf-8 chars wrongly)
                    var cp850 = System.Text.Encoding.GetEncoding(850);
                    // ensure same "mis-interpretation"
                    string boundary = cp850.GetString(System.Text.Encoding.UTF8.GetBytes("--" + retVal.Boundary));
                    const string newLinePattern = @"(\r\n|\n)";
                    const string headersPattern = @"((?<headerId>[^:\r\n]+):\s?(?<headerValue>[^\r\n]+)" + newLinePattern + ")+";
                    const string valuePattern = @"(?<value>[\s\S]*?)";
                    string boundaryPattern = CommonFunctions.CreateRegExPattern(boundary);
                    string pattern = "(?<=" + boundaryPattern + newLinePattern + ")" + headersPattern + newLinePattern + valuePattern + "(?=(" + newLinePattern + ")?" + boundaryPattern + ")";
                    byte[] data;
                    var regEx = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.Multiline);

                    using (var ms = new System.IO.MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ms.Position = 0L;
                        data = ms.ToArray();
                        string content = cp850.GetString(data);

                        foreach (System.Text.RegularExpressions.Match match in regEx.Matches(content))
                        {
                            byte[] value = (byte[])Array.CreateInstance(typeof(byte), match.Groups["value"].Length);
                            var httpContent = new HttpContent(value);
                            var headerIds = match.Groups["headerId"].Captures;
                            var headerValues = match.Groups["headerValue"].Captures;

                            if (value.Length > 0)
                                Array.Copy(data, match.Groups["value"].Index, value, 0, value.Length);
                            for (int index = 0, loopTo = Math.Min(headerIds.Count, headerValues.Count) - 1; index <= loopTo; index++)
                            {
                                string headerId = System.Text.Encoding.UTF8.GetString(data, headerIds[index].Index, headerIds[index].Length);
                                string headerValue = System.Text.Encoding.UTF8.GetString(data, headerValues[index].Index, headerValues[index].Length);
                                httpContent.Headers[headerId] = headerValue;
                            }
                            retVal.Add(httpContent);
                        }
                        ms.Close();
                    }
                }
            }

            return retVal;
        }

        /// <summary>
      /// Returns the aces (add or remove) stored in this instance
      /// </summary>
        public ccs.cmisAccessControlListType GetACEs(Enums.enumCollectionAction action)
        {
            // evaluate HttpContents
            if (_aces is null)
                ExtractAll();
            if (_aces.ContainsKey(action))
            {
                return new ccs.cmisAccessControlListType() { Permissions = (from ace in _aces[action] let principal = new ccs.cmisAccessControlPrincipalType() { PrincipalId = ace.get_Value() } let permissions = ace.get_ItemsValues() select new ccs.cmisAccessControlEntryType() { Principal = principal, Permissions = permissions }).ToArray() };
            }
            else
            {
                return null;
            }
        }


        /// <summary>
      /// Returns the aclPropagation stored in this instance
      /// </summary>
        public CmisObjectModel.Core.enumACLPropagation? GetACLPropagation()
        {
            // evaluate HttpContents (an empty _autoIndexedValues signals ExtractAll() has not been called at this moment)
            if (_autoIndexedValues is null)
                ExtractAll();
            return _aclPropagation;
        }

        /// <summary>
      /// Returns auto indexed values like changeTokens or policies stored in this instance
      /// </summary>
        public string[] GetAutoIndexedValues(Enums.enumValueType type)
        {
            // evaluate HttpContents
            if (_autoIndexedValues is null)
                ExtractAll();
            if (_autoIndexedValues.ContainsKey(type))
            {
                var autoIndexedValues = _autoIndexedValues[type];
                return autoIndexedValues is null ? null : autoIndexedValues.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Returns the properties stored in this instance
      /// </summary>
      /// <remarks></remarks>
        public CmisObjectModel.Core.Collections.cmisPropertiesType GetProperties(Func<string, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType> fnGetTypeDefinition, params CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType[] typeDefinitions)
        {
            {
                var withBlock = new Dictionary<string, CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType>();
                string[] values;
                var properties = new Dictionary<string, string[]>();

                // evaluate HttpContents
                if (_properties is null)
                    ExtractAll();
                // create an index for quick property access
                foreach (Matrix<string> matrix in _properties)
                {
                    if (!properties.ContainsKey(matrix.get_Value()))
                        properties.Add(matrix.get_Value(), matrix.get_ItemsValues());
                }
                // object type id and secondary type ids
                foreach (string propertyName in new string[] { Constants.CmisPredefinedPropertyNames.ObjectTypeId, Constants.CmisPredefinedPropertyNames.SecondaryObjectTypeIds })
                {
                    if (properties.ContainsKey(propertyName))
                    {
                        values = properties[propertyName];
                        if (values is not null)
                        {
                            foreach (string value in values)
                            {
                                if (!(string.IsNullOrEmpty(value) || withBlock.ContainsKey(value)))
                                {
                                    var typeDefinition = fnGetTypeDefinition(value);
                                    if (typeDefinition is not null)
                                        withBlock.Add(value, typeDefinition);
                                }
                            }
                        }
                    }
                }
                // append typedefinitions
                if (typeDefinitions is not null)
                {
                    foreach (CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType td in typeDefinitions)
                    {
                        if (!(td is null || withBlock.ContainsKey(td.Id)))
                            withBlock.Add(td.Id, td);
                    }
                }

                return GetProperties(withBlock.Values.ToArray());
            }
        }

        /// <summary>
        /// Returns the properties stored in this instance
        /// </summary>
        /// <remarks>Overload object changed</remarks>
        private CmisObjectModel.Core.Collections.cmisPropertiesType GetProperties(params CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType[] typeDefinitions)
        {
            List<CmisObjectModel.Core.Properties.cmisProperty> properties = new List<CmisObjectModel.Core.Properties.cmisProperty>();
            CmisObjectModel.Extensions.Extension[] extensions = null;
            Dictionary<string, CmisObjectModel.Core.Definitions.Properties.cmisPropertyDefinitionType> propertyDefinitions = new Dictionary<string, CmisObjectModel.Core.Definitions.Properties.cmisPropertyDefinitionType>();

            // evaluate HttpContents
            if (_properties == null)
                ExtractAll();
            if (typeDefinitions != null)
            {
                foreach (CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType td in typeDefinitions)
                {
                    foreach (KeyValuePair<string, CmisObjectModel.Core.Definitions.Properties.cmisPropertyDefinitionType> de in td.GetPropertyDefinitions(enumKeySyntax.lowerCase))
                    {
                        if (!propertyDefinitions.ContainsKey(de.Key))
                            propertyDefinitions.Add(de.Key, de.Value);
                    }
                }
            }
            foreach (Matrix<string> matrix in _properties)
            {
                string key = matrix.get_Value().ToLowerInvariant();
                CmisObjectModel.Core.Properties.cmisProperty cmisProperty = propertyDefinitions.ContainsKey(key) ? propertyDefinitions[key].CreateProperty() : new CmisObjectModel.Core.Properties.cmisPropertyString();
                Type propertyType = cmisProperty.PropertyType;
                // JSON transfers date properties as longs
                Common.GenericRuntimeHelper grh = Common.GenericRuntimeHelper.GetInstance(CommonFunctions.GetJSONType(propertyType));
                Func<string, object> fnConvert = propertyType == typeof(string) ? new Func<string, object>(value => value) : new Func<string, object>(value =>
                {
                    object retVal = grh.ConvertBack(value, null/* TODO Change to default(_) if this is not a reference type */);

                    if (propertyType == typeof(DateTimeOffset) || propertyType == typeof(DateTime))
                    {
                        DateTime? dateValue = retVal is long ? System.Convert.ToInt64(retVal).FromJSONTime() : (DateTime?)null;

                        if (propertyType == typeof(DateTimeOffset))
                            retVal = (DateTimeOffset)dateValue.Value;
                        else
                            retVal = dateValue.Value;
                    }
                    else if (propertyType == typeof(DateTimeOffset?) || propertyType == typeof(DateTime?))
                    {
                        DateTime? dateValue;

                        if (retVal is long?)
                        {
                            {
                                var withBlock = (long?)retVal;
                                if (withBlock.HasValue)
                                    dateValue = withBlock.Value.FromJSONTime();
                                else
                                    dateValue = default(DateTime?);
                            }

                            if (propertyType == typeof(DateTime?))
                                retVal = dateValue;
                            else if (dateValue.HasValue)
                                retVal = (DateTimeOffset?)dateValue.Value;
                            else
                                retVal = (DateTimeOffset?)null/* TODO Change to default(_) if this is not a reference type */;
                        }
                    }

                    return retVal;
                });

                // property values
                if (matrix.Count > 0)
                    cmisProperty.Values = (from value in matrix.get_ItemsValues()
                                           select fnConvert(value)).ToArray();
                properties.Add(cmisProperty);
            }
            if (!string.IsNullOrEmpty(_propertiesExtensions))
            {
                Serialization.JavaScriptSerializer serializer = new Serialization.JavaScriptSerializer();
                CmisObjectModel.Core.cmisObjectType.PropertiesExtensions propertiesExtensions = serializer.Deserialize<CmisObjectModel.Core.cmisObjectType.PropertiesExtensions>(_propertiesExtensions);
                if (propertiesExtensions != null)
                    extensions = propertiesExtensions.Extensions;
            }

            return new CmisObjectModel.Core.Collections.cmisPropertiesType(properties.ToArray()) { Extensions = extensions };
        }

        /// <summary>
        /// Gets or creates a valid value for key
        /// </summary>
        private TValue GetSafeDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                var retVal = new TValue();
                dictionary.Add(key, retVal);
                return retVal;
            }
        }

        /// <summary>
      /// Returns the secondaryTypeIds (add or remove) stored in this instance
      /// </summary>
        public string[] GetSecondaryTypeIds(Enums.enumCollectionAction action)
        {
            // evaluate HttpContents
            if (_secondaryTypeIds is null)
                ExtractAll();
            if (_secondaryTypeIds.ContainsKey(action))
            {
                var secondaryTypeIds = _secondaryTypeIds[action];
                return secondaryTypeIds is null ? null : secondaryTypeIds.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Returns the type definition stored in this instance
      /// </summary>
      /// <returns></returns>
      /// <remarks>chapter 5.4.4.3.31 Type in cmis documentation</remarks>
        public CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType GetTypeDefinition()
        {
            if (_typeDefinition is null)
                ExtractAll();
            if (!string.IsNullOrEmpty(_typeDefinition))
            {
                var serializer = new Serialization.JavaScriptSerializer();
                return serializer.Deserialize<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType>(_typeDefinition);
            }
            else
            {
                return null;
            }
        }

        public readonly bool IsUrlEncoded;
        private Matrix<string> _properties;
        private string _propertiesExtensions;
        private static System.Text.RegularExpressions.Regex _regExContentType = new System.Text.RegularExpressions.Regex("(" + CommonFunctions.CreateRegExPattern(Constants.MediaTypes.MultipartFormData) + @";\sboundary\=(?<Boundary>[\s\S]*)|(?<UrlEncoded>" + CommonFunctions.CreateRegExPattern(Constants.MediaTypes.UrlEncoded) + @"[^;\r\n]*)(;\s*" + charsetPattern + ")?)", System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.Singleline);
        private Dictionary<Enums.enumCollectionAction, List<string>> _secondaryTypeIds;
        private string _typeDefinition;

        /// <summary>
      /// Sets value in list at position index
      /// </summary>
        private void SetAt(List<string> list, string value, int index)
        {
            if (list.Count <= index)
            {
                list.AddRange((string[])Array.CreateInstance(typeof(string), 1 + index - list.Count));
            }
            list[index] = value;
        }

        /// <summary>
      /// Returns ToString()-result from nested HttpContent
      /// </summary>
        public string ToString(string key)
        {
            var content = get_Content(key);
            return content is null ? null : content.ToString();
        }

        public readonly System.Text.Encoding UrlEncoding = System.Text.Encoding.UTF8;

        /// <summary>
      /// Returns Value from nested HttpContent
      /// </summary>
        public new byte[] get_Value(string key)
        {
            var content = get_Content(key);
            return content is null ? null : content.Value;
        }

        protected override void WriteHeaders(System.IO.Stream stream)
        {
            // a multipart-content has no headers to write
        }

        protected override void WriteValue(System.IO.Stream stream)
        {
            if (IsUrlEncoded)
            {
                // write as urlencoded
                if (_contents.Count > 0)
                {
                    var buffer = UrlEncoding.GetBytes(string.Join("&", (from content in _contents
                                                                        where !content.IsBinary
                                                                        select (sw.HttpUtility.UrlPathEncode(content.ContentDisposition) + "=" + sw.HttpUtility.UrlEncode(content.ToString()))).ToArray()));
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            else
            {
                // write as multipart
                var boundaryStart = System.Text.Encoding.UTF8.GetBytes("--" + Boundary + Microsoft.VisualBasic.Constants.vbCrLf);
                var boundaryEnd = System.Text.Encoding.UTF8.GetBytes("--" + Boundary + "--");
                foreach (HttpContent content in Contents)
                {
                    stream.Write(boundaryStart, 0, boundaryStart.Length);
                    content.WriteTo(stream);
                }
                stream.Write(boundaryEnd, 0, boundaryEnd.Length);
            }
        }
    }
}