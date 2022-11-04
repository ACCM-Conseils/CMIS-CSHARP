using System;
using System.Collections.Generic;
using sn = System.Net;
using sri = System.Runtime.InteropServices;
using ssw = System.ServiceModel.Web;
using Microsoft.VisualBasic;

namespace CmisObjectModel.Common
{
    /// <summary>
   /// Simple class to encapsulate information about user and password
   /// </summary>
   /// <remarks></remarks>
    public class AuthenticationInfo
    {

        #region Constructors
        protected AuthenticationInfo(string user, System.Security.SecureString password)
        {
            _user = user;
            _password = password;
        }

        /// <summary>
      /// Creates a new instance
      /// </summary>
      /// <param name="user"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AuthenticationInfo CreateInstance(string user, System.Security.SecureString password)
        {
            return new AuthenticationInfo(user, password);
        }
        #endregion

        /// <summary>
      /// Copies the password into given utf8Bytes-buffer
      /// </summary>
      /// <param name="utf8Bytes"></param>
      /// <remarks></remarks>
        public void CopyPasswordTo(List<byte> utf8Bytes)
        {
            CopyPasswordTo(_password, utf8Bytes);
        }
        /// <summary>
      /// Copies the password into given utf8Bytes-buffer
      /// </summary>
      /// <param name="password"></param>
      /// <param name="utf8Bytes"></param>
      /// <remarks></remarks>
        public static void CopyPasswordTo(System.Security.SecureString password, List<byte> utf8Bytes)
        {
            var chars = new List<char>();

            CopyPasswordTo(password, chars);
            if (chars.Count > 0)
                utf8Bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(chars.ToArray()));
        }

        /// <summary>
      /// Copies the password into given chars-buffer
      /// </summary>
      /// <param name="chars"></param>
      /// <remarks></remarks>
        public void CopyPasswordTo(List<char> chars)
        {
            CopyPasswordTo(_password, chars);
        }
        /// <summary>
      /// Copies the password into given chars-buffer
      /// </summary>
      /// <param name="password"></param>
      /// <param name="chars"></param>
      /// <remarks></remarks>
        public static void CopyPasswordTo(System.Security.SecureString password, List<char> chars)
        {
            if (password is not null && password.Length > 0)
            {
                var valuePtr = IntPtr.Zero;

                try
                {
                    valuePtr = sri.Marshal.SecureStringToGlobalAllocUnicode(password);
                    for (int index = 0, loopTo = password.Length - 1; index <= loopTo; index++)
                        chars.Add(Strings.ChrW(sri.Marshal.ReadInt16(valuePtr, index << 1)));
                }
                finally
                {
                    if (!valuePtr.Equals(IntPtr.Zero))
                        sri.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                }
            }
        }

        /// <summary>
      /// Returns authenticationInfo from the current incoming web-request
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AuthenticationInfo FromCurrentWebRequest()
        {
            return FromCurrentWebRequest(CreateInstance);
        }
        /// <summary>
      /// Returns authenticationInfo from the current incoming web-request
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static TResult FromCurrentWebRequest<TResult>(Func<string, System.Security.SecureString, TResult> factory) where TResult : AuthenticationInfo
        {
            string authentication = ssw.WebOperationContext.Current.IncomingRequest.Headers[sn.HttpRequestHeader.Authorization];
            var regEx = new System.Text.RegularExpressions.Regex(@"Basic (?<Authentication>[\s\S]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
            var match = string.IsNullOrEmpty(authentication) ? null : regEx.Match(authentication);

            if (match is null || !match.Success)
            {
                return factory.Invoke(null, null);
            }
            else
            {
                var chars = System.Text.Encoding.UTF8.GetChars(Convert.FromBase64String(match.Groups["Authentication"].Value));
                var sbUser = new System.Text.StringBuilder();
                var password = new System.Security.SecureString();
                bool infoSelector = true;

                foreach (char ch in chars)
                {
                    if (infoSelector)
                    {
                        if (ch == ':')
                        {
                            // password starts from this point
                            infoSelector = false;
                        }
                        else
                        {
                            sbUser.Append(ch);
                        }
                    }
                    else
                    {
                        password.AppendChar(ch);
                    }
                }
                return factory.Invoke(sbUser.ToString(), password);
            }
        }

        /// <summary>
      /// Returns password as char-array
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public char[] GetPasswordChars()
        {
            var chars = new List<char>();

            CopyPasswordTo(chars);
            return chars.ToArray();
        }

        protected readonly System.Security.SecureString _password;
        public System.Security.SecureString Password
        {
            get
            {
                return _password;
            }
        }

        /// <summary>
      /// Returns the password as char-array
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public char[] PasswordToCharArray()
        {
            return PasswordToCharArray(_password);
        }
        /// <summary>
      /// Returns the given password as char-array
      /// </summary>
      /// <param name="password"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static char[] PasswordToCharArray(System.Security.SecureString password)
        {
            var chars = new List<char>();

            CopyPasswordTo(password, chars);
            return chars.ToArray();
        }

        /// <summary>
      /// Returns password as utf8Byte-array
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public byte[] PasswordToUtf8ByteArray()
        {
            return PasswordToUtf8ByteArray(_password);
        }
        /// <summary>
      /// Returns the given password as utf8Byte-array
      /// </summary>
      /// <param name="password"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static byte[] PasswordToUtf8ByteArray(System.Security.SecureString password)
        {
            var utf8Buffer = new List<byte>();

            CopyPasswordTo(password, utf8Buffer);
            return utf8Buffer.ToArray();
        }

        protected readonly string _user;
        public string User
        {
            get
            {
                return _user;
            }
        }
    }
}