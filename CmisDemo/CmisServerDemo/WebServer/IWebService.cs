
namespace WebServer
{

    [System.ServiceModel.ServiceContract(Namespace = "http://demo.bsw/cmis")]
    public interface IWebService
    {

        [System.ServiceModel.OperationContract()]
        [System.ServiceModel.Web.WebGet(UriTemplate = "obj?id={objectId}")]
        System.IO.Stream ShowObject(string objectId);

        [System.ServiceModel.OperationContract()]
        [System.ServiceModel.Web.WebGet(UriTemplate = "file?id={objectId}")]
        System.IO.Stream GetContent(string objectId);

        [System.ServiceModel.OperationContract()]
        [System.ServiceModel.Web.WebGet(UriTemplate = "meta?id={objectId}")]
        System.IO.Stream GetMetadata(string objectId);

    }
}