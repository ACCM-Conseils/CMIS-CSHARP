using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class removeObjectFromFolder
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
                _folderId = Read(requestParams["folderId"], _folderId);
            }
        }

    }
}