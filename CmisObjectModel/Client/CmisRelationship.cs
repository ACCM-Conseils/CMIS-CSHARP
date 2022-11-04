using ccg = CmisObjectModel.Common.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    public class CmisRelationship : CmisObject
    {

        #region Constructors
        public CmisRelationship(Core.cmisObjectType cmisObject, Contracts.ICmisClient client, Core.cmisRepositoryInfoType repositoryInfo) : base(cmisObject, client, repositoryInfo)
        {
        }
        #endregion

        #region Predefined properties
        public virtual ccg.Nullable<string> SourceId
        {
            get
            {
                return _cmisObject.SourceId;
            }
            set
            {
                _cmisObject.SourceId = value;
            }
        }

        public virtual ccg.Nullable<string> TargetId
        {
            get
            {
                return _cmisObject.TargetId;
            }
            set
            {
                _cmisObject.TargetId = value;
            }
        }
        #endregion

    }
}