using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Common
{
    /// <summary>
   /// Decodes and encodes key-value assignments
   /// </summary>
   /// <remarks></remarks>
    public static class RFC2231Helper
    {

        private const string _mimeSpecials = @"()<>@,;:\""/[]?= " + Microsoft.VisualBasic.Constants.vbTab;
        private const string _rfc2231Specials = "*'%" + _mimeSpecials;
        private static char[] _hexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        #region Content-Disposition
        public const string ContentDispositionHeaderName = "Content-Disposition";
        public const string ContentTransferEncoding = "Content-Transfer-Encoding";
        public const string ContentTypeHeaderName = "Content-Type";
        private const string _attachment = "attachment";
        private const string _fileName = "filename";

        /// <summary>
        /// Tries to evaluate expression as a content-disposition assignment
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="disposition"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DecodeContentDisposition(string expression, ref string disposition)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"(?<Disposition>[^;]*);\s" + _fileName + @"\s*(?<Encoded>\*)?\s*=\s*((?<=\*\s*=\s*)(?<Encoding>[^']*)'(?<Language>[^']*)')?(?<FileName>(%(?<Char>[0-9A-F]{2})|[\s\S])*)", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(expression))
            {
                System.Text.RegularExpressions.Match match = regEx.Match(expression);

                if (match != null && match.Success)
                {
                    System.Text.Encoding encoding = GetEncoding(match.Groups["Encoding"]);
                    System.Text.RegularExpressions.Group group = match.Groups["Char"];

                    disposition = match.Groups["Disposition"].Value;
                    if (group == null || !group.Success)
                        return match.Groups["FileName"].Value;
                    else if (encoding == null)
                    {
                        System.Text.RegularExpressions.Regex regExDecode = new System.Text.RegularExpressions.Regex("%(?<Char>[0-9A-F]{2})", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchEvaluator evaluator = currentMatch =>
                        {
                            return Strings.ChrW(System.Convert.ToInt32(currentMatch.Groups["Char"].Value, 16)).ToString();
                        };
                        return regExDecode.Replace(match.Groups["FileName"].Value, evaluator);
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regExDecode = new System.Text.RegularExpressions.Regex("(%(?<Char>[0-9A-F]{2}))+", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.ExplicitCapture | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchEvaluator evaluator = currentMatch =>
                        {
                            {
                                var withBlock = currentMatch.Groups["Char"];
                                byte[] bytes = (from item in (IEnumerable<System.Text.RegularExpressions.Capture>)withBlock.Captures
                                                let capture = (System.Text.RegularExpressions.Capture)item
                                                select System.Convert.ToByte(capture.Value, 16)).ToArray();
                                return encoding.GetString(bytes);
                            }
                        };
                        return regExDecode.Replace(match.Groups["FileName"].Value, evaluator);
                    }
                }
            }

            disposition = null;
            return null;
        }

        /// <summary>
        /// Encodes given fileName and dispositionType as content-disposition assignment
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dispositionType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string EncodeContentDisposition(string filename, string dispositionType = _attachment)
        {
            if (dispositionType is null)
                dispositionType = _attachment;
            return dispositionType + "; " + EncodeKeyValuePair(_fileName, filename);
        }
        #endregion

        /// <summary>
      /// Encodes a key-value-pair
      /// </summary>
      /// <param name="key"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static string EncodeKeyValuePair(string key, string value)
        {
            return key + (EncodeValue(ref value) ? "*" : "") + "=" + value;
        }

        /// <summary>
      /// Encodes a value
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool EncodeValue(ref string value)
        {
            var sb = new System.Text.StringBuilder();
            bool encoded = false;

            sb.Append("UTF-8");
            sb.Append("''"); // no language
            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(value);

                for (int index = 0, loopTo = bytes.Length - 1; index <= loopTo; index++)
                {
                    int ch = bytes[index] & 0xFF;
                    if (ch <= 32 || ch >= 127 || _rfc2231Specials.IndexOf(Strings.ChrW(ch)) != -1)
                    {
                        sb.Append('%');
                        sb.Append(_hexDigits[ch >> 4]);
                        sb.Append(_hexDigits[ch & 0xF]);
                        encoded = true;
                    }
                    else
                    {
                        sb.Append(Strings.ChrW(ch));
                    }
                }

                if (encoded)
                {
                    value = sb.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                value = sb.ToString();
                return true;
            }
        }

        /// <summary>
      /// Tries to convert grEncoding.Value to the corresponding Encoding
      /// </summary>
        private static System.Text.Encoding GetEncoding(System.Text.RegularExpressions.Group grEncoding)
        {
            string name = grEncoding is null || !grEncoding.Success ? null : grEncoding.Value;
            try
            {
                return string.IsNullOrEmpty(name) ? null : System.Text.Encoding.GetEncoding(name);
            }
            catch
            {
                return null;
            }
        }
    }
}