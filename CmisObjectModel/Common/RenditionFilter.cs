using System.Collections.Generic;

namespace CmisObjectModel.Common
{
    /// <summary>
   /// Implementation of the redition filter grammar
   /// </summary>
   /// <remarks>
   /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
   /// </remarks>
    public class RenditionFilter
    {
        private RenditionFilter(string filter)
        {
            // remove whitespaces
            if (!string.IsNullOrEmpty(filter))
            {
                var regEx = new System.Text.RegularExpressions.Regex(@"\s");
                filter = regEx.Replace(filter, "");
            }
            // evaluate filter expression
            if (!string.IsNullOrEmpty(filter) && filter != "cmis:none")
            {
                // comma separated terms
                foreach (System.Text.RegularExpressions.Match match in _regEx.Matches(filter))
                {
                    var grInvalid = match.Groups["Invalid"];

                    // type and subtype MUST NOT contain a whitespace
                    if (grInvalid is null || !grInvalid.Success)
                    {
                        string type = match.Groups["Type"].Value;
                        var grSubType = match.Groups["SubType"];
                        string subType = grSubType is null || !grSubType.Success ? "" : grSubType.Value;
                        Dictionary<string, object> verify;

                        if (_filters.ContainsKey(type))
                        {
                            verify = _filters[type];
                        }
                        else
                        {
                            verify = new Dictionary<string, object>();
                            _filters.Add(type, verify);
                        }
                        if (!verify.ContainsKey(subType))
                            verify.Add(subType, null);
                    }
                }
            }
        }

        public static implicit operator RenditionFilter(string value)
        {
            return new RenditionFilter(value);
        }

        /// <summary>
      /// Returns True if mimeType is defined in this instance
      /// </summary>
      /// <param name="mimeType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool ContainsMimeType(string mimeType)
        {
            var match = _regEx.Match(mimeType is null ? "" : mimeType);

            if (match is not null && match.Success)
            {
                var grInvalid = match.Groups["Invalid"];

                if (grInvalid is null || !grInvalid.Success)
                {
                    if (_filters.ContainsKey("*"))
                        return true;
                    if (_filters.ContainsKey(match.Groups["Type"].Value))
                    {
                        var verify = _filters[match.Groups["Type"].Value];
                        var grSubType = match.Groups["SubType"];
                        string subType = grSubType is null || !grSubType.Success ? "" : grSubType.Value;
                        return verify.ContainsKey(subType) || !string.IsNullOrEmpty(subType) && verify.ContainsKey("*");
                    }
                }
            }

            return false;
        }

        private Dictionary<string, Dictionary<string, object>> _filters = new Dictionary<string, Dictionary<string, object>>();
        private System.Text.RegularExpressions.Regex _regEx = new System.Text.RegularExpressions.Regex(@"(?<Term>(?<Type>([^\s,\/]+|(?<Invalid>\s))+)(\/(?<SubType>([^\s,]+|(?<Invalid>[\s\/]))+))?)");

    }
}