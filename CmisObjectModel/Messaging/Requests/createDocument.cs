using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class createDocument
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
                _folderId = Read(requestParams["folderId"], _folderId);
                _versioningState = ReadOptionalEnum(requestParams["versioningState"], _versioningState);
            }
        }

        /// <summary>
      /// Wraps the request-parameters of the createDocument-Service
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator AtomPub.AtomEntry(createDocument value)
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
                if (value._contentStream is null)
                {
                    return new AtomPub.AtomEntry(cmisraObject);
                }
                else
                {
                    return new AtomPub.AtomEntry(cmisraObject, new RestAtom.cmisContentType(value._contentStream.Stream, value._contentStream.MimeType));
                }
            }
        }

    }
}