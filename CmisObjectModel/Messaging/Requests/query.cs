using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class query
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
                _statement = Read(requestParams["statement"], _statement);
                _searchAllVersions = Read(requestParams["searchAllVersions"], _searchAllVersions);
                _includeAllowableActions = Read(requestParams["includeAllowableActions"], _includeAllowableActions);
                _includeRelationships = ReadOptionalEnum(requestParams["includeRelationships"], _includeRelationships);
                _renditionFilter = Read(requestParams["renditionFilter"], _renditionFilter);
                _maxItems = Read(requestParams["maxItems"], _maxItems);
                _skipCount = Read(requestParams["skipCount"], _skipCount);
            }
        }

    }
}