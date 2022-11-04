using System.Data;
using System.Linq;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Base-class for sql-expressions (orderBy-expression, select-expression, from-expression and where-expression)
   /// </summary>
   /// <remarks></remarks>
    public abstract class SqlPartExpression : CompositeExpression
    {

        #region Constructors
        protected SqlPartExpression(str.Match match, string groupName, int rank, int index, string childrenSeparator, string childBlockSeparator) : base(match, groupName, rank, index, childrenSeparator, childBlockSeparator)
        {
        }
        #endregion

        /// <summary>
      /// Returns the SqlExpression of the sql-part without the introductory keyword
      /// </summary>
        public string GetSqlExpression()
        {
            return string.Join(_childrenSeparator ?? "", from child in _children
                                                         let childExpression = child.Value
                                                         select childExpression);
        }

    }
}