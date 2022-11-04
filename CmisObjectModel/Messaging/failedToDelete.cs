
namespace CmisObjectModel.Messaging
{
    public partial class failedToDelete
    {

        public failedToDelete(params string[] objectIds)
        {
            _objectIds = objectIds;
        }

    }
}