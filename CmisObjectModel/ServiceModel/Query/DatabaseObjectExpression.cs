using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using st = System.Text;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Base class for column and table expressions
   /// </summary>
   /// <remarks></remarks>
    public abstract class DatabaseObjectExpression : CompositeExpression
    {

        #region Constructors
        public DatabaseObjectExpression(Expression innerExpression) : base(innerExpression.Match, innerExpression.GroupName, innerExpression.Rank, innerExpression.Index, null, null)
        {
            _innerExpression = innerExpression;
            _children.Add(innerExpression);
            SetParent(innerExpression, this);
        }
        #endregion

        protected AliasExpression _alias;
        public AliasExpression Alias
        {
            get
            {
                return _alias;
            }
        }
        public void SetAlias(string value)
        {
            if (AllowAlias)
            {
                // keyword 'as' is optional, therefore an expression is built to ensure detection of alias name
                var expressions = QueryParser.GetExpressions("Select fieldName " + value);

                if (expressions is not null)
                {
                    foreach (Expression expression in expressions)
                    {
                        if (expression is AliasExpression)
                        {
                            _alias = (AliasExpression)expression;
                            break;
                        }
                    }
                }
            }
        }
        public void SetAlias(AliasExpression value)
        {
            if (AllowAlias)
                _alias = value;
        }

        protected abstract bool AllowAlias { get; }

        protected override string GetValue(Type executingType)
        {
            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(_innerExpression.Value);

                if (!(_alias is null || string.IsNullOrEmpty(_alias.Value)))
                {
                    sb.Append(" ");
                    sb.Append(_alias.Value);
                }

                return sb.ToString();
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

        protected Expression _innerExpression;
        public Expression InnerExpression
        {
            get
            {
                return _innerExpression;
            }
        }

        /// <summary>
      /// Returns the complete prefix of the database object (column or table)
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
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public override int? Seal(List<Expression> expressions)
        {
            if (!_sealed)
            {
                _sealed = true;
                _sealResult = _innerExpression.Seal(expressions);

                if (!_sealResult.HasValue && AllowAlias)
                {
                    _alias = GetRightExpression(expressions) as AliasExpression;
                    if (_alias is not null)
                    {
                        _children.Add(_alias);
                        SetParent(_alias, this);
                        _sealResult = _alias.Seal(expressions);
                    }
                }
            }

            return _sealResult;
        }
    }
}