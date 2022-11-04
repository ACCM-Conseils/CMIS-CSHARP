using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Extensions.Alfresco
{
    /// <summary>
   /// Converter for Aspects-instances
   /// </summary>
   /// <remarks></remarks>
    public class AspectsConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Extensions.Alfresco.Aspects>
    {

        #region Constructors
        public AspectsConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Extensions.Alfresco.Aspects>())
        {
        }
        public AspectsConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Extensions.Alfresco.Aspects> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            var appliedAspects = context.Dictionary.ContainsKey("appliedAspects") ? context.Dictionary["appliedAspects"] as IEnumerable : null;

            if (appliedAspects is not null)
            {
                var propertiesType = typeof(CmisObjectModel.Core.Collections.cmisPropertiesType);
                var propertiesConverter = context.Serializer.GetJavaScriptConverter(propertiesType);
                var aspects = new List<CmisObjectModel.Extensions.Alfresco.Aspects.Aspect>();

                foreach (object rawAspect in appliedAspects)
                {
                    IDictionary<string, object> aspect = rawAspect as IDictionary<string, object>;

                    if (aspect is not null)
                    {
                        string aspectName = aspect.ContainsKey("aspectName") ? Conversions.ToString(aspect.ContainsKey("aspectName")) : null;
                        var propertyCollection = aspect.ContainsKey("properties") ? aspect["properties"] as IDictionary<string, object> : null;
                        var properties = propertyCollection is null ? null : propertiesConverter.Deserialize(propertyCollection, propertiesType, context.Serializer) as CmisObjectModel.Core.Collections.cmisPropertiesType;
                        aspects.Add(new CmisObjectModel.Extensions.Alfresco.Aspects.Aspect(aspectName, properties));
                    }
                }

                context.Object.AppliedAspects = aspects.ToArray();
            }
        }

        protected override void Serialize(SerializationContext context)
        {
            if (context.Object.AppliedAspects is not null)
            {
                var propertiesType = typeof(CmisObjectModel.Core.Collections.cmisPropertiesType);
                var propertiesConverter = context.Serializer.GetJavaScriptConverter(propertiesType);
                var aspects = new List<IDictionary<string, object>>();

                foreach (CmisObjectModel.Extensions.Alfresco.Aspects.Aspect appliedAspect in context.Object.AppliedAspects)
                {
                    if (appliedAspect is not null)
                    {
                        var aspect = new Dictionary<string, object>();

                        aspect.Add("aspectName", appliedAspect.AspectName);
                        if (appliedAspect.Properties is not null)
                        {
                            aspect.Add("properties", propertiesConverter.Serialize(appliedAspect.Properties, context.Serializer));
                        }
                        aspects.Add(aspect);
                    }
                }
                context.Add("appliedAspects", aspects.ToArray());
            }
            context.Add("extensionTypeName", context.Object.ExtensionTypeName);
        }

    }
}