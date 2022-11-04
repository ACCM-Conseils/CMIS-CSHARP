using System;
using System.Collections.Generic;
using st = System.Text;

namespace CmisObjectModel.ServiceModel.Query
{
    public class TableExpression : DatabaseObjectExpression
    {

        #region Constructors
        public TableExpression(Expression table) : base(table)
        {
        }
        #endregion

        protected override bool AllowAlias
        {
            get
            {
                return true;
            }
        }

        protected override string GetValue(Type executingType)
        {
            string myBaseResult = base.GetValue(executingType);

            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(myBaseResult);

                if (_join is not null)
                {
                    sb.Append(" ");
                    sb.Append(_join.Value);
                }

                return sb.ToString();
            }
            else
            {
                return myBaseResult;
            }
        }

        private JoinExpression _join;
        public JoinExpression Join
        {
            get
            {
                return _join;
            }
            set
            {
                if (_join is not null)
                    _children.Remove(_join);
                _join = value;
                if (value is not null)
                {
                    _children.Add(value);
                    SetParent(value, this);
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
                _sealResult = base.Seal(expressions);
                if (!_sealResult.HasValue)
                {
                    Join = GetRightExpression(expressions) as JoinExpression;
                    if (_join is not null)
                        _sealResult = _join.Seal(expressions);
                }
            }

            return _sealResult;
        }

    }
}