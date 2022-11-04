using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class OrderByExpression : FieldContainerExpression
    {

        #region Constructors
        public OrderByExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {
        }
        #endregion

        public override bool CanSetValue()
        {
            return false;
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
                    var rightExpression = GetRightExpression(expressions);

                    while (rightExpression is not null)
                    {
                        if (_separatorExpected != (rightExpression.GroupName == "Separator"))
                        {
                            // end of order-by expression
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
                        else if (!AddField(rightExpression, expressions))
                        {
                            // expected field not present
                            _sealResult = rightExpression.Match.Index;
                            break;
                        }
                        _separatorExpected = !_separatorExpected;
                        rightExpression = rightExpression.GetRightExpression(expressions);
                    }
                    // check, if there is at least one field
                    if (!_sealResult.HasValue && _fields.Count == 0)
                    {
                        _sealResult = Match.Index + Match.Length;
                    }
                }
            }

            return _sealResult;
        }

    }
}