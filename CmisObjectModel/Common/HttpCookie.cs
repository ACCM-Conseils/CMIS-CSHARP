using System;
using System.Collections.Generic;
using System.Linq;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Common
{
    /// <summary>
   /// Simple cookie class
   /// </summary>
   /// <remarks></remarks>
    public class HttpCookie
    {

        #region Constructors
        public HttpCookie(string name)
        {
            _name = name;
        }

        public HttpCookie(string name, string value) : this(name)
        {
            Value = value;
        }
        #endregion

        public void AddExtension(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _extensions.Remove(key);
                _extensions.Add(key, value);
            }
        }

        private string _domain;
        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                _domain = value;
                RefreshOwner();
            }
        }

        private DateTime? _expires;
        public DateTime? Expires
        {
            get
            {
                return _expires;
            }
            set
            {
                _expires = value;
                RefreshOwner();
            }
        }

        public string get_Extension(string key)
        {
            return !string.IsNullOrEmpty(key) && _extensions.ContainsKey(key) ? _extensions[key] : null;
        }
        private Dictionary<string, string> _extensions = new Dictionary<string, string>();
        public KeyValuePair<string, string>[] Extensions
        {
            get
            {
                return _extensions.ToArray();
            }
        }

        private bool _httpOnly;
        public bool HttpOnly
        {
            get
            {
                return _httpOnly;
            }
            set
            {
                _httpOnly = value;
                RefreshOwner();
            }
        }

        private int? _maxAge;
        public int? MaxAge
        {
            get
            {
                return _maxAge;
            }
            set
            {
                _maxAge = value;
                RefreshOwner();
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        internal Collections.HttpCookieContainer Owner { get; set; }

        private string _path;
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RefreshOwner();
            }
        }

        private void RefreshOwner()
        {
            if (Owner is not null)
                Owner.Refresh();
        }

        private static System.Text.RegularExpressions.Regex _regEx = new System.Text.RegularExpressions.Regex(@"((?<DoubleQuoted>\A""(""""|[^""])*""\z)|["";,])", System.Text.RegularExpressions.RegexOptions.Singleline);

        private bool _secure;
        public bool Secure
        {
            get
            {
                return _secure;
            }
            set
            {
                _secure = value;
                RefreshOwner();
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_name))
            {
                return null;
            }
            else
            {
                var sb = new System.Text.StringBuilder(_name);

                sb.Append("=");
                if (!string.IsNullOrEmpty(Value))
                {
                    sb.Append(Uri.EscapeDataString(Value));
                }
                if (_expires.HasValue)
                {
                    sb.Append("; Expires=");
                    sb.Append(_expires.Value.ToString(System.Globalization.DateTimeFormatInfo.InvariantInfo.RFC1123Pattern, System.Globalization.DateTimeFormatInfo.InvariantInfo));
                }
                if (_maxAge.HasValue)
                {
                    sb.Append("; Max-Age=");
                    sb.Append(_maxAge.Value);
                }
                if (!string.IsNullOrEmpty(_domain))
                {
                    sb.Append("; Domain=");
                    sb.Append(_domain);
                }
                if (!string.IsNullOrEmpty(_path))
                {
                    sb.Append("; Path=");
                    sb.Append(_path);
                }
                if (_secure)
                    sb.Append("; Secure");
                if (_httpOnly)
                    sb.Append("; HttpOnly");
                foreach (KeyValuePair<string, string> de in _extensions)
                {
                    sb.Append("; ");
                    sb.Append(de.Key);
                    if (de.Value is not null)
                    {
                        sb.Append("=");
                        // extensions always as doublequoted strings
                        sb.Append("\"");
                        sb.Append(Uri.EscapeDataString(de.Value));
                        sb.Append("\"");
                    }
                }

                return sb.ToString();
            }
        }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RefreshOwner();
            }
        }

    }
}