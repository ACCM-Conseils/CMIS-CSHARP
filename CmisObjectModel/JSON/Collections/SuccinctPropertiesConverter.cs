using System.Collections.Generic;
using ccc = CmisObjectModel.Core.Collections;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Collections
{
    /// <summary>
   /// Converter for the cmisPropertiesType when requested as succinctProperties-collection
   /// </summary>
   /// <remarks></remarks>
    public class SuccinctPropertiesConverter : Serialization.Generic.JavaScriptConverter<ccc.cmisPropertiesType>
    {

        #region Constructors
        public SuccinctPropertiesConverter() : base(new Serialization.Generic.DefaultObjectResolver<ccc.cmisPropertiesType>())
        {
        }
        public SuccinctPropertiesConverter(Serialization.Generic.ObjectResolver<ccc.cmisPropertiesType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        /// <summary>
      /// Deserializes the cmisPropertiesType from a string-to-object-map
      /// </summary>
        protected override void Deserialize(SerializationContext context)
        {
            var map = new SuccinctProperties(context.Object);
            map.JavaImport(context.Dictionary);
        }

        /// <summary>
      /// Serializes the cmisPropertiesType as a string-to-object-map
      /// </summary>
        protected override void Serialize(SerializationContext context)
        {
            var map = new SuccinctProperties(context.Object);
            var data = map.JavaExport();

            if (data is not null)
            {
                foreach (KeyValuePair<string, object> de in data)
                    context.Add(de.Key, de.Value);
            }
        }
    }
}