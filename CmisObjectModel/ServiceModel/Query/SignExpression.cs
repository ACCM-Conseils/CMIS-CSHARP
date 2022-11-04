using System;
using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public class SignExpression : OperatorExpression
    {

        #region Constructors
        public SignExpression(str.Match match, int rank, int index) : base(match, "Sign", rank, index, false)
        {
        }
        #endregion

        private static HashSet<string> _allowedRights = new HashSet<string>() { "OpenParenthesis", "Constant", "Identifier", "Method", "Sign" };
        /// <summary>
      /// Returns the valid group names for the right side of the operator
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        protected override HashSet<string> AllowedRights
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
                var sign = this;
                bool isNegative = base.GetValue(typeof(Expression)) == "-";

                while (sign._right is SignExpression)
                {
                    sign = (SignExpression)sign._right;
                    isNegative = isNegative != (sign.Value == "-");
                }

                return (isNegative ? "-" : null) + (sign._right is null ? null : sign._right.Value);
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

    }
}