using ca = CmisObjectModel.AtomPub;

namespace CmisObjectModel.Messaging.Responses
{
    public partial class deleteContentStreamResponse
    {

        public deleteContentStreamResponse(string objectId, string changeToken)
        {
            _objectId = objectId;
            _changeToken = changeToken;
        }

        /// <summary>
      /// Creates a new instance from the reader
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks>If the reader points to an AtomEntry-instance, the ObjectId and ChangeToken properties are used
      /// to create a deleteContentStreamResponse-instance</remarks>
        public static deleteContentStreamResponse CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            if (string.Compare(reader.LocalName, "entry", true) == 0)
            {
                {
                    var withBlock = ca.AtomEntry.CreateInstance(reader);
                    return new deleteContentStreamResponse(withBlock.ObjectId, withBlock.ChangeToken);
                }
            }
            else
            {
                var retVal = new deleteContentStreamResponse();
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

    }
}