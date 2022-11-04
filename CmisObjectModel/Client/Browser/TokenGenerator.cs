using System;
using System.Collections.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Browser
{
    /// <summary>
   /// Simple TokenGenerator for CmisObjectModel driven CMIS servers
   /// </summary>
   /// <remarks></remarks>
    public class TokenGenerator
    {

        #region Constructors
        protected TokenGenerator()
        {
        }

        public TokenGenerator(Func<System.Net.CookieContainer> fnGetCookies, Uri uri, string sessionIdCookieName)
        {
            if (!(string.IsNullOrEmpty(sessionIdCookieName) || uri is null))
            {
                _fnGetCookies = fnGetCookies;
                _sessionIdCookieName = sessionIdCookieName;
                _uri = uri;
            }
            else
            {
                _fnGetCookies = null;
            }
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Implementation of an unchangeable token
      /// </summary>
      /// <remarks></remarks>
        private class StaticToken : TokenGenerator
        {

            public StaticToken(string token)
            {
                _token = token;
            }

            public override string NextToken()
            {
                return _token;
            }

            private string _token;
        }
        #endregion

        /// <summary>
      /// Sets the current token generator for the current thread
      /// </summary>
        public static void BeginToken(TokenGenerator token)
        {
            var thread = System.Threading.Thread.CurrentThread;

            lock (_tokenStacks)
            {
                Stack<TokenGenerator> stack;

                if (_tokenStacks.ContainsKey(thread))
                {
                    stack = _tokenStacks[thread];
                }
                else
                {
                    stack = new Stack<TokenGenerator>();
                    _tokenStacks.Add(thread, stack);
                }
                stack.Push(token);
            }
        }

        /// <summary>
      /// Returns the current token generator for the current thread
      /// </summary>
        public static TokenGenerator Current
        {
            get
            {
                var thread = System.Threading.Thread.CurrentThread;

                lock (_tokenStacks)
                    return _tokenStacks.ContainsKey(thread) ? _tokenStacks[thread].Peek() : null;
            }
        }

        /// <summary>
      /// Removes the current token generator for the current thread
      /// </summary>
        public static TokenGenerator EndToken()
        {
            var thread = System.Threading.Thread.CurrentThread;
            TokenGenerator retVal;

            lock (_tokenStacks)
            {
                if (_tokenStacks.ContainsKey(thread))
                {
                    var stack = _tokenStacks[thread];
                    int count = stack.Count;

                    if (count > 0)
                    {
                        retVal = stack.Pop();
                        count -= 1;
                    }
                    else
                    {
                        retVal = null;
                    }
                    if (count == 0)
                        _tokenStacks.Remove(thread);
                }
                else
                {
                    retVal = null;
                }
            }

            return retVal;
        }

        private Func<System.Net.CookieContainer> _fnGetCookies;

        /// <summary>
      /// Returns the next token (sessionId + "\r\n" + Guid.NewGuid.ToString())
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual string NextToken()
        {
            var cookies = _fnGetCookies is null ? null : _fnGetCookies.Invoke();
            string prefix = null;

            if (cookies is not null)
            {
                var cookieCollection = cookies.GetCookies(_uri);
                var cookie = cookieCollection is null ? null : cookieCollection[_sessionIdCookieName];

                if (cookie is not null)
                    prefix = cookie.Value + Microsoft.VisualBasic.Constants.vbCrLf;
            }

            return prefix + Guid.NewGuid().ToString();
        }

        private string _sessionIdCookieName;
        private static Dictionary<System.Threading.Thread, Stack<TokenGenerator>> _tokenStacks = new Dictionary<System.Threading.Thread, Stack<TokenGenerator>>();
        private Uri _uri;

        public static implicit operator string(TokenGenerator value)
        {
            return value is null ? null : value.NextToken();
        }

        public static implicit operator TokenGenerator(string value)
        {
            return new StaticToken(value);
        }

    }
}