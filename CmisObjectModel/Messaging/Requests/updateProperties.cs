using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class updateProperties
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
                _objectId = Read(requestParams["objectId"], _objectId);
                _changeToken = Read(requestParams["changeToken"], _changeToken);
            }
        }

        /// <summary>
      /// Wraps the request-parameters of the updateProperties-Service
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator AtomPub.AtomEntry(updateProperties value)
        {
            if (value is null || value._properties is null)
            {
                return null;
            }
            else
            {
                var cmisraObject = new Core.cmisObjectType(value._properties);

                return new AtomPub.AtomEntry(cmisraObject);
            }
        }

    }
}