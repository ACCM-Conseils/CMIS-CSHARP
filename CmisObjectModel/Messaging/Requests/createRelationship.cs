using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class createRelationship
    {

        /// <summary>
      /// Reads transmitted parameters from queryString
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public override void ReadQueryString(string repositoryId)
        {
            var requestParams = ssw.WebOperationContext.Current is null ? null : ssw.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;

            base.ReadQueryString(repositoryId);

            if (requestParams is not null)
            {
                _repositoryId = Read(repositoryId, _repositoryId);
            }
        }

        /// <summary>
      /// Wraps the request-parameters of the createRelationship-Service
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator AtomPub.AtomEntry(createRelationship value)
        {
            if (value is null || value._properties is null)
            {
                return null;
            }
            else
            {
                var cmisraObject = new Core.cmisObjectType(value._properties);

                if (value._policies is not null && value._policies.Length > 0)
                {
                    cmisraObject.PolicyIds = new Core.Collections.cmisListOfIdsType() { Ids = value._policies };
                }

                return new AtomPub.AtomEntry(cmisraObject);
            }
        }

    }
}