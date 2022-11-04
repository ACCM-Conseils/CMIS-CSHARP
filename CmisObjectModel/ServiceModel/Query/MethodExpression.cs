using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class MethodExpression : CompositeExpression
    {

        #region Constructors
        public MethodExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, null, null)
        {
            MethodName = match.Groups["MethodName"].Value;
        }
        #endregion

        private ParenthesisExpression _parenthesis;
        public ParenthesisExpression Parenthesis
        {
            get
            {
                return _parenthesis;
            }
            set
            {
                if (_parenthesis is not null)
                    _children.Remove(_parenthesis);
                _parenthesis = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

        public readonly string MethodName;

        /// <summary>
      /// Returns the complete prefix of the identifier
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string Prefix
        {
            get
            {
                return string.Join(".", PrefixParts);
            }
        }

        /// <summary>
      /// Returns the parts of the prefix; parts are separated by . from each other
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string[] PrefixParts
        {
            get
            {
                var prefix = Match.Groups["Prefix"];

                if (prefix is null || !prefix.Success)
                {
                    return new string[] { };
                }
                else
                {
                    return (from capture in (IEnumerable<Capture>)prefix.Captures
                            select ((str.Capture)capture).Value).ToArray();
                }
            }
        }

        /// <summary>
      /// Returns null if the expression is accepted in the parsed query, otherwise the position of the match
      /// that invalidates the query
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public override int? Seal(List<Expression> expressions)
        {
            if (!_sealed)
            {
                _sealResult = base.Seal(expressions);

                if (!_sealResult.HasValue)
                {
                    var rightExpression = GetRightExpression(expressions);

                    if (!(rightExpression is ParenthesisExpression) || rightExpression.Parent is not null)
                    {
                        _sealResult = Match.Index + Match.Length;
                    }
                    else
                    {
                        Parenthesis = (ParenthesisExpression)rightExpression;
                    }
                }
            }

            return _sealResult;
        }

    }
}