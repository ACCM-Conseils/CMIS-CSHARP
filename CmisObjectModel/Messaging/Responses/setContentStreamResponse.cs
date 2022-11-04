using ca = CmisObjectModel.AtomPub;

namespace CmisObjectModel.Messaging.Responses
{
    public partial class setContentStreamResponse
    {

        public setContentStreamResponse(string objectId, string changeToken, enumSetContentStreamResult result)
        {
            _objectId = objectId;
            _changeToken = changeToken;
            _result = result;
        }

        /// <summary>
      /// Creates a new instance from the reader
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks>If the reader points to an AtomEntry-instance, the ObjectId and ChangeToken properties are used
      /// to create a setContentStreamResponse-instance</remarks>
        public static setContentStreamResponse CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            if (string.Compare(reader.LocalName, "entry", true) == 0)
            {
                {
                    var withBlock = ca.AtomEntry.CreateInstance(reader);
                    return new setContentStreamResponse(withBlock.ObjectId, withBlock.ChangeToken, enumSetContentStreamResult.NotSet);
                }
            }
            else
            {
                var retVal = new setContentStreamResponse();
                retVal.ReadXml(reader);
                return retVal;
            }
        }

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

        private enumSetContentStreamResult _result = enumSetContentStreamResult.NotSet;
        public enumSetContentStreamResult Result
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