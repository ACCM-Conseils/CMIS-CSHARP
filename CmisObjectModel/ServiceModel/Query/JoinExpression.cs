using System;
using System.Collections.Generic;
using st = System.Text;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class JoinExpression : CompositeExpression
    {

        #region Constructors
        public JoinExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, null, " ")
        {
        }
        #endregion

        public override bool CanSetValue()
        {
            return false;
        }

        protected override string GetValue(Type executingType)
        {
            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(base.GetValue(typeof(Expression)));

                if (_table is not null)
                {
                    sb.Append(" ");
                    sb.Append(_table.Value);
                    if (_on is not null)
                    {
                        sb.Append(" ");
                        sb.Append(_on.Value);
                    }
                }

                return sb.ToString();
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

        private WhereExpression _on;
        public WhereExpression On
        {
            get
            {
                return _on;
            }
            set
            {
                if (_on is not null)
                    _children.Remove(_on);
                _on = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

        private static HashSet<string> _allowedTables = new HashSet<string>() { "Identifier", "OpenParenthesis" };
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

                    if (rightExpression is null)
                    {
                        _sealResult = Match.Index + Match.Length;
                    }
                    else if (rightExpression.Parent is null && _allowedTables.Contains(rightExpression.GroupName))
                    {
                        Table = new TableExpression(rightExpression);
                        _sealResult = Table.Seal(expressions);
                        if (!_sealResult.HasValue)
                        {
                            rightExpression = rightExpression.GetRightExpression(expressions);
                            if (rightExpression is WhereExpression && rightExpression.Parent is null && string.Compare(rightExpression.Value, "On", true) == 0)
                            {
                                On = (WhereExpression)rightExpression;
                                _sealResult = On.Seal(expressions);
                                if (!_sealResult.HasValue)
                                {
                                    _sealResult = _table.Seal(expressions);
                                }
                            }
                            else
                            {
                                _sealResult = rightExpression.Match.Index;
                            }
                        }
                    }
                    else
                    {
                        _sealResult = rightExpression.Match.Index;
                    }
                }
            }

            return _sealResult;
        }

        private TableExpression _table;
        public TableExpression Table
        {
            get
            {
                return _table;
            }
            set
            {
                if (_table is not null)
                    _children.Remove(_table);
                _table = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

    }
}