using ccg = CmisObjectModel.Common.Generic;
using cmr = CmisObjectModel.Messaging.Requests;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Simplifies requests to a cmis policy
   /// </summary>
   /// <remarks></remarks>
    public class CmisPolicy : CmisObject
    {

        #region Constructors
        public CmisPolicy(Core.cmisObjectType cmisObject, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(cmisObject, client, repositoryInfo)
        {
        }
        #endregion

        #region Predefined properties
        public virtual ccg.Nullable<string> PolicyText
        {
            get
            {
                return _cmisObject.PolicyText;
            }
            set
            {
                _cmisObject.PolicyText = value;
            }
        }
        #endregion

        #region Policy
        /// <summary>
      /// Applies a current policy to the specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new bool ApplyPolicy(string objectId)
        {
            {
                var withBlock = _client.ApplyPolicy(new cmr.applyPolicy() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = objectId, PolicyId = _cmisObject.ObjectId });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }

        /// <summary>
      /// Removes the current policy from an specified object
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public new bool RemovePolicy(string objectId)
        {
            {
                var withBlock = _client.RemovePolicy(new cmr.removePolicy() { RepositoryId = _repositoryInfo.RepositoryId, ObjectId = objectId, PolicyId = _cmisObject.ObjectId });
                _lastException = withBlock.Exception;
                return _lastException is null;
            }
        }
        #endregion

    }
}