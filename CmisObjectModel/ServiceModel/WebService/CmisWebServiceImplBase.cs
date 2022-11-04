using System;
using cm = CmisObjectModel.Messaging;

namespace CmisObjectModel.ServiceModel.WebService
{
    public abstract class CmisWebServiceImplBase
    {

        private Uri _baseUri;
        protected CmisWebServiceImplBase(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        #region Repository
        public abstract cm.Responses.createTypeResponse CreateType(cm.Requests.createType request);
        #endregion

    }
}