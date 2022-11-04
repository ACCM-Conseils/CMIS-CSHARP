using System.Collections.Generic;
using System.Data;
using System.Linq;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class FromExpression : SqlPartExpression
    {

        #region Constructors
        public FromExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, ", ", " ")
        {
        }
        #endregion

        public override bool CanSetValue()
        {
            return false;
        }

        private static HashSet<string> _allowedTables = new HashSet<string>() { "Identifier", "OpenParenthesis" };
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
                        bool isSeparator = rightExpression.GroupName == "Separator";
                        bool isTable = _allowedTables.Contains(rightExpression.GroupName);

                        if (!(isSeparator || isTable))
                        {
                            break;
                        }
                        else if (_separatorExpected != isSeparator)
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
                        else if (isTable)
                        {
                            // the next expression MUST be a table expression without a parent
                            if (rightExpression.Parent is null)
                            {
                                var tableExpression = new TableExpression(rightExpression);
                                _children.Add(tableExpression);
                                SetParent(tableExpression, this);
                                _sealResult = tableExpression.Seal(expressions);
                                if (_sealResult.HasValue)
                                    break;
                            }
                            else
                            {
                                _sealResult = rightExpression.Match.Index;
                                break;
                            }
                        }
                        else
                        {
                            // the next expression is neither a table expression nor a separator
                            break;
                        }
                        // get the next expression
                        rightExpression = rightExpression.GetRightExpression(expressions);
                        _separatorExpected = !_separatorExpected;
                    }
                    // check, if there is at least one table defined and the last expression MUST NOT be a separator
                    if (!(_sealResult.HasValue || _separatorExpected))
                    {
                        var tables = this.Tables;
                        int length = tables.Length;

                        {
                            //A remplacer
                            /*var withBlock = length == 0 ? (Expression)this : (Expression)tables[length - 1].Match;
                            _sealResult = withBlock.Index + withBlock.Length;*/
                        }
                    }
                }
            }

            return _sealResult;
        }

        /// <summary>
      /// Returns the children of type TableExpression
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>Normally it should be the complete list of _children</remarks>
        public TableExpression[] Tables
        {
            get
            {
                return (from child in _children
                        let table = child as TableExpression
                        where table is not null
                        select table).ToArray();
            }
        }

    }
}