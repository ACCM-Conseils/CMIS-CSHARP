using System;
using ss = System.ServiceModel;
using sss = System.ServiceModel.Syndication;
using sws = System.Web.Services;
using CmisObjectModel.Constants;
using cm = CmisObjectModel.Messaging;

namespace CmisObjectModel.ServiceModel.WebService
{
    /// <summary>
   /// Cmis Webservice-implementation
   /// </summary>
   /// <remarks>under construction</remarks>
    [sws.WebService(Namespace = Namespaces.cmisw, Description = "CMIS-WebService")]
    [sws.WebServiceBinding(ConformsTo = sws.WsiProfiles.BasicProfile1_1)]
    [ss.XmlSerializerFormat(SupportFaults = true)]
    public class CmisWebService : sws.WebService
    {

        #region Repository
        [sws.WebMethod(EnableSession = false, Description = "Creates a new type in the repository")]
        [ss.OperationContract()]
        [ss.FaultContract(typeof(cm.cmisFaultType), Name = "cmisFault", Namespace = Namespaces.cmism)]
        public cm.Responses.createTypeResponse CreateType(cm.Requests.createType request)
        {
            var response = CmisWebServiceImpl.CreateType(request);

            if (response is null)
            {
                throw cm.cmisFaultType.CreateUnknownException();
            }
            else
            {
                return response;
            }
        }
        #endregion

        /// <summary>
      /// Encapsulates the identity of requesting user as SyndicationPerson
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public sss.SyndicationPerson Author
        {
            get
            {
                var user = Context is null ? null : Context.User;
                var identity = user is null ? null : user.Identity;
                string userName = identity is null ? null : identity.Name;

                return string.IsNullOrEmpty(userName) ? null : new sss.SyndicationPerson(null, userName, null);
            }
        }

        /// <summary>
      /// Encapsulates the identity of requesting user as SyndicationPerson()
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public sss.SyndicationPerson[] Authors
        {
            get
            {
                var author = Author;
                return author is null ? null : (new sss.SyndicationPerson[] { author });
            }
        }

        public Uri BaseUri
        {
            get
            {
                return new Uri(Context.Request.RawUrl);
            }
        }

        private Generic.ServiceImplFactory<CmisWebServiceImplBase> _cmisServiceImplFactory;
        /// <summary>
      /// Returns ICmisService-instance that implements the services declared in cmis
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        private CmisWebServiceImplBase CmisWebServiceImpl
        {
            get
            {
                if (_cmisServiceImplFactory is null)
                    _cmisServiceImplFactory = new Generic.ServiceImplFactory<CmisWebServiceImplBase>(BaseUri);
                return _cmisServiceImplFactory.CmisServiceImpl;
            }
        }
    }
}