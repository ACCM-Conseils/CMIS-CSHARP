using System;
using System.Collections.Generic;
using st = System.Text;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Represents a fieldexpression (i.e. in 'select' or 'order by' part of a query string)
   /// </summary>
   /// <remarks></remarks>
    public class FieldExpression : DatabaseObjectExpression
    {

        #region Constructors
        public FieldExpression(Expression field) : base(field)
        {
        }
        #endregion

        protected override bool AllowAlias
        {
            get
            {
                return _parent is not null && _parent.GroupName == "Select";
            }
        }

        protected bool AllowOrderDirection
        {
            get
            {
                return _parent is not null && _parent.GroupName == "OrderBy";
            }
        }

        protected override string GetValue(Type executingType)
        {
            string myBaseValue = base.GetValue(executingType);

            if (GetType().IsAssignableFrom(executingType))
            {
                var sb = new st.StringBuilder(myBaseValue);

                if (!(_orderDirection is null || string.IsNullOrEmpty(_orderDirection.Value)))
                {
                    sb.Append(" ");
                    sb.Append(_orderDirection.Value);
                }

                return sb.ToString();
            }
            else
            {
                return myBaseValue;
            }
        }

        private OrderDirectionExpression _orderDirection;
        public OrderDirectionExpression OrderDirection
        {
            get
            {
                return _orderDirection;
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
                if (!_sealResult.HasValue && AllowOrderDirection)
                {
                    _orderDirection = GetRightExpression(expressions) as OrderDirectionExpression;
                    if (_orderDirection is not null)
                    {
                        _children.Add(_orderDirection);
                        SetParent(_orderDirection, this);
                        _sealResult = _orderDirection.Seal(expressions);
                    }
                }
            }

            return _sealResult;
        }

    }
}