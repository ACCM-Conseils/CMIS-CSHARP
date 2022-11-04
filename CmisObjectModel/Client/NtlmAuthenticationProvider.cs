using sn = System.Net;
using ssd = System.ServiceModel.Description;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    public class NtlmAuthenticationProvider : AuthenticationProvider
    {

        public NtlmAuthenticationProvider(string user, System.Security.SecureString password) : base(user, password)
        {
        }

        #region Authentication
        /// <summary>
      /// Authentication AtomPub-Binding
      /// </summary>
      /// <param name="request"></param>
      /// <remarks></remarks>
        protected override void HttpAuthenticate(sn.HttpWebRequest request)
        {
            if (string.IsNullOrEmpty(_user) && (_password is null || _password.Length == 0))
            {
                request.Credentials = sn.CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                request.Credentials = new sn.NetworkCredential(_user, _password);
            }
            request.CookieContainer = _cookies;
            request.AllowWriteStreamBuffering = true;
        }

        /// <summary>
      /// Authentication WebService-Binding
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="clientCredentials"></param>
      /// <remarks></remarks>
        protected override void AddWebServiceCredentials(ssd.ServiceEndpoint endPoint, ssd.ClientCredentials clientCredentials)
        {
            System.ServiceModel.Channels.CustomBinding binding = endPoint.Binding as System.ServiceModel.Channels.CustomBinding;

            if (binding is not null)
            {
                // remove SecurityBindingElement (reset before setting the credentials)
                binding.Elements.RemoveAll<System.ServiceModel.Channels.SecurityBindingElement>();
                if (string.IsNullOrEmpty(_user) && (_password is null || _password.Length == 0))
                {
                    clientCredentials.Windows.ClientCredential = sn.CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    clientCredentials.Windows.ClientCredential = new sn.NetworkCredential(_user, _password);
                }
            }
        }
        #endregion

    }
}