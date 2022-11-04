using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using sn = System.Net;
using sri = System.Runtime.InteropServices;
using ssd = System.ServiceModel.Description;
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
using System.Net;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    public class AuthenticationProvider : AuthenticationInfo
    {

        public AuthenticationProvider(string user, System.Security.SecureString password) : base(user, password)
        {
            _cookies = new sn.CookieContainer();
        }

        /// <summary>
      /// Creates a secure NtlmAuthenticationProvider if the password only contains digits and letters.
      /// Otherwise it creates a AuthenticationProvider to prevent problems with special characters.
      /// </summary>
      /// <param name="user"></param>
      /// <param name="password"></param>
      /// <returns></returns>
        public static new AuthenticationProvider CreateInstance(string user, System.Security.SecureString password)
        {
            foreach (char ch in PasswordToCharArray(password))
            {
                if (!char.IsLetterOrDigit(ch))
                    return new AuthenticationProvider(user, password);
            }
            return new NtlmAuthenticationProvider(user, password);
        }

        #region Authentication
        public void Authenticate(object portOrRequest)
        {
            if (portOrRequest is sn.HttpWebRequest)
            {
                HttpAuthenticate((sn.HttpWebRequest)portOrRequest);
            }
            else
            {
                WebServiceAuthenticate(portOrRequest);
            }
        }

        /// <summary>
      /// Authentication AtomPub-Binding
      /// </summary>
      /// <param name="request"></param>
      /// <remarks></remarks>
        protected virtual void HttpAuthenticate(sn.HttpWebRequest request)
        {
            request.AllowWriteStreamBuffering = false;
            request.CookieContainer = _cookies;
            if (request.Headers.GetValues("Authorization") is null && !(string.IsNullOrEmpty(_user) && (_password is null || _password.Length == 0)))
            {
                var chars = new List<char>();
                var valuePtr = IntPtr.Zero;

                if (!string.IsNullOrEmpty(_user))
                    chars.AddRange(_user.ToCharArray());
                chars.Add(':');

                if (_password is not null && _password.Length > 0)
                {
                    try
                    {
                        valuePtr = sri.Marshal.SecureStringToGlobalAllocUnicode(_password);
                        for (int index = 0, loopTo = _password.Length - 1; index <= loopTo; index++)
                            chars.Add(Strings.ChrW(sri.Marshal.ReadInt16(valuePtr, index << 1)));
                    }
                    finally
                    {
                        sri.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    }
                }
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(chars.ToArray())));
            }
        }

        /// <summary>
      /// Authentication WebService-Binding
      /// </summary>
      /// <param name="port"></param>
      /// <remarks>Not supported</remarks>
        protected void WebServiceAuthenticate(object port)
        {
        }

        /// <summary>
      /// Authentication WebService-Binding
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="clientCredentials"></param>
      /// <remarks></remarks>
        protected virtual void AddWebServiceCredentials(ssd.ServiceEndpoint endPoint, ssd.ClientCredentials clientCredentials)
        {
            if (string.IsNullOrEmpty(_user) && (_password is null || _password.Length == 0))
            {
                System.ServiceModel.Channels.CustomBinding binding = endPoint.Binding as System.ServiceModel.Channels.CustomBinding;
                // remove SecurityBindingElement because neither a username nor a password have been set
                if (binding is not null)
                    binding.Elements.RemoveAll<System.ServiceModel.Channels.SecurityBindingElement>();
            }
            else
            {
                var valuePtr = IntPtr.Zero;

                clientCredentials.UserName.UserName = _user ?? "";
                if (_password is not null && _password.Length > 0)
                {
                    try
                    {
                        valuePtr = sri.Marshal.SecureStringToGlobalAllocUnicode(_password);
                        clientCredentials.UserName.Password = sri.Marshal.PtrToStringUni(valuePtr);
                    }
                    finally
                    {
                        sri.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    }
                }
            }
        }
        #endregion

        protected Collections.Generic.DictionaryTree<string, sn.Cookie> _caseInsensitiveCookies = new Collections.Generic.DictionaryTree<string, sn.Cookie>();
        public sn.CookieCollection get_CaseInsensitiveCookies(Uri uri)
        {
            var retVal = new sn.CookieCollection();
            string host = uri.Host;
            string hostLowerInvariant = host.ToLowerInvariant();

            if (_caseInsensitiveCookies.ContainsKeys(hostLowerInvariant))
            {
                var domainTrees = _caseInsensitiveCookies.get_Tree(hostLowerInvariant).SubTrees;
                string key = string.Empty;
                string absolutePath = uri.AbsolutePath;
                var regEx = new System.Text.RegularExpressions.Regex(@"(\/|[^\/]*)");
                var existingCookies = (from item in (IEnumerable<Cookie>)_cookies.GetCookies(uri)
                                       let cookie = item as sn.Cookie
                                       where cookie is not null
                                       select cookie).ToDictionary(current => current.Name);
                foreach (System.Text.RegularExpressions.Match match in regEx.Matches(absolutePath.ToLowerInvariant()))
                {
                    key += match.Value.ToLowerInvariant();
                    if (domainTrees.ContainsKey(key))
                    {
                        foreach (KeyValuePair<string, Collections.Generic.DictionaryTree<string, sn.Cookie>> de in domainTrees[key].SubTrees)
                        {
                            if (!existingCookies.ContainsKey(de.Key))
                            {
                                {
                                    var withBlock = de.Value.get_Item();
                                    retVal.Add(new sn.Cookie(withBlock.Name, withBlock.Value, absolutePath.Substring(0, key.Length), host));
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }
        public sn.CookieCollection CaseInsensitiveCookies
        {
            set
            {
                foreach (sn.Cookie cookie in value)
                    _caseInsensitiveCookies.set_Item(new string[] { cookie.Domain.ToLowerInvariant(), cookie.Path.ToLowerInvariant(), cookie.Name }, value: cookie);
            }
        }

        protected sn.CookieContainer _cookies;
        public sn.CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
            set
            {
                _cookies = value;
            }
        }

    }
}