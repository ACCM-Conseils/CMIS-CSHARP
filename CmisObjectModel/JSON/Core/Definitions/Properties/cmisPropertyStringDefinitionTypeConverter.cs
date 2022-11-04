﻿using ccdp = CmisObjectModel.Core.Definitions.Properties;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Properties
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Definitions.Properties.cmisPropertyStringDefinitionTypeConverter))]
    public partial class cmisPropertyStringDefinitionType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Definitions.Properties
{
    [Attributes.AutoGenerated()]
    public class cmisPropertyStringDefinitionTypeConverter : Generic.cmisPropertyDefinitionTypeConverter<string, CmisObjectModel.Core.Choices.cmisChoiceString, CmisObjectModel.Core.Properties.cmisPropertyString, ccdp.cmisPropertyStringDefinitionType>
    {

        #region Constructors
        public cmisPropertyStringDefinitionTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<ccdp.cmisPropertyDefinitionType, ccdp.cmisPropertyStringDefinitionType>())
        {
        }
        public cmisPropertyStringDefinitionTypeConverter(Serialization.Generic.ObjectResolver<ccdp.cmisPropertyDefinitionType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            base.Deserialize(context);
            context.Object.MaxLength = ReadNullable(context.Dictionary, "maxLength", context.Object.MaxLength);
        }

        protected override void Serialize(SerializationContext context)
        {
            base.Serialize(context);
            if (context.Object.MaxLength.HasValue)
                context.Add("maxLength", context.Object.MaxLength.Value);
        }
    }
}