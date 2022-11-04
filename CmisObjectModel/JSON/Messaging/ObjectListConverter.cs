
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Responses
{
    [Attributes.JavaScriptConverter(typeof(JSON.Messaging.ObjectListConverter))]
    public partial class getContentChangesResponse
    {
    }
}

namespace CmisObjectModel.JSON.Messaging
{
    /// <summary>
   /// Converter for a cmisObjectListType expanded with changeLogToken-property
   /// </summary>
   /// <remarks>BrowserBinding uses for object-lists (except query result lists) an additional property changeLogToken
   /// to use the http://docs.oasis-open.org/ns/cmis/browser/201103/objectList specification for all methods returning
   /// object-lists (the property is only used for getContentChanges())</remarks>
    public class ObjectListConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Messaging.Responses.getContentChangesResponse>
    {

        #region Constructors
        public ObjectListConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Messaging.Responses.getContentChangesResponse>())
        {
        }
        public ObjectListConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Messaging.Responses.getContentChangesResponse> objectObserver) : base(objectObserver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            var objects = new CmisObjectModel.Messaging.cmisObjectListType();

            context.Object.Objects = objects;
            objects.Objects = ReadArray(context, "objects", objects.Objects);
            objects.HasMoreItems = Read(context.Dictionary, "hasMoreItems", objects.HasMoreItems);
            objects.NumItems = ReadNullable(context.Dictionary, "numItems", objects.NumItems);
            context.Object.ChangeLogToken = Read(context.Dictionary, "changeLogToken", context.Object.ChangeLogToken);
        }

        protected override void Serialize(SerializationContext context)
        {
            var objects = (context.Object.Objects is null ? null : context.Object.Objects.Objects) ?? (new CmisObjectModel.Core.cmisObjectType[] { });
            bool hasMoreItems = context.Object.Objects is null ? false : context.Object.Objects.HasMoreItems;
            var numItems = context.Object.Objects is null ? default : context.Object.Objects.NumItems;

            WriteArray(context, objects, "objects");
            context.Add("hasMoreItems", hasMoreItems);
            if (numItems.HasValue)
                context.Add("numItems", numItems.Value);
            if (!string.IsNullOrEmpty(context.Object.ChangeLogToken))
                context.Add("changeLogToken", context.Object.ChangeLogToken);
        }
    }
}