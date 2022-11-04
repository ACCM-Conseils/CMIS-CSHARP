using str = System.Text.RegularExpressions;

namespace CmisObjectModel.ServiceModel.Query
{
    /// <summary>
   /// Contains the name of a database object (table or column)
   /// </summary>
   /// <remarks></remarks>
    public class IdentifierExpression : Expression
    {

        #region Constructors
        public IdentifierExpression(str.Match match, string groupName, int rank, int index) : base(match, groupName, rank, index)
        {

            var groupAny = match.Groups["Any"];
            _anyIsPresent = groupAny is not null && groupAny.Success;
            _identifier = match.Groups[groupName].Value;
        }
        #endregion

        protected bool _anyIsPresent;
        public bool AnyIsPresent
        {
            get
            {
                return _anyIsPresent;
            }
        }

        protected string _identifier;
        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

    }
}