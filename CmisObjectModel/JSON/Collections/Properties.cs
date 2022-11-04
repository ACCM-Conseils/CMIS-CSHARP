using System.Collections.Generic;
using System.Linq;
using ccg = CmisObjectModel.Collections.Generic;
using CmisObjectModel.Common.Generic;
using ccc = CmisObjectModel.Core.Collections;
using ccp = CmisObjectModel.Core.Properties;
using cjs = CmisObjectModel.JSON.Serialization;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Collections
{
    public partial class cmisPropertiesType
    {
        public DynamicProperty<ccp.cmisProperty[]> DefaultArrayProperty
        {
            get
            {
                return new DynamicProperty<ccp.cmisProperty[]>(() => _properties.ToArray(), value => Properties = value, "Properties");
            }
        }
    }
}

namespace CmisObjectModel.JSON.Collections
{
    /// <summary>
   /// Representation of cmisPropertiesType as a string-to-cmisProperty-map
   /// </summary>
   /// <remarks></remarks>
    public class Properties : ccg.ArrayMapper<ccc.cmisPropertiesType, ccp.cmisProperty, string>
    {

        public Properties(ccc.cmisPropertiesType owner) : base(owner, owner.DefaultArrayProperty, ccp.cmisProperty.DefaultKeyProperty)
        {
        }

        #region IJavaSerializationProvider
        /// <summary>
      /// More comfortable access to JavaExport
      /// </summary>
        public new IDictionary<string, IDictionary<string, object>> JavaExport(cjs.JavaScriptSerializer serializer)
        {
            return base.JavaExport(this, serializer) as IDictionary<string, IDictionary<string, object>>;
        }

        /// <summary>
      /// More comfortable access to JavaImport
      /// </summary>
        public new ccc.cmisPropertiesType JavaImport(IDictionary<string, IDictionary<string, object>> source, cjs.JavaScriptSerializer serializer)
        {
            base.JavaImport(source, serializer);
            return _owner;
        }
        #endregion

    }
}