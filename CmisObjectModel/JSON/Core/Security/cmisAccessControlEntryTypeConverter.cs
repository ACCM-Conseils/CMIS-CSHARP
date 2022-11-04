using ccs = CmisObjectModel.Core.Security;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Security
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Security.cmisAccessControlEntryTypeConverter))]
    public partial class cmisAccessControlEntryType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Security
{
    public class cmisAccessControlEntryTypeConverter : Serialization.Generic.JavaScriptConverter<ccs.cmisAccessControlEntryType>
    {

        #region Constructors
        public cmisAccessControlEntryTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<ccs.cmisAccessControlEntryType>())
        {
        }
        public cmisAccessControlEntryTypeConverter(Serialization.Generic.ObjectResolver<ccs.cmisAccessControlEntryType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Principal = Read(context, "principal", context.Object.Principal);
            context.Object.Permissions = ReadArray(context.Dictionary, "permissions", context.Object.Permissions);
            context.Object.IsDirect = Read(context.Dictionary, "isDirect", context.Object.IsDirect);
        }

        protected override void Serialize(SerializationContext context)
        {
            Write(context, context.Object.Principal, "principal");
            WriteArray(context, context.Object.Permissions, "permissions");
            context.Add("isDirect", context.Object.IsDirect);
        }
    }
}