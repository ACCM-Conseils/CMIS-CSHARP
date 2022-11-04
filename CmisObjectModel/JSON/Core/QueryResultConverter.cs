
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Core
{
    /// <summary>
   /// Converter for cmisObjectType-instances in query result list
   /// </summary>
   /// <remarks></remarks>
    public class QueryResultConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Core.cmisObjectType>
    {

        #region Constructors
        public QueryResultConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Core.cmisObjectType>())
        {
        }
        public QueryResultConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.cmisObjectType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Properties = Read(context, "properties", context.Object.Properties);
            context.Object.AllowableActions = Read(context, "allowableActions", context.Object.AllowableActions);
            context.Object.Relationships = ReadArray(context, "relationships", context.Object.Relationships);
            context.Object.Renditions = ReadArray(context, "renditions", context.Object.Renditions);
        }

        protected override void Serialize(SerializationContext context)
        {
            Write(context, context.Object.Properties, "properties");
            Write(context, context.Object.AllowableActions, "allowableActions");
            WriteArray(context, context.Object.Relationships, "relationships");
            WriteArray(context, context.Object.Renditions, "renditions");
        }
    }
}