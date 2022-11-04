using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class BetweenExpression : OperatorExpression
    {

        #region Constructors
        public BetweenExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, true)
        {
        }
        #endregion

        /// <summary>
      /// Searches for the And expression that belongs to this between
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private OperatorExpression FindAndExpression(List<Expression> expressions)
        {
            var rightExpression = GetRightExpression(expressions);

            // skip method followed by open parenthesis
            if (rightExpression is not null && rightExpression.GroupName == "Method")
                rightExpression = rightExpression.GetRightExpression(expressions);
            // skip parenthesis-block
            if (rightExpression is not null && rightExpression.GroupName == "OpenParenthesis")
            {
                int openCounter = 1;
                var offsets = new Dictionary<string, int>() { { "OpenParenthesis", 1 }, { "CloseParenthesis", -1 } };
                for (int index = rightExpression.Index + 1, loopTo = expressions.Count - 1; index <= loopTo; index++)
                {
                    if (offsets.ContainsKey(expressions[index].GroupName))
                    {
                        openCounter += offsets[expressions[index].GroupName];
                        if (openCounter == 0)
                        {
                            rightExpression = expressions[index];
                            break;
                        }
                    }
                }
                // at least one open parenthesis is not closed
                if (openCounter > 0)
                    return null;
            }
            // the next expression MUST be the and expression
            if (rightExpression is not null)
                rightExpression = rightExpression.GetRightExpression(expressions);
            return rightExpression is null || string.Compare(rightExpression.Match.Value, "And", true) == 0 ? (OperatorExpression)rightExpression : null;
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
                _sealed = true;
                if (Index == 0 || Index + 3 > expressions.Count - 1)
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

                        var andExpression = FindAndExpression(expressions);
                        if (andExpression is null)
                        {
                            _sealResult = Match.Index + Match.Length;
                        }
                        else
                        {
                            _sealResult = andExpression.Seal(expressions);
                            Right = andExpression;
                        }
                    }
                    else
                    {
                        _sealResult = leftExpression.Match.Index;
                    }
                }
            }

            return _sealResult;
        }

    }
}