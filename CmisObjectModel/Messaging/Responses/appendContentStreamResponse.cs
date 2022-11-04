using ca = CmisObjectModel.AtomPub;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Responses
{
    public partial class appendContentStreamResponse
    {

        public appendContentStreamResponse(string objectId, string changeToken)
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
      /// to create a appendContentStreamResponse-instance</remarks>
        public static appendContentStreamResponse CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            if (string.Compare(reader.LocalName, "entry", true) == 0)
            {
                {
                    var withBlock = ca.AtomEntry.CreateInstance(reader);
                    return new appendContentStreamResponse(withBlock.ObjectId, withBlock.ChangeToken);
                }
            }
            else
            {
                var retVal = new appendContentStreamResponse();
                retVal.ReadXml(reader);
                return retVal;
            }
        }

    }
}