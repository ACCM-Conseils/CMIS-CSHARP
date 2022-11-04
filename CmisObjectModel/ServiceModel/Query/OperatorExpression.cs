using System;
using System.Collections.Generic;
using st = System.Text;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class OperatorExpression : CompositeExpression
    {

        #region Constructors
        public OperatorExpression(str.Match match, string groupName, int rank, int index, bool leftOperandSupported) : base(match, groupName, rank, index, null, null)
        {

            var grNegation = match.Groups["Negation"];

            LeftOperandSupported = leftOperandSupported;
            HasNegation = grNegation is not null && grNegation.Success && string.Compare(groupName, "Negation") != 0;
            Operator = base.GetValue(typeof(Expression));
        }
        #endregion

        protected static HashSet<string> _allowedLefts = new HashSet<string>() { "CloseParenthesis", "Constant", "Identifier" };
        private static HashSet<string> _allowedRights = new HashSet<string>() { "OpenParenthesis", "Constant", "Identifier", "Method", "Negation" };
        /// <summary>
      /// Returns the valid group names for the right side of the operator
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual HashSet<string> AllowedRights
        {
            get
            {
                return _allowedRights;
            }
        }

        public override bool CanSetValue()
        {
            return false;
        }

        protected override string GetValue(Type executingType)
        {
            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder();

                if (_left is not null && LeftOperandSupported)
                {
                    sb.Append(_left.Value);
                    sb.Append(" ");
                }
                if (!HasNegation)
                {
                    sb.Append(Operator);
                }
                else if (string.Compare("is", Operator, true) == 0)
                {
                    sb.Append(Operator);
                    sb.Append(" Not");
                }
                else
                {
                    sb.Append("Not ");
                    sb.Append(Operator);
                }
                if (_right is not null)
                {
                    sb.Append(" ");
                    sb.Append(_right.Value);
                }

                return sb.ToString();
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

        protected Expression _left;
        public Expression Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (_left is not null)
                    _children.Remove(_left);
                _left = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }
        protected void _left_ParentChanged(object sender, EventArgs e)
        {
            var newParent = _left.Root;

            _left.ParentChanged -= _left_ParentChanged;
            _left = null;
            Left = newParent;
        }

        public readonly bool HasNegation;
        public readonly string Operator;
        public readonly bool LeftOperandSupported;

        public override bool ReplaceChild(Expression oldChild, Expression newChild)
        {
            if (base.ReplaceChild(oldChild, newChild))
            {
                if (ReferenceEquals(oldChild, _left))
                {
                    _left = newChild;
                }
                else if (ReferenceEquals(oldChild, _right))
                {
                    _right = newChild;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected Expression _right;
        public Expression Right
        {
            get
            {
                return _right;
            }
            set
            {
                if (_right is not null)
                    _children.Remove(_right);
                _right = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
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
                    if (LeftOperandSupported)
                    {
                        if (Index == 0)
                        {
                            _sealResult = Match.Index;
                        }
                        else
                        {
                            var leftExpression = expressions[Index - 1];

                            if (_allowedLefts.Contains(leftExpression.GroupName))
                            {
                                leftExpression = leftExpression.Root;
                                if (leftExpression is ParenthesisExpression && leftExpression.Match.Value == ")")
                                {
                                    _left = leftExpression;
                                    leftExpression.ParentChanged += _left_ParentChanged;
                                }
                                else
                                {
                                    Left = leftExpression;
                                }
                            }
                            else
                            {
                                _sealResult = leftExpression.Match.Index;
                            }
                        }
                    }
                    if (!_sealResult.HasValue)
                    {
                        int rightIndex = Index + 1;

                        if (expressions.Count <= rightIndex)
                        {
                            _sealResult = Match.Index + Match.Length;
                        }
                        else
                        {
                            var rightExpression = expressions[rightIndex];

                            if (AllowedRights.Contains(rightExpression.GroupName))
                            {
                                Right = rightExpression.Root;
                            }
                            else
                            {
                                _sealResult = rightExpression.Match.Index;
                            }
                        }
                    }
                }
            }

            return _sealResult;
        }

    }
}