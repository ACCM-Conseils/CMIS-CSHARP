using System;
using System.Collections.Generic;
using CmisObjectModel.Constants;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Common.Generic
{
    /// <summary>
   /// Generic UriBuilder
   /// </summary>
   /// <typeparam name="TEnum"></typeparam>
   /// <remarks></remarks>
    public class LinkUriBuilder<TEnum> where TEnum : struct
    {

        private string _baseServiceUri;
        protected int _flags = 0;
        protected List<string> _searchAndReplace;
        private static Dictionary<int, Tuple<string, Enum>> _values = ServiceURIs.GetValues(typeof(TEnum));

        #region Constructors
        protected LinkUriBuilder()
        {
        }
        public LinkUriBuilder(string baseServiceUri)
        {
            _baseServiceUri = baseServiceUri;
            _searchAndReplace = new List<string>();
        }
        public LinkUriBuilder(string baseServiceUri, string repositoryId)
        {
            _baseServiceUri = baseServiceUri;
            _searchAndReplace = new List<string>() { "repositoryId", repositoryId };
        }
        #endregion

        #region Add overloads
        public void Add(TEnum flag, bool value)
        {
            Add(flag, value ? "true" : "false");
        }
        public void Add(TEnum flag, bool? value)
        {
            if (value.HasValue)
                Add(flag, value.Value);
            else
                this.Add(flag, false);
        }
        public void Add<TValue>(TEnum flag, TValue value) where TValue : struct
        {
            Add(flag, ((Enum)(object)value).GetName());
        }
        public void Add<TValue>(TEnum flag, TValue? value) where TValue : struct
        {
            if (value.HasValue)
                Add(flag, value.Value);
            else
                this.Add(flag, false);
        }
        public void Add(TEnum flag, int value)
        {
            Add(flag, value.ToString());
        }
        public void Add(TEnum flag, int? value)
        {
            if (value.HasValue)
                Add(flag, value.Value);
            else
                this.Add(flag, 0);
        }
        public void Add(TEnum flag, long value)
        {
            Add(flag, value.ToString());
        }
        public void Add(TEnum flag, long? value)
        {
            if (value.HasValue)
                Add(flag, value.Value);
            else
                this.Add(flag, 0);
        }
        public void Add(TEnum flag, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int currentFlag = Conversions.ToInteger(flag);

                if (_values.ContainsKey(currentFlag))
                {
                    _flags = _flags | currentFlag;
                    _searchAndReplace.Add(_values[currentFlag].Item2.GetName());
                    _searchAndReplace.Add(value);
                }
            }
        }
        #endregion

        public virtual Uri ToUri()
        {
            return new Uri(ServiceURIs.GetServiceUri(_baseServiceUri, Conversions.ToGenericParameter<TEnum>(_flags)).ReplaceUri(_searchAndReplace.ToArray()));
        }

    }
}