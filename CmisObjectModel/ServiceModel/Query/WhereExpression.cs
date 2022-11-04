using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class WhereExpression : SqlPartExpression
    {

        #region Constructors
        public WhereExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, null, " ")
        {
        }
        #endregion

        public override bool CanSetValue()
        {
            return false;
        }

        private Expression _condition;
        public Expression Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                if (_condition is not null)
                    _children.Remove(_condition);
                _condition = value;
                if (value is not null)
                {
                    SetParent(value, this);
                    _children.Add(value);
                }
            }
        }

        private static HashSet<string> _allowedConditions = new HashSet<string>() { "CompareOperator", "LogicalOperator", "OpenParenthesis", "Method", "Negation" };
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
                        _sealResult = Match.Index;
                    }
                    else
                    {
                        rightExpression = rightExpression.Root;
                        if (!_allowedConditions.Contains(rightExpression.GroupName))
                        {
                            _sealResult = Match.Index + Match.Length;
                        }
                        else
                        {
                            Condition = rightExpression;
                            _sealResult = rightExpression.Seal(expressions);
                        }
                    }
                }
            }

            return _sealResult;
        }

    }
}