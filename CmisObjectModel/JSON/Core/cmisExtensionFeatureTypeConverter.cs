using ccg = CmisObjectModel.Collections.Generic;
using ccg1 = CmisObjectModel.Common.Generic;
using cc = CmisObjectModel.Core;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    public partial class cmisExtensionFeatureKeyValuePair
    {
        public static ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair, string> DefaultKeyProperty = new ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair, string>(item => item._key, (item, value) => item.Key = value, "Key");

        public static ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair, string> DefaultValueProperty = new ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair, string>(item => item._value, (item, value) => item.Value = value, "Value");
    }

    [Attributes.JavaScriptConverter(typeof(JSON.Core.cmisExtensionFeatureTypeConverter))]
    public partial class cmisExtensionFeatureType
    {
        public ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair[]> DefaultArrayProperty
        {
            get
            {
                return new ccg1.DynamicProperty<cmisExtensionFeatureKeyValuePair[]>(() => _featureDatas, value => FeatureDatas = value, "FeatureDatas");
            }
        }
    }
}

namespace CmisObjectModel.JSON.Core
{
    public class cmisExtensionFeatureTypeConverter : Serialization.Generic.JavaScriptConverter<cc.cmisExtensionFeatureType>
    {

        #region Constructors
        public cmisExtensionFeatureTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<cc.cmisExtensionFeatureType>())
        {
        }
        public cmisExtensionFeatureTypeConverter(Serialization.Generic.ObjectResolver<cc.cmisExtensionFeatureType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.CommonName = Read(context.Dictionary, "commonName", context.Object.CommonName);
            context.Object.Description = Read(context.Dictionary, "description", context.Object.Description);
            if (context.Dictionary.ContainsKey("featureData"))
                CreateFeaturesMap(context.Object).JavaImport(context.Dictionary["featureData"], context.Serializer);
            context.Object.Id = Read(context.Dictionary, "id", context.Object.Id);
            context.Object.Url = Read(context.Dictionary, "url", context.Object.Url);
            context.Object.VersionLabel = Read(context.Dictionary, "versionLabel", context.Object.VersionLabel);
        }

        /// <summary>
      /// Creates an ArrayMapper for the featureDatas-property
      /// </summary>
        private ccg.ArrayMapper<cc.cmisExtensionFeatureType, cc.cmisExtensionFeatureKeyValuePair, string, string> CreateFeaturesMap(cc.cmisExtensionFeatureType features)
        {
            return new ccg.ArrayMapper<cc.cmisExtensionFeatureType, cc.cmisExtensionFeatureKeyValuePair, string, string>(features, features.DefaultArrayProperty, cc.cmisExtensionFeatureKeyValuePair.DefaultKeyProperty, cc.cmisExtensionFeatureKeyValuePair.DefaultValueProperty);
        }

        protected override void Serialize(SerializationContext context)
        {
            if (!string.IsNullOrEmpty(context.Object.CommonName))
                context.Add("commonName", context.Object.CommonName);
            if (!string.IsNullOrEmpty(context.Object.Description))
                context.Add("description", context.Object.Description);
            if (context.Object.FeatureDatas is not null)
                context.Add("featureData", CreateFeaturesMap(context.Object).JavaExport(null, context.Serializer));
            context.Add("id", context.Object.Id);
            if (!string.IsNullOrEmpty(context.Object.Url))
                context.Add("url", context.Object.Url);
            if (!string.IsNullOrEmpty(context.Object.VersionLabel))
                context.Add("versionLabel", context.Object.VersionLabel);
        }
    }
}