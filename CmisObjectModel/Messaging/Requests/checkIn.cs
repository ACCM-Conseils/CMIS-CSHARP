using ssw = System.ServiceModel.Web;

namespace CmisObjectModel.Messaging.Requests
{
    public partial class checkIn
    {

        protected bool _pwcRequired = false;
        /// <summary>
      /// Set this property to True to make sure, that CheckIn is supported for private working copies only. The default is False.
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool PWCLinkRequired
        {
            get
            {
                return _pwcRequired;
            }
            set
            {
                if (value != _pwcRequired)
                {
                    _pwcRequired = value;
                    OnPropertyChanged("PWCLinkRequired", value, !value);
                }
            }
        } // PWCLinkRequired

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
                _checkinComment = Read(requestParams["checkinComment"], _checkinComment);
                _major = Read(requestParams["major"], _major);
            }
        }

    }
}