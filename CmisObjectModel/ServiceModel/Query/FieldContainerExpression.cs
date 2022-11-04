using System.Collections.Generic;
using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    public abstract class FieldContainerExpression : SqlPartExpression
    {

        #region Constructors
        public FieldContainerExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index, ", ", " ")
        {
        }
        #endregion

        protected List<FieldExpression> _fields = new List<FieldExpression>();
        public FieldExpression[] Fields
        {
            get
            {
                return _fields.ToArray();
            }
        }

        private static HashSet<string> _allowedFields = new HashSet<string>() { "Constant", "Identifier", "MathOperator", "Method", "OpenParenthesis", "Sign", "StringOperator" };
        protected bool AddField(Expression field, List<Expression> expressions)
        {
            var root = field.Root;

            if (_allowedFields.Contains(root.GroupName))
            {
                var newField = new FieldExpression(root);

                _children.Add(newField);
                SetParent(newField, this);
                _fields.Add(newField);
                _sealResult = newField.Seal(expressions);
            }
            else
            {
                _sealResult = field.Match.Index;
            }

            return !_sealResult.HasValue;
        }

    }
}