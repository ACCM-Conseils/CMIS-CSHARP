
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON
{
    /// <summary>
   /// Converter-class for transactions
   /// </summary>
   /// <remarks></remarks>
    public class TransactionConverter : Serialization.Generic.JavaScriptConverter<Transaction>
    {

        #region Constructors
        public TransactionConverter() : base(new Serialization.Generic.DefaultObjectResolver<Transaction>())
        {
        }
        public TransactionConverter(Serialization.Generic.ObjectResolver<Transaction> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Code = Read(context.Dictionary, "code", context.Object.Code);
            context.Object.Exception = Read(context.Dictionary, "exception", context.Object.Exception);
            context.Object.Message = Read(context.Dictionary, "message", context.Object.Message);
            context.Object.ObjectId = Read(context.Dictionary, "objectId", context.Object.ObjectId);
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("code", context.Object.Code);
            if (!string.IsNullOrEmpty(context.Object.Exception))
                context.Add("exception", context.Object.Exception);
            if (!string.IsNullOrEmpty(context.Object.Message))
                context.Add("message", context.Object.Message);
            if (!string.IsNullOrEmpty(context.Object.ObjectId))
                context.Add("objectId", context.Object.ObjectId);
        }
    }
}