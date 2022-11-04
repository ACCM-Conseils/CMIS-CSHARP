using System.Collections.Generic;
using System.Linq;
using ccc = CmisObjectModel.Core.Collections;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Collections
{
    /// <summary>
   /// Default converter for the cmisPropertiesType
   /// </summary>
   /// <remarks></remarks>
    public class PropertiesConverter : Serialization.Generic.JavaScriptConverter<ccc.cmisPropertiesType>
    {

        public PropertiesConverter(Serialization.Generic.ObjectResolver<ccc.cmisPropertiesType> objectResolver) : base(objectResolver)
        {
        }

        /// <summary>
      /// Deserializes the cmisPropertiesType from a string-to-cmisProperty-map
      /// </summary>
        protected override void Deserialize(SerializationContext context)
        {
            var map = new Properties(context.Object);
            var data = context.Dictionary.ToDictionary(de => de.Key, de => de.Value as IDictionary<string, object>);
            map.JavaImport(data, context.Serializer);
        }

        /// <summary>
      /// Serializes the cmisPropertiesType as a string-to-cmisProperty-map
      /// </summary>
        protected override void Serialize(SerializationContext context)
        {
            var map = new Properties(context.Object);
            var data = map.JavaExport(context.Serializer);

            if (data is not null)
            {
                foreach (KeyValuePair<string, IDictionary<string, object>> de in data)
                    context.Add(de.Key, de.Value);
            }
        }
    }
}