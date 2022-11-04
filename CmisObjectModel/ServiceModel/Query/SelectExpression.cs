using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using st = System.Text;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class SelectExpression : FieldContainerExpression
    {

        #region Constructors
        public SelectExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {
        }
        #endregion

        #region Helper classes
        private delegate void FieldSetter(ref Expression field, Expression value, bool condition);
        #endregion

        public override bool CanSetValue()
        {
            return false;
        }

        private FromExpression _from;
        public FromExpression From
        {
            get
            {
                return _from;
            }
            set
            {
                if (_from is not null)
                    _children.Remove(_from);
                _from = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

        /// <summary>
      /// Returns True if classField is set to value, otherwise
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        private bool GenericFieldSetter<T>(ref T classField, Expression value, List<Expression> expressions, bool condition = true) where T : Expression
        {
            if (classField is null && value.Parent is null && condition)
            {
                classField = (T)value;
                _children.Add(value);
                SetParent(value, this);
                _sealResult = value.Seal(expressions);
            }
            else
            {
                _sealResult = value.Match.Index;
            }

            return !_sealResult.HasValue;
        }

        protected override string GetValue(Type executingType)
        {
            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(base.GetValue(typeof(Expression)));

                sb.Append(" ");
                sb.Append(string.Join(", ", (from field in _fields
                                             let fieldExpression = field.Value
                                             select fieldExpression).ToArray()));
                if (_from is not null)
                {
                    sb.Append(" ");
                    sb.Append(_from.Value);
                    if (_where is not null)
                    {
                        sb.Append(" ");
                        sb.Append(_where.Value);
                    }
                    if (_orderBy is not null)
                    {
                        sb.Append(" ");
                        sb.Append(_orderBy.Value);
                    }
                }

                return sb.ToString();
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

        private OrderByExpression _orderBy;
        public OrderByExpression OrderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                if (_orderBy is not null)
                    _children.Remove(_orderBy);
                _orderBy = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

        private bool _separatorExpected = false;
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
                    Expression rightExpression = this.GetRightExpression(expressions);

                    while (rightExpression != null)
                    {
                        if (rightExpression is FromExpression)
                        {
                            if (!GenericFieldSetter(ref _from, rightExpression, expressions))
                                break;
                        }
                        else if (rightExpression is WhereExpression)
                        {
                            if (!GenericFieldSetter(ref _where, rightExpression, expressions, _from != null && _orderBy == null))
                                break;
                        }
                        else if (rightExpression is OrderByExpression)
                        {
                            if (!GenericFieldSetter(ref _orderBy, rightExpression, expressions, _from != null))
                                break;
                        }
                        else if (_from == null)
                        {
                            if (_separatorExpected != (rightExpression.GroupName == "Separator"))
                            {
                                // expected expression missed
                                _sealResult = rightExpression.Match.Index;
                                break;
                            }
                            else if (_separatorExpected)
                            {
                                // the next separator MUST NOT have a parent at this time
                                if (rightExpression.Parent == null)
                                    SetParent(rightExpression, this);
                                else
                                {
                                    _sealResult = rightExpression.Match.Index;
                                    break;
                                }
                            }
                            else if (!AddField(rightExpression, expressions))
                                break;
                        }
                        else
                            // other expression types will not belong to this select
                            break;
                        _separatorExpected = !_separatorExpected;
                        rightExpression = rightExpression.GetRightExpression(expressions);
                    }
                    // check, if there is at least one field and a from expression defined
                    if (!_sealResult.HasValue)
                    {
                        if (_fields.Count == 0)
                            _sealResult = this.Match.Index + this.Match.Length;
                        else if (_from == null)
                        {
                            {
                                var withBlock = _fields[_fields.Count - 1].Match;
                                _sealResult = withBlock.Index + withBlock.Length;
                            }
                        }
                    }
                }
            }

            return _sealResult;
        }

        private WhereExpression _where;
        public WhereExpression Where
        {
            get
            {
                return _where;
            }
            set
            {
                if (_where is not null)
                    _children.Remove(_where);
                _where = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

    }
}