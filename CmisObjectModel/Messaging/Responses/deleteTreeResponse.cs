
namespace CmisObjectModel.Messaging.Responses
{
    public partial class deleteTreeResponse
    {

        public deleteTreeResponse(enumDeleteTreeResult result, params string[] failedToDeleteObjectIds)
        {
            if (failedToDeleteObjectIds is not null && failedToDeleteObjectIds.Length > 0)
            {
                _failedToDelete = new failedToDelete(failedToDeleteObjectIds);
            }
            else
            {
                _failedToDelete = new failedToDelete();
            }
        }

        private enumDeleteTreeResult _result = enumDeleteTreeResult.OK;
        public enumDeleteTreeResult Result
        {
            get
            {
                return _result;
            }
        }

        public System.Net.HttpStatusCode StatusCode
        {
            get
            {
                return (System.Net.HttpStatusCode)(int)_result;
            }
        }

    }
}