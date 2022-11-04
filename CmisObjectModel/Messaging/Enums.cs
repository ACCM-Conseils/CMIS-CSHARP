
namespace CmisObjectModel.Messaging
{
    public enum enumDeleteTreeResult : int
    {
        OK = System.Net.HttpStatusCode.OK,
        Accepted = System.Net.HttpStatusCode.Accepted,
        NoContent = System.Net.HttpStatusCode.NoContent,
        Unauthorized = System.Net.HttpStatusCode.Unauthorized,
        Forbidden = System.Net.HttpStatusCode.Forbidden,
        InternalServerError = System.Net.HttpStatusCode.InternalServerError
    }

    public enum enumGetContentStreamResult : int
    {
        Content = System.Net.HttpStatusCode.OK,
        PartialContent = System.Net.HttpStatusCode.PartialContent,

        NotSet = System.Net.HttpStatusCode.InternalServerError
    }

    public enum enumSetContentStreamResult : int
    {
        HasContent = System.Net.HttpStatusCode.OK,
        NoContent = System.Net.HttpStatusCode.NoContent,
        Created = System.Net.HttpStatusCode.Created,

        NotSet = System.Net.HttpStatusCode.InternalServerError
    }
}