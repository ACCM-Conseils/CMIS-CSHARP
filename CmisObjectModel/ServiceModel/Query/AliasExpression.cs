using System;
using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Alias for table or column
   /// </summary>
   /// <remarks></remarks>
    public class AliasExpression : Expression
    {

        #region Constructors
        public AliasExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {
            _aliasName = match.Groups["AliasName"].Value;
        }
        #endregion

        protected string _aliasName;
        public string AliasName
        {
            get
            {
                return _aliasName;
            }
        }

        protected override string GetValue(Type executingType)
        {
            if (GetType().IsAssignableFrom(executingType))
            {
                return "As " + _aliasName;
            }
            else
            {
                return base.GetValue(executingType);
            }
        }

        /// <summary>
      /// Test if the instance is bound to a DatabaseObjectExpression. If not the parsed query expression is not valid.
      /// </summary>
      /// <param name="expressions"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public override int? Seal(List<Expression> expressions)
        {
            if (!_sealed)
            {
                _sealResult = base.Seal(expressions);

                if (!_sealResult.HasValue && !(_parent is DatabaseObjectExpression))
                    _sealResult = Match.Index;
            }

            return _sealResult;
        }

    }
}