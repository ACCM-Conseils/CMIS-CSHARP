
namespace CmisObjectModel.Messaging.Responses
{
    public partial class updatePropertiesResponse
    {

        public override Core.cmisObjectType Object
        {
            get
            {
                return base.Object;
            }
            set
            {
                if (!ReferenceEquals(_object, value))
                {
                    base.Object = value;
                    ChangeToken = value is null ? default : value.ChangeToken;
                }
            }
        }

    }
}