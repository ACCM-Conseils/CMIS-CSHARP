using System;
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Collections
{
    public class HttpCookieContainer
    {

        public HttpCookieContainer(System.Net.WebHeaderCollection headers, string header)
        {
            var regEx = new System.Text.RegularExpressions.Regex(@"(?<Name>[^,\s=]+)\s*=\s*" + GetStringPattern("Value") + @"(\s*;\s*((?<CookieAV>Expires)\s*=\s*(?<Expires>[\s\S]+GMT)" + @"|(?<CookieAV>Max\-Age)\s*=\s*(?<MaxAge>\d+)" + @"|(?<CookieAV>Domain)\s*=\s*" + GetStringPattern("Domain") + @"|(?<CookieAV>Path)\s*=\s*" + GetStringPattern("Path") + "|(?<CookieAV>(Secure|HttpOnly))" + @"|(?<CookieAV_Name>[^\s,;]+)\s*=\s*" + GetStringPattern("CookieAV_Value") + "|" + GetStringPattern("CookieAV") + "))*", System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
            _headers = headers;
            _header = header;

            if (!(_headers is null || string.IsNullOrEmpty(header)))
            {
                string cookies = headers[header];
                if (!string.IsNullOrEmpty(cookies))
                {
                    foreach (System.Text.RegularExpressions.Match match in regEx.Matches(cookies))
                    {
                        var cookie = new HttpCookie(match.Groups["Name"].Value, TryUnescapeDataString(match.Groups["Value"].Value ?? string.Empty));
                        var group = match.Groups["CookieAV"];

                        if (group is not null && group.Success)
                        {
                            foreach (System.Text.RegularExpressions.Capture capture in group.Captures)
                            {
                                switch ((capture.Value ?? string.Empty).ToLowerInvariant() ?? "")
                                {
                                    case "domain":
                                        {
                                            cookie.Domain = match.Groups["Domain"].Value;
                                            break;
                                        }
                                    case "expires":
                                        {
                                            DateTime expires;
                                            if (DateTime.TryParse(match.Groups["Expires"].Value, out expires))
                                                cookie.Expires = expires;
                                            break;
                                        }
                                    case "httponly":
                                        {
                                            cookie.HttpOnly = true;
                                            break;
                                        }
                                    case "max-age":
                                        {
                                            int maxAge;
                                            if (int.TryParse(match.Groups["MaxAge"].Value, out maxAge))
                                                cookie.MaxAge = maxAge;
                                            break;
                                        }
                                    case "path":
                                        {
                                            cookie.Path = match.Groups["Path"].Value;
                                            break;
                                        }
                                    case "secure":
                                        {
                                            cookie.Secure = true;
                                            break;
                                        }

                                    default:
                                        {
                                            cookie.AddExtension(capture.Value, null);
                                            break;
                                        }
                                }
                            }
                        }

                        group = match.Groups["CookieAV_Name"];
                        if (group is not null && group.Success)
                        {
                            var extensionKeys = group.Captures;
                            var extensionValues = match.Groups["CookieAV_Value"].Captures;

                            for (int index = 0, loopTo = extensionKeys.Count - 1; index <= loopTo; index++)
                                cookie.AddExtension(extensionKeys[index].Value, TryUnescapeDataString(extensionValues[index].Value));
                        }
                        AddOrReplaceCore(cookie);
                    }
                }
            }
        }

        /// <summary>
      /// Adds a cookie, if cookie.Name does not exists in the container. Otherwise cookie replaces the existing cookie.
      /// </summary>
      /// <param name="cookie"></param>
      /// <remarks></remarks>
        public void AddOrReplace(HttpCookie cookie)
        {
            if (AddOrReplaceCore(cookie))
                Refresh();
        }
        private bool AddOrReplaceCore(HttpCookie cookie)
        {
            string name = cookie is null ? null : cookie.Name;

            if (!string.IsNullOrEmpty(name))
            {
                if (_cookies.ContainsKey(name))
                {
                    _cookies[name].Owner = null;
                    _cookies.Remove(name);
                }
                _cookies.Add(name, cookie);
                cookie.Owner = this;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
      /// Returns the cookie with specified name. If the cookie does not exists the method returns null.
      /// </summary>
      /// <param name="name"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public HttpCookie get_Cookie(string name)
        {
            return !string.IsNullOrEmpty(name) && _cookies.ContainsKey(name) ? _cookies[name] : null;
        }
        private Dictionary<string, HttpCookie> _cookies = new Dictionary<string, HttpCookie>();
        public HttpCookie[] Cookies
        {
            get
            {
                return _cookies.Values.ToArray();
            }
        }

        /// <summary>
      /// Returns the pattern for a string expression (may or may not be doublequoted)
      /// </summary>
      /// <param name="groupName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static string GetStringPattern(string groupName)
        {
            return "(\"(?<" + groupName + ">(\"\"|[^\"])*)\"|(?<" + groupName + @">[^,;\s]*))";
        }

        private string _header;
        private System.Net.WebHeaderCollection _headers;

        /// <summary>
      /// Updates headers if a cookie is modified
      /// </summary>
      /// <remarks></remarks>
        internal void Refresh()
        {
            if (!(string.IsNullOrEmpty(_header) || _headers is null))
            {
                _headers.Remove(_header);
                _headers.Add(_header, string.Join(",", from cookie in _cookies.Values
                                                       let value = cookie.ToString()
                                                       select value));
            }
        }

        /// <summary>
      /// Uri.UnescapeDataString()
      /// </summary>
        private string TryUnescapeDataString(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    return Uri.UnescapeDataString(value);
                }
            }
            catch (Exception ex)
            {
                return value;
            }
        }

        /// <summary>
      /// Returns the value of the specified cookie. If the cookie does not exists the function returns null.
      /// </summary>
        public string get_Value(string name)
        {
            var cookie = get_Cookie(name);
            return cookie is null ? null : cookie.Value;
        }

    }
}