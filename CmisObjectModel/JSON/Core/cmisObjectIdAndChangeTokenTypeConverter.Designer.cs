﻿
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.cmisObjectIdAndChangeTokenTypeConverter))]
    public partial class cmisObjectIdAndChangeTokenType
    {
    }
}

namespace CmisObjectModel.JSON.Core
{
    [Attributes.AutoGenerated()]
    public partial class cmisObjectIdAndChangeTokenTypeConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Core.cmisObjectIdAndChangeTokenType>
    {

        #region Constructors
        public cmisObjectIdAndChangeTokenTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Core.cmisObjectIdAndChangeTokenType>())
        {
        }
        public cmisObjectIdAndChangeTokenTypeConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.cmisObjectIdAndChangeTokenType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Id = Read(context.Dictionary, "id", context.Object.Id);
            context.Object.NewId = Read(context.Dictionary, "newId", context.Object.NewId);
            context.Object.ChangeToken = Read(context.Dictionary, "changeToken", context.Object.ChangeToken);
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("id", context.Object.Id);
            if (!string.IsNullOrEmpty(context.Object.NewId))
                context.Add("newId", context.Object.NewId);
            if (!string.IsNullOrEmpty(context.Object.ChangeToken))
                context.Add("changeToken", context.Object.ChangeToken);
        }
    }
}