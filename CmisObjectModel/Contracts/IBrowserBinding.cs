using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Contracts
{
    /// <summary>
   /// CMIS-BrowserBinding services supported in this assembly
   /// </summary>
   /// <remarks>
   /// WCF Service
   /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
   /// 5.3 URLs</remarks>
    [ss.ServiceContract(SessionMode = ss.SessionMode.NotAllowed, Namespace = Namespaces.cmisw)]
    public interface IBrowserBinding
    {

        #region 5.3.1. Service URL
        [ss.OperationContract(Name = "dispatchWebGetService")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetRepositories, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebGetService();

        [ss.OperationContract(Name = "dispatchWebPostService")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.GetRepositories, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebPostService(System.IO.Stream stream);
        #endregion

        #region 5.3.2 Repository URL
        [ss.OperationContract(Name = "dispatchWebGetRepository")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetRepositoryInfo, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebGetRepository(string repositoryId);

        [ss.OperationContract(Name = "dispatchWebPostRepository")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.GetRepositoryInfo, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebPostRepository(string repositoryId, System.IO.Stream stream);
        #endregion

        #region 5.3.3 Root Folder URL
        [ss.OperationContract(Name = "dispatchWebGetRootFolder")]
        [ssw.WebGet(UriTemplate = ServiceURIs.RootFolder + "?objectId={objectId}", ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebGetRootFolder(string repositoryId, string objectId);

        [ss.OperationContract(Name = "dispatchWebPostRootFolder")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.RootFolder + "?objectId={objectId}", ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebPostRootFolder(string repositoryId, string objectId, System.IO.Stream stream);
        #endregion

        #region 5.3.4 Object URL
        [ss.OperationContract(Name = "dispatchWebGetObjects")]
        [ssw.WebGet(UriTemplate = ServiceURIs.AbsoluteObject, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebGetObjects(string repositoryId, string path);

        [ss.OperationContract(Name = "dispatchWebPostObjects")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.AbsoluteObject, ResponseFormat = ssw.WebMessageFormat.Json)]
        System.IO.Stream DispatchWebPostObjects(string repositoryId, string path, System.IO.Stream stream);
        #endregion

    }
}