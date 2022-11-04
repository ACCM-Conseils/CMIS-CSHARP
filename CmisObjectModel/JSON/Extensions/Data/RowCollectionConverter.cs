
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Extensions.Data
{
    /// <summary>
   /// Converter for RowCollection-instances
   /// </summary>
   /// <remarks></remarks>
    public class RowCollectionConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Extensions.Data.RowCollection>
    {

        #region Constructors
        public RowCollectionConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Extensions.Data.RowCollection>())
        {
        }
        public RowCollectionConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Extensions.Data.RowCollection> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        #region Helper-classes
        private sealed class RowCollectionWriter : CmisObjectModel.Extensions.Data.RowCollection
        {

            private RowCollectionWriter() : base()
            {
            }

            public static void Write(CmisObjectModel.Extensions.Data.RowCollection instance, string rowIndexPropertyDefinitionId, string rowTypeId, string tableName, CmisObjectModel.Extensions.Data.Row[] rows)
            {
                SilentInitialization(instance, rowIndexPropertyDefinitionId, rowTypeId, tableName, rows);
            }
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            CmisObjectModel.Extensions.Data.Row[] rows = null;

            rows = ReadArray(context, "rows", rows);
            RowCollectionWriter.Write(context.Object, Read(context.Dictionary, "rowIndexPropertyDefinitionId", context.Object.RowIndexPropertyDefinitionId), Read(context.Dictionary, "rowTypeId", context.Object.RowTypeId), Read(context.Dictionary, "tableName", context.Object.TableName), rows);
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("extensionTypeName", context.Object.ExtensionTypeName);
            WriteArray(context, context.Object.Rows, "rows");
            context.Add("rowIndexPropertyDefinitionId", context.Object.RowIndexPropertyDefinitionId);
            context.Add("rowTypeId", context.Object.RowTypeId);
            context.Add("tableName", context.Object.TableName);
        }
    }
}