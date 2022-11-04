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
using CmisObjectModel.Common;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON
{
    /// <summary>
   /// A simple class to store content data separated from multipart content data
   /// </summary>
   /// <remarks></remarks>
    public class HttpContent
    {

        protected const string charsetPattern = @"charset\=(?<" + charsetGroupName + @">[^;\r\n]*)";
        protected const string charsetGroupName = "charset";

        public HttpContent(byte[] rawData)
        {

            Headers = new HeadersCollection(this);

            _toStringResult = () => base.ToString();
            _toStringResultDefault = _toStringResult;
            RawData = rawData;
            _value = rawData;
        }

        #region Helper classes
        /// <summary>
      /// Simple header collection that triggers the owners Reset()-method on content changes
      /// </summary>
      /// <remarks></remarks>
        public class HeadersCollection : IDictionary<string, string>
        {

            public HeadersCollection(HttpContent owner)
            {
                _owner = owner;
            }

            #region IDictionary
            public void Add(KeyValuePair<string, string> item)
            {
                this[item.Key] = item.Value;
            }

            public void Add(string key, string value)
            {
                this[key] = value;
            }

            public void Clear()
            {
                _headers.Clear();
            }

            public bool Contains(KeyValuePair<string, string> item)
            {
                return item.Key is not null && _headers.ContainsKey(item.Key) && (item.Value ?? "") == (_headers[item.Key] ?? "");
            }

            public bool ContainsKey(string key)
            {
                return key is not null && _headers.ContainsKey(key);
            }

            public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
            {
                ((IDictionary<string, string>)_headers).CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get
                {
                    return _headers.Count;
                }
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                return _headers.GetEnumerator();
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => IEnumerable_GetEnumerator();

            public bool IsReadOnly
            {
                get
                {
                    return ((IDictionary<string, string>)_headers).IsReadOnly;
                }
            }

            public string this[string key]
            {
                get
                {
                    return key is not null && _headers.ContainsKey(key) ? _headers[key] : null;
                }
                set
                {
                    if (_headers.ContainsKey(key))
                    {
                        _headers[key] = value;
                    }
                    else
                    {
                        _headers.Add(key, value);
                    }
                    _owner.Reset();
                }
            }

            public ICollection<string> Keys
            {
                get
                {
                    return _headers.Keys;
                }
            }

            public bool Remove(KeyValuePair<string, string> item)
            {
                return Contains(item) && _headers.Remove(item.Key);
            }

            public bool Remove(string key)
            {
                return _headers.Remove(key);
            }

            public bool TryGetValue(string key, out string value)
            {
                return _headers.TryGetValue(key, out value);
            }

            public ICollection<string> Values
            {
                get
                {
                    return _headers.Values;
                }
            }
            #endregion

            private Dictionary<string, string> _headers = new Dictionary<string, string>();
            private HttpContent _owner;
        }
        #endregion

        private static System.Text.RegularExpressions.Regex _contentDispositionRegEx = new System.Text.RegularExpressions.Regex(@"form-data;\s+name=""(?<Name>[^""]*)""", System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        public string ContentDisposition
        {
            get
            {
                lock (_contentDispositionRegEx)
                {
                    string headerRawValue = Headers.ContainsKey(RFC2231Helper.ContentDispositionHeaderName) ? Headers[RFC2231Helper.ContentDispositionHeaderName] : null;
                    if (!string.IsNullOrEmpty(headerRawValue))
                    {
                        var match = _contentDispositionRegEx.Match(headerRawValue);
                        return match is null || !match.Success ? null : match.Groups["Name"].Value;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
      /// Gets or sets the contenttype of this instance
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string ContentType
        {
            get
            {
                return Headers.ContainsKey(RFC2231Helper.ContentTypeHeaderName) ? Headers[RFC2231Helper.ContentTypeHeaderName] : null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Headers.Remove(RFC2231Helper.ContentTypeHeaderName);
                }
                else if (Headers.ContainsKey(RFC2231Helper.ContentTypeHeaderName))
                {
                    Headers[RFC2231Helper.ContentTypeHeaderName] = value;
                }
                else
                {
                    Headers.Add(RFC2231Helper.ContentTypeHeaderName, value);
                }
            }
        }

        /// <summary>
      /// Encodes RawData with given encoding
      /// </summary>
      /// <param name="encoding"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private string GetString(System.Text.Encoding encoding)
        {
            if (RawData is null)
            {
                return null;
            }
            else if (RawData.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return encoding.GetString(RawData);
            }
        }

        public readonly HeadersCollection Headers;

        public bool IsBinary
        {
            get
            {
                return ReferenceEquals(_toStringResult, _toStringResultDefault);
            }
        }

        public readonly byte[] RawData;

        /// <summary>
      /// Sets Value and ToStringResult dependent on header information
      /// </summary>
      /// <remarks></remarks>
        private void Reset()
        {
            string contentType = Headers.ContainsKey(RFC2231Helper.ContentTypeHeaderName) ? Headers[RFC2231Helper.ContentTypeHeaderName] : null;
            string contentTransferEncoding = Headers.ContainsKey(RFC2231Helper.ContentTransferEncoding) ? Headers[RFC2231Helper.ContentTransferEncoding] : null;
            var regExCharset = new System.Text.RegularExpressions.Regex(charsetPattern, System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.Singleline);
            var match = regExCharset.Match(contentType ?? string.Empty);
            string charset = match is null ? null : match.Groups[charsetGroupName].Value;

            switch ((contentTransferEncoding ?? string.Empty).ToLowerInvariant() ?? "")
            {
                case var @case when @case == "":
                    {
                        // text
                        try
                        {
                            var encoding = string.IsNullOrEmpty(charset) ? System.Text.Encoding.GetEncoding(850) : System.Text.Encoding.GetEncoding(charset);
                            string toString = GetString(encoding);
                            _toStringResult = new Func<string>(() => toString);
                        }
                        catch (Exception ex)
                        {
                            _toStringResult = _toStringResultDefault;
                        }
                        _value = RawData;
                        break;
                    }
                case "7bit":
                    {
                        // value 'as is'
                        _value = RawData;
                        // ascii
                        try
                        {
                            string toString = GetString(System.Text.Encoding.ASCII);
                            _toStringResult = new Func<string>(() => toString);
                        }
                        catch (Exception ex)
                        {
                            _toStringResult = _toStringResultDefault;
                        }

                        break;
                    }
                case "8bit":
                    {
                        // value 'as is'
                        _value = RawData;
                        // charset or cp850
                        try
                        {
                            var encoding = string.IsNullOrEmpty(charset) ? System.Text.Encoding.GetEncoding(850) : System.Text.Encoding.GetEncoding(charset);
                            string toString = GetString(encoding);
                            _toStringResult = new Func<string>(() => toString);
                        }
                        catch (Exception ex)
                        {
                            _toStringResult = _toStringResultDefault;
                        }

                        break;
                    }
                case "base64":
                    {
                        try
                        {
                            var encoding = string.IsNullOrEmpty(charset) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(charset);
                            string toString = GetString(encoding);

                            if (RawData is null)
                            {
                                _value = null;
                            }
                            else
                            {
                                _value = Convert.FromBase64String(toString);
                            }
                            _toStringResult = new Func<string>(() => toString);
                        }
                        catch (Exception ex)
                        {
                            _value = RawData;
                            _toStringResult = _toStringResultDefault;
                        }

                        break;
                    }
                case "binary":
                    {
                        // value 'as is'
                        _value = RawData;
                        // no text representation
                        _toStringResult = _toStringResultDefault;
                        break;
                    }
                case "quoted-printable":
                    {
                        try
                        {
                            var encoding = string.IsNullOrEmpty(charset) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(charset);
                            string toString = GetString(encoding);

                            if (RawData is null)
                            {
                                _value = null;
                            }
                            else
                            {
                                System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(\r\n|=(?<Hex>[a-f0-9]{2}))", System.Text.RegularExpressions.RegexOptions.Singleline);
                                System.Text.RegularExpressions.MatchEvaluator evaluator = current =>
                                {
                                    return Strings.Chr(System.Convert.ToInt32(System.Convert.ToUInt32(current.Value, 16))).ToString();
                                };
                                toString = regEx.Replace(toString, evaluator);
                                _value = encoding.GetBytes(toString);
                            }
                            _toStringResult = new Func<string>(() => toString);
                        }
                        catch (Exception ex)
                        {
                            _value = RawData;
                            _toStringResult = _toStringResultDefault;
                        }

                        break;
                    }
            }
        }

        private Func<string> _toStringResult;
        private Func<string> _toStringResultDefault;
        public override string ToString()
        {
            return _toStringResult.Invoke();
        }

        private byte[] _value;
        public byte[] Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
      /// Writes the headers
      /// </summary>
      /// <remarks></remarks>
        protected virtual void WriteHeaders(System.IO.Stream stream)
        {
            foreach (KeyValuePair<string, string> de in Headers)
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(de.Key + ": " + de.Value + Microsoft.VisualBasic.Constants.vbCrLf);
                stream.Write(buffer, 0, buffer.Length);
            }
            WriteLine(stream);
        }

        /// <summary>
      /// Writes a CrLf into the stream
      /// </summary>
      /// <param name="stream"></param>
      /// <remarks></remarks>
        protected void WriteLine(System.IO.Stream stream)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Constants.vbCrLf);
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
      /// Writes current content to stream
      /// </summary>
      /// <param name="stream"></param>
      /// <remarks></remarks>
        public void WriteTo(System.IO.Stream stream)
        {
            WriteHeaders(stream);
            WriteValue(stream);
        }

        /// <summary>
      /// Writes current content to streamwriter
      /// </summary>
        protected virtual void WriteValue(System.IO.Stream stream)
        {
            if (Value is not null)
            {
                stream.Write(Value, 0, Value.Length);
                WriteLine(stream);
            }
        }


    }
}