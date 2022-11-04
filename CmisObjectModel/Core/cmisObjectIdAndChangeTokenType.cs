
namespace CmisObjectModel.Core
{
    public partial class cmisObjectIdAndChangeTokenType
    {

        /// <summary>
      /// Converts AtomEntry of object
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisObjectIdAndChangeTokenType(AtomPub.AtomEntry value)
        {
            // see 3.8.6.1 HTTP POST:
            // The property cmis:objectId MUST be set.
            // The value MUST be the original object id even if the repository created a new version and therefore generated a new object id.
            // New object ids are not exposed by AtomPub binding. 
            // The property cmis:changeToken MUST be set if the repository supports change tokens
            return value is null ? null : new cmisObjectIdAndChangeTokenType() { _id = value.ObjectId, _changeToken = value.ChangeToken };
        }

    }
}