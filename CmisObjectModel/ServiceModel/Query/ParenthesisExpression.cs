using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class ParenthesisExpression : CompositeExpression
    {

        #region Constructors
        public ParenthesisExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, ", ", null)
        {
        }
        #endregion

        private bool _separatorExpected = false;
        private bool Add(Expression expression)
        {
            bool retVal = _termination is null && _separatorExpected == (expression.GroupName == "Separator");

            if (retVal)
            {
                SetParent(expression, this);
                _separatorExpected = !_separatorExpected;
                if (_separatorExpected)
                    _children.Add(expression);
            }

            return retVal;
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
                    if (Match.Value == ")")
                    {
                        if (_parent is null)
                            _sealResult = Match.Index;
                    }
                    else
                    {
                        var rightExpression = GetRightExpression(expressions);

                        while (rightExpression is not null)
                        {
                            if (rightExpression is ParenthesisExpression && rightExpression.Match.Value == ")")
                            {
                                _termination = rightExpression;
                                SetParent(rightExpression, this);
                                break;
                            }
                            else if (_separatorExpected != (rightExpression.GroupName == "Separator"))
                            {
                                // expected expression missed
                                _sealResult = rightExpression.Match.Index;
                                break;
                            }
                            else if (_separatorExpected)
                            {
                                // the next separator MUST NOT have a parent at this time
                                if (rightExpression.Parent is null)
                                {
                                    SetParent(rightExpression, this);
                                }
                                else
                                {
                                    _sealResult = rightExpression.Match.Index;
                                    break;
                                }
                            }
                            else
                            {
                                rightExpression = rightExpression.Root;
                                _children.Add(rightExpression);
                                SetParent(rightExpression, this);
                                _sealResult = rightExpression.Seal(expressions);
                                if (_sealResult.HasValue)
                                    break;
                            }
                            _separatorExpected = !_separatorExpected;
                            rightExpression = rightExpression.GetRightExpression(expressions);
                        }
                        // check if the open parenthesis is followed by a close parenthesis
                        if (_termination is null)
                            _sealResult = Match.Index;
                    }
                }
            }

            return _sealResult;
        }

    }
}